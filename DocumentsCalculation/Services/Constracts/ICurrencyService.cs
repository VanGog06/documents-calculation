using System.Collections.Generic;

namespace DocumentsCalculation.Services.Constracts
{
    public interface ICurrencyService
    {
        IDictionary<string, decimal> PrepareExchangeRates(string currencies);

        KeyValuePair<string, decimal> RetrieveDefaultCurrency(IDictionary<string, decimal> currencies);
    }
}