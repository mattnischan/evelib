using System;
#if NET451
using System.Configuration;
#endif
using System.IO;
using eZet.EveLib.Core.Cache;
#if DOTNET5_4
using Microsoft.Extensions.Configuration;
using System.Reflection;
#endif

namespace eZet.EveLib.Core {
    /// <summary>
    ///     Provides configuration and constants for the library.
    /// </summary>
    public static class Config {

        /// <summary>
        ///     relCachePath to ApplicationData folder.
        /// </summary>
        public static string AppData { get; private set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public static string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the cache path.
        /// </summary>
        /// <value>The cache path.</value>
        public static string CachePath { get; set; }

        /// <summary>
        ///     The cache factory
        /// </summary>
        public static Func<string, IEveLibCache> CacheFactory { get; set; }

        /// <summary>
        ///     UserAgent used for HTTP requests
        /// </summary>
        public static string UserAgent { get; private set; }

        /// <summary>
        ///     The application name used for storing cache data.
        /// </summary>
        public static string ApplicationName { get; private set; }

        static Config()
        {
            SetConfig();
        }

#if DOTNET5_4
        private static void SetConfig()
        {
            //For DNX, use the current application folder as the base path
            var baseLocation = Path.GetDirectoryName(typeof(Config).GetTypeInfo().Assembly.Location);

            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("config.json");
                var config = builder.Build();

                BuildConfigValues(baseLocation, config["eveLib:appName"], config["eveLib:userAgent"]);
            }
            catch (FileNotFoundException) { }
        }
#endif

#if NET451
        private static void SetConfig()
        {
            var baseLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = ConfigurationManager.AppSettings["eveLib.AppData"];
            var userAgent = ConfigurationManager.AppSettings["eveLib.UserAgent"];

            BuildConfigValues(baseLocation, appName, userAgent);
        }
#endif

        private static void BuildConfigValues(string baseLocation, string appName, string userAgent)
        {
            appName = String.IsNullOrEmpty(appName) ? "EveLib" : appName;
            UserAgent = String.IsNullOrEmpty(userAgent) ? "EveLib" : appName;

            AppData = Path.Combine(baseLocation, appName);
            ImagePath = Path.Combine(AppData, "Images");
            CachePath = Path.Combine(AppData, "Cache");
            CacheFactory = module => new EveLibFileCache(Path.Combine(CachePath, module), "register");
        }
    }
}