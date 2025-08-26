using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class AttributeModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("isRequired")]
        public bool IsRequired { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("sortOrder")]
        public int SortOrder { get; set; }
        
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;
        
        [JsonPropertyName("createdOn")]
        public DateTime? CreatedOn { get; set; }
        
        [JsonPropertyName("modifiedOn")]
        public DateTime? ModifiedOn { get; set; }
        
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;
        
        [JsonPropertyName("modifiedBy")]
        public string ModifiedBy { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeOptions")]
        public List<AttributeOption> AttributeOptions { get; set; } = new List<AttributeOption>();
        
        // Navigation properties
        public Company Company { get; set; } = null!;
        public ICollection<ProductAttributeModel> ProductAttributes { get; set; } = new HashSet<ProductAttributeModel>();
    }

    public class AttributeOption
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
        
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("sortOrder")]
        public int SortOrder { get; set; }
        
        [JsonPropertyName("attributeId")]
        public string AttributeId { get; set; } = string.Empty;
        
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeName")]
        public string AttributeName { get; set; } = string.Empty;
        
        [JsonPropertyName("createdOn")]
        public DateTime? CreatedOn { get; set; }
        
        [JsonPropertyName("modifiedOn")]
        public DateTime? ModifiedOn { get; set; }
        
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;
        
        [JsonPropertyName("modifiedBy")]
        public string ModifiedBy { get; set; } = string.Empty;
        
        // Navigation properties
        public AttributeModel Attribute { get; set; } = null!;
    }

    public class AttributeListResponse
    {
        public List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();
        public List<AttributeModel> Data { get; set; } = new List<AttributeModel>();
        public List<AttributeModel> Items { get; set; } = new List<AttributeModel>();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public int TotalCount { get; set; }

        // Helper method to get attributes from any of the possible arrays
        public List<AttributeModel> GetAttributes()
        {
            if (Attributes.Any()) return Attributes;
            if (Data.Any()) return Data;
            if (Items.Any()) return Items;
            return new List<AttributeModel>();
        }
    }
}
