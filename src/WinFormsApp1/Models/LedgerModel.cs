using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class LedgerModel 
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;  // e.g., "Customer", "Supplier", "Bank", etc.

        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(20)]
        public string? ZipCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        [Url]
        public string? Website { get; set; }

        [StringLength(50)]
        public string? TaxId { get; set; }

        public bool IsGroup { get; set; }
        public bool? IsRegistered { get; set; }

        // Parent-Child relationship
        public Guid? ParentId { get; set; }
        public LedgerModel? Parent { get; set; }
        public ICollection<LedgerModel> Children { get; set; } = new HashSet<LedgerModel>();

        // Navigation properties
        public Guid CompanyId { get; set; }
        
        // Display property for list
        public string DisplayName => $"{Name} ({Category})";
       
    }
} 