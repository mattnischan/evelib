using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace eZet.EveLib.Core.RequestHandlers {
    /// <summary>
    ///     Simple implementation for requesting images
    /// </summary>
    public class ImageRequestHandler : IImageRequestHandler {
        private readonly TraceSource _trace = new TraceSource("EveLib", SourceLevels.All);

        /// <summary>
        ///     Requests and returns image data
        /// </summary>
        /// <param name="uri">URI to request</param>
        /// <returns>The image data</returns>
        public Task<byte[]> RequestImageDataAsync(Uri uri) {
            var client = new HttpClient();
            return client.GetByteArrayAsync(uri);
        }

        /// <summary>
        ///     Requests image and saves it to a file.
        /// </summary>
        /// <param name="uri">URI to request</param>
        /// <param name="file">File to save image as.</param>
        /// <returns>The task</returns>
        public async Task RequestImageAsync(Uri uri, string file) {
            var client = new HttpClient();
            var response = await client.GetStreamAsync(uri).ConfigureAwait(false);

            using (var fileStream = File.Open(file, FileMode.Create))
            {
                await response.CopyToAsync(fileStream).ConfigureAwait(false);
            }
        }
    }
}