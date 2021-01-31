using DocumentsCalculation.Exceptions;
using DocumentsCalculation.Services.Constracts;
using DocumentsCalculation.Services.Implementations;
using System.Collections.Generic;
using Xunit;

namespace DocumentsCalculation.Tests
{
    public class CurrencyServiceTests
    {
        public const string FirstSampleCurrencies = "EUR:1,USD:0.987,GBP:0.878";
        public const string SecondSampleCurrencies = "EUR:1.22,USD:1,GBP:0.878";

        public static IDictionary<string, decimal> FirstSamplePreparedExchangeRates = new Dictionary<string, decimal>()
        {
            { "eur", 1M },
            { "usd", 0.987M },
            { "gbp", 0.878M },
        };

        public static IDictionary<string, decimal> SecondSamplePreparedExchangeRates = new Dictionary<string, decimal>()
        {
            { "eur", 1.22M },
            { "usd", 1M },
            { "gbp", 0.878M },
        };

        public static IEnumerable<object[]> FirstSampleCalculatedInvoices =>
            new List<object[]>
            {
                new object[]
                {
                    FirstSampleCurrencies,
                    FirstSamplePreparedExchangeRates
                },
                new object[]
                {
                    SecondSampleCurrencies,
                    SecondSamplePreparedExchangeRates
                }
            };

        [Theory]
        [MemberData(nameof(FirstSampleCalculatedInvoices))]
        public void PrepareExchangeRangeShouldReturnCorrectData(string currencies, IDictionary<string, decimal> expectedResult)
        {
            ICurrencyService currencyService = new CurrencyService();

            IDictionary<string, decimal> result = currencyService.PrepareExchangeRates(currencies);

            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData("EUR:1,USD:SomethingRandom,GBP:0.878")]
        [InlineData("EUR:1,USD:0.987,GBP:Test")]
        public void PrepareExchangeRangeShouldThrowExceptionWithWrongData(string currencies)
        {
            ICurrencyService currencyService = new CurrencyService();

            Assert.Throws<AppException>(() => currencyService.PrepareExchangeRates(currencies));
        }

        [Theory]
        [InlineData("EUR:1,USD:0.987,GBP:0.878", "EUR")]
        [InlineData("EUR:1,USD:0.987,GBP:0.878", "GBP")]
        public void GetCurrencyExchangeRateShouldReturnCorrectData(string currencies, string currency)
        {
            ICurrencyService currencyService = new CurrencyService();
            IDictionary<string, decimal> preparedExchangeRates = currencyService.PrepareExchangeRates(currencies);

            decimal result = currencyService.GetCurrencyExchangeRate(preparedExchangeRates, currency);

            Assert.Equal(preparedExchangeRates[currency.ToLower()], result);
        }

        [Theory]
        [InlineData("EUR:1,USD:0.987,GBP:0.878", "BGN")]
        [InlineData("EUR:1,USD:0.987,GBP:0.878", "Test")]
        public void GetCurrencyExchangeRatesShouldThrowExceptionWithWrongData(string currencies, string currency)
        {
            ICurrencyService currencyService = new CurrencyService();
            IDictionary<string, decimal> preparedExchangeRates = currencyService.PrepareExchangeRates(currencies);

            Assert.Throws<AppException>(() => currencyService.GetCurrencyExchangeRate(preparedExchangeRates, currency));
        }
    }
}