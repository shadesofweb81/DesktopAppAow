using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class TransactionTax 
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public Guid TaxId { get; set; }      

        public decimal TaxableAmount { get; set; }

        public decimal TaxAmount { get; set; }

        [StringLength(50)]
        public string CalculationMethod { get; set; } = "ItemSubtotal"; // Default to ItemSubtotal

        public bool IsApplied { get; set; } = false;

        public DateTime? AppliedDate { get; set; }

        [StringLength(50)]
        public string? ReferenceNumber { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        
        public int SerialNumber { get; set; } // Serial number for ordering taxes based on calculation method

        public ICollection<TransactionTaxComponent> Components { get; set; } = new HashSet<TransactionTaxComponent>();
    }

    public class TransactionTaxComponent
    {
        public Guid Id { get; set; }
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