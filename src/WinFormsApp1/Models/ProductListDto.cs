using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class ProductListDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        
        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
        
        [JsonPropertyName("sellingPrice")]
        public decimal SellingPrice { get; set; }
        
        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        // Display property for UI
        public string DisplayName => !string.IsNullOrEmpty(ProductCode) ? $"{ProductCode} - {Name}" : Name;
        
        // Additional display property with category
        public string DisplayNameWithCategory => !string.IsNullOrEmpty(ProductCode) ? $"{ProductCode} - {Name} ({Category})" : $"{Name} ({Category})";
    }


}
