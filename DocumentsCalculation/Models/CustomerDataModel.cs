using DocumentsCalculation.Models.Enums;

namespace DocumentsCalculation.Models
{
    public class CustomerDataModel
    {
        public string Customer { get; set; }

        public string VatNumber { get; set; }

        public string DocumentNumber { get; set; }

        public InvoiceType Type { get; set; }

        public string ParentDocument { get; set; }

        public string Currency { get; set; }

        public decimal Total { get; set; }
    }
}