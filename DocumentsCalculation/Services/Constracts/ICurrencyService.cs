using System.Collections.Generic;

namespace DocumentsCalculation.Services.Constracts
{
    public interface ICurrencyService
    {
        IDictionary<string, decimal> PrepareExchangeRates(string currencies);

        decimal GetCurrencyExchangeRate(IDictionary<string, decimal> currencies, string currency);
    }
}