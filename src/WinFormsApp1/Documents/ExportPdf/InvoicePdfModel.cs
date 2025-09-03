namespace WinFormsApp1.Documents
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string CustomerGSTIN { get; set; } = string.Empty;
        public string CustomerMobile { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyGSTIN { get; set; } = string.Empty;
        public string CompanyPhone { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string PlaceOfSupply { get; set; } = string.Empty;
        public List<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public decimal NetPayable { get; set; }
        public string AmountInWords { get; set; } = string.Empty;
        public string CopyType { get; set; } = "Original"; // Original, Duplicate, Triplicate
        public string InvoiceFormat { get; set; } = "Standard A4";
        public string TransactionType { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class InvoiceItemModel
    {
        public int SerialNumber { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string HSNCode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineTotal { get; set; }
    }
}