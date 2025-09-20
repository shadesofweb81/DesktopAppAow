

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinFormsApp1.Models.request
{
    public class CreatePaymentRequest
    {
        [JsonPropertyName("companyId")]
        [Required]
        public Guid CompanyId { get; set; }

        [JsonPropertyName("financialYearId")]
        [Required]
        public Guid FinancialYearId { get; set; }

        [JsonPropertyName("transactionType")]
        [Required]
        public string TransactionType { get; set; } = "CashPayment";

        [JsonPropertyName("transactionNumber")]
        public string TransactionNumber { get; set; } = string.Empty;

        [JsonPropertyName("invoiceNumber")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [JsonPropertyName("payFromLedgerId")]
        [Required]
        public Guid PayFromLedgerId { get; set; }

        [JsonPropertyName("payToLedgerId")]
        [Required]
        public Guid PayToLedgerId { get; set; }

        [JsonPropertyName("amount")]
        [Required]
        public decimal Amount { get; set; }

        [JsonPropertyName("transactionDate")]
        [Required]
        public DateTime TransactionDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = "Bank";

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; } = string.Empty;

        [JsonPropertyName("invoices")]
        [Required]
        public List<InvoicePaymentItem> Invoices { get; set; } = new();
    }

    public class InvoicePaymentItem
    {
        [JsonPropertyName("transactionId")]
        [Required]
        public Guid TransactionId { get; set; }

        [JsonPropertyName("amount")]
        [Required]
        public decimal Amount { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdatePaymentRequest
    {
        [JsonPropertyName("companyId")]
        [Required]
        public Guid CompanyId { get; set; }

        [JsonPropertyName("financialYearId")]
        [Required]
        public Guid FinancialYearId { get; set; }

        [JsonPropertyName("transactionType")]
        [Required]
        public string TransactionType { get; set; } = "CashPayment";

        [JsonPropertyName("transactionNumber")]
        public string TransactionNumber { get; set; } = string.Empty;

        [JsonPropertyName("invoiceNumber")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [JsonPropertyName("payFromLedgerId")]
        [Required]
        public Guid PayFromLedgerId { get; set; }

        [JsonPropertyName("payToLedgerId")]
        [Required]
        public Guid PayToLedgerId { get; set; }

        [JsonPropertyName("amount")]
        [Required]
        public decimal Amount { get; set; }

        [JsonPropertyName("transactionDate")]
        [Required]
        public DateTime TransactionDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = "Bank";

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; } = string.Empty;

        [JsonPropertyName("invoices")]
        [Required]
        public List<InvoicePaymentItem> Invoices { get; set; } = new();
    }

  

}
