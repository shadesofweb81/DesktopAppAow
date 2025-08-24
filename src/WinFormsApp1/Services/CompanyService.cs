using System.Text;
using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class CompanyService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public CompanyService(AuthService authService)
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

        public async Task<List<SelectCompanyModel>> GetAllCompaniesAsync()
        {
            try
            {
                SetAuthHeader();
                var getCompaniesUrl = new Uri(_httpClient.BaseAddress + "api/v1/company/all");
                
                Console.WriteLine($"Fetching companies from: {getCompaniesUrl}");

                var response = await _httpClient.GetAsync("api/v1/company/all");
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
                        // First, try to parse as direct array using SelectCompanyModel (most common case)
                        var companiesArray = JsonSerializer.Deserialize<List<SelectCompanyModel>>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (companiesArray != null && companiesArray.Any())
                        {
                            Console.WriteLine($"Successfully parsed {companiesArray.Count} companies as SelectCompanyModel array");
                            return companiesArray;
                        }

                        // If that doesn't work, try to parse as CompanyListResponse and convert to SelectCompanyModel
                        var companyListResponse = JsonSerializer.Deserialize<CompanyListResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (companyListResponse != null)
                        {
                            var companies = companyListResponse.GetCompanies();
                            if (companies.Any())
                            {
                                // Convert Company objects to SelectCompanyModel
                                var selectCompanies = companies.Select(c => new SelectCompanyModel
                                {
                                    Id = c.Id.ToString(),
                                    CompanyId = c.Id.ToString(),
                                    CompanyName = c.Name,
                                    CompanyCode = c.Code,
                                    IsActive = c.IsActive
                                }).ToList();
                                
                                Console.WriteLine($"Successfully parsed {selectCompanies.Count} companies from CompanyListResponse and converted to SelectCompanyModel");
                                return selectCompanies;
                            }
                        }

                     
                        Console.WriteLine("Could not parse companies from any expected format");
                        return new List<SelectCompanyModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<SelectCompanyModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<SelectCompanyModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get companies exception: {ex.Message}");
                return new List<SelectCompanyModel>();
            }
        }

        public async Task<Company?> GetCompanyByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                
                var response = await _httpClient.GetAsync($"api/v1/company/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get company {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as CompanyResponse first
                        var companyResponse = JsonSerializer.Deserialize<CompanyResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (companyResponse != null)
                        {
                            return companyResponse.GetCompany();
                        }

                        // If that doesn't work, try to parse as direct Company object
                        var company = JsonSerializer.Deserialize<Company>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return company;
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
                Console.WriteLine($"Get company exception: {ex.Message}");
                return null;
            }
        }

        public async Task<EditCompanyModel?> GetEditCompanyByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                
                var response = await _httpClient.GetAsync($"api/v1/company/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get edit company {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as EditCompanyModel directly
                        var editCompany = JsonSerializer.Deserialize<EditCompanyModel>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (editCompany != null)
                        {
                            Console.WriteLine($"Successfully parsed EditCompanyModel: ID={editCompany.Id}, Name={editCompany.Name}");
                            return editCompany;
                        }

                        Console.WriteLine("Failed to parse response as EditCompanyModel");
                        return null;
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
                Console.WriteLine($"Get edit company exception: {ex.Message}");
                return null;
            }
        }

        public async Task<Company?> CreateCompanyAsync(CompanyCreateRequest request)
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

                Console.WriteLine($"Creating company with data: {json}");

                var response = await _httpClient.PostAsync("api/v1/company", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create company - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as CompanyResponse first
                        var companyResponse = JsonSerializer.Deserialize<CompanyResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (companyResponse != null && companyResponse.Success)
                        {
                            return companyResponse.GetCompany();
                        }

                        // If that doesn't work, try to parse as direct Company object
                        var company = JsonSerializer.Deserialize<Company>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return company;
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
                Console.WriteLine($"Create company exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateCompanyAsync(Guid id, CompanyCreateRequest request)
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

                Console.WriteLine($"Updating company {id} with data: {json}");

                var response = await _httpClient.PutAsync($"api/v1/company/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update company - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update company exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                var response = await _httpClient.DeleteAsync($"api/v1/company/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Delete company {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete company exception: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetRawApiResponseAsync()
        {
            try
            {
                SetAuthHeader();
                
                var response = await _httpClient.GetAsync("api/v1/company/all");
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"Raw API Response - Status: {response.StatusCode}");
                Console.WriteLine($"Raw API Response - Content: {responseContent}");
                
                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting raw API response: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
