using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class PaymentModel
    {
        public string Id { get; set; } = string.Empty;
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
        public List<PaymentDetailDto> PaymentDetails { get; set; } = new List<PaymentDetailDto>();
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

    // Request models for API calls
    public class CreatePaymentRequest
    {
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public List<CreatePaymentDetailRequest> PaymentDetails { get; set; } = new List<CreatePaymentDetailRequest>();
    }

    public class CreatePaymentDetailRequest
    {
        public string LedgerId { get; set; } = string.Empty;
        public PaymentDetailType DetailType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
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
