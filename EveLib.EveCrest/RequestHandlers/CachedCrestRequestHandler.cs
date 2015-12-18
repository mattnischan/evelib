using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using eZet.EveLib.Core;
using eZet.EveLib.Core.Cache;
using eZet.EveLib.Core.RequestHandlers;
using eZet.EveLib.Core.Serializers;
using eZet.EveLib.Core.Util;
using eZet.EveLib.EveCrestModule.Exceptions;
using eZet.EveLib.EveCrestModule.Models.Resources;
using eZet.EveLib.EveCrestModule.Models.Shared;
using eZet.EveLib.EveCrestModule.RequestHandlers.eZet.EveLib.Core.RequestHandlers;
using System.Net.Http;
using System.Linq;

namespace eZet.EveLib.EveCrestModule.RequestHandlers {
    /// <summary>
    ///     Performs requests on the Eve Online CREST API.
    /// </summary>
    public class CachedCrestRequestHandler : ICachedCrestRequestHandler {
        /// <summary>
        ///     The default public max concurrent requests
        /// </summary>
        public const int DefaultPublicMaxConcurrentRequests = 20;

        /// <summary>
        ///     The default authed max concurrent requests
        /// </summary>
        public const int DefaultAuthedMaxConcurrentRequests = 10;

        /// <summary>
        ///     The defualt charset
        /// </summary>
        public const string DefaultCharset = "utf-8";

        /// <summary>
        ///     The token type
        /// </summary>
        public const string TokenType = "Bearer";

        /// <summary>
        ///     Gets or sets the cache mode.
        /// </summary>
        /// <value>The cache mode.</value>
        public CacheLevel CacheLevel { get; set; }

        /// <summary>
        ///     Gets or sets the cache used by this request handler
        /// </summary>
        /// <value>The cache.</value>
        public IEveLibCache Cache { get; set; }

        private readonly TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);
        private int _authedMaxConcurrentRequests;
        private Semaphore _authedPool;
        private int _publicMaxConcurrentRequests;
        private Semaphore _publicPool;

        /// <summary>
        ///     Creates a new CachedCrestRequestHandler
        /// </summary>
        /// <param name="serializer"></param>
        public CachedCrestRequestHandler(ISerializer serializer) {
            Serializer = serializer;
            PublicMaxConcurrentRequests = DefaultPublicMaxConcurrentRequests;
            AuthedMaxConcurrentRequests = DefaultAuthedMaxConcurrentRequests;
            _publicPool = new Semaphore(PublicMaxConcurrentRequests, PublicMaxConcurrentRequests);
            _authedPool = new Semaphore(AuthedMaxConcurrentRequests, AuthedMaxConcurrentRequests);
            UserAgent = Config.UserAgent;
            Charset = DefaultCharset;
            Cache = new EveLibFileCache(Path.Combine(Config.AppData, "EveCrestCache"), "register");
            CacheLevel = CacheLevel.Default;
        }

        /// <summary>
        ///     Gets or sets the size of the public burst.
        /// </summary>
        /// <value>The size of the public burst.</value>
        public int PublicMaxConcurrentRequests {
            get { return _publicMaxConcurrentRequests; }
            set {
                _publicMaxConcurrentRequests = value;
                _publicPool = new Semaphore(value, value);
            }
        }

        /// <summary>
        ///     Gets or sets the size of the authed burst.
        /// </summary>
        /// <value>The size of the authed burst.</value>
        public int AuthedMaxConcurrentRequests {
            get { return _authedMaxConcurrentRequests; }
            set {
                _authedMaxConcurrentRequests = value;
                _authedPool = new Semaphore(value, value);
            }
        }

        /// <summary>
        ///     Sets or gets whether to throw a DeprecatedResourceException when requesting a deprecated resource. This will help
        ///     discover outdated models.
        /// </summary>
        public bool ThrowOnDeprecated { get; set; }

        /// <summary>
        ///     Sets or gets whether to throw a NotImplementedException when requesting a resource model with no ContentType. Used
        ///     for debugging.
        /// </summary>
        public bool ThrowOnMissingContentType { get; set; }

        /// <summary>
        ///     Gets or sets the serializer used to deserialize CREST errors.
        /// </summary>
        public ISerializer Serializer { get; set; }

        /// <summary>
        ///     Gets or sets the x requested with.
        /// </summary>
        /// <value>The x requested with.</value>
        public string XRequestedWith { get; set; }

        /// <summary>
        ///     Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        ///     Gets or sets the charset.
        /// </summary>
        /// <value>The charset.</value>
        public string Charset { get; set; }

