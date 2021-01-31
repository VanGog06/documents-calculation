using DocumentsCalculation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Constracts
{
    public interface ICalculationService
    {
        Task<IEnumerable<CalculateInvoiceOutputModel>> CalculateDocumentsAsync(CalculateInvoiceInputModel input);
    }
}