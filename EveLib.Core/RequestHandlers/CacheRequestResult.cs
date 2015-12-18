using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eZet.EveLib.Core.RequestHandlers
{
    /// <summary>
    /// A result returned when attempting to grab data from an EveLib cache.
    /// </summary>
    public class CacheRequestResult<T>
    {
        /// <summary>
        /// The value returned by the cache request.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// True if the cache request found a cached value, false otherwise.
        /// </summary>
        public bool IsCached { get; set; }
    }
}
