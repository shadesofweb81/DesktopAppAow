using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class TransactionTypeSettings
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }      

        [Required]
        public TransactionType TransactionType { get; set; }

        // Invoice Generation Settings
        public bool AutoGenerateInvoice { get; set; } = true;
        public bool AllowDuplicateInvoiceNumber { get; set; } = false;

        // Address Display Settings
        public bool ShowBillingAddress { get; set; } = true;
        public bool ShowShippingAddress { get; set; } = true;
        public bool RequireBillingAddress { get; set; } = false;
        public bool RequireShippingAddress { get; set; } = false;

        // Invoice Number Settings
        [StringLength(20)]
        public string? InvoiceNumberPrefix { get; set; }
        
        [StringLength(20)]
        public string? InvoiceNumberSuffix { get; set; }
        
        public int? InvoiceNumberLength { get; set; } = 6;
        
        public int? NextInvoiceNumber { get; set; } = 1;

        // Default address settings
        public bool CopyBillingToShipping { get; set; } = false;
        public bool CopyShippingToBilling { get; set; } = false;
    }
} 