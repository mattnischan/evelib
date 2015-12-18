using System.Linq;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Misc;
using eZet.EveLib.EveXmlModule.Models.Corporation;
using Xunit;

namespace eZet.EveLib.Test {
    public class Misc_Tests {
        private const string CharName = "CCP Garthagk";

        private const long CharId = 797400947;
        private readonly Eve api = new Eve();

        [Fact]
        public void GetAllianceList_ValidRequest_HasResult() {
            EveXmlResponse<AllianceList> xml = api.GetAllianceList(true);
            Assert.NotNull(xml.Result.Alliances.First());
            Assert.NotNull(xml.Result.Alliances.First().Corporations);
        }

        /// <summary>
        ///     Currently disabled by CCP
        /// </summary>
        [Fact]
        public void GetCertificateTree_ValidRequest_HasResult() {
            EveXmlResponse<CertificateTree> xml = api.GetCertificateTree();
            Assert.NotNull(xml.Result);
        }

        /// <summary>
        ///     Not yet implemented by CCP
        /// </summary>
        [Fact]
        public void GetCharacterAffiliation_ValidRequest_HasResult() {
            EveXmlResponse<CharacterAffiliation> xml = api.GetCharacterAffiliation(CharId);
            Assert.NotNull(xml.Result.Characters.First());
        }

        [Fact]
        public void GetCharacterId_ValidName_IdIsEqual() {
            EveXmlResponse<CharacterNameId> xml = api.GetCharacterId(CharName);
            Assert.Equal(CharId, xml.Result.Characters.First().CharacterId);
        }

        [Fact]
        public void GetCharacterInfo_ValidId_IdIsEqual() {
            EveXmlResponse<CharacterInfo> xml = api.GetCharacterInfo(CharId);
            Assert.Equal(CharId, xml.Result.CharacterId);
        }

        [Fact]
        public void GetCharacterName_ValidId_NameIsEqual() {
            EveXmlResponse<CharacterNameId> xml = api.GetCharacterName(CharId);
            Assert.Equal(CharName, xml.Result.Characters.First().CharacterName);
        }

        [Fact]
        public void GetConquerableStations_ValidRequest_HasResult() {
            EveXmlResponse<ConquerableStations> xml = api.GetConquerableStations();
            Assert.NotNull(xml.Result.Stations.First().StationName);
        }

        [Fact]
        public void GetErrorList_ValidRequest_HasResult() {
            EveXmlResponse<ErrorList> xml = api.GetErrorList();
            Assert.NotNull(xml.Result.Errors.First().ErrorText);
        }

        [Fact]
        public void GetFactionWarfareStats_ValidRequest_HasResult() {
            EveXmlResponse<EveXmlModule.Models.Misc.FactionWarfareStats> xml = api.GetFactionWarfareStats();
            Assert.NotNull(xml.Result.Factions.First().FactionName);
        }

        [Fact]
        public void GetFactionWarfareTopList_ValidRequest_HasResult() {
            EveXmlResponse<FactionWarTopStats> xml = api.GetFactionWarfareTopList();
            Assert.NotNull(xml.Result.Characters.KillsYesterday.First().CharacterName);
        }

        [Fact]
        public void GetReferenceTypes_ValidRequest_HasResult() {
            EveXmlResponse<ReferenceTypes> xml = api.GetReferenceTypes();
            Assert.NotNull(xml.Result.RefTypes.First().RefTypeName);
        }

        [Fact]
        public void GetSkillTree_ValidRequest_HasResult() {
            EveXmlResponse<SkillTree> xml = api.GetSkillTree();
            Assert.NotNull(xml.Result.Groups.First());
        }

        [Fact]
        public void GetTypeName_ValidId_HasResult() {
            EveXmlResponse<TypeName> xml = api.GetTypeName(12345);
            Assert.Equal("200mm Railgun I Blueprint", xml.Result.Types.First().TypeName);
        }

        [Fact]
        public void GetServerStatus_ValidRequest_HasResult() {
            EveXmlResponse<ServerStatus> xml = api.GetServerStatus();
            Assert.NotNull(xml.Result.ServerOpen);
        }

        [Fact]
        public void GetCallList_ValidRequest_HasResult() {
            EveXmlResponse<CallList> xml = api.GetCallList();
            Assert.NotNull(xml.Result.CallGroups.First());
            Assert.NotNull(xml.Result.Calls.First());
        }

        [Fact]
        public void GetOwner_ValidRequest_HasResult() {
            EveXmlResponse<OwnerCollection> xml = api.GetOwnerId(CharName);
            Assert.NotNull(xml.Result.Owners.First());
            Assert.Equal(CharId, xml.Result.Owners.First().OwnerId);
        }

        [Fact]
        public void GetCorporationSheet_ValidRequest_HasResult() {
            EveXmlResponse<CorporationSheet> xml = api.GetCorporationSheet(109299958);
            Assert.Equal("C C P", xml.Result.CorporationName);
        }


    }
}