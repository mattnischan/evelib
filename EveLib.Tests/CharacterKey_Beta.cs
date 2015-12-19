﻿using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Character;
using Xunit;

namespace eZet.EveLib.Test {
    public class CharacterKey_Beta {
        private const string SisiVCode = "nNsODzp8SwufvWeMUmE0UINIMxTpNpEqqr2MOn6DmAXgQBRjKwFc7C4i5pp5uVRX";

        private const int SisiKeyId = 1467543;

        private readonly CharacterKey _sisiKey = new CharacterKey(SisiKeyId, SisiVCode);


        public CharacterKey_Beta() {
            _sisiKey.BaseUri = "https://api.testeveonline.com";
            _sisiKey.Characters.First().BaseUri = "https://api.testeveonline.com";
        }

        [Fact]
        public async Task GetBlueprints() {
            EveXmlResponse<BlueprintList> result = await _sisiKey.Characters.First().GetBlueprintsAsync();
        }
    }
}