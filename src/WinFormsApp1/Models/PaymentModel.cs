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
    }

    public class PaymentByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string? InvoiceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public string Status { get; set; } = "Draft";
        public string? JournalEntryType { get; set; }
        public string? NatureOfTransaction { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string PartyLedgerId { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string AccountLedgerId { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Freight { get; set; }
        public bool IsFreightIncluded { get; set; }
        public decimal RoundOff { get; set; }
        public List<object> Items { get; set; } = new List<object>();
        public List<object> Taxes { get; set; } = new List<object>();
        
        // Ledger Entries (replaces PaymentDetails)
        public List<LedgerEntryDto> LedgerEntries { get; set; } = new List<LedgerEntryDto>();
        
        // Computed properties for backward compatibility
        public string PaymentNumber => TransactionNumber;
        public DateTime PaymentDate => TransactionDate;
        public string ReferenceNumber => string.Empty; // Not in API response
        public PaymentType PaymentType => GetPaymentTypeFromTransactionType(TransactionType);
        public decimal Amount => Total;
        
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


    public class UpdatePaymentRequest
    {
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<UpdatePaymentDetailRequest> PaymentDetails { get; set; } = new List<UpdatePaymentDetailRequest>();
    }

    public class UpdatePaymentDetailRequest
    {
        public string Id { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public PaymentDetailType DetailType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
    }
}
