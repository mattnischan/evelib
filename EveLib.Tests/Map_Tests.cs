using System.Linq;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Map;
using Xunit;

namespace eZet.EveLib.Test {
    /// <summary>
    ///     Summary description for MapTest
    /// </summary>
    public class Map_Tests {
        private readonly Map _api = new Map();


        [Fact]
        public void TestFacWarSystems_ValidRequest_HasResult() {
            EveXmlResponse<FactionWarfareSystems> res = _api.GetFactionWarSystems();
            Assert.NotNull(res.Result.SolarSystems.First());
        }

        [Fact]
        public void TestJumps_ValidRequest_HasResult() {
            EveXmlResponse<Jumps> res = _api.GetJumps();
            Assert.NotEqual(0, res.Result.SolarSystems.First());
        }

        [Fact]
        public void TestKills_ValidRequest_HasResult() {
            EveXmlResponse<Kills> res = _api.GetKills();
            Assert.NotEqual(0, res.Result.SolarSystems.First());
        }

        [Fact]
        public void TestSovereignty_ValidRequest_HasResult() {
            EveXmlResponse<Sovereignty> res = _api.GetSovereignty();
            Assert.NotNull(res.Result.SolarSystems.First());
        }

        /// <summary>
        ///     Disabled by CCP
        /// </summary>
        [Fact]
        public void TestSovereigntyStatus_ValidRequest_HasResult() {
            //Assert.Inconclusive("Disabled by CCP.");
            //var res = _api.GetSovereigntyStatus();
        }
    }
}