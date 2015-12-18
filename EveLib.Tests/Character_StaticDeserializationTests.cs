using System.Linq;
using eZet.EveLib.Core.Serializers;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Character;
using eZet.EveLib.EveXmlModule.Models.Misc;
using eZet.EveLib.Test.Mocks;
using FactionWarfareStats = eZet.EveLib.EveXmlModule.Models.Character.FactionWarfareStats;
using Xunit;

namespace eZet.EveLib.Test {
    public class Character_StaticDeserializationTests {
        private readonly Character _character;

        public Character_StaticDeserializationTests() {
            _character = new Character(new CharacterKey(0, ""), 0);
            _character.RequestHandler = new StaticXmlRequestHandler(new XmlSerializer());
        }

        [Fact]
        public void GetCharacterInfo() {
            EveXmlResponse<CharacterInfo> xml = _character.GetCharacterInfoAsync().Result;
            Assert.Equal(99999999, xml.Result.CharacterId);
        }

        [Fact]
        public void GetAccountBalance() {
            EveXmlResponse<AccountBalance> xml = _character.GetAccountBalanceAsync().Result;
            Assert.Equal(4759, xml.Result.Accounts.First().AccountId);
        }

        [Fact]
        public void GetAssetList() {
            EveXmlResponse<AssetList> xml = _character.GetAssetListAsync().Result;
            Assert.Equal(150354641, xml.Result.Items.First().ItemId);
        }

        [Fact]
        public void GetCalendarEventAttendees() {
            EveXmlResponse<CalendarEventAttendees> xml = _character.GetCalendarEventAttendees(0);
            Assert.Equal(123456789, xml.Result.Attendees.First().CharacterId);
        }

        [Fact]
        public void GetCharacterSheet() {
            EveXmlResponse<CharacterSheet> xml = _character.GetCharacterSheet();
            Assert.Equal(150337897, xml.Result.CharacterId);
        }

        [Fact]
        public void GetContactList() {
            EveXmlResponse<ContactList> xml = _character.GetContactList();
            Assert.Equal(90000002, xml.Result.PersonalContacts.First().ContactId);
            Assert.Equal(90000002, xml.Result.CorporationContacts.First().ContactId);
            Assert.Equal(90000002, xml.Result.AllianceContacts.First().ContactId);
        }

        [Fact]
        public void GetContactNotifications() {
            EveXmlResponse<ContactNotifications> xml = _character.GetContactNotifications();
            Assert.Equal(308734131, xml.Result.Notifications.First().NotificationId);
        }

        [Fact]
        public void GetContracts() {
            EveXmlResponse<ContractList> xml = _character.GetContracts();
            // TODO Get sample
        }

        [Fact]
        public void GetContractItems() {
            EveXmlResponse<ContractItems> xml = _character.GetContractItems(0);
            Assert.Equal(600515136, xml.Result.Items.First().RecordId);
        }

        [Fact]
        public void GetContractBids() {
            EveXmlResponse<ContractBids> xml = _character.GetContractBids();
            Assert.Equal(123123123, xml.Result.Bids.First().BidId);
        }

        [Fact]
        public void GetFactionWarfareStats() {
            EveXmlResponse<FactionWarfareStats> xml = _character.GetFactionWarfareStats();
            Assert.Equal(500001, xml.Result.FactionId);
        }

        [Fact]
        public void GetIndustryJobs() {
            EveXmlResponse<IndustryJobs> xml = _character.GetIndustryJobs();
            Assert.Equal(23264063, xml.Result.Jobs.First().JobId);
        }

        [Fact]
        public void GetKillLog() {
            EveXmlResponse<KillLog> xml = _character.GetKillLog();
            Assert.Equal(63, xml.Result.Kills.First().KillId);
            Assert.Equal(150340823, xml.Result.Kills.First().Victim.CharacterId);
            Assert.Equal(1000127, xml.Result.Kills.First().Attackers.First().CorporationId);
        }

        [Fact]
        public void getLocations() {
            EveXmlResponse<Locations> xml = _character.GetLocations(0);
            Assert.Equal(887875612, xml.Result.Items.First().ItemId);
        }

        [Fact]
        public void GetMailBodies() {
            EveXmlResponse<MailBodies> xml = _character.GetMailBodies(0);
            Assert.Equal(297023723, xml.Result.Messages.First().MessageId);
        }

        [Fact]
        public void GetMailingLists() {
            EveXmlResponse<MailingLists> xml = _character.GetMailingLists();
            Assert.Equal(128250439, xml.Result.Lists.First().ListId);
        }

        [Fact]
        public void GetMailMessages() {
            EveXmlResponse<MailMessages> xml = _character.GetMailMessages();
            Assert.Equal(290285276, xml.Result.Messages.First().MessageId);
        }

        [Fact]
        public void GetMarketOrders() {
            EveXmlResponse<MarketOrders> xml = _character.GetMarketOrders();
            Assert.Equal(5630641, xml.Result.Orders.First().OrderId);
        }

        [Fact]
        public void GetMedals() {
            EveXmlResponse<MedalList> xml = _character.GetMedals();
            Assert.Equal(95079, xml.Result.Medals.First().MedalId);
        }

        [Fact]
        public void GetNotifications() {
            EveXmlResponse<NotificationList> xml = _character.GetNotifications();
            Assert.Equal(304084087, xml.Result.Notifications.First().NotificationId);
        }

        [Fact]
        public void GetNotificationTexts() {
            EveXmlResponse<NotificationTexts> xml = _character.GetNotificationTexts(0);
            Assert.Equal(374044083, xml.Result.Notifications.First().NotificationId);
        }

        [Fact]
        public void GetResearch() {
            EveXmlResponse<Research> xml = _character.GetResearch();
            Assert.Equal(3011113, xml.Result.Entries.First().AgentId);
        }

        [Fact]
        public void GetSkillQueue() {
            EveXmlResponse<SkillQueue> xml = _character.GetSkillQueue();
            Assert.Equal(11441, xml.Result.Queue.First().TypeId);
        }

        [Fact]
        public void GetSkillTraining() {
            EveXmlResponse<SkillTraining> xml = _character.GetSkillTraining();
            Assert.Equal(3305, xml.Result.TypeId);
        }

        [Fact]
        public void GetStandings() {
            EveXmlResponse<StandingsList> xml = _character.GetStandings();
            Assert.Equal(3009841, xml.Result.CharacterStandings.Agents.First().FromId);
        }

        [Fact]
        public void GetUpcomingCalendarEvents() {
            EveXmlResponse<UpcomingCalendarEvents> xml = _character.GetUpcomingCalendarEvents();
            Assert.Equal(93264, xml.Result.Events.First().EventId);
        }

        [Fact]
        public void GetWalletJournal() {
            EveXmlResponse<WalletJournal> xml = _character.GetWalletJournal();
            Assert.Equal(150337897, xml.Result.Journal.First().OwnerId);
        }

        [Fact]
        public void GetWalletTransactions() {
            EveXmlResponse<WalletTransactions> xml = _character.GetWalletTransactions();
            Assert.Equal(1309776438, xml.Result.Transactions.First().TransactionId);
        }

        [Fact]
        public void GetChatChannels_ValidRequest_Hasresult() {
            var res = _character.GetChatChannels();
            Assert.Equal(92168909, res.Result.Channels.First().Operators.First().AccessorId);
        }

        [Fact]
        public void GetBookmarks_ValidRequest_Hasresult() {
            var res = _character.GetBookmarks();
            Assert.Equal(0, res.Result.Folders.First().FolderId);
        }
    }
}