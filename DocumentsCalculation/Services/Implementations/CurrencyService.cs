using DocumentsCalculation.Services.Constracts;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DocumentsCalculation.Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        public IDictionary<string, decimal> PrepareExchangeRates(string currencies)
        {
            IDictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();
            IEnumerable<string> splitCurrencies = currencies.Split(",");

            foreach (string splitCurrency in splitCurrencies)
            {
                string[] currencyPair = splitCurrency.Split(":");
                string currency = currencyPair[0].ToLower();
                bool isValidCurrency = decimal.TryParse(currencyPair[1], NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal exchangeRate);

                if (isValidCurrency)
                {
                    exchangeRates.Add(currency, exchangeRate);
                }
            }

            return exchangeRates;
        }

        public KeyValuePair<string, decimal> RetrieveDefaultCurrency(IDictionary<string, decimal> currencies)
            => currencies.SingleOrDefault(c => c.Value == 1M);
    }
}