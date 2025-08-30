
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
}