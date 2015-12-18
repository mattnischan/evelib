using System;
using System.Linq;
using System.Threading.Tasks;
using eZet.EveLib.EveMarketDataModule;
using eZet.EveLib.EveMarketDataModule.Models;
using Xunit;

namespace eZet.EveLib.Test {
    public class EveMarketData_Tests {
        private const int RegionId = 10000002;

        private const int TypeId = 34;
        private readonly EveMarketData _api;
        private readonly EmdOptions _invalidOptions;
        private readonly EmdOptions _validOptions;

        public EveMarketData_Tests() {
            _api = new EveMarketData();
            _validOptions = new EmdOptions();
            _validOptions.Items.Add(TypeId);
            _validOptions.Regions.Add(RegionId);
            _validOptions.AgeSpan = TimeSpan.FromDays(5);
            _invalidOptions = new EmdOptions();
        }

        [Fact]
        public void GetRecentUploads_ValidRequest_ValidResponse() {
            EmdResponse<RecentUploads> res = _api.GetRecentUploads(_validOptions, UploadType.Orders);
            RecentUploads.RecentUploadsEntry entry = res.Result.Uploads.First();
            Assert.Equal(UploadType.Orders, entry.UploadType);
            Assert.Equal(TypeId, res.Result.Uploads.First().TypeId);
            Assert.Equal(RegionId, res.Result.Uploads.First().RegionId);
            Assert.NotEqual(default(UploadType), entry.UploadType);
            Assert.NotEqual(default(DateTime), entry.Updated);
        }

        [Fact]
        public async Task GetRecentUploadsAsync_ValidRequest_ValidResponse() {
            EmdResponse<RecentUploads> res =
                await _api.GetRecentUploadsAsync(_validOptions, UploadType.Orders);
            RecentUploads.RecentUploadsEntry entry = res.Result.Uploads.First();
            Assert.Equal(UploadType.Orders, entry.UploadType);
            Assert.Equal(TypeId, res.Result.Uploads.First().TypeId);
            Assert.Equal(RegionId, res.Result.Uploads.First().RegionId);
            Assert.NotEqual("", entry.UploadType);
            Assert.NotEqual("", entry.Updated);
        }

        [Fact]
        public void GetRecentUploads_NoOptions_NoException() {
            EmdResponse<RecentUploads> res = _api.GetRecentUploads(_invalidOptions, UploadType.Orders);
        }

        [Fact]
        public void GetItemPrice_ValidRequest_ValidResponse() {
            EmdResponse<ItemPrices> res = _api.GetItemPrice(_validOptions, OrderType.Buy, MinMax.Min);
            ItemPrices.ItemPriceEntry entry = res.Result.Prices.First();
            Assert.Equal(OrderType.Buy, entry.OrderType);
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal(RegionId, entry.RegionId);
            Assert.NotEqual(0, entry.Price);
            Assert.NotEqual("", entry.OrderType);
            Assert.NotEqual("", entry.Updated);
        }


        [Fact]
        public async Task GetItemPriceAsync_ValidRequest_ValidResponse() {
            EmdResponse<ItemPrices> res =
                await _api.GetItemPriceAsync(_validOptions, OrderType.Buy, MinMax.Min);
            ItemPrices.ItemPriceEntry entry = res.Result.Prices.First();
            Assert.Equal(OrderType.Buy, entry.OrderType);
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal(RegionId, entry.RegionId);
            Assert.NotEqual(0, entry.Price);
            Assert.NotEqual("", entry.OrderType);
            Assert.NotEqual("", entry.Updated);
        }

        [Fact]
        public void GetItemPrice_NoOptions_NoException() {
            EmdResponse<ItemPrices> res = _api.GetItemPrice(_invalidOptions, OrderType.Buy, MinMax.Min);
        }

        [Fact]
        public void GetItemOrders_ValidRequest_ValidResponse() {
            EmdResponse<ItemOrders> res = _api.GetItemOrders(_validOptions, OrderType.Buy);
            ItemOrders.ItemOrderEntry entry = res.Result.Orders.First();
            Assert.Equal(OrderType.Buy, entry.OrderType);
            Assert.NotEqual(0, entry.OrderId);
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal(RegionId, entry.RegionId);
            Assert.NotEqual(0, entry.StationId);
            Assert.NotEqual(0, entry.SolarSystemId);
            Assert.NotEqual(0, entry.Price);
            Assert.NotEqual(0, entry.VolEntered);
            Assert.NotEqual(0, entry.VolRemaining);
            Assert.NotEqual(0, entry.MinVolume);
            Assert.NotEqual("", entry.IssuedDate);
            Assert.NotEqual("", entry.ExpiresDate);
            Assert.NotEqual("", entry.CreatedDate);
        }

        [Fact]
        public void GetItemOrders_NoOptions_NoException() {
            EmdResponse<ItemOrders> res = _api.GetItemOrders(_invalidOptions, OrderType.Buy);
        }

        [Fact]
        public void GetItemHistory_ValidRequest_ValidResponse() {
            EmdResponse<ItemHistory> res = _api.GetItemHistory(_validOptions);
            ItemHistory.ItemHistoryEntry entry = res.Result.History.First();
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal(RegionId, entry.RegionId);
            Assert.NotEqual(0, entry.AvgPrice);
            Assert.NotEqual(0, entry.MaxPrice);
            Assert.NotEqual(0, entry.MinPrice);
            Assert.NotEqual(0, entry.Orders);
            Assert.NotEqual(0, entry.Volume);
            Assert.NotEqual("", entry.Date);
        }

        [Fact]
        public void GetItemHistory_InvalidArgument_ContractException() {
            Assert.Throws<Exception>(() =>
            {
                EmdResponse<ItemHistory> res = _api.GetItemHistory(_invalidOptions);
            });
        }

        //[Fact]
        public void GetStationRank_ValidRequest_ValidResponse() {
            EmdResponse<StationRank> res = _api.GetStationRank(_validOptions);
            StationRank.StationRankEntry entry = res.Result.Stations.First();
            Assert.NotEqual(0, entry.StationId);
            Assert.NotEqual("", entry.Date);
            Assert.NotEqual(0, entry.RankByOrders);
            Assert.NotEqual(0, entry.RankByPrice);
        }

        //[Fact]
        public async Task GetStationRank_ValidRequestAsync_ValidResponse() {
            EmdResponse<StationRank> res = await _api.GetStationRankAsync(_validOptions);
            StationRank.StationRankEntry entry = res.Result.Stations.First();
            Assert.NotEqual(0, entry.StationId);
            Assert.NotEqual("", entry.Date);
            Assert.NotEqual(0, entry.RankByOrders);
            Assert.NotEqual(0, entry.RankByPrice);
        }

        [Fact]
        public void GetStationRank_InvalidArgument_ContractException() {
            Assert.Throws<Exception>(() =>
            {
                EmdResponse<StationRank> res = _api.GetStationRank(_invalidOptions);
            });
        }
    }
}