

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WinFormsApp1.Models.request;

namespace WinFormsApp1.Models
{
    public class PaymentModel
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
        public string Status { get; set; } = "Draft";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Notes { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        // Payment Details
        public List<PaymentDetailModel> PaymentDetails { get; set; } = new List<PaymentDetailModel>();
    }

    public class PaymentDetailModel
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public string PaymentId { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string LedgerName { get; set; } = string.Empty;
        public string LedgerCode { get; set; } = string.Empty;
        public PaymentDetailType DetailType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
    }

    public class PaymentListDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = "Draft";
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PayFromLedgerName { get; set; }
        public string? PayToLedgerName { get; set; }
    }

    public class PaymentByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = string.Empty; // This is the TransactionType string from API
        public string Status { get; set; } = "Draft";
        public decimal Total { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string PayFromLedgerId { get; set; } = string.Empty;
        public string PayFromLedgerName { get; set; } = string.Empty;
        public string PayToLedgerId { get; set; } = string.Empty;
        public string PayToLedgerName { get; set; } = string.Empty;
        
        public List<LedgerEntryDto> LedgerEntries { get; set; } = new List<LedgerEntryDto>();
        public List<PaidInvoiceDto> PaidInvoices { get; set; } = new List<PaidInvoiceDto>();
        
        // Computed properties for backward compatibility
        public string PaymentNumber => TransactionNumber;
        public DateTime PaymentDate => TransactionDate;
        public PaymentType PaymentType => GetPaymentTypeFromTransactionType(Type);
        public decimal Amount => Total;
        public string TransactionType => Type; // Expose the raw API type
        public string? InvoiceNumber => PaidInvoices.Count == 1 ? PaidInvoices[0].InvoiceNumber : 
                                       PaidInvoices.Count > 1 ? "Multiple invoices" : null;
        public string PartyName => PayToLedgerName; // Assuming PayToLedger is the party
        public string AccountName => PayFromLedgerName; // Assuming PayFromLedger is the account
        
        private PaymentType GetPaymentTypeFromTransactionType(string transactionType)
        {
            return transactionType switch
            {
                "CashPayment" or "BankPayment" => PaymentType.PaymentMade,
                "CashReceipt" or "BankReceipt" => PaymentType.PaymentReceived,
                _ => PaymentType.PaymentMade
            };
        }
    }

    public class PaymentDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string LedgerName { get; set; } = string.Empty;
        public string LedgerCode { get; set; } = string.Empty;
        public PaymentDetailType DetailType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
    }

    public class LedgerEntryDto
    {
        public string Id { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string LedgerName { get; set; } = string.Empty;
        public string EntryType { get; set; } = string.Empty; // "Debit" or "Credit"
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsMainEntry { get; set; }
        public bool IsSystemEntry { get; set; }
    }

    public class PaidInvoiceDto
    {
        public string PaymentId { get; set; } = string.Empty;
        public string InvoiceId { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string InvoiceType { get; set; } = string.Empty;
        public decimal InvoiceTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class PaginatedPaymentListResponse
    {
        public List<PaymentListDto> Items { get; set; } = new List<PaymentListDto>();
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public enum PaymentType
    {
        PaymentReceived = 1,  // Customer payments (money coming in)
        PaymentMade = 2       // Supplier payments (money going out)
    }


    public enum PaymentDetailType
    {
        Debit = 1,
        Credit = 2
    }
}
