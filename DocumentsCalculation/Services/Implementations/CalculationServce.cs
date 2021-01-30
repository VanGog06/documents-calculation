using DocumentsCalculation.Models;
using DocumentsCalculation.Models.Enums;
using DocumentsCalculation.Services.Constracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Implementations
{
    public class CalculationServce : ICalculationService
    {
        public readonly ICsvReaderService csvReaderService;
        public readonly ICurrencyService currencyService;

        public CalculationServce(ICsvReaderService csvReaderService, ICurrencyService currencyService)
        {
            this.csvReaderService = csvReaderService;
            this.currencyService = currencyService;
        }

        public async Task<string> CalculateDocumentsAsync(CalculateInvoiceInputModel input)
        {
            ICollection<CustomerDataModel> customerData = await this.csvReaderService.ReadCsvAsync(input.UploadedFile);

            IDictionary<string, decimal> currencies = this.currencyService.PrepareExchangeRates(input.Currencies);
            KeyValuePair<string, decimal> defaultCurrency = this.currencyService.RetrieveDefaultCurrency(currencies);

            this.CalculateInvoice(customerData, currencies, defaultCurrency);

            return "";
        }

        private void CalculateInvoice(ICollection<CustomerDataModel> customerData, IDictionary<string, decimal> currencies, KeyValuePair<string, decimal> defaultCurrency)
        {
            IDictionary<string, decimal> result = new Dictionary<string, decimal>();

            foreach (CustomerDataModel data in customerData)
            {
                decimal total = data.Total;

                if (!data.Currency.ToLower().Equals(defaultCurrency.Key.ToLower()))
                {
                    KeyValuePair<string, decimal> neededCurrency = currencies
                        .FirstOrDefault(c => c.Key.ToLower().Equals(data.Currency.ToLower()));

                    if (neededCurrency.Equals(default(KeyValuePair<string, decimal>)))
                    {
                        //Handle currency not present
                    }

                    total *= neededCurrency.Value;
                }

                if (!result.ContainsKey(data.Customer))
                {
                    result.Add(data.Customer, 0M);
                }

                if (data.Type == InvoiceType.Invoice || data.Type == InvoiceType.DebitNote)
                {
                    result[data.Customer] += total;
                }
                else
                {
                    result[data.Customer] -= total;
                }
            }

            System.Console.WriteLine();
        }
    }
}