using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.Core.Serializers;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Character;
using eZet.EveLib.EveXmlModule.Models.Corporation;
using eZet.EveLib.Test.Mocks;
using ContactList = eZet.EveLib.EveXmlModule.Models.Corporation.ContactList;
using FactionWarfareStats = eZet.EveLib.EveXmlModule.Models.Corporation.FactionWarfareStats;
using MedalList = eZet.EveLib.EveXmlModule.Models.Corporation.MedalList;
using StandingsList = eZet.EveLib.EveXmlModule.Models.Corporation.StandingsList;
using Xunit;

namespace eZet.EveLib.Test {
    public class Corporation_StaticDeserializationTests {
        private readonly Corporation _corp;

        public Corporation_StaticDeserializationTests() {
            _corp = new Corporation(new CorporationKey(0, ""), 0);
            _corp.RequestHandler = new StaticXmlRequestHandler(new XmlSerializer());
        }

        [Fact]
        public void GetAccountBalance() {
            EveXmlResponse<AccountBalance> xml = _corp.GetAccountBalance();
            Assert.Equal(4759, xml.Result.Accounts.First().AccountId);
        }

        [Fact]
        public void GetAssetList() {
            EveXmlResponse<AssetList> xml = _corp.GetAssetList();
            Assert.Equal(150354641, xml.Result.Items.First().ItemId);
        }

        [Fact]
        public void GetContactList() {
            EveXmlResponse<ContactList> xml = _corp.GetContactList();
            Assert.Equal(797400947, xml.Result.CorporationContacts.First().ContactId);
        }

        [Fact]
        public void GetContainerLog() {
            EveXmlResponse<ContainerLog> xml = _corp.GetContainerLog();
            Assert.Equal(2051471251, xml.Result.LogEntries.First().ItemId);
        }

        [Fact]
        public void GetContracts() {
            EveXmlResponse<ContractList> xml = _corp.GetContracts();
            // TODO Get sample
        }

        [Fact]
        public void GetContractItems() {
            EveXmlResponse<ContractItems> xml = _corp.GetContractItems(0);
            Assert.Equal(600515136, xml.Result.Items.First().RecordId);
        }

        [Fact]
        public void GetContractBids() {
            EveXmlResponse<ContractBids> xml = _corp.GetContractBids();
            Assert.Equal(123123123, xml.Result.Bids.First().BidId);
        }

        [Fact]
        public void GetCorporationSheet() {
            EveXmlResponse<CorporationSheet> xml = _corp.GetCorporationSheet();
            Assert.Equal(150212025, xml.Result.CorporationId);
        }

        [Fact]
        public void GetFactionalWarfareStats() {
            EveXmlResponse<FactionWarfareStats> xml = _corp.GetFactionWarfareStats();
            Assert.Equal(500001, xml.Result.FactionId);
        }

        [Fact]
        public void GetIndustryJobs() {
            EveXmlResponse<IndustryJobs> xml = _corp.GetIndustryJobs();
        }

        [Fact]
        public void GetKillLog() {
            EveXmlResponse<KillLog> xml = _corp.GetKillLog();
            Assert.Equal(63, xml.Result.Kills.First().KillId);
        }

        [Fact]
        public void GetLocations() {
            EveXmlResponse<Locations> xml = _corp.GetLocations(0);
            Assert.Equal(887875612, xml.Result.Items.First().ItemId);
        }

        [Fact]
        public void GetMarketOrders() {
            EveXmlResponse<MarketOrders> xml = _corp.GetMarketOrders();
            Assert.Equal(5630641, xml.Result.Orders.First().OrderId);
        }

        [Fact]
        public void GetMedals() {
            EveXmlResponse<MedalList> xml = _corp.GetMedals();
            Assert.Equal(123123, xml.Result.Medals.First().MedalId);
        }

        [Fact]
        public void GetMemberMedals() {
            EveXmlResponse<MemberMedals> xml = _corp.GetMemberMedals();
            Assert.Equal(24216, xml.Result.Medals.First().MedalId);
        }

        [Fact]
        public void GetMemberSecurity() {
            EveXmlResponse<MemberSecurity> xml = _corp.GetMemberSecurity();
            Assert.Equal(123456789, xml.Result.Members.First().CharacterId);
        }

        [Fact]
        public void GetMemberSecurityLog() {
            EveXmlResponse<MemberSecurityLog> xml = _corp.GetMemberSecurityLog();
            Assert.Equal(1234567890, xml.Result.RoleHistory.First().CharacterId);
        }

        [Fact]
        public void GetMemberTracking() {
            EveXmlResponse<MemberTracking> xml = _corp.GetMemberTracking(true);
            Assert.Equal(921331161, xml.Result.Members.First().CharacterId);
        }

        [Fact]
        public void GetOutpostList() {
            EveXmlResponse<OutpostList> xml = _corp.GetOutpostList();
            Assert.Equal(61000368, xml.Result.Outposts.First().StationId);
        }

        [Fact]
        public void GetOutpostServiceDetail() {
            EveXmlResponse<OutpostServiceDetails> xml = _corp.GetOutpostServiceDetails(0);
            Assert.Equal(61000368, xml.Result.Services.First().StationId);
        }

        [Fact]
        public void GetShareholders() {
            EveXmlResponse<ShareholderList> xml = _corp.GetShareholders();
            Assert.Equal(126891489, xml.Result.Shareholders.First().ShareholderId);
        }

        [Fact]
        public void GetStandings() {
            EveXmlResponse<StandingsList> xml = _corp.GetStandings();
            Assert.Equal(3009841, xml.Result.CorporationStandings.Agents.First().FromId);
        }

        [Fact]
        public void GetStarbaseDetail() {
            EveXmlResponse<StarbaseDetails> xml = _corp.GetStarbaseDetails(0);
            Assert.Equal(4, xml.Result.State);
        }

        [Fact]
        public void GetStarbaseList() {
            EveXmlResponse<StarbaseList> xml = _corp.GetStarbaseList();
            Assert.Equal(100449451, xml.Result.Starbases.First().ItemId);
        }

        [Fact]
        public void GetTitles() {
            EveXmlResponse<TitleList> xml = _corp.GetTitles();
            Assert.Equal(8192, xml.Result.Titles.First().RolesAtHq.First().RoleId);
        }

        [Fact]
        public void GetWalletJournal() {
            EveXmlResponse<WalletJournal> xml = _corp.GetWalletJournal();
            Assert.Equal(150337897, xml.Result.Journal.First().OwnerId);
        }

        [Fact]
        public void GetWalletTransactions() {
            EveXmlResponse<WalletTransactions> xml = _corp.GetWalletTransactions();
            Assert.Equal(1309776438, xml.Result.Transactions.First().TransactionId);
        }

        [Fact]
        public async Task GetBlueprints() {
            EveXmlResponse<BlueprintList> xml = await _corp.GetBlueprintsAsync();
            Assert.IsTrue(xml.Result.Blueprints.Any());
        }
    }
}