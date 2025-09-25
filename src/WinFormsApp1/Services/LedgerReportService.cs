using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class LedgerReportService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl = "https://readapi.accountingonweb.com";
        
        public AuthService AuthService => _authService;
        
        public LedgerReportService(AuthService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);

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

        public async Task<LedgerReportResponse?> GetLedgerReportAsync(LedgerReportRequest request)
        {
            try
            {
                SetAuthHeader();

                // Build query string parameters
                var queryParams = new List<string>
                {
                    $"companyId={request.CompanyId}",
                    $"ledgerId={request.PartyLedgerId}",
                    $"fromDate={request.FromDate:yyyy-MM-dd}",
                    $"toDate={request.ToDate:yyyy-MM-dd}"
                };

                var queryString = string.Join("&", queryParams);
                var url = $"/api/v2/reports/ledger/ledger-transactions?{queryString}";
                Console.WriteLine($"Making GET request to: {_baseUrl}{url}");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ledger report response: {json}");

                    var result = JsonSerializer.Deserialize<LedgerReportResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error getting ledger report: {response.StatusCode} - {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetLedgerReportAsync: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
