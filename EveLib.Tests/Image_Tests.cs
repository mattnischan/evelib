using eZet.EveLib.EveXmlModule;
using Xunit;

namespace eZet.EveLib.Test {
    public class Image_Tests {
        private const long CharId = 797400947;
        private readonly Image image = new Image();

        [Fact]
        public void GetCharacterPortrait_ValidRequest_NoExceptions() {
            //image.GetCharacterPortrait(CharId, Image.CharacterPortraitSize.X256);
        }

        [Fact]
        public void GetCharacterPortraitData_ValidRequest_NoExceptions() {
            image.GetCharacterPortraitData(CharId, Image.CharacterPortraitSize.X30);
        }
    }
}