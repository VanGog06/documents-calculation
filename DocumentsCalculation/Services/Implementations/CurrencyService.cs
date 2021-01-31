using DocumentsCalculation.Exceptions;
using DocumentsCalculation.Services.Constracts;
using System.Collections.Generic;
using System.Globalization;

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

                if (!isValidCurrency)
                {
                    throw new AppException($"Invalid exchange rate for {currency}");
                }

                if (isValidCurrency)
                {
                    exchangeRates.Add(currency, exchangeRate);
                }
            }

            return exchangeRates;
        }

        public decimal GetCurrencyExchangeRate(IDictionary<string, decimal> currencies, string currency)
        {
            if (!currencies.ContainsKey(currency.ToLower()))
            {
                throw new AppException($"Currency {currency} does not exist.");
            }

            return currencies[currency.ToLower()];
        }
    }
}