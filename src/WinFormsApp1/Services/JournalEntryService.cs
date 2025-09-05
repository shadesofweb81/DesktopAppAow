using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class JournalEntryService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "api/v1/journal-entry";

        public JournalEntryService(AuthService authService)
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

        /// <summary>
        /// Get all journal entries with pagination
        /// </summary>
        public async Task<PaginatedJournalEntryListResponse?> GetJournalEntriesAsync(int page = 1, int pageSize = 50, string? searchTerm = null, JournalEntryType? type = null, string? status = null)
        {
            try
            {
                SetAuthHeader();
                
                var queryParams = new List<string>
                {
                    $"page={page}",
                    $"pageSize={pageSize}"
                };

                if (!string.IsNullOrEmpty(searchTerm))
                    queryParams.Add($"search={Uri.EscapeDataString(searchTerm)}");
                
                if (type.HasValue)
                    queryParams.Add($"type={type.Value}");
                
                if (!string.IsNullOrEmpty(status))
                    queryParams.Add($"status={Uri.EscapeDataString(status)}");

                var queryString = string.Join("&", queryParams);
                var url = $"{_baseUrl}?{queryString}";

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get journal entries - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = JsonSerializer.Deserialize<PaginatedJournalEntryListResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return result;
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
                Console.WriteLine($"Get journal entries exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get journal entry by ID
        /// </summary>
        public async Task<JournalEntryByIdDto?> GetJournalEntryByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get journal entry {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var journalEntry = JsonSerializer.Deserialize<JournalEntryByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return journalEntry;
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
                Console.WriteLine($"Get journal entry exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create a new journal entry
        /// </summary>
        public async Task<JournalEntryByIdDto?> CreateJournalEntryAsync(CreateJournalEntryRequest request)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Creating journal entry via: {_baseUrl} with data: {json}");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create journal entry - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var createdEntry = JsonSerializer.Deserialize<JournalEntryByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return createdEntry;
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
                Console.WriteLine($"Create journal entry exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update an existing journal entry
        /// </summary>
        public async Task<JournalEntryByIdDto?> UpdateJournalEntryAsync(Guid id, UpdateJournalEntryRequest request)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Updating journal entry {id} via: {_baseUrl}/{id} with data: {json}");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update journal entry {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var updatedEntry = JsonSerializer.Deserialize<JournalEntryByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return updatedEntry;
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
                Console.WriteLine($"Update journal entry exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Delete a journal entry
        /// </summary>
        public async Task<bool> DeleteJournalEntryAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Deleting journal entry {id} via: {_baseUrl}/{id}");

                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

                Console.WriteLine($"Delete journal entry {id} - Status: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete journal entry exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Post a journal entry (change status from Draft to Posted)
        /// </summary>
        public async Task<bool> PostJournalEntryAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Posting journal entry {id} via: {_baseUrl}/{id}/post");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{id}/post", null);

                Console.WriteLine($"Post journal entry {id} - Status: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Post journal entry exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unpost a journal entry (change status from Posted to Draft)
        /// </summary>
        public async Task<bool> UnpostJournalEntryAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Unposting journal entry {id} via: {_baseUrl}/{id}/unpost");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{id}/unpost", null);

                Console.WriteLine($"Unpost journal entry {id} - Status: {response.StatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unpost journal entry exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get journal entry statistics
        /// </summary>
        public async Task<JournalEntryStatistics?> GetJournalEntryStatisticsAsync(string companyId, string financialYearId)
        {
            try
            {
                SetAuthHeader();
                var url = $"{_baseUrl}/statistics?companyId={companyId}&financialYearId={financialYearId}";

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get journal entry statistics - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var statistics = JsonSerializer.Deserialize<JournalEntryStatistics>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return statistics;
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
                Console.WriteLine($"Get journal entry statistics exception: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    // Statistics model for journal entries
    public class JournalEntryStatistics
    {
        public int TotalEntries { get; set; }
        public int DraftEntries { get; set; }
        public int PostedEntries { get; set; }
        public int CancelledEntries { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal UnbalancedAmount { get; set; }
        public int UnbalancedEntries { get; set; }
        public Dictionary<JournalEntryType, int> EntriesByType { get; set; } = new Dictionary<JournalEntryType, int>();
    }
}
