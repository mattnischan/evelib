using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.EveCrestModule;
using eZet.EveLib.EveCrestModule.Exceptions;
using eZet.EveLib.EveCrestModule.Models.Links;
using eZet.EveLib.EveCrestModule.Models.Resources;
using eZet.EveLib.EveCrestModule.Models.Resources.Industry;
using eZet.EveLib.EveCrestModule.Models.Resources.Market;
using eZet.EveLib.EveCrestModule.Models.Resources.Tournaments;
using Xunit;

// ReSharper disable UnusedVariable

namespace eZet.EveLib.Test {
    public class EveCrest_Authed_Tests {
        private const int AllianceId = 99000006;

        private const int RegionId = 10000002; // The Forge

        private const int TypeId = 34; // Tritanium

        private const string Killmail = "30290604/787fb3714062f1700560d4a83ce32c67640b1797";

        private const string RefreshToken = "E9nZjXvx_tFu-PdpTC6yT_j4FupJ-84ybEtNsE8iMko1";

        private const string EncodedKey =
            "NDZkYWEyYjM3OGJkNGJjMTg5ZGY0YzNhNzNhZjIyNmE6SzhHY1dBRGxqZ25MWnlyS0dGZmlxekhWdlZpR2hhcE9ZU0NFeTgzaA==";

        private readonly EveCrest crest = new EveCrest(RefreshToken, EncodedKey);

        public EveCrest_Authed_Tests() {
            //crest.AccessToken =
            //    "UsIcawIKnTkLBknGg6Tjx-zFkU_XK0LOMWucbKXoaWrHjYtrldb8bZPjEEkj9rueXD97lYkInjg0urr7SbJ1UA2";
            crest.RefreshToken = RefreshToken;
            crest.EncodedKey = EncodedKey;
            crest.RequestHandler.ThrowOnDeprecated = true;
            crest.RequestHandler.ThrowOnMissingContentType = true;
            crest.EnableAutomaticPaging = true;
        }

        //[Fact]
        //public void CollectionPaging_Automatic() {
        //    IEnumerable<Alliance> result =
        //        crest.GetRoot().Query(r => r.Alliances).Query(r => r.Where(f => f.Name == "Brave Collective"));
        //    Debug.WriteLine(result.FirstOrDefault());
        //}

        //[Fact]
        //public void CollectionPaging_Manual() {
        //    AllianceCollection allianceLinks = crest.GetRoot().Query(r => r.Alliances);
        //    AllianceCollection.Alliance alliance = allianceLinks.Items.SingleOrDefault(f => f.Id == 99000738);
        //    while (alliance == null && allianceLinks.Next != null) {
        //        allianceLinks = allianceLinks.Query(f => f.Next);
        //        alliance = allianceLinks.Items.SingleOrDefault(f => f.Id == 99000738);
        //    }
        //    Debug.WriteLine(allianceLinks.Query(f => alliance).Name);
        //}

        //[Fact]
        //public void CollectionPaging_Manual_NullReference() {
        //    AllianceCollection allianceLinks = crest.GetRoot().Query(r => r.Alliances);
        //    AllianceCollection.Alliance alliance = allianceLinks.Items.SingleOrDefault(f => f.Id == 99000738);
        //    alliance = null;
        //    allianceLinks.Query(f => alliance);
        //}

        [Fact]
        public async Task RefreshAccessTokenAsync() {
            crest.RefreshToken = RefreshToken;
            crest.EncodedKey = EncodedKey;
            await crest.RefreshAccessTokenAsync();
        }

        [Fact]
        public void GetRoot() {
            CrestRoot root = crest.GetRoot();
            Assert.Equal(EveCrest.DefaultAuthHost, root.CrestEndpoint.Uri);
        }

        [Fact]
        public void GetKillmail_NoErrors() {
            Killmail data = crest.GetKillmail(28694894, "3d9702696cf8e75d6168734ad26a772e17efc9ba");
            Assert.Equal(30000131, data.SolarSystem.Id);
            Assert.Equal(99000652, data.Victim.Alliance.Id);
        }

        [Fact]
        public void Motd() {
            Assert.NotNull(crest.GetRoot().Motd.Eve.Uri);
        }

        [Fact]
        public async Task CrestEndpoint() {
            CrestRoot result = await crest.GetRootAsync();
        }

