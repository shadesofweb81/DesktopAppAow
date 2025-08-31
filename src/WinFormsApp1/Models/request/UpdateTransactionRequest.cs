
namespace AccountingERP.WebApi.Models.Requests
{
    public class UpdateTransactionRequest
    {
        public Guid CompanyId { get; set; }
        public Guid PartyLedgerId { get; set; }
        public Guid AccountLedgerId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string TransactionType { get; set; } = "SaleInvoice";
        public DateTime TransactionDate { get; set; }
        public int PaymentTermDays { get; set; } = 30;
        public string? Status { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Freight { get; set; }
        public bool IsFreightIncluded { get; set; }
        public decimal RoundOff { get; set; }
        public List<UpdateTransactionItemRequest> Items { get; set; } = new();
        public List<UpdateTransactionTaxRequest>? Taxes { get; set; }
    }

    public class UpdateTransactionItemRequest
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public List<UpdateTransactionItemVariantRequest> Variants { get; set; } = new();
    }

    public class UpdateTransactionItemVariantRequest
    {
        public Guid? Id { get; set; }
        public Guid ProductVariantId { get; set; }
        public string VariantCode { get; set; } = string.Empty;
        public string VariantName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UpdateTransactionTaxRequest
    {
        public Guid? Id { get; set; }
        public Guid TaxId { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Amount { get; set; }
        public string CalculationMethod { get; set; } = "ItemSubtotal"; // Default to ItemSubtotal
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int SerialNumber { get; set; } // Serial number for ordering taxes based on calculation method
        public List<UpdateTransactionTaxComponentRequest>? Components { get; set; }
    }

    public class UpdateTransactionTaxComponentRequest
    {
        public Guid? Id { get; set; }
        public Guid TaxComponentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public Guid? PayableLedgerId { get; set; }
        public bool IsDeleted { get; set; }
    }
} 