using eZet.EveLib.StaticDataModule;
using eZet.EveLib.StaticDataModule.Models;
using Xunit;

namespace eZet.EveLib.Test {
    public class StaticData_Tests {
        public readonly EveStaticData Api = new EveStaticData();

        [Fact]
        public void GetInvTypes_ValidRequest_NoErrors() {
            StaticDataCollection<InvType> result = Api.GetInvTypes();
        }
    }
}