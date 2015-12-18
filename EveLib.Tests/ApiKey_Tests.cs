using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Account;
using Xunit;

namespace eZet.EveLib.Test {
    public class ApiKey_Tests {
        private const int KeyId = 4438372;

        private const string VCode = "v7NFEMJ8tMb74tN5XYPUZ5LwZuFlnFEbLCv2WhtH1UxYoDMrvEQkNljcI2hdBgjl";

        private readonly ApiKey _validKey = new ApiKey(KeyId, VCode);

        [Fact]
        public void Init_NoExceptions() {
            ApiKey res = _validKey.Init();
        }

        [Fact]
        public async Task InitAsync_NoExceptions() {
            ApiKey res = await _validKey.InitAsync();
        }

        [Fact]
        public void GetApiKeyInfo_NoExceptions() {
            EveXmlResponse<ApiKeyInfo> res = _validKey.GetApiKeyInfoAsync().Result;
        }

        [Fact]
        public void GetCharacterList_NoExceptions() {
            EveXmlResponse<CharacterList> res = _validKey.GetCharacterListAsync().Result;
        }

        [Fact]
        public void Properties_LazyLoaded() {
            Assert.Equal(ApiKeyType.Character, _validKey.KeyType);
            Assert.NotNull(_validKey.ExpiryDate);
            Assert.Equal(268435455, _validKey.AccessMask);
        }

        /// <summary>
        ///     Returns 403 Forbidden with error content on invalid key requests
        /// </summary>
        [Fact]
        public void IsValidKey_InvalidKey_NoExceptions() {
            var key = new ApiKey(0, "invalid");
            Assert.Equal(false, key.IsValidKey());
        }

        /// <summary>
        ///     Returns 403 Forbidden with error content on invalid key requests
        /// </summary>
        [Fact]
        public async Task IsValidKeyAsync_InvalidKey_NoExceptions() {
            var key = new ApiKey(0, "invalid");
            Assert.Equal(false, await key.IsValidKeyAsync());
        }
    }
}