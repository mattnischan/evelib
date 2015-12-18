using eZet.EveLib.ZKillboardModule;
using eZet.EveLib.ZKillboardModule.Models;
using Xunit;

namespace eZet.EveLib.Test {
    public class ZKillboard_Tests {
        public ZKillboard Api = new ZKillboard();

        public ZKillboardOptions Options = new ZKillboardOptions();

        public ZKillboard_Tests() {
            Options.Limit = 1;
            Options.WSpace = true;
        }

        [Fact]
        public void GetKills_ValidRequest_NoErrors() {
            ZkbResponse result = Api.GetKills(Options);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetLosses_ValidRequest_NoErrors() {
            ZkbResponse result = Api.GetLosses(Options);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAll_ValidRequest_NoErrors() {
            ZkbResponse result = Api.GetLosses(Options);
            Assert.NotNull(result);
        }
    }
}