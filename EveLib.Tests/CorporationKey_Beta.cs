﻿using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Character;
using Xunit;

namespace eZet.EveLib.Test {
    public class CorporationKey_Beta {
        private const int SisiKeyId = 1467544;

        private const string SisiVCode = "Y7Uopdrv1CJ6iCp0LydYDQ932kkz0p0NvE4FLWydnbCHFZGzaE85erfczpR2XuPj";

        private readonly CorporationKey _sisiKey = new CorporationKey(SisiKeyId, SisiVCode);


        public CorporationKey_Beta() {
            _sisiKey.BaseUri = "https://api.testeveonline.com";
            _sisiKey.Corporation.BaseUri = "https://api.testeveonline.com";
        }

        [Fact]
        public async Task GetBlueprints() {
            EveXmlResponse<BlueprintList> result = await _sisiKey.Corporation.GetBlueprintsAsync();
        }
    }
}