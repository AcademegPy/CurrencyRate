using System;
using System.Linq;
using CurrencyRate.Api.Services;
using Xunit;

namespace CurrencyRate.Api.Tests
{
    public class ValuteCursServiceTests
    {
        private readonly ValuteCursService _valuteCursService;
        public ValuteCursServiceTests()
        {
            var client = new TestSoapClient(
                WebServiceReference.DailyInfoSoapClient.EndpointConfiguration.DailyInfoSoap);
            _valuteCursService = new ValuteCursService(client);
        }
        [Fact]
        public void GetCursReturnsAllRecordsWhenInputIsNull()
        {

            var result = _valuteCursService.GetCurs(null, null).Result;
            Assert.Equal(3, result.Data.Count());
        }

        [Fact]
        public void GetCursReturnsCurrencyRateForValidCode()
        {
            var result = _valuteCursService.GetCurs(null, 826).Result;
            var currency = result.Data.FirstOrDefault();

            Assert.Equal("GBP", currency.VchCode);
        }
    }
}
