using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using eZet.EveLib.Core.Exceptions;
using eZet.EveLib.Core.Serializers;
using eZet.EveLib.Core.Util;
using System.Net.Http;

namespace eZet.EveLib.Core.RequestHandlers {
    /// <summary>
    ///     A basic RequestHandler with no special handling.
    /// </summary>
    public class RequestHandler : HttpClientRequestHandler {

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="serializer">Serializer to use for deserialization</param>
        public RequestHandler(ISerializer serializer)
            : base(serializer)
        { }
    }
}