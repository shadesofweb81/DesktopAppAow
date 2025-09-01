
using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class Transaction 
    {
        public Guid Id { get; set; } 

        [Required]
        [StringLength(50)]
        public string TransactionNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string? InvoiceNumber { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Freight { get; set; }
        public bool IsFreightIncluded { get; set; }
        public decimal RoundOff { get; set; }
        public decimal Total { get; set; }
        
        public decimal? PaidAmount { get; set; }
        public decimal? BalanceDue => Total - (PaidAmount ?? 0);
        public bool IsPaid => (PaidAmount ?? 0) >= Total;

        public string Status { get; set; } = "Draft"; // Draft, Pending, Approved, Rejected, Completed, Cancelled
        
        public string? ReferenceNumber { get; set; }
        public string? PaymentMethod { get; set; }
        
        [StringLength(200)]
        public string? PartyName { get; set; } // Party name (customer/supplier)

        // Journal voucher subtype (applies when Type == TransactionType.JournalEntry)
        public JournalEntryType? JournalEntryType { get; set; }

        // Navigation properties
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        
        public Guid? FinancialYearId { get; set; }
      

        // Collection of ledger entries (both debit and credit)
        public ICollection<TransactionLedger> LedgerEntries { get; set; } = new HashSet<TransactionLedger>();
        
        // Collection of items in the transaction
        public ICollection<TransactionItem> Items { get; set; } = new HashSet<TransactionItem>();

        // Collection of taxes applied to the transaction
        public ICollection<TransactionTax> Taxes { get; set; } = new HashSet<TransactionTax>();
        

    }

    public enum JournalEntryType
    {
        Journal,
        Sale,
        Purchase,
        Receipt,
        Payment,
        Expense,
        OpeningBalance
    }


    public enum TransactionLedgerType
    {
        Debit,
        Credit
    }

    public class TransactionItem
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public Guid ProductId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal CurrentQuantity { get; set; }

        public int SerialNumber { get; set; } // Serial number for ordering transaction items


    }

    public class TransactionLedger 
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public Guid LedgerId { get; set; }      

        [Required]
        public TransactionLedgerType Type { get; set; }

        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public bool? IsMainEntry { get; set; } // For primary ledger (customer/supplier)
        public bool? IsSystemEntry { get; set; } // For system generated entries (sales/purchase account)
    }

    // Paginated response model for transactions
    public class PaginatedTransactionListResponse
    {
        public List<Transaction> Items { get; set; } = new List<Transaction>();
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    // DTO for transaction list display (lightweight version)
    public class TransactionListDto
    {
        public Guid Id { get; set; }
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? Notes { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? BalanceDue { get; set; }
        public bool IsPaid { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PartyName { get; set; }
    }

    // Paginated response model for transaction list DTOs
    public class PaginatedTransactionListDtoResponse
    {
        public List<TransactionListDto> Items { get; set; } = new List<TransactionListDto>();
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    // DTO for GetTransactionById API response
    public class TransactionByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }      
        public string? TransactionType { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? JournalEntryType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string? Notes { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? PartyLedgerId { get; set; }
        public string? PartyName { get; set; }
        public string? AccountLedgerId { get; set; }
        public string? AccountName { get; set; }
        public decimal Discount { get; set; }
        public decimal Freight { get; set; }
        public bool IsFreightIncluded { get; set; }
        public decimal RoundOff { get; set; }
        public List<TransactionItemDto> Items { get; set; } = new List<TransactionItemDto>();
        public List<TransactionTaxDto> Taxes { get; set; } = new List<TransactionTaxDto>();
        public List<TransactionLedgerEntryDto> LedgerEntries { get; set; } = new List<TransactionLedgerEntryDto>();
    }

    public class TransactionItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string? ProductName { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }

    public class ProductVariantDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductVariantId { get; set; } = string.Empty;
        public string? VariantCode { get; set; }
        public string? VariantName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CurrentQuantity { get; set; }
        public int SerialNumber { get; set; }
        public string? Description { get; set; }
    }

    public class TransactionTaxDto
    {
        public string Id { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string? TaxName { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string? CalculationMethod { get; set; }
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
        public List<TransactionTaxComponentDto> Components { get; set; } = new List<TransactionTaxComponentDto>();
    }

    public class TransactionTaxComponentDto
    {
        public string Id { get; set; } = string.Empty;
        public string TaxComponentId { get; set; } = string.Empty;
        public string? ComponentName { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
    }

    public class TransactionLedgerEntryDto
    {
        public string Id { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string? LedgerName { get; set; }
        public string? EntryType { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsMainEntry { get; set; }
        public bool IsSystemEntry { get; set; }
    }
}