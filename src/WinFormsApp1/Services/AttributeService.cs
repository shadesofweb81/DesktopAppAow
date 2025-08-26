using System.Text;
using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class AttributeService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public AttributeService(AuthService authService)
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

        public async Task<List<AttributeModel>> GetAttributesByCompanyAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/v1/attribute/all?companyId={companyId}";
                
                Console.WriteLine($"Fetching attributes for company {companyId} from: {url}");

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
                        // Configure JSON options to handle the API response format
                        var jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };

                     
                        var attributesArray = JsonSerializer.Deserialize<List<AttributeModel>>(responseContent, jsonOptions);

                        if (attributesArray != null)
                        {
                            Console.WriteLine($"Successfully parsed {attributesArray.Count} attributes as direct array");
                            foreach (var attribute in attributesArray)
                            {
                                Console.WriteLine($"  - Attribute: {attribute.Name} (Options: {attribute.AttributeOptions.Count})");
                            }
                            return attributesArray;
                        }

                        Console.WriteLine("Could not parse attributes from any expected format");
                        return new List<AttributeModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<AttributeModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<AttributeModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get attributes by company exception: {ex.Message}");
                return new List<AttributeModel>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        // Test method to verify JSON parsing for GetAttributesByCompanyAsync response with new fields
        public static void TestGetAttributesByCompanyJsonParsing()
        {
            var testJson = @"[
                {
                    ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                    ""name"": ""Color"",
                    ""description"": ""Product color attribute"",
                    ""isRequired"": true,
                    ""isActive"": true,
                    ""sortOrder"": 1,
                    ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                    ""createdOn"": ""2024-01-15T10:30:00Z"",
                    ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                    ""createdBy"": ""admin@example.com"",
                    ""modifiedBy"": ""admin@example.com"",
                    ""attributeOptions"": [
                        {
                            ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""value"": ""red"",
                            ""displayName"": ""Red"",
                            ""description"": ""Red color option"",
                            ""isActive"": true,
                            ""sortOrder"": 1,
                            ""attributeId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""attributeName"": ""Color"",
                            ""createdOn"": ""2024-01-15T10:30:00Z"",
                            ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                            ""createdBy"": ""admin@example.com"",
                            ""modifiedBy"": ""admin@example.com""
                        },
                        {
                            ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa7"",
                            ""value"": ""blue"",
                            ""displayName"": ""Blue"",
                            ""description"": ""Blue color option"",
                            ""isActive"": true,
                            ""sortOrder"": 2,
                            ""attributeId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""attributeName"": ""Color"",
                            ""createdOn"": ""2024-01-15T10:30:00Z"",
                            ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                            ""createdBy"": ""admin@example.com"",
                            ""modifiedBy"": ""admin@example.com""
                        }
                    ]
                },
                {
                    ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa8"",
                    ""name"": ""Size"",
                    ""description"": ""Product size attribute"",
                    ""isRequired"": false,
                    ""isActive"": true,
                    ""sortOrder"": 2,
                    ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                    ""createdOn"": ""2024-01-15T10:30:00Z"",
                    ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                    ""createdBy"": ""admin@example.com"",
                    ""modifiedBy"": ""admin@example.com"",
                    ""attributeOptions"": [
                        {
                            ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afa9"",
                            ""value"": ""small"",
                            ""displayName"": ""Small"",
                            ""description"": ""Small size option"",
                            ""isActive"": true,
                            ""sortOrder"": 1,
                            ""attributeId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa8"",
                            ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""attributeName"": ""Size"",
                            ""createdOn"": ""2024-01-15T10:30:00Z"",
                            ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                            ""createdBy"": ""admin@example.com"",
                            ""modifiedBy"": ""admin@example.com""
                        },
                        {
                            ""id"": ""3fa85f64-5717-4562-b3fc-2c963f66afaa"",
                            ""value"": ""large"",
                            ""displayName"": ""Large"",
                            ""description"": ""Large size option"",
                            ""isActive"": true,
                            ""sortOrder"": 2,
                            ""attributeId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa8"",
                            ""companyId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
                            ""attributeName"": ""Size"",
                            ""createdOn"": ""2024-01-15T10:30:00Z"",
                            ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                            ""createdBy"": ""admin@example.com"",
                            ""modifiedBy"": ""admin@example.com""
                        }
                    ]
                }
            ]";

            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var attributes = JsonSerializer.Deserialize<List<AttributeModel>>(testJson, jsonOptions);
                if (attributes != null)
                {
                    Console.WriteLine($"Test GetAttributesByCompany parsing successful!");
                    Console.WriteLine($"  - Found {attributes.Count} attributes");
                    
                    foreach (var attribute in attributes)
                    {
                        Console.WriteLine($"  - Attribute: {attribute.Name} (ID: {attribute.Id})");
                        Console.WriteLine($"    Description: {attribute.Description}");
                        Console.WriteLine($"    Required: {attribute.IsRequired}");
                        Console.WriteLine($"    Active: {attribute.IsActive}");                   
                        Console.WriteLine($"    Options Count: {attribute.AttributeOptions.Count}");
                        
                        foreach (var option in attribute.AttributeOptions)
                        {
                            Console.WriteLine($"      - Option: {option.DisplayName} (Value: {option.Value})");
                        
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Test GetAttributesByCompany parsing failed - no attributes found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test GetAttributesByCompany parsing error: {ex.Message}");
            }
        }

        // Test method specifically for the new API response format with createdOn, modifiedOn, etc.
        public static void TestNewAttributeJsonFormat()
        {
            var testJson = @"[
                {
                    ""id"": ""attr-001"",
                    ""name"": ""Test Attribute"",
                    ""description"": ""Test Description"",
                    ""isRequired"": true,
                    ""isActive"": true,
                    ""sortOrder"": 1,
                    ""companyId"": ""company-001"",
                    ""createdOn"": ""2024-01-15T10:30:00Z"",
                    ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                    ""createdBy"": ""admin@example.com"",
                    ""modifiedBy"": ""admin@example.com"",
                    ""attributeOptions"": [
                        {
                            ""id"": ""opt-001"",
                            ""value"": ""test-value"",
                            ""displayName"": ""Test Option"",
                            ""description"": ""Test Option Description"",
                            ""isActive"": true,
                            ""sortOrder"": 1,
                            ""attributeId"": ""attr-001"",
                            ""companyId"": ""company-001"",
                            ""attributeName"": ""Test Attribute"",
                            ""createdOn"": ""2024-01-15T10:30:00Z"",
                            ""modifiedOn"": ""2024-01-20T14:45:00Z"",
                            ""createdBy"": ""admin@example.com"",
                            ""modifiedBy"": ""admin@example.com""
                        }
                    ]
                }
            ]";

            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var attributes = JsonSerializer.Deserialize<List<AttributeModel>>(testJson, jsonOptions);
                if (attributes != null && attributes.Any())
                {
                    Console.WriteLine($"New format test successful!");
                    var attribute = attributes.First();
                    Console.WriteLine($"  - Attribute: {attribute.Name}");
                
                    
                    if (attribute.AttributeOptions.Any())
                    {
                        var option = attribute.AttributeOptions.First();
                        Console.WriteLine($"  - Option: {option.DisplayName}");
                     
                    }
                }
                else
                {
                    Console.WriteLine("New format test failed - no attributes found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"New format test error: {ex.Message}");
            }
        }
    }
}
