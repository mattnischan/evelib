using System.Threading.Tasks;
using eZet.EveLib.EveWhoModule;
using eZet.EveLib.EveWhoModule.Models;
using Xunit;

namespace eZet.EveLib.Test {
    public class EveWho_Tests {
        private readonly EveWho _api = new EveWho();

        [Fact]
        public async Task GetCharacter_NoErrors() {
            var data = await _api.GetCharacterAsync(1633218082);
            Assert.NotNull(data.History);
        }

        [Fact]
        public async Task GetCorporation_NoErrors() {
            var data = await _api.GetCorporationAsync(869043665);
        }

        [Fact]
        public async Task GetCorporationMember_NoErrors() {
            var data = await _api.GetCorporationMembersAsync(869043665);
            Assert.NotNull(data.Members);
        }

        [Fact]
        public async Task GetAlliance_NoErrors() {
            var data = await _api.GetAllianceAsync(99001433);
        }

        [Fact]
        public async Task GetAllianceMembers_NoErrors() {
            var data = await _api.GetAllianceMembersAsync(99001433);
            Assert.NotNull(data.Members);
        }
    }
}