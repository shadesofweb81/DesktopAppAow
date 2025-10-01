using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class TrialBalanceService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
       
        
        public AuthService AuthService => _authService;
        
        public TrialBalanceService(AuthService authService)
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

        public async Task<TrialBalanceResponse?> GetTrialBalanceAllAccountsAsync(
            Guid companyId, 
            DateTime fromDate, 
            DateTime toDate, 
            bool includeZeroBalances = false, 
            string? accountFilter = null, 
            string? accountTypeFilter = null)
        {
            try
            {
                SetAuthHeader();

                // Build query string parameters
                var queryParams = new List<string>
                {
                    $"companyId={companyId}",
                    $"fromDate={fromDate:yyyy-MM-dd}",
                    $"toDate={toDate:yyyy-MM-dd}",
                    $"includeZeroBalances={includeZeroBalances.ToString().ToLower()}"
                };

                if (!string.IsNullOrEmpty(accountFilter))
                    queryParams.Add($"accountFilter={Uri.EscapeDataString(accountFilter)}");

                if (!string.IsNullOrEmpty(accountTypeFilter))
                    queryParams.Add($"accountTypeFilter={Uri.EscapeDataString(accountTypeFilter)}");

                var queryString = string.Join("&", queryParams);
                var url = $"/api/v2/reports/trial-balance?{queryString}";               

                Console.WriteLine($"Fetching Trial Balance Report for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Trial Balance Report Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var trialBalance = JsonSerializer.Deserialize<TrialBalanceResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            NumberHandling = JsonNumberHandling.AllowReadingFromString,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        return trialBalance;
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
                Console.WriteLine($"Exception in GetTrialBalanceAllAccountsAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<TrialBalanceResponse?> GetTrialBalanceReportAsync(
            Guid companyId, 
            DateTime fromDate, 
            DateTime toDate, 
            bool includeZeroBalances = false, 
            string? accountFilter = null, 
            string? accountTypeFilter = null,
            string? reportType = null)
        {
            try
            {
                SetAuthHeader();
                
                // Build query string parameters
                var queryParams = new List<string>
                {
                    $"companyId={companyId}",
                    $"fromDate={fromDate:yyyy-MM-dd}",
                    $"toDate={toDate:yyyy-MM-dd}",
                    $"includeZeroBalances={includeZeroBalances.ToString().ToLower()}"
                };

                if (!string.IsNullOrEmpty(accountFilter))
                    queryParams.Add($"accountFilter={Uri.EscapeDataString(accountFilter)}");

                if (!string.IsNullOrEmpty(accountTypeFilter))
                    queryParams.Add($"accountTypeFilter={Uri.EscapeDataString(accountTypeFilter)}");

                if (!string.IsNullOrEmpty(reportType))
                    queryParams.Add($"reportType={Uri.EscapeDataString(reportType)}");

                var queryString = string.Join("&", queryParams);
                var url = $"/api/v2/reports/trialbalance/{queryString}";

                Console.WriteLine($"Fetching Trial Balance Report for company {companyId} from: {url}");

                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Trial Balance Report Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var trialBalance = JsonSerializer.Deserialize<TrialBalanceResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            NumberHandling = JsonNumberHandling.AllowReadingFromString,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        return trialBalance;
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
                Console.WriteLine($"Exception in GetTrialBalanceReportAsync: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
