using System.ComponentModel.DataAnnotations;

namespace AccountingERP.Infrastructure.Entities
{
    public class TransactionTaxComponent : BaseEntity
    {
        public Guid TransactionTaxId { get; set; }
        public TransactionTax TransactionTax { get; set; } = null!;

        public Guid TaxComponentId { get; set; }
        public TaxComponent TaxComponent { get; set; } = null!;

        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsApplied { get; set; } = false;

        public DateTime? AppliedDate { get; set; }

        public string? ReferenceNumber { get; set; }
    }
} 