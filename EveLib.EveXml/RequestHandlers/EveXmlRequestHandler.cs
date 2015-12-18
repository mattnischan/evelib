using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using eZet.EveLib.Core.Cache;
using eZet.EveLib.Core.RequestHandlers;
using eZet.EveLib.Core.Serializers;
using eZet.EveLib.Core.Util;
using eZet.EveLib.EveXmlModule.Exceptions;
using eZet.EveLib.EveXmlModule.Models;
using System.Net.Http;
using eZet.EveLib.Core;

namespace eZet.EveLib.EveXmlModule.RequestHandlers {
    /// <summary>
    ///     Handles requests to the Eve API using a cache.
    /// </summary>
    public class EveXmlRequestHandler : ICachedRequestHandler {
        private readonly TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);

        /// <summary>
        ///     Gets or sets the Cache.
        /// </summary>
        public IEveLibCache Cache { get; set; }

        /// <summary>
        ///     Gets or sets the cache level.
        /// </summary>
        /// <value>The cache level.</value>
        public CacheLevel CacheLevel { get; set; }

        /// <summary>
        ///     Gets or sets the serializer.
        /// </summary>
        public ISerializer Serializer { get; set; }

        /// <summary>
        ///     Requests data from uri, with error handling specific to the Eve Online API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<T> RequestAsync<T>(Uri uri)
        {
            var cacheResult = await RequestFromCacheAsync<T>(uri);
            if(cacheResult.IsCached)
            {
                return cacheResult.Value;
            }

            var client = GetClient();
            var response = await client.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();

                var xml = Serializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
                if (CacheLevel == CacheLevel.Default || CacheLevel == CacheLevel.Refresh)
                {
                    await Cache.StoreAsync(uri, GetCacheExpirationTime(xml), data).ConfigureAwait(false);
                }

                return xml;
            }
            catch (HttpRequestException e)
            {
                _trace.TraceEvent(TraceEventType.Error, 0, "Http Request failed");
                var data = await response.Content.ReadAsStringAsync();

                var error = Serializer.Deserialize<EveXmlError>(data);
                _trace.TraceEvent(TraceEventType.Verbose, 0, "Error: {0}, Code: {1}", error.Error.ErrorText,
                    error.Error.ErrorCode);

                throw new EveXmlException(error.Error.ErrorText, error.Error.ErrorCode, new WebException(e.Message));
            } 
        }

        /// <summary>
        ///     Gets the CachedUntil value from a EveXmlResponse object.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private DateTime GetCacheExpirationTime(dynamic xml) {
            //if (o.GetType().Is) throw new System.Exception("Should never occur.");
            return xml.CachedUntil;
        }

        /// <summary>
        /// Requests a resource from the CREST cache.
        /// </summary>
        /// <typeparam name="T">The type of resource to request.</typeparam>
        /// <param name="uri">The Uri of the resource.</param>
        /// <returns>A CacheRequestResult indicating if the resource was cached and if so, the value of that resource.</returns>
        private async Task<CacheRequestResult<T>> RequestFromCacheAsync<T>(Uri uri)
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

        /// <summary>
        /// Gets a HTTP client from the client pool.
        /// </summary>
        /// <returns>A new HTTP client.</returns>
        private HttpClient GetClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", Config.UserAgent);

            return new HttpClient(httpClientHandler);
        }
    }
}