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

        public async Task<ProductModel?> GetProductByIdAsync(Guid productId)
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
                        // Try to parse as ProductResponse first
                        var productResponse = JsonSerializer.Deserialize<ProductResponse>(responseContent);

                        if (productResponse != null)
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

        public async Task<bool> UpdateProductAsync(Guid productId, ProductCreateRequest request)
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

                Console.WriteLine($"Updating product {productId} with data: {json}");

                var response = await _httpClient.PutAsync($"api/v1/product/{productId}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update product - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update product exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
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

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        // Test method to verify JSON parsing
        public static void TestJsonParsing()
        {
            var testJson = @"[
                {
                    ""id"": ""0070e4be-0e34-434c-a64f-249188028b4d"",
                    ""name"": ""3 pound carton box"",
                    ""productCode"": """",
                    ""category"": """",
                    ""description"": """",
                    ""unit"": """",
                    ""sku"": """",
                    ""purchasePrice"": 0.0,
                    ""sellingPrice"": 0.0,
                    ""stockQuantity"": 0,
                    ""reorderLevel"": null,
                    ""companyId"": ""e6a993e0-6229-4ced-94e1-bfa1bfb92c20"",
                    ""parentId"": ""cd2c1069-4d46-4daa-b5ff-1f9af8b45fa1"",
                    ""attributeIds"": [],
                    ""productVariants"": []
                }
            ]";

            try
            {
                var products = JsonSerializer.Deserialize<List<ProductModel>>(testJson);
                if (products != null && products.Any())
                {
                    Console.WriteLine($"Test parsing successful! Found {products.Count} products");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"  - {product.Name} (ID: {product.Id})");
                    }
                }
                else
                {
                    Console.WriteLine("Test parsing failed - no products found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test parsing error: {ex.Message}");
            }
        }
    }
}

