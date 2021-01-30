using DocumentsCalculation.Models;
using System.Threading.Tasks;

namespace DocumentsCalculation.Services.Constracts
{
    public interface ICalculationService
    {
        Task<string> CalculateDocumentsAsync(CalculateInvoiceInputModel input);
    }
}