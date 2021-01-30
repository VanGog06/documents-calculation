using DocumentsCalculation.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Constracts
{
    public interface ICsvReaderService
    {
        Task<ICollection<CustomerDataModel>> ReadCsvAsync(IFormFile csvFile);
    }
}