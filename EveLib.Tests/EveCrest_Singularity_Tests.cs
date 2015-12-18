using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.Core.RequestHandlers;
using eZet.EveLib.EveCrestModule;
using Xunit;

namespace eZet.EveLib.Test {

    public class EveCrest_Singularity_Tests {

        private string accessToken =
            "ujBFKrtM80XwqTaFX_TgHtAX25wDEoCNzA6wxMs25av_YMuQ6ixsrmJCxr918vBw0UXLDDQeyPA79gPx0Vz4fA2";

        private string refreshToken = "UHtPutexwinz1u6xWC5S2Dj39GTDLgnLgUTQSyTvM1nTryMU7NbsmKWL9I32vTPd0";

        private string encodedKey =
            "Y2VmZTYwMWQ5ZjVhNDQ0MTgzZjhjNzMyNjc2NzA5ZmI6R3dnM0pOVDhWMERMWndiN1ptUmtlOXpKRFlwMWVQblVtOVY1enZqWQ==";


        public EveCrest Crest { get; set; }

        public EveCrest_Singularity_Tests() {
            Crest = new EveCrest(refreshToken, encodedKey);
            Crest.Host = "https://api-sisi.testeveonline.com/";
            Crest.EveAuth.Host = "sisilogin.testeveonline.com";
            Crest.RequestHandler.ThrowOnDeprecated = true;
            Crest.RequestHandler.ThrowOnMissingContentType = true;
            Crest.EnableAutomaticPaging = true;
            Crest.RequestHandler.CacheLevel = CacheLevel.BypassCache;
        }

        [Fact]
        public void RefreshToken() {
            var result = Crest.RefreshAccessToken();
        }

        [Fact]
        public void GetRoot() {
            var root = Crest.GetRoot();
        }

        [Fact]
        public async Task GetFittings() {
            var fittings =
                await (await (await (await Crest.GetRootAsync()).QueryAsync(r => r.Decode)).QueryAsync(r => r.Character)).QueryAsync(r => r.Fittings);
        }






    }
}
