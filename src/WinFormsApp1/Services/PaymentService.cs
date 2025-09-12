using System.ComponentModel.Design;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;
using WinFormsApp1.Models.request;

namespace WinFormsApp1.Services
{
    public class PaymentService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "api/v1/transaction";

        public PaymentService(AuthService authService)
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
        /// Get all payments with pagination
        /// </summary>
        public async Task<List<PaymentListDto>> GetPaymentsListAsync(Guid companyId, Guid financialYearId, int pageNumber = 1, int pageSize = 50, string? type = null)
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


                if (!string.IsNullOrEmpty(type))
                    queryParams.Add($"type={Uri.EscapeDataString(type)}");

                var queryString = string.Join("&", queryParams);
                var url = $"{_baseUrl}/company/{companyId}?{queryString}";

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get payments - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var paginatedResponse = JsonSerializer.Deserialize<PaginatedPaymentListResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        if (paginatedResponse != null)
                        {
                            Console.WriteLine($"Deserialized successfully. Items count: {paginatedResponse.Items?.Count ?? 0}");
                            return paginatedResponse.Items ?? new List<PaymentListDto>();
                        }
                        else
                        {
                            Console.WriteLine("Deserialization returned null");
                            return new List<PaymentListDto>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return null;
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get payments exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get payment by ID
        /// </summary>
        public async Task<PaymentByIdDto?> GetPaymentByIdAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var url = $"{_baseUrl}/payment/{id}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get payment {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var payment = JsonSerializer.Deserialize<PaymentByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return payment;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return null;
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get payment exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        public async Task<PaymentByIdDto?> CreatePaymentAsync(CreatePaymentRequest request)
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

                Console.WriteLine($"Creating payment via: {_baseUrl} with data: {json}");


                var url = $"{_baseUrl}/invoice-payment";                
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Create payment - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var createdPayment = JsonSerializer.Deserialize<PaymentByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return createdPayment;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return null;
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create payment exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update an existing payment
        /// </summary>
        public async Task<PaymentByIdDto?> UpdatePaymentAsync(Guid id, UpdatePaymentRequest request)
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

                Console.WriteLine($"Updating payment {id} via: {_baseUrl}/{id} with data: {json}");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Update payment {id} - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var updatedPayment = JsonSerializer.Deserialize<PaymentByIdDto>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                        return updatedPayment;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return null;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return null;
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update payment exception: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Delete a payment
        /// </summary>
        public async Task<bool> DeletePaymentAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Deleting payment {id} via: {_baseUrl}/{id}");

                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

                Console.WriteLine($"Delete payment {id} - Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return false;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete payment exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Post a payment (change status from Draft to Posted)
        /// </summary>
        public async Task<bool> PostPaymentAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Posting payment {id} via: {_baseUrl}/{id}/post");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{id}/post", null);

                Console.WriteLine($"Post payment {id} - Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return false;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Post payment exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unpost a payment (change status from Posted to Draft)
        /// </summary>
        public async Task<bool> UnpostPaymentAsync(Guid id)
        {
            try
            {
                SetAuthHeader();

                Console.WriteLine($"Unposting payment {id} via: {_baseUrl}/{id}/unpost");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{id}/unpost", null);

                Console.WriteLine($"Unpost payment {id} - Status: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return false;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unpost payment exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get payment statistics
        /// </summary>
        public async Task<PaymentStatistics?> GetPaymentStatisticsAsync(string companyId, string financialYearId)
        {
            try
            {
                SetAuthHeader();
                var url = $"{_baseUrl}/statistics?companyId={companyId}&financialYearId={financialYearId}";

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Get payment statistics - Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var statistics = JsonSerializer.Deserialize<PaymentStatistics>(responseContent, new JsonSerializerOptions
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
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Unauthorized response received - triggering logout");
                    _authService.TriggerUnauthorized();
                    return null;
                }
                else
                {
                    Console.WriteLine($"API Error: HTTP {(int)response.StatusCode}: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get payment statistics exception: {ex.Message}");
                return null;
            }
        }


        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    // Statistics model for payments
    public class PaymentStatistics
    {
        public int TotalPayments { get; set; }
        public int DraftPayments { get; set; }
        public int PostedPayments { get; set; }
        public int CancelledPayments { get; set; }
        public decimal TotalPaymentReceived { get; set; }
        public decimal TotalPaymentMade { get; set; }
        public decimal NetPaymentAmount { get; set; }
        public Dictionary<PaymentType, int> PaymentsByType { get; set; } = new Dictionary<PaymentType, int>();
        public Dictionary<string, int> PaymentsByStatus { get; set; } = new Dictionary<string, int>();
    }
}
