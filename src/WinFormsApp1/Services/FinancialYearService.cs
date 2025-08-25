using System.Text;
using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class FinancialYearService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public FinancialYearService(AuthService authService)
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

        public async Task<List<FinancialYearModel>> GetFinancialYearsByCompanyAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/financialyears/company/{companyId}";
                
                Console.WriteLine($"Fetching financial years for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        Console.WriteLine("Attempting to parse response...");
                        var financialYears = JsonSerializer.Deserialize<List<FinancialYearModel>>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (financialYears != null)
                        {
                            Console.WriteLine($"Successfully parsed {financialYears.Count} financial years");
                            return financialYears;
                        }

                        Console.WriteLine("Could not parse financial years from response");
                        return new List<FinancialYearModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<FinancialYearModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<FinancialYearModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get financial years by company exception: {ex.Message}");
                return new List<FinancialYearModel>();
            }
        }

        public async Task<FinancialYearModel?> GetFinancialYearByIdAsync(Guid financialYearId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/financialyears/{financialYearId}";
                
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var financialYear = JsonSerializer.Deserialize<FinancialYearModel>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        
                        return financialYear;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to get financial year. Status: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting financial year: {ex.Message}");
                return null;
            }
        }

        public async Task<FinancialYearModel?> CreateFinancialYearAsync(FinancialYearModel financialYear)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(financialYear);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("api/financialyears", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var createdFinancialYear = JsonSerializer.Deserialize<FinancialYearModel>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return createdFinancialYear;
                }
                else
                {
                    Console.WriteLine($"Failed to create financial year. Status: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating financial year: {ex.Message}");
                return null;
            }
        }

        public async Task<FinancialYearModel?> UpdateFinancialYearAsync(Guid financialYearId, FinancialYearModel financialYear)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(financialYear);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"api/financialyears/{financialYearId}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var updatedFinancialYear = JsonSerializer.Deserialize<FinancialYearModel>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return updatedFinancialYear;
                }
                else
                {
                    Console.WriteLine($"Failed to update financial year. Status: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating financial year: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteFinancialYearAsync(Guid financialYearId)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"api/financialyears/{financialYearId}");
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to delete financial year. Status: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting financial year: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SetActiveFinancialYearAsync(Guid companyId, Guid financialYearId)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.PutAsync($"api/financialyears/{companyId}/setactive/{financialYearId}", null);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to set active financial year. Status: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting active financial year: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
