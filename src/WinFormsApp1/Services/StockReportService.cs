using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class StockReportService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
     

        public AuthService AuthService => _authService;
        
        public StockReportService(AuthService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_authService.ReportBaseUrl);

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

        public async Task<StockReportResponse?> GetStockReportAsync(StockReportRequest request)
        {
            try
            {
                SetAuthHeader();

                // Build query string parameters
                var queryParams = new List<string>
                {
                    $"companyId={request.CompanyId}"
                };

                if (!string.IsNullOrEmpty(request.ProductFilter))
                {
                    queryParams.Add($"productFilter={Uri.EscapeDataString(request.ProductFilter)}");
                }

                if (request.ShowOnlyLowStock)
                {
                    queryParams.Add("showOnlyLowStock=true");
                }

                if (request.ShowOnlyOutOfStock)
                {
                    queryParams.Add("showOnlyOutOfStock=true");
                }

                if (!request.IncludeVariants)
                {
                    queryParams.Add("includeVariants=false");
                }

                if (request.AsOfDate.HasValue)
                {
                    queryParams.Add($"asOfDate={request.AsOfDate.Value:yyyy-MM-dd}");
                }

                var queryString = string.Join("&", queryParams);
                var url = $"/api/v2/reports/stock/current?{queryString}";              

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Stock report response: {json}");

                    var result = JsonSerializer.Deserialize<StockReportResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error getting stock report: {response.StatusCode} - {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetStockReportAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<StockItemReportResponse?> GetStockItemReportAsync(StockItemReportRequest request)
        {
            try
            {
                SetAuthHeader();

                // Build query string parameters
                var queryParams = new List<string>();

                if (request.FromDate.HasValue)
                {
                    queryParams.Add($"fromDate={request.FromDate.Value:yyyy-MM-dd}");
                }

                if (request.ToDate.HasValue)
                {
                    queryParams.Add($"toDate={request.ToDate.Value:yyyy-MM-dd}");
                }

                if (!request.IncludeTransactionDetails)
                {
                    queryParams.Add("includeTransactionDetails=false");
                }

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var url = $"/api/v2/reports/stock/product-summary/{request.ProductId}{queryString}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Stock item report response: {json}");

                    var result = JsonSerializer.Deserialize<StockItemReportResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error getting stock item report: {response.StatusCode} - {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetStockItemReportAsync: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

