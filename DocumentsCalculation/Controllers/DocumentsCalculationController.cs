using DocumentsCalculation.Models;
using DocumentsCalculation.Services.Constracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DocumentsCalculation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsCalculationController : ControllerBase
    {
        public readonly ICalculationService calculationService;

        public DocumentsCalculationController(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult> CalculateInvoice([FromForm] CalculateInvoiceInputModel input)
        {
            await this.calculationService.CalculateDocumentsAsync(input);

            return Ok();
        }
    }
}