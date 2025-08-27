using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class LedgerModel 
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;  // e.g., "Customer", "Supplier", "Bank", etc.

     
        public string Code { get; set; } = string.Empty;

      
        public string? Address { get; set; }

      
        public string? City { get; set; }

      
        public string? State { get; set; }

     
        public string? ZipCode { get; set; }

       
        public string? Country { get; set; }

        public string? Phone { get; set; }

      
        public string? Email { get; set; }

        public string? Website { get; set; }

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

    public class UpdateLedgerModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Category { get; set; } = string.Empty;

        public string? Code { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxId { get; set; }

        public bool IsGroup { get; set; }
        public bool? IsRegistered { get; set; }

        // Parent-Child relationship
        public Guid? ParentId { get; set; }

        // Company ID is required for updates
        public Guid CompanyId { get; set; }
    }
} 