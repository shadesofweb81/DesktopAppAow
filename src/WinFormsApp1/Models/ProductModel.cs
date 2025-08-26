

using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class ProductModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
        
        [JsonPropertyName("sku")]
        public string SKU { get; set; } = string.Empty;
        
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
        
        [JsonPropertyName("sellingPrice")]
        public decimal SellingPrice { get; set; }
        
        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }
        
        [JsonPropertyName("reorderLevel")]
        public int? ReorderLevel { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; } = string.Empty;
        
        // Hierarchical structure
        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }
        
        // Foreign keys
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;
        
        // Arrays from API
        [JsonPropertyName("attributeIds")]
        public List<string> AttributeIds { get; set; } = new List<string>();
        
        [JsonPropertyName("productVariants")]
        public List<ProductVariantModel> ProductVariants { get; set; } = new List<ProductVariantModel>();
        
        // Navigation properties
        public ProductModel? Parent { get; set; }
        public ICollection<ProductModel> Children { get; set; } = new HashSet<ProductModel>();
        public Company Company { get; set; } = null!;
        public ICollection<ProductAttributeModel> ProductAttributes { get; set; } = new HashSet<ProductAttributeModel>();
        public ICollection<ProductVariant> ProductVariantsCollection { get; set; } = new HashSet<ProductVariant>();
        
        // Display property for UI
        public string DisplayName => !string.IsNullOrEmpty(ProductCode) ? $"{ProductCode} - {Name}" : Name;
    }

    public class ProductCreateRequest
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;
        
        [JsonPropertyName("sku")]
        public string SKU { get; set; } = string.Empty;
        
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
        
        [JsonPropertyName("sellingPrice")]
        public decimal SellingPrice { get; set; }
        
        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }
        
        [JsonPropertyName("reorderLevel")]
        public int? ReorderLevel { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; } = string.Empty;
        
        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }
        
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeIds")]
        public List<string> AttributeIds { get; set; } = new List<string>();
    }

    public class ProductListResponse
    {
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        public List<ProductModel> Data { get; set; } = new List<ProductModel>();
        public List<ProductModel> Items { get; set; } = new List<ProductModel>();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public int TotalCount { get; set; }

        // Helper method to get products from any of the possible arrays
        public List<ProductModel> GetProducts()
        {
            if (Products.Any()) return Products;
            if (Data.Any()) return Data;
            if (Items.Any()) return Items;
            return new List<ProductModel>();
        }
    }

    public class ProductResponse
    {
        public ProductModel? Product { get; set; }
        public ProductModel? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        // Helper method to get product from response
        public ProductModel? GetProduct()
        {
            return Product ?? Data;
        }
    }

    public class PaginatedProductListResponse
    {
        public List<ProductModel> Items { get; set; } = new List<ProductModel>();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        // Helper property to maintain compatibility with existing code
        public List<ProductModel> Data => Items;
        public int TotalCount => TotalItems;
        public int PageNumber => CurrentPage;
    }


    public class ProductAttributeModel
    {
        // Foreign keys
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }
        public Guid CompanyId { get; set; }

        // Additional properties
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ProductModel Product { get; set; } = null!;
        public Attribute Attribute { get; set; } = null!;
        public Company Company { get; set; } = null!;
    }

    public class ProductVariantModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("productId")]
        public string ProductId { get; set; } = string.Empty;
        
        [JsonPropertyName("variantCode")]
        public string VariantCode { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }
        
        [JsonPropertyName("sellingPrice")]
        public decimal SellingPrice { get; set; }
        
        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }
        
        [JsonPropertyName("sku")]
        public string SKU { get; set; } = string.Empty;
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
        
        [JsonPropertyName("productVariantAttributes")]
        public List<ProductVariantAttributeModel> ProductVariantAttributes { get; set; } = new List<ProductVariantAttributeModel>();
    }

    public class ProductVariantAttributeModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("productVariantId")]
        public string ProductVariantId { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeId")]
        public string AttributeId { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeName")]
        public string AttributeName { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeOptionId")]
        public string AttributeOptionId { get; set; } = string.Empty;
        
        [JsonPropertyName("attributeOptionName")]
        public string AttributeOptionName { get; set; } = string.Empty;
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class ProductVariant
    {
        public string VariantCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int MinStockLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public bool IsActive { get; set; } = true;
        public string SKU { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        // Foreign keys
        public Guid ProductId { get; set; }
        public Guid CompanyId { get; set; }

        // Navigation properties
        public ProductModel Product { get; set; } = null!;
        public Company Company { get; set; } = null!;

    }
} 