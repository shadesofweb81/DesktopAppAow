using System.Text;
using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class ProductService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public ProductService(AuthService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_authService.ErpBaseUrl);
            
            // Add headers
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "WinFormsApp1/1.0");
        }

        private void SetAuthHeader()
        {
            if (!string.IsNullOrEmpty(_authService.JwtToken))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authService.JwtToken}");
            }
        }

        public async Task<List<ProductModel>> GetProductsByCompanyAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/v1/product/company/{companyId}";
                
                Console.WriteLine($"Fetching products for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");
                Console.WriteLine($"Response Content Preview: {responseContent?.Substring(0, Math.Min(500, responseContent.Length)) ?? "null"}");
                if (responseContent?.Length > 500)
                {
                    Console.WriteLine($"... (truncated, full content available in logs)");
                }

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        Console.WriteLine("Attempting to parse response...");
                        
                      
                        // If that doesn't work, try to parse as direct array
                        Console.WriteLine("ProductListResponse parsing failed, trying direct array...");
                        var productsArray = JsonSerializer.Deserialize<List<ProductModel>>(responseContent);

                        if (productsArray != null)
                        {
                            Console.WriteLine($"Successfully parsed {productsArray.Count} products as direct array");
                            foreach (var product in productsArray)
                            {
                                Console.WriteLine($"  - Product: {product.Name} (ID: {product.Id})");
                            }
                            return productsArray;
                        }

                        Console.WriteLine("Could not parse products from any expected format");
                        return new List<ProductModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<ProductModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<ProductModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get products by company exception: {ex.Message}");
                return new List<ProductModel>();
            }
        }

        public async Task<PaginatedProductListResponse?> GetProductListAsync(Guid companyId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/v1/product/company/{companyId}?pageNumber={pageNumber}&pageSize={pageSize}";
                
                Console.WriteLine($"Fetching paginated product list for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");
                Console.WriteLine($"Response Content Preview: {responseContent?.Substring(0, Math.Min(500, responseContent.Length)) ?? "null"}");
                if (responseContent?.Length > 500)
                {
                    Console.WriteLine($"... (truncated, full content available in logs)");
                }

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var paginatedResponse = JsonSerializer.Deserialize<PaginatedProductListResponse>(responseContent);

                        if (paginatedResponse != null)
                        {
                            Console.WriteLine($"Successfully parsed paginated product list: {paginatedResponse.Items.Count} products, Page {paginatedResponse.CurrentPage}/{paginatedResponse.TotalPages}, Total: {paginatedResponse.TotalItems}");
                            return paginatedResponse;
                        }

                        Console.WriteLine("Could not parse paginated product list response");
                        return null;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get product list exception: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductModel?> GetProductByIdAsync(string productId)
        {
            try
            {
                SetAuthHeader();
                
                var response = await _httpClient.GetAsync($"api/v1/product/{productId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get product {productId} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Configure JSON options to handle the API response format
                        var jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };

                  
                        var product = JsonSerializer.Deserialize<ProductModel>(responseContent, jsonOptions);
                        
                        if (product != null)
                        {
                            Console.WriteLine($"Successfully parsed product: {product.Name} (ID: {product.Id})");
                            return product;
                        }

                        Console.WriteLine("Could not parse product from any expected format");
                        return null;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get product exception: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductModel?> CreateProductAsync(ProductCreateRequest request)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Creating product with data: {json}");

                var response = await _httpClient.PostAsync("api/v1/product", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create product - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as ProductResponse first
                        var productResponse = JsonSerializer.Deserialize<ProductResponse>(responseContent);

                        if (productResponse != null && productResponse.Success)
                        {
                            return productResponse.GetProduct();
                        }

                        // If that doesn't work, try to parse as direct ProductModel object
                        var product = JsonSerializer.Deserialize<ProductModel>(responseContent);

                        return product;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create product exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateProductAsync(string productId, ProductModel product)
        {
            try
            {
                SetAuthHeader();

                var updateRequest = new UpdateProductRequest
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.ProductCode,
                    Description = product.Description,
                    Unit = product.Unit,
                    SKU = product.SKU,
                    PurchasePrice = product.PurchasePrice,
                    SellingPrice = product.SellingPrice,
                    StockQuantity = product.StockQuantity,
                    ReorderLevel = product.ReorderLevel,
                    CompanyId = product.CompanyId,
                    ParentId = product.ParentId,
                    AttributeIds = product.AttributeIds ?? new List<string>()
                };

                var json = JsonSerializer.Serialize(updateRequest, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Updating product {productId} with data: {json}");

                var response = await _httpClient.PutAsync($"api/v1/product/{productId}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update product {productId} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update product exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            try
            {
                SetAuthHeader();

                var response = await _httpClient.DeleteAsync($"api/v1/product/{productId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Delete product {productId} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete product exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductListDto>> GetProductListForTransactionAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/v1/product/company/{companyId}/transactions";
                
                Console.WriteLine($"Fetching product list for transaction from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as direct array first
                        var productsArray = JsonSerializer.Deserialize<List<ProductListDto>>(responseContent);

                        if (productsArray != null)
                        {
                            Console.WriteLine($"Successfully parsed {productsArray.Count} products as direct array");
                            return productsArray;
                        }

                        // If that doesn't work, try to parse as response wrapper
                        Console.WriteLine("Direct array parsing failed, trying response wrapper...");
                        var responseWrapper = JsonSerializer.Deserialize<dynamic>(responseContent);
                        
                        if (responseWrapper != null)
                        {
                            // Try to extract products from various possible properties
                            if (responseWrapper.Products != null)
                            {
                                var products = JsonSerializer.Deserialize<List<ProductListDto>>(responseWrapper.Products.ToString());
                                if (products != null)
                                {
                                    Console.WriteLine($"Successfully parsed {products.Count} products from Products property");
                                    return products;
                                }
                            }
                            
                            if (responseWrapper.Data != null)
                            {
                                var products = JsonSerializer.Deserialize<List<ProductListDto>>(responseWrapper.Data.ToString());
                                if (products != null)
                                {
                                    Console.WriteLine($"Successfully parsed {products.Count} products from Data property");
                                    return products;
                                }
                            }
                            
                            if (responseWrapper.Items != null)
                            {
                                var products = JsonSerializer.Deserialize<List<ProductListDto>>(responseWrapper.Items.ToString());
                                if (products != null)
                                {
                                    Console.WriteLine($"Successfully parsed {products.Count} products from Items property");
                                    return products;
                                }
                            }
                        }

                        Console.WriteLine("Could not parse product list from any expected format");
                        return new List<ProductListDto>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<ProductListDto>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<ProductListDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get product list for transaction exception: {ex.Message}");
                return new List<ProductListDto>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        // Test method to verify JSON parsing for GetProductByIdAsync response
        public static void TestGetProductByIdJsonParsing()
        {
            var testJson = @"{
                ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                ""name"": ""Test Product"",
                ""productCode"": ""TEST001"",
                ""category"": ""Electronics"",
                ""description"": ""A test product"",
                ""unit"": ""PCS"",
                ""sku"": ""SKU001"",
                ""purchasePrice"": 10.50,
                ""sellingPrice"": 15.00,
                ""stockQuantity"": 100,
                ""reorderLevel"": 10,
                ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                ""parentId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                ""attributeIds"": [
                    ""3fa85f64-5717-4562-b3fc-2c963f66afa6""
                ],
                ""productVariants"": [
                    {
                        ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                        ""productId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                        ""variantCode"": ""VAR001"",
                        ""name"": ""Test Variant"",
                        ""description"": ""A test variant"",
                        ""purchasePrice"": 12.00,
                        ""sellingPrice"": 18.00,
                        ""stockQuantity"": 50,
                        ""sku"": ""SKU001-VAR"",
                        ""isActive"": true,
                        ""productVariantAttributes"": [
                            {
                                ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                ""productVariantId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                ""attributeId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                ""attributeName"": ""Color"",
                                ""attributeOptionId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                                ""attributeOptionName"": ""Red"",
                                ""isActive"": true
                            }
                        ]
                    }
                ]
            }";

            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var product = JsonSerializer.Deserialize<ProductModel>(testJson, jsonOptions);
                if (product != null)
                {
                    Console.WriteLine($"Test GetProductById parsing successful!");
                    Console.WriteLine($"  - Product: {product.Name} (ID: {product.Id})");
                    Console.WriteLine($"  - Product Code: {product.ProductCode}");
                    Console.WriteLine($"  - Category: {product.Category}");
                    Console.WriteLine($"  - Company ID: {product.CompanyId}");
                    Console.WriteLine($"  - Parent ID: {product.ParentId}");
                    Console.WriteLine($"  - Attribute IDs Count: {product.AttributeIds.Count}");
                    Console.WriteLine($"  - Product Variants Count: {product.ProductVariants.Count}");
                    
                    if (product.ProductVariants.Any())
                    {
                        var variant = product.ProductVariants.First();
                        Console.WriteLine($"  - First Variant: {variant.Name} (Code: {variant.VariantCode})");
                        Console.WriteLine($"  - Variant Attributes Count: {variant.ProductVariantAttributes.Count}");
                    }
                }
                else
                {
                    Console.WriteLine("Test GetProductById parsing failed - no product found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test GetProductById parsing error: {ex.Message}");
            }
        }
    }
}

