using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DocumentsCalculation.Models
{
    public class CalculateInvoiceInputModel
    {
        [Required]
        public string Currencies { get; set; }

        [Required]
        public string OutputCurrency { get; set; }

        public string Customer { get; set; }

        [Required]
        public IFormFile UploadedFile { get; set; }
    }
}