using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class TaxListDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("category")]
        public TaxCategory Category { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("isComposite")]
        public bool IsComposite { get; set; }
        
        [JsonPropertyName("defaultRate")]
        public decimal DefaultRate { get; set; }
        
        [JsonPropertyName("hsnCode")]
        public string? HSNCode { get; set; }
        
        [JsonPropertyName("components")]
        public List<TaxComponentDto> Components { get; set; } = new List<TaxComponentDto>();
        
        // Display property for UI
        public string DisplayName => !string.IsNullOrEmpty(HSNCode) ? $"{Name} ({HSNCode})" : Name;
        
        // Display property with rate
        public string DisplayNameWithRate => !string.IsNullOrEmpty(HSNCode) ? $"{Name} ({DefaultRate}%) - {HSNCode}" : $"{Name} ({DefaultRate}%)";
    }

    public class TaxComponentDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("type")]
        public TaxType Type { get; set; }
        
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
        
        [JsonPropertyName("ledgerCode")]
        public string LedgerCode { get; set; } = string.Empty;
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        // Display property for UI
        public string DisplayName => $"{Name} ({Rate}%)";
        
        // Display property with type
        public string DisplayNameWithType => $"{Name} ({Type}) - {Rate}%";
    }

    public class TaxListResponse
    {
        public List<TaxListDto> Taxes { get; set; } = new List<TaxListDto>();
        public List<TaxListDto> Data { get; set; } = new List<TaxListDto>();
        public List<TaxListDto> Items { get; set; } = new List<TaxListDto>();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public int TotalCount { get; set; }

        // Helper method to get taxes from any of the possible arrays
        public List<TaxListDto> GetTaxes()
        {
            if (Taxes.Any()) return Taxes;
            if (Data.Any()) return Data;
            if (Items.Any()) return Items;
            return new List<TaxListDto>();
        }
    }
}
