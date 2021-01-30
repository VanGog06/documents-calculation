using DocumentsCalculation.Models;
using DocumentsCalculation.Models.Enums;
using DocumentsCalculation.Services.Constracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Implementations
{
    public class CsvReaderService : ICsvReaderService
    {
        public async Task<ICollection<CustomerDataModel>> ReadCsvAsync(IFormFile csvFile)
        {
            ICollection<CustomerDataModel> customerData = new List<CustomerDataModel>();

            using (StreamReader reader = new StreamReader(csvFile.OpenReadStream()))
            {
                //Skip headers
                await reader.ReadLineAsync();

                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    string[] values = line.Split(',');

                    bool isValidType = Enum.TryParse(values[3], true, out InvoiceType type);
                    bool isValidTotal = decimal.TryParse(values[6], out decimal total);

                    customerData.Add(new CustomerDataModel
                    {
                        Customer = values[0],
                        VatNumber = values[1],
                        DocumentNumber = values[2],
                        Type = isValidType ? type : InvoiceType.Invalid,
                        ParentDocument = values[4],
                        Currency = values[5],
                        Total = isValidTotal ? total : default(decimal)
                    });
                }
            }

            return customerData;
        }
    }
}