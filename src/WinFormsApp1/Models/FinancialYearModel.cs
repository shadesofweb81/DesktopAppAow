

using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class FinancialYearModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        public string YearLabel { get; set; } = string.Empty; // e.g. "2025-2026"

    
        public DateTime StartDate { get; set; }

     
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

     
    }
}
