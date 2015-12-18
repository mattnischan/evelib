using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Account;
using eZet.EveLib.EveXmlModule.Models.Character;
using eZet.EveLib.EveXmlModule.Models.Misc;
using FactionWarfareStats = eZet.EveLib.EveXmlModule.Models.Character.FactionWarfareStats;
using Xunit;

namespace eZet.EveLib.Test {
    public class CharacterKey_ValidKeyTests {
        private const int KeyId = 3361423;
        private const string VCode = "g0BIlsWkwyr592n0QCh8FNoxnU8ddMHVc2APpv9FmICb3n4lAljMPfvYuS7oTzU7";


        private readonly CharacterKey _piKey = new CharacterKey(3460081,
            "wpzj1BPcUMrSGj9L4YmkHLtoRWXCOsIipMECdUDElUiA3GUWvC67cy5xUZltyYoI");

        private readonly CharacterKey _validKey = new CharacterKey(KeyId, VCode);

        [Fact]
        public void Characters_NoExceptions() {
            Assert.NotNull(_validKey.Characters.First().CharacterName);
        }

        [Fact]
        public void GetAccountStatus_ValidRequest_HasResult() {
            EveXmlResponse<AccountStatus> res = _validKey.GetAccountStatus();
            Assert.NotNull(res.Result.CreationDateAsString);
        }

