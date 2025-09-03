
namespace WinFormsApp1.Documents.ExportPdf
{
    public class TransactionPdfModel
    {
        public string CompanyName { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public string ReportTitle { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public List<TransactionPdfItem> Transactions { get; set; } = new List<TransactionPdfItem>();
        public string FilterApplied { get; set; } = "All";
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class TransactionPdfItem
    {
        public string TransactionNumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime? DueDate { get; set; }
    }
}