        [Fact]
        public async Task CorporationRoles() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.CorporationRoles);
        }

        [Fact]
        public async Task ItemGroups() {
            ItemGroupCollection response = await crest.GetRoot().QueryAsync(r => r.ItemGroups);
        }

        [Fact]
        public async Task Channels() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.Channels);
        }

        [Fact]
        public async Task Corporations() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.Corporations);
        }

        [Fact]
        public async Task Alliances() {
            AllianceCollection response = await crest.GetRoot().QueryAsync(r => r.Alliances);
        }

        [Fact]
        public async Task ItemTypes() {
            ItemTypeCollection response = await crest.GetRoot().QueryAsync(r => r.ItemTypes);
            MarketTypeCollection types = crest.GetRoot().Query(r => r.MarketTypes);
            List<MarketTypeCollection.Item> list = types.Items.ToList();
            while (types.Next != null) {
                types = types.Query(t => t.Next);
                list.AddRange(types.Items);
            }
        }

        [Fact]
        public async Task Decode() {
            TokenDecode response = await crest.GetRoot().QueryAsync(r => r.Decode);
            Debug.WriteLine(response.Character);
        }

        [Fact]
        public async Task BattleTheatres() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.BattleTheatres);
        }

        [Fact]
        public async Task MarketPrices() {
            MarketTypePriceCollection response = await crest.GetRoot().QueryAsync(r => r.MarketPrices);
        }

        [Fact]
        public async Task ItemCategories() {
            MarketTypePriceCollection response = await crest.GetRoot().QueryAsync(r => r.ItemCategories);
        }

        [Fact]
        public async Task Regions() {
            RegionCollection response = await crest.GetRoot().QueryAsync(r => r.Regions);
        }

        [Fact]
        public async Task Bloodlines() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.Bloodlines);
        }

        [Fact]
        public async Task MarketGroups() {
            MarketGroupCollection response = await crest.GetRoot().QueryAsync(r => r.MarketGroups);
        }

        [Fact]
        public async Task Tournaments() {
            TournamentCollection response = await crest.GetRoot().QueryAsync(r => r.Tournaments);
        }

        [Fact]
        public void Map() {
            Href<string> response = crest.GetRoot().VirtualGoodStore;
        }

        [Fact]
        public void VirtualGoodStore() {
            Assert.NotNull(crest.GetRoot().VirtualGoodStore);
        }

        [Fact]
        public void ServerVersion() {
            Assert.NotNull(crest.GetRoot().ServerVersion);
        }

        [Fact]
        public async Task Wars() {
            WarCollection response = await crest.GetRoot().QueryAsync(r => r.Wars);
        }

        [Fact]
        public async Task Incursions() {
            IncursionCollection response = await crest.GetRoot().QueryAsync(r => r.Incursions);
        }

        [Fact]
        public async Task Races() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.Races);
        }

        [Fact]
        public void AuthEndpoints() {
            Assert.Equal("https://login-tq.eveonline.com/oauth/token/", crest.GetRoot().AuthEndpoint.Uri);
        }

        [Fact]
        public void ServiceStatis() {
            CrestRoot.ServerStatus status = crest.GetRoot().ServiceStatus;
            Assert.Equal(CrestRoot.ServiceStatusType.Online, status.Dust);
            Assert.Equal(CrestRoot.ServiceStatusType.Online, status.Eve);
            Assert.Equal(CrestRoot.ServiceStatusType.Online, status.Server);
        }

        [Fact]
        public void UserCounts() {
            CrestRoot.UserCount counts = crest.GetRoot().UserCounts;
            Assert.NotEqual(0, counts.Dust);
            Assert.NotEqual(0, counts.Eve);
        }

        [Fact]
        public async Task IndustryFacilities() {
            IndustryFacilityCollection response = await crest.GetRoot().QueryAsync(r => r.Industry.Facilities);
        }

        [Fact]
        public async Task IndustrySpecialities() {
            IndustrySpecialityCollection response = await crest.GetRoot().QueryAsync(r => r.Industry.Specialities);
        }

        [Fact]
        public async Task IndustryTeamsInAuction() {
            IndustryTeamCollection response = await crest.GetRoot().QueryAsync(r => r.Industry.TeamsInAuction);
        }

        [Fact]
        public async Task IndustrySystems() {
            IndustrySystemCollection response = await crest.GetRoot().QueryAsync(r => r.Industry.Systems);
        }

        [Fact]
        public async Task IndustryTeams() {
            IndustryTeamCollection response = await crest.GetRoot().QueryAsync(r => r.Industry.Teams);
        }

        [Fact]
        public async Task Clients() {
            EveRoot eve = await crest.GetRoot().QueryAsync(r => r.Clients.Eve);
            DustRoot dust = await crest.GetRoot().QueryAsync(r => r.Clients.Dust);
        }

        [Fact]
        public async Task Time() {
            NotImplemented response = await crest.GetRoot().QueryAsync(r => r.Time);
        }


        [Fact]
        public async Task MarketTypes() {
            MarketTypeCollection response = await crest.GetRoot().QueryAsync(r => r.MarketTypes);
        }

        [Fact]
        public void ServerName() {
            Assert.Equal("TRANQUILITY", crest.GetRoot().ServerName);
        }

        [Fact]
        public async Task GetMarketHistory_NoErrors() {
            MarketHistoryCollection data = await crest.GetMarketHistoryAsync(RegionId, TypeId);
        }

        [Fact]
        public async Task GetMarketPrices() {
            MarketTypePriceCollection result = await crest.GetMarketPricesAsync();
            Console.WriteLine(result.Items.Count);
        }


        [Fact]
        public async Task GetWar_NoErrors() {
            War data = await crest.GetWarAsync(291410);
            byte[] image = crest.LoadImage(data.Aggressor.Icon);
            Debug.WriteLine(image);
        }

        [Fact]
        public async Task GetWarKillmails_NoErrors() {
            KillmailCollection data = await crest.GetWarKillmailsAsync(1);
        }

        [Fact]
        public async Task GetWar_InvalidId_EveCrestException() {
            await Assert.ThrowsAsync<AggregateException>(async () =>
            {
                War data = await crest.GetWarAsync(999999999);
            });
        }
    }
}