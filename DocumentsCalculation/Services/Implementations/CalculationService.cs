using DocumentsCalculation.Exceptions;
using DocumentsCalculation.Models;
using DocumentsCalculation.Models.Enums;
using DocumentsCalculation.Services.Constracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Implementations
{
    public class CalculationService : ICalculationService
    {
        public readonly ICsvReaderService csvReaderService;
        public readonly ICurrencyService currencyService;

        public CalculationService(ICsvReaderService csvReaderService, ICurrencyService currencyService)
        {
            this.csvReaderService = csvReaderService;
            this.currencyService = currencyService;
        }

        public async Task<IEnumerable<CalculateInvoiceOutputModel>> CalculateDocumentsAsync(CalculateInvoiceInputModel input)
        {
            IDictionary<string, decimal> currencies = this.currencyService.PrepareExchangeRates(input.Currencies);

            ICollection<CustomerDataModel> customerData = await this.csvReaderService.ReadCsvAsync(input.UploadedFile);

            IEnumerable<CalculateInvoiceOutputModel> calculatedInvoices = this.CalculateInvoice(customerData, currencies, input);

            if (this.CheckForNegativeInvoices(calculatedInvoices))
            {
                throw new AppException("The total of all credit notes is bigger than the invoice.");
            }

            return calculatedInvoices;
        }

        private IEnumerable<CalculateInvoiceOutputModel> CalculateInvoice(ICollection<CustomerDataModel> customerData, IDictionary<string, decimal> currencies,
            CalculateInvoiceInputModel input)
        {
            IDictionary<string, decimal> result = new Dictionary<string, decimal>();

            foreach (CustomerDataModel data in customerData)
            {
                // Skip current row if filter by specific customer is present
                if (!string.IsNullOrEmpty(input.Customer) && !input.Customer.Equals(data.Customer)) continue;

                if (!string.IsNullOrEmpty(data.ParentDocument) && !this.CheckIfParentDocumentExists(customerData, data.ParentDocument, data.Customer))
                {
                    throw new AppException($"Parent document for document number - {data.DocumentNumber} does not exist.");
                }

                decimal total = data.Total;

                if (!data.Currency.ToLower().Equals(input.OutputCurrency.ToLower()))
                {
                    // Convert to base currency
                    decimal baseExchangeRate = this.currencyService.GetCurrencyExchangeRate(currencies, data.Currency);
                    total /= baseExchangeRate;

                    // Convert to output currency
                    decimal outputExchangeRage = this.currencyService.GetCurrencyExchangeRate(currencies, input.OutputCurrency);
                    total *= outputExchangeRage;
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

            return result.Select(r => new CalculateInvoiceOutputModel
            {
                Customer = r.Key,
                Total = Math.Round(r.Value, 2, MidpointRounding.ToEven)
            }); ;
        }

        private bool CheckIfParentDocumentExists(ICollection<CustomerDataModel> customerData, string parentDocument, string customer)
            => customerData.Any(cd => cd.DocumentNumber.Equals(parentDocument) && cd.Customer.Equals(customer));

        private bool CheckForNegativeInvoices(IEnumerable<CalculateInvoiceOutputModel> calculatedInvoices)
            => calculatedInvoices.Any(ci => ci.Total < 0);
    }
}