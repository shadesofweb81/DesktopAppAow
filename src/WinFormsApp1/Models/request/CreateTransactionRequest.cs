namespace AccountingERP.WebApi.Models.Requests
{
    public class CreateTransactionRequest
    {
        public Guid CompanyId { get; set; }
        public Guid FinancialYearId { get; set; }
        public Guid PartyLedgerId { get; set; }
        public Guid AccountLedgerId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public int PaymentTermDays { get; set; } = 30;
        public string? Notes { get; set; }
        public string TransactionType { get; set; } = "SaleInvoice";
        public string? Status { get; set; }
        public decimal Discount { get; set; }
        public decimal Freight { get; set; }
        public bool IsFreightIncluded { get; set; }
        public decimal RoundOff { get; set; }
        public List<CreateTransactionItemRequest> Items { get; set; } = new();
        public List<CreateTransactionTaxRequest> Taxes { get; set; } = new();
    }

    public class CreateTransactionItemRequest
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public string? HsnCode { get; set; }
        public List<CreateTransactionItemVariantRequest> Variants { get; set; } = new();
    }

    public class CreateTransactionItemVariantRequest
    {
        public Guid ProductVariantId { get; set; }
        public string VariantCode { get; set; } = string.Empty;
        public string VariantName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public string? Description { get; set; }
    }

    public class CreateTransactionTaxRequest
    {
        public Guid Id { get; set; }
        public Guid TaxId { get; set; }
        public string TaxName { get; set; } = string.Empty;
        public decimal TaxableAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string CalculationMethod { get; set; } = "ItemSubtotal";
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
        public List<CreateTransactionTaxComponentRequest> Components { get; set; } = new();
    }

    public class CreateTransactionTaxComponentRequest
    {
        public Guid Id { get; set; }
        public Guid TaxComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public Guid? LedgerId { get; set; }
        public Guid? ReceivableLedgerId { get; set; }
        public Guid? PayableLedgerId { get; set; }
    }
}