        /// <summary>
        ///     Performs a request, and returns the response content.
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="uri">URI to request</param>
        /// <param name="accessToken">CREST acces token</param>
        /// <returns>T.</returns>
        /// <exception cref="DeprecatedResourceException">The CREST resource is deprecated.</exception>
        /// <exception cref="EveCrestException">
        ///     Undefined error
        ///     or
        ///     or
        /// </exception>
        public async Task<T> RequestAsync<T>(Uri uri, string accessToken) where T : class, ICrestResource<T> {

            var cacheRequest = await RequestFromCacheAsync<T>(uri);
            if(cacheRequest.IsCached)
            {
                return cacheRequest.Value;
            }

            var crestAccessMode = (accessToken == null) ? CrestMode.Public : CrestMode.Authenticated;
            var client = GetClient<T>(crestAccessMode, accessToken);

            _trace.TraceEvent(TraceEventType.Error, 0, "Initiating Request: " + uri);
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            try
            {
                return await HandleResponse<T>(uri, response);
            }
            catch (HttpRequestException e)
            {
                _trace.TraceEvent(TraceEventType.Error, 0, "CREST Request Failed.");
                var data = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.BadGateway)
                {
                    throw new EveCrestException(data, new WebException(e.Message));
                }

                var error = Serializer.Deserialize<CrestError>(data);
                _trace.TraceEvent(TraceEventType.Verbose, 0, "Message: {0}, Key: {1}",
                    "Exception Type: {2}, Ref ID: {3}", error.Message, error.Key, error.ExceptionType,
                    error.RefId);
                throw new EveCrestException(error.Message, new WebException(e.Message), error.Key, error.ExceptionType, error.RefId);
            }
            finally
            {
                //Release semaphores
                if (crestAccessMode == CrestMode.Authenticated)
                {
                    _authedPool.Release();
                }
                else
                {
                    _publicPool.Release();
                }
            }
        }

        /// <summary>
        /// Handles the response and gets the underlying data from the CREST endpoint.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="uri">The Uri of the endpoint that was requested.</param>
        /// <param name="response">The response from the API.</param>
        /// <returns>A deserialized response object.</returns>
        private async Task<T> HandleResponse<T>(Uri uri, HttpResponseMessage response) where T : class, ICrestResource<T>
        {
            response.EnsureSuccessStatusCode();
            if (response.Headers.Contains("X-Deprecated"))
            {
                _trace.TraceEvent(TraceEventType.Warning, 0,
                    "This CREST resource is deprecated. Please update to the newest EveLib version or notify the developers.");
                if (ThrowOnDeprecated)
                {
                    throw new DeprecatedResourceException("The CREST resource is deprecated.", null);
                }
            }

            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (CacheLevel == CacheLevel.Default || CacheLevel == CacheLevel.Refresh)
            {
                var expirationTime = GetCacheExpirationTime(response.Headers.GetValues("Cache-Control").FirstOrDefault());
                await Cache.StoreAsync(uri, expirationTime, data).ConfigureAwait(false);
            }

            var result = Serializer.Deserialize<T>(data);
            return result;
        }

        /// <summary>
        /// Gets a HTTP client from the client pool.
        /// </summary>
        /// <typeparam name="T">The type of request content that the client will recieve.</typeparam>
        /// <param name="authenticationMode">The authentication mode for this client's requests.</param>
        /// <param name="accessToken">The authentication access token, if any.</param>
        /// <returns>A new HTTP client.</returns>
        private HttpClient GetClient<T>(CrestMode authenticationMode, string accessToken) where T : class, ICrestResource<T>
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            };

            var client = new HttpClient(httpClientHandler);
            var acceptHeader = String.IsNullOrEmpty(Charset) ? ContentTypes.Get<T>(ThrowOnMissingContentType)
                : String.Format("{0} {1}", ContentTypes.Get<T>(ThrowOnMissingContentType), Charset);

            client.DefaultRequestHeaders.Add("Accept", acceptHeader);

            if(!String.IsNullOrEmpty(XRequestedWith))
            {
                client.DefaultRequestHeaders.Add("X-Requested-With", XRequestedWith);
            }

            if(!String.IsNullOrEmpty(UserAgent))
            {
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            }

            if (authenticationMode == CrestMode.Authenticated)
            {
                client.DefaultRequestHeaders.Add("Authorization", String.Format("{0} {1}", TokenType, accessToken));
                _authedPool.WaitOne();
            }
            else {
                _publicPool.WaitOne();
            }

            return client;
        }

        /// <summary>
        /// Gets the cache expiration time from a Cache-Control header value.
        /// </summary>
        /// <param name="headerValue">The Cache-Control header string value.</param>
        /// <returns>A DateTime representing the cache expiration.</returns>
        private static DateTime GetCacheExpirationTime(string headerValue)
        {
            if (headerValue == null)
            {
                return DateTime.UtcNow;
            }

            var str = headerValue.Substring(headerValue.IndexOf('=') + 1);
            var sec = int.Parse(str);
            return DateTime.UtcNow.AddSeconds(sec);
        }

        /// <summary>
        /// Requests a resource from the CREST cache.
        /// </summary>
        /// <typeparam name="T">The type of resource to request.</typeparam>
        /// <param name="uri">The Uri of the resource.</param>
        /// <returns>A CacheRequestResult indicating if the resource was cached and if so, the value of that resource.</returns>
        private async Task<CacheRequestResult<T>> RequestFromCacheAsync<T>(Uri uri) where T : class, ICrestResource<T>
        {
            string cachedResponse = null;
            if (CacheLevel == CacheLevel.Default || CacheLevel == CacheLevel.CacheOnly)
            {
                cachedResponse = await Cache.LoadAsync(uri).ConfigureAwait(false);
            }

            if (cachedResponse != null)
            {
                return new CacheRequestResult<T> { Value = Serializer.Deserialize<T>(cachedResponse), IsCached = true };
            }

            if (CacheLevel == CacheLevel.CacheOnly)
            {
                return new CacheRequestResult<T> { Value = default(T), IsCached = true };
            }

            return new CacheRequestResult<T> { Value = default(T), IsCached = false };
        }
    }
}