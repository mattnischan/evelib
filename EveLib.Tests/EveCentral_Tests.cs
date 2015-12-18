using System.Linq;
using eZet.EveLib.EveCentralModule;
using eZet.EveLib.EveCentralModule.Models;
using Xunit;

namespace eZet.EveLib.Test {
    public class EveCentral_Tests {
        private const int RegionId = 10000002;
        private const int TypeId = 34;
        private const int HourLimit = 5;
        private const int MinQty = 5;
        private readonly EveCentral _api;
        private readonly EveCentralOptions _validOptions;

        public EveCentral_Tests() {
            _api = new EveCentral();
            _validOptions = new EveCentralOptions {HourLimit = HourLimit, MinQuantity = MinQty};
            _validOptions.Items.Add(TypeId);
            _validOptions.Regions.Add(RegionId);
        }

        [Fact]
        public void GetMarketStat_ValidRequest_ValidResponse() {
            var options = new EveCentralOptions() {System = 30000142};
            options.Items.Add(34);
            MarketStatResponse res = _api.GetMarketStat(options);
            EveCentralMarketStatItem entry = res.Result.First();
            Assert.Equal(TypeId, entry.TypeId);
            Assert.NotEqual(0, entry.All.Average);
            Assert.NotEqual(0, entry.All.Volume);
            Assert.NotEqual(0, entry.All.Max);
            Assert.NotEqual(0, entry.All.Min);
            Assert.NotEqual(0, entry.All.StdDev);
            Assert.NotEqual(0, entry.All.Median);
            Assert.NotEqual(0, entry.All.Percentile);
        }

        [Fact]
        public void GetQuicklook_ValidRequest_ValidReseponse() {
            QuickLookResponse res = _api.GetQuicklook(_validOptions);
            QuicklookResult entry = res.Result;
            EveCentralQuicklookOrder order = entry.BuyOrders.First();
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal("Tritanium", entry.TypeName);
            Assert.Equal(HourLimit, entry.HourLimit);
            Assert.Equal(MinQty, entry.MinQuantity);
            Assert.NotEqual("", entry.Regions.First());
            Assert.NotEqual(0, order.MinVolume);
            Assert.NotEqual(0, order.OrderId);
            Assert.NotEqual(0, order.VolRemaining);
            Assert.NotEqual(0, order.Price);
            Assert.NotEqual(0, order.SecurityRating);
            Assert.NotEqual(0, order.StationId);
            Assert.NotEqual("", order.StationName);
            Assert.NotEqual("", order.Expires);
            Assert.NotEqual("", order.ReportedTime);
        }

        [Fact]
        public void GetQuicklookPath_ValidRequest_ValidResponse() {
            QuickLookResponse res = _api.GetQuicklookPath("Jita", "Amarr", 34, _validOptions);
            QuicklookResult entry = res.Result;
            EveCentralQuicklookOrder order = entry.BuyOrders.First();
            Assert.Equal(TypeId, entry.TypeId);
            Assert.Equal("Tritanium", entry.TypeName);
            Assert.Equal(HourLimit, entry.HourLimit);
            Assert.Equal(MinQty, entry.MinQuantity);
            Assert.NotEqual(0, order.MinVolume);
            Assert.NotEqual(0, order.OrderId);
            Assert.NotEqual(0, order.VolRemaining);
            Assert.NotEqual(0, order.Price);
            Assert.NotEqual(0, order.SecurityRating);
            Assert.NotEqual(0, order.StationId);
            Assert.NotEqual("", order.StationName);
            Assert.NotEqual("", order.Expires);
            Assert.NotEqual("", order.ReportedTime);
        }

        [Fact]
        public void GetHistory_ValidRequest_ValidResponse() {
            //api.GetHistory();
        }
    }
}