using System.ComponentModel.DataAnnotations;

namespace WinFormsApp1.Models
{
    public class TaxByIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsComposite { get; set; }
        public decimal DefaultRate { get; set; }
        public string? HSNCode { get; set; }
        public string? SectionCode { get; set; }
        public bool IsReverseChargeApplicable { get; set; }
        public bool IsDeductibleAtSource { get; set; }
        public bool IsCollectedAtSource { get; set; }
        public string? ReturnFormNumber { get; set; }
        public string CompanyId { get; set; } = string.Empty;
        public List<TaxComponentDetailDto> Components { get; set; } = new List<TaxComponentDetailDto>();
        public List<TaxRateDto> Rates { get; set; } = new List<TaxRateDto>();
    }

    public class TaxComponentDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public string LedgerCode { get; set; } = string.Empty;
        public bool IsCreditAllowed { get; set; }
        public string? ReturnFormNumber { get; set; }
        public string TaxId { get; set; } = string.Empty;
        public string LedgerId { get; set; } = string.Empty;
        public string? ReceivableLedgerId { get; set; }
        public string? PayableLedgerId { get; set; }
        public bool IsActive { get; set; }
    }

    public class TaxRateDto
    {
        public string Id { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
