using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class LedgerService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "api/v1/ledger";
        
        public AuthService AuthService => _authService;
        
        public LedgerService(AuthService authService)
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

        public async Task<List<LedgerModel>> GetAllLedgersAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/company/{companyId}/select");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var selectLedgers = JsonSerializer.Deserialize<List<SelectLedgerList>>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            NumberHandling = JsonNumberHandling.AllowReadingFromString,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        if (selectLedgers != null)
                        {
                            // Convert SelectLedgerList to LedgerModel and build parent relationships
                            var ledgers = new List<LedgerModel>();
                            var ledgerDict = new Dictionary<Guid, LedgerModel>();

                            // First pass: create all ledgers
                            foreach (var selectLedger in selectLedgers)
                            {
                                var ledger = selectLedger.ToLedgerModel();
                                ledgers.Add(ledger);
                                ledgerDict[ledger.Id] = ledger;
                            }

                            // Second pass: build parent relationships
                            foreach (var selectLedger in selectLedgers)
                            {
                                if (Guid.TryParse(selectLedger.Id, out var ledgerId) && 
                                    Guid.TryParse(selectLedger.ParentId, out var parentId) &&
                                    ledgerDict.TryGetValue(ledgerId, out var ledger) &&
                                    ledgerDict.TryGetValue(parentId, out var parent))
                                {
                                    ledger.Parent = parent;
                                    parent.Children.Add(ledger);
                                }
                            }

                            Console.WriteLine($"Successfully loaded {ledgers.Count} ledgers with parent relationships");
                            return ledgers;
                        }

                        return new List<LedgerModel>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Response content: {responseContent}");
                        return new List<LedgerModel>();
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
                    return new List<LedgerModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllLedgersAsync: {ex.Message}");
                return new List<LedgerModel>();
            }
        }

        public async Task<LedgerModel?> GetLedgerByIdAsync(Guid id)
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
                        var ledger = JsonSerializer.Deserialize<LedgerModel>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return ledger;
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
                Console.WriteLine($"Exception in GetLedgerByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateLedgerAsync(LedgerModel ledger)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(ledger);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create Ledger Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateLedgerAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateLedgerAsync(Guid id, UpdateLedgerModel ledger)
        {
            try
            {
                SetAuthHeader();
                var json = JsonSerializer.Serialize(ledger);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", ledger);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update Ledger Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateLedgerAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLedgerAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Delete Ledger Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteLedgerAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<SelectLedgerList>> GetSelectLedgerListAsync(Guid companyId)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/company/{companyId}/select");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var selectLedgers = JsonSerializer.Deserialize<List<SelectLedgerList>>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            NumberHandling = JsonNumberHandling.AllowReadingFromString,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        Console.WriteLine($"Successfully loaded {selectLedgers?.Count ?? 0} select ledgers");
                        return selectLedgers ?? new List<SelectLedgerList>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Response content: {responseContent}");
                        return new List<SelectLedgerList>();
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
                    return new List<SelectLedgerList>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetSelectLedgerListAsync: {ex.Message}");
                return new List<SelectLedgerList>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
