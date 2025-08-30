using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class TransactionService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "api/v1/transaction";

        public TransactionService(AuthService authService)
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

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid companyId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                SetAuthHeader();
                var url = $"{_baseUrl}?companyId={companyId}&pageNumber={pageNumber}&pageSize={pageSize}";
                
                Console.WriteLine($"Fetching transactions for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var paginatedResponse = JsonSerializer.Deserialize<PaginatedTransactionListResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return paginatedResponse?.Items ?? new List<Transaction>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return new List<Transaction>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get transactions by company exception: {ex.Message}");
                return new List<Transaction>();
            }
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get transaction {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var transaction = JsonSerializer.Deserialize<Transaction>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return transaction;
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
                Console.WriteLine($"Get transaction exception: {ex.Message}");
                return null;
            }
        }

        public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Creating transaction with data: {json}");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create transaction - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var createdTransaction = JsonSerializer.Deserialize<Transaction>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return createdTransaction;
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
                Console.WriteLine($"Create transaction exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateTransactionAsync(Guid id, Transaction transaction)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(transaction, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Updating transaction {id} with data: {json}");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update transaction {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update transaction exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTransactionAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Delete transaction {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete transaction exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<TransactionListDto>> GetTransactionListAsync(Guid companyId, Guid financialYearId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                SetAuthHeader();

                var queryParams = new List<string>
        {
            $"financialYearId={financialYearId}",
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };


                var queryString = string.Join("&", queryParams);
                var url = $"{_baseUrl}/company/{companyId}?{queryString}";


                Console.WriteLine($"Fetching transaction list for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0} characters");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        Console.WriteLine($"Response Content: {responseContent}");
                        
                        var paginatedResponse = JsonSerializer.Deserialize<PaginatedTransactionListDtoResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        if (paginatedResponse != null)
                        {
                            Console.WriteLine($"Deserialized successfully. Items count: {paginatedResponse.Items?.Count ?? 0}");
                            return paginatedResponse.Items ?? new List<TransactionListDto>();
                        }
                        else
                        {
                            Console.WriteLine("Deserialization returned null");
                            return new List<TransactionListDto>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        Console.WriteLine($"Response content: {responseContent}");
                        return new List<TransactionListDto>();
                    }
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return new List<TransactionListDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get transaction list exception: {ex.Message}");
                return new List<TransactionListDto>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