        [Fact]
        public void GetCharacterInfo_ValidRequest_HasResult() {
            EveXmlResponse<CharacterInfo> res = _validKey.Characters[0].GetCharacterInfo();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetAccountBalance_ValidRequest_HasResult() {
            EveXmlResponse<AccountBalance> res = _validKey.Characters[0].GetAccountBalance();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetAssetList_ValidRequest_HasResult() {
            EveXmlResponse<AssetList> res = _validKey.Characters[0].GetAssetList();
            IEnumerable<AssetList.Item> list = res.Result.Flatten();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetCalendarEventAttendees_InvalidId_NoException() {
            EveXmlResponse<CalendarEventAttendees> res = _validKey.Characters[0].GetCalendarEventAttendees(1);
        }

        [Fact]
        public void GetCharacterSheet_ValidRequest_HasResult() {
            EveXmlResponse<CharacterSheet> res = _validKey.Characters[0].GetCharacterSheet();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetContactList_ValidRequest_HasResult() {
            EveXmlResponse<ContactList> res = _validKey.Characters[0].GetContactList();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetContactNotifications_ValidRequest_HasResult() {
            EveXmlResponse<ContactNotifications> res = _validKey.Characters[0].GetContactNotifications();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetContracts_ValidRequest_HasResult() {
            EveXmlResponse<ContractList> res = _validKey.Characters[0].GetContracts();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetContractItems_InvalidRequest_InvalidRequestException() {
            Assert.Throws<AggregateException>(() => 
            {
                EveXmlResponse<ContractItems> res = _validKey.Characters[0].GetContractItems(0);
            });
        }

        [Fact]
        public void GetContractBids_ValidRequest_HasResult() {
            EveXmlResponse<ContractBids> res = _validKey.Characters[0].GetContractBids();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetFactionWarfareStats_InvalidRequest_InvalidRequestException() {
            Assert.Throws<AggregateException>(() =>
            {
                EveXmlResponse<FactionWarfareStats> res = _validKey.Characters[0].GetFactionWarfareStats();
            });
        }

        [Fact]
        public async Task GetIndustryJobs_NoErrors() {
            EveXmlResponse<IndustryJobs> data = await _validKey.Characters.First().GetIndustryJobsAsync();
        }

        [Fact]
        public async Task GetIndustryJobsHistory_NoErrors() {
            EveXmlResponse<IndustryJobs> data = await _validKey.Characters.First().GetIndustryJobsAsync();
        }

        [Fact]
        public void GetKillLog() {
            // BUG Returns 000 OK when exhausted
            EveXmlResponse<KillLog> res = _validKey.Characters[0].GetKillLog();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetLocations_InvalidId_InvalidRequestException() {
            Assert.Throws<AggregateException>(() =>
            {
                EveXmlResponse<Locations> res = _validKey.Characters[0].GetLocations(0);
            });
        }

        [Fact]
        public void GetMailBodies_InvalidId_HasMissingMessageIds() {
            EveXmlResponse<MailBodies> res = _validKey.Characters[0].GetMailBodies(0);
            Assert.NotEqual(string.Empty, res.Result.MissingMessageIds);
        }

        [Fact]
        public void GetMailingLists_ValidRequest_HasResult() {
            EveXmlResponse<MailingLists> res = _validKey.Characters[0].GetMailingLists();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetMailMessages_ValidRequest_HasResult() {
            EveXmlResponse<MailMessages> res = _validKey.Characters[0].GetMailMessages();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetMarketOrders_ValidRequest_HasResult() {
            EveXmlResponse<MarketOrders> res = _validKey.Characters[0].GetMarketOrders();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetMedals_ValidRequest_HasResult() {
            EveXmlResponse<MedalList> res = _validKey.Characters[0].GetMedals();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetNotifications_ValidRequest_HasResult() {
            EveXmlResponse<NotificationList> res = _validKey.Characters[0].GetNotifications();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetNotificationTexts_InvalidId_HasMissingIds() {
            EveXmlResponse<NotificationTexts> res = _validKey.Characters[0].GetNotificationTexts(0);
            Assert.NotEqual(string.Empty, res.Result.MissingIds);
        }

        [Fact]
        public void GetResearch_ValidRequest_HasResult() {
            EveXmlResponse<Research> res = _validKey.Characters[0].GetResearch();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetSkillQueue_ValidRequest_HasResult() {
            EveXmlResponse<SkillQueue> res = _validKey.Characters[0].GetSkillQueue();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetSkillTraining_ValidRequest_HasResult() {
            EveXmlResponse<SkillTraining> res = _validKey.Characters[0].GetSkillTraining();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetStandings_ValidRequest_HasResult() {
            EveXmlResponse<StandingsList> res = _validKey.Characters[0].GetStandings();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetUpcomingCalendarEvents_ValidRequest_HasResult() {
            EveXmlResponse<UpcomingCalendarEvents> res = _validKey.Characters[0].GetUpcomingCalendarEvents();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetWalletJournal_ValidRequest_HasResult() {
            EveXmlResponse<WalletJournal> res = _validKey.Characters[0].GetWalletJournal();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetWalletJournalUntil_ValidRequest_HasResult() {
            List<WalletJournal.JournalEntry> res = _validKey.Characters[0].GetWalletJournalUntil(0);
            Assert.NotNull(res);
        }

        [Fact]
        public void GetWalletTransactions_ValidRequest_HasResult() {
            EveXmlResponse<WalletTransactions> res = _validKey.Characters[0].GetWalletTransactions();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetWalletTransactionsUntil_ValidRequest_HasResult() {
            List<WalletTransactions.Transaction> res = _validKey.Characters[0].GetWalletTransactionsUntil(0);
            Assert.NotNull(res);
        }

        [Fact]
        public void GetPlanetaryColonies_ValidRequest_HasResult() {
            EveXmlResponse<PlanetaryColonies> res = _piKey.Characters[0].GetPlanetaryColonies();
            Assert.NotNull(res.Result);
        }

        [Fact]
        public async Task GetPlanetaryPins_ValidRequest_HasResult() {
            EveXmlResponse<PlanetaryPins> res = await _piKey.Characters[0].GetPlanetaryPinsAsync(40003660);
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetPlanetaryRoutes_ValidRequest_HasResult() {
            EveXmlResponse<PlanetaryRoutes> res = _piKey.Characters[0].GetPlanetaryRoutes(40003660);
            Assert.NotNull(res.Result);
        }

        [Fact]
        public void GetPlanetaryLinks_ValidRequest_HasResult() {
            EveXmlResponse<PlanetaryLinks> res = _piKey.Characters[0].GetPlanetaryLinks(40003660);
            Assert.NotNull(res.Result);
        }

        [Fact]
        public async Task GetBlueprints_ValidRequest_HasResult() {
            BlueprintList res = (await _validKey.Characters[0].GetBlueprintsAsync()).Result;
        }

        [Fact]
        public void GetChatChannels_ValidRequest_Hasresult() {
            var res = _validKey.Characters[0].GetChatChannels();

        }
    }
}