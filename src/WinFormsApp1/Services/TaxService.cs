using System.Drawing.Printing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class TaxService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "api/v1/tax";
        
        public AuthService AuthService => _authService;
        
        public TaxService(AuthService authService)
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

        public async Task<List<TaxModel>> GetAllTaxesAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"api/v1/tax?companyId={companyId}&pageNumber={1}&pageSize={100}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var paginatedResponse = JsonSerializer.Deserialize<PaginatedTaxListResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return paginatedResponse?.Items ?? new List<TaxModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return new List<TaxModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
                    return new List<TaxModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllTaxesAsync: {ex.Message}");
                return new List<TaxModel>();
            }
        }

        public async Task<TaxModel?> GetTaxByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var tax = JsonSerializer.Deserialize<TaxModel>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return tax;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetTaxByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateTaxAsync(TaxModel tax)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(tax);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create Tax Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateTaxAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTaxAsync(Guid id, TaxModel tax)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(tax);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update Tax Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateTaxAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTaxAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Delete Tax Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteTaxAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<TaxListDto>> GetTaxListForTransactionAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var url = $"api/v1/tax/GetTaxesForSelect/{companyId}";
                
                Console.WriteLine($"Fetching tax list for transaction from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Try to parse as direct array first
                        var taxesArray = JsonSerializer.Deserialize<List<TaxListDto>>(responseContent);

                        if (taxesArray != null)
                        {
                            Console.WriteLine($"Successfully parsed {taxesArray.Count} taxes as direct array");
                            return taxesArray;
                        }

                        // If that doesn't work, try to parse as response wrapper
                        Console.WriteLine("Direct array parsing failed, trying response wrapper...");
                        var responseWrapper = JsonSerializer.Deserialize<dynamic>(responseContent);
                        
                        if (responseWrapper != null)
                        {
                            // Try to extract taxes from various possible properties
                            if (responseWrapper.Taxes != null)
                            {
                                var taxes = JsonSerializer.Deserialize<List<TaxListDto>>(responseWrapper.Taxes.ToString());
                                if (taxes != null)
                                {
                                    Console.WriteLine($"Successfully parsed {taxes.Count} taxes from Taxes property");
                                    return taxes;
                                }
                            }
                            
                            if (responseWrapper.Data != null)
                            {
                                var taxes = JsonSerializer.Deserialize<List<TaxListDto>>(responseWrapper.Data.ToString());
                                if (taxes != null)
                                {
                                    Console.WriteLine($"Successfully parsed {taxes.Count} taxes from Data property");
                                    return taxes;
                                }
                            }
                            
                            if (responseWrapper.Items != null)
                            {
                                var taxes = JsonSerializer.Deserialize<List<TaxListDto>>(responseWrapper.Items.ToString());
                                if (taxes != null)
                                {
                                    Console.WriteLine($"Successfully parsed {taxes.Count} taxes from Items property");
                                    return taxes;
                                }
                            }
                        }

                        Console.WriteLine("Could not parse tax list from any expected format");
                        return new List<TaxListDto>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Raw response: {responseContent}");
                        return new List<TaxListDto>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<TaxListDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get tax list for transaction exception: {ex.Message}");
                return new List<TaxListDto>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
