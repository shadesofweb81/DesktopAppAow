using System.Net;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;
using AccountingERP.WebApi.Models.Requests;

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

        public async Task<TransactionByIdDto?> GetTransactionByIdAsync(Guid id)
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
                        var transactionDto = JsonSerializer.Deserialize<TransactionByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return transactionDto;
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

                var subPath = transaction.Type.ToString().StartsWith("Purchase", StringComparison.OrdinalIgnoreCase) ? "purchase"
                    : transaction.Type.ToString().StartsWith("Sale", StringComparison.OrdinalIgnoreCase) ? "sale"
                    : string.Empty;
                var url = string.IsNullOrEmpty(subPath) ? _baseUrl : $"{_baseUrl}/{subPath}";

                Console.WriteLine($"Creating transaction ({transaction.Type}) via: {url} with data: {json}");

                var response = await _httpClient.PostAsync(url, content);
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

        public async Task<bool> UpdateTransactionAsync(Guid id, UpdateTransactionRequest updateRequest)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(updateRequest, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var typeString = updateRequest.TransactionType ?? string.Empty;
                var url = $"{_baseUrl}/";
                if (typeString.StartsWith("Purchase", StringComparison.OrdinalIgnoreCase))
                {
                    url = $"{_baseUrl}/purchase";
                }
                else if (typeString.StartsWith("Sale", StringComparison.OrdinalIgnoreCase))
                {
                    url = $"{_baseUrl}/sale";
                }

                url = $"{url}/{id}";

                Console.WriteLine($"Updating transaction {id} ({typeString}) via: {url} with data: {json}");

                var response = await _httpClient.PutAsync(url, content);
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

        private UpdateTransactionRequest BuildUpdateTransactionRequest(Transaction transaction)
        {
            var companyId = transaction.CompanyId ?? Guid.Empty;

            // Derive payment term days from dates if possible
            var paymentTermDays = 30;
            try
            {
                if (transaction.DueDate != default && transaction.TransactionDate != default)
                {
                    var span = (transaction.DueDate - transaction.TransactionDate).Days;
                    if (span >= 0)
                    {
                        paymentTermDays = span;
                    }
                }
            }
            catch { }

            // Attempt to infer ledger ids from ledger entries if available
            Guid partyLedgerId = Guid.Empty;
            Guid accountLedgerId = Guid.Empty;

            if (transaction.LedgerEntries != null && transaction.LedgerEntries.Any())
            {
                var mainEntry = transaction.LedgerEntries.FirstOrDefault(le => le.IsMainEntry == true);
                if (mainEntry != null)
                {
                    partyLedgerId = mainEntry.LedgerId;
                }

                var systemEntry = transaction.LedgerEntries.FirstOrDefault(le => le.IsSystemEntry == true);
                if (systemEntry != null)
                {
                    accountLedgerId = systemEntry.LedgerId;
                }

                if (accountLedgerId == Guid.Empty)
                {
                    var nonMain = transaction.LedgerEntries.FirstOrDefault(le => le.IsMainEntry != true);
                    if (nonMain != null)
                    {
                        accountLedgerId = nonMain.LedgerId;
                    }
                }
            }

            var items = (transaction.Items ?? Array.Empty<TransactionItem>())
                .OrderBy(i => i.SerialNumber)
                .Select(i => new UpdateTransactionItemRequest
                {
                    Id = i.Id == Guid.Empty ? null : i.Id,
                    ProductId = i.ProductId,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    DiscountRate = i.DiscountRate,
                    DiscountAmount = i.DiscountAmount,
                    CurrentQuantity = i.CurrentQuantity,
                    SerialNumber = i.SerialNumber,
                    IsDeleted = false,
                    Variants = new List<UpdateTransactionItemVariantRequest>()
                })
                .ToList();

            var taxes = (transaction.Taxes ?? Array.Empty<TransactionTax>())
                .OrderBy(t => t.SerialNumber)
                .Select(t => new UpdateTransactionTaxRequest
                {
                    Id = t.Id == Guid.Empty ? null : t.Id,
                    TaxId = t.TaxId,
                    TaxableAmount = t.TaxableAmount,
                    Amount = t.TaxAmount,
                    CalculationMethod = string.IsNullOrWhiteSpace(t.CalculationMethod) ? "ItemSubtotal" : t.CalculationMethod,
                    IsApplied = t.IsApplied,
                    AppliedDate = t.AppliedDate,
                    ReferenceNumber = t.ReferenceNumber,
                    Description = t.Description,
                    IsDeleted = false,
                    SerialNumber = t.SerialNumber,
                    Components = null
                })
                .ToList();

            return new UpdateTransactionRequest
            {
                CompanyId = companyId,
                PartyLedgerId = partyLedgerId,
                AccountLedgerId = accountLedgerId,
                InvoiceNumber = transaction.InvoiceNumber,
                TransactionType = transaction.Type.ToString(),
                TransactionDate = transaction.TransactionDate,
                PaymentTermDays = paymentTermDays,
                Status = transaction.Status,
                Notes = transaction.Notes ?? string.Empty,
                Discount = transaction.Discount,
                Freight = transaction.Freight,
                IsFreightIncluded = transaction.IsFreightIncluded,
                RoundOff = transaction.RoundOff,
                Items = items,
                Taxes = taxes
            };
        }
    }
}

