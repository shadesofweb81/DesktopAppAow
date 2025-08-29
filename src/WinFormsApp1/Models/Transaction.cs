using AccountingERP.Infrastructure.Entities;
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
        
        // Collection of payments received (for invoices/bills)
        public ICollection<TransactionPayment> ReceivedPayments { get; set; } = new HashSet<TransactionPayment>();
        
        // Collection of payments made (for payment transactions)
        public ICollection<TransactionPayment> PaymentsMade { get; set; } = new HashSet<TransactionPayment>();
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
    public enum TransactionType
    {
        Invoice,
        Bill,
        PaymentReceived,
        PaymentMade,
        JournalEntry,
        CreditNote,
        DebitNote
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
}