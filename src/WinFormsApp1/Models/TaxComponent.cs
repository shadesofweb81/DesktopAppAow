using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class TaxComponent 
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaxType Type { get; set; }

        public decimal Rate { get; set; }

        [Required]
        [StringLength(50)]
        public string LedgerCode { get; set; } = string.Empty;

        public bool IsCreditAllowed { get; set; } // Whether input credit is allowed

        public string? ReturnFormNumber { get; set; }

        public Guid TaxId { get; set; }
        public TaxModel Tax { get; set; } = null!;

        // Primary ledger for this component
        public Guid LedgerId { get; set; }
      
        
        // Specific ledgers for tax receivable and payable accounting
        public Guid? ReceivableLedgerId { get; set; }
        public Guid? PayableLedgerId { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
} 