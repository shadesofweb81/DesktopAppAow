using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class JournalEntry
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string EntryNumber { get; set; } = string.Empty;
        
        [Required]
        public JournalEntryType Type { get; set; }
        
        public DateTime EntryDate { get; set; }
        
        [StringLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public string Status { get; set; } = "Draft"; // Draft, Posted, Cancelled
        
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }
        public bool IsBalanced => Math.Abs(Difference) < 0.01m;
        
        // Navigation properties
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        
        public Guid FinancialYearId { get; set; }
        public FinancialYearModel FinancialYear { get; set; } = null!;
        
        // Collection of ledger entries
        public ICollection<JournalEntryLedger> LedgerEntries { get; set; } = new HashSet<JournalEntryLedger>();
    }


    public enum JournalEntryLedgerType
    {
        Debit,
        Credit
    }

    public class JournalEntryLedger
    {
        public Guid Id { get; set; }
        public Guid JournalEntryId { get; set; }
        public JournalEntry JournalEntry { get; set; } = null!;
        
        public Guid LedgerId { get; set; }
        public LedgerModel Ledger { get; set; } = null!;
        
        [Required]
        public JournalEntryLedgerType Type { get; set; }
        
        public decimal Amount { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public int SerialNumber { get; set; }
    }

    // DTO for creating journal entries
    public class CreateJournalEntryRequest
    {
        [Required]
        public string EntryNumber { get; set; } = string.Empty;
        
        [Required]
        public JournalEntryType Type { get; set; }
        
        [Required]
        public DateTime EntryDate { get; set; }
        
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Draft";
        
        [Required]
        public string CompanyId { get; set; } = string.Empty;
        
        [Required]
        public string FinancialYearId { get; set; } = string.Empty;
        
        public List<CreateJournalEntryLedgerRequest> LedgerEntries { get; set; } = new List<CreateJournalEntryLedgerRequest>();
    }

    public class CreateJournalEntryLedgerRequest
    {
        [Required]
        public string LedgerId { get; set; } = string.Empty;
        
        [Required]
        public JournalEntryLedgerType Type { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
    }

    // DTO for updating journal entries
    public class UpdateJournalEntryRequest
    {
        [Required]
        public string EntryNumber { get; set; } = string.Empty;
        
        [Required]
        public JournalEntryType Type { get; set; }
        
        [Required]
        public DateTime EntryDate { get; set; }
        
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Draft";
        
        public List<UpdateJournalEntryLedgerRequest> LedgerEntries { get; set; } = new List<UpdateJournalEntryLedgerRequest>();
    }

    public class UpdateJournalEntryLedgerRequest
    {
        public string? Id { get; set; } // Null for new entries
        public string LedgerId { get; set; } = string.Empty;
        public JournalEntryLedgerType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
    }

    // DTO for journal entry list display
    public class JournalEntryListDto
    {
        public string Id { get; set; } = string.Empty;
        public string EntryNumber { get; set; } = string.Empty;
        public JournalEntryType Type { get; set; }
        public DateTime EntryDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }
        public bool IsBalanced { get; set; }
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // DTO for detailed journal entry view
    public class JournalEntryByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string EntryNumber { get; set; } = string.Empty;
        public JournalEntryType Type { get; set; }
        public DateTime EntryDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }
        public bool IsBalanced { get; set; }
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<JournalEntryLedgerDto> LedgerEntries { get; set; } = new List<JournalEntryLedgerDto>();
    }

    public class JournalEntryLedgerDto
    {
        public string Id { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string? LedgerName { get; set; }
        public string? LedgerCode { get; set; }
        public JournalEntryLedgerType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
    }

    // Paginated response for journal entry list
    public class PaginatedJournalEntryListResponse
    {
        public List<JournalEntryListDto> Items { get; set; } = new List<JournalEntryListDto>();
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    // Display model for journal entry form
    public class JournalEntryDisplay
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LedgerId { get; set; }
        public string LedgerName { get; set; } = string.Empty;
        public string LedgerCode { get; set; } = string.Empty;
        public JournalEntryLedgerType EntryType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SerialNumber { get; set; }
        
        // Properties for separate debit/credit display
        public decimal DebitAmount => EntryType == JournalEntryLedgerType.Debit ? Amount : 0;
        public decimal CreditAmount => EntryType == JournalEntryLedgerType.Credit ? Amount : 0;
    }
}
