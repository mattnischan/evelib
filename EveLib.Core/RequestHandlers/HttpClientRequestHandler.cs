using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.Core.Serializers;
using System.Net.Http;
using System.Net;
using eZet.EveLib.Core;
using System.Diagnostics;
using eZet.EveLib.Core.Exceptions;

namespace eZet.EveLib.Core.RequestHandlers
{
    /// <summary>
    /// A base class for request handlers implemented using HttpClient.
    /// </summary>
    public class HttpClientRequestHandler : IPostRequestHandler
    {
        /// <summary>
        /// Gets or sets the serializer used to deserialize data
        /// </summary>
        public ISerializer Serializer { get; set; }

        /// <summary>
        /// The trace source for this request handler.
        /// </summary>
        protected TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);

        /// <summary>
        /// Creates a new HttpClientRequestHandler.
        /// </summary>
        public HttpClientRequestHandler() { }

        /// <summary>
        /// Creates a new HttpClientRequestHandler.
        /// </summary>
        /// <param name="serializer">The serializer to use to deserialize request responses.</param>
        public HttpClientRequestHandler(ISerializer serializer)
        {
            Serializer = serializer;
        }

        /// <summary>
        /// Performs a request and returns the deserialized response content.
        /// </summary>
        /// <typeparam name="T">Type to deserialize to</typeparam>
        /// <param name="uri">URI to request</param>
        /// <returns>Deserialized response</returns>
        public virtual async Task<T> RequestAsync<T>(Uri uri)
        {
            _trace.TraceEvent(TraceEventType.Verbose, 0, "HttpClientRequestHandler.Deserialize:Start");

            var client = CreateClient();
            return await HandleResponse<T>(await client.GetAsync(uri));
        }

        /// <summary>
        /// Performs an HTTP post request and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="uri">The URI of the endpoint to post to.</param>
        /// <param name="postData">The data to post to the endpoint.</param>
        /// <returns>The deserialized response data.</returns>
        public virtual async Task<T> PostRequestAsync<T>(Uri uri, string postData)
        {
            _trace.TraceEvent(TraceEventType.Verbose, 0, "HttpClientRequestHandler.Deserialize:Start");

            var client = CreateClient();
            var content = new StringContent(postData);
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            return await HandleResponse<T>(await client.PostAsync(uri, content));
        }

        /// <summary>
        /// Creates a new HttpClient with default EveLib values.
        /// </summary>
        /// <returns>A new HttpClient.</returns>
        protected HttpClient CreateClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.GZip
            };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", Config.UserAgent);

            return client;
        }

        /// <summary>
        /// Handles an HTTP response returned from a client request.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize to.</typeparam>
        /// <param name="response">The response to handle.</param>
        /// <returns>The deserialized response data.</returns>
        protected async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                var webException = new WebException(ex.Message);
                throw new EveLibWebException("A request caused a WebException.", webException);
            }

            var data = Serializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
            _trace.TraceEvent(TraceEventType.Verbose, 0, "HttpClientRequestHandler.Deserialize:Complete");
            return data;
        }
    }
}
