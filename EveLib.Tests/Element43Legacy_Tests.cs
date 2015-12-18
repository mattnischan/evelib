using System.Linq;
using eZet.EveLib.Element43Module;
using eZet.EveLib.Element43Module.Models;
using Xunit;
using System;

namespace eZet.EveLib.Test {
    public class Element43Legacy_Tests {
        private const int RegionId = 10000002;
        private const int TypeId = 34;
        private readonly Element43Legacy _api;
        private readonly Element43Options _validOptions;

        public Element43Legacy_Tests() {
            _api = new Element43Legacy();
            _validOptions = new Element43Options();
            _validOptions.Items.Add(TypeId);
            _validOptions.Region = RegionId;
        }

        [Fact]
        public void GetMarketStat_ValidRequest_ValidResponse() {
            MarketStatResponse res = _api.GetMarketStat(_validOptions);
            Element43MarketStatItem entry = res.Result.First();
            Assert.Equal(TypeId, entry.TypeId);
            Assert.NotEqual(0, entry.SellOrders.Average);
            Assert.NotEqual(0, entry.SellOrders.Volume);
            Assert.NotEqual(0, entry.SellOrders.Max);
            Assert.NotEqual(0, entry.SellOrders.Min);
            Assert.NotEqual(0, entry.SellOrders.StdDev);
            Assert.NotEqual(0, entry.SellOrders.Median);
            Assert.NotEqual(0, entry.SellOrders.Percentile);
            Assert.NotEqual(default(DateTime), entry.LastUpdate);
            Assert.NotEqual(0, entry.VolumeLastWeek);
        }
    }
}