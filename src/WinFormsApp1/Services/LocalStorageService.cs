using System.Text.Json;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class LocalStorageService
    {
        private readonly string _dataDirectory;
        private readonly string _selectedCompanyFile;
        private readonly string _companyCacheFile;

        public LocalStorageService()
        {
            // Create data directory in user's AppData
            _dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinFormsApp1");
            _selectedCompanyFile = Path.Combine(_dataDirectory, "selected_company.json");
            _companyCacheFile = Path.Combine(_dataDirectory, "company_cache.json");
            
            // Ensure directory exists
            Directory.CreateDirectory(_dataDirectory);
        }

        #region Selected Company Management

        public async Task SaveSelectedCompanyAsync(Company company)
        {
            try
            {
                var json = JsonSerializer.Serialize(company, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_selectedCompanyFile, json);
                Console.WriteLine($"Selected company saved: {company.DisplayName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving selected company: {ex.Message}");
                throw;
            }
        }

        public async Task<Company?> GetSelectedCompanyAsync()
        {
            try
            {
                if (!File.Exists(_selectedCompanyFile))
                    return null;

                var json = await File.ReadAllTextAsync(_selectedCompanyFile);
                
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var company = JsonSerializer.Deserialize<Company>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading selected company: {ex.Message}");
                return null;
            }
        }

        public async Task ClearSelectedCompanyAsync()
        {
            try
            {
                if (File.Exists(_selectedCompanyFile))
                {
                    File.Delete(_selectedCompanyFile);
                    Console.WriteLine("Selected company cleared");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing selected company: {ex.Message}");
            }
        }

        public bool HasSelectedCompany()
        {
            return File.Exists(_selectedCompanyFile);
        }

        #endregion

        #region Company Cache Management

        public async Task SaveCompanyCacheAsync(List<Company> companies)
        {
            try
            {
                var cacheData = new CompanyCache
                {
                    Companies = companies,
                    LastUpdated = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddHours(1) // Cache for 1 hour
                };

                var json = JsonSerializer.Serialize(cacheData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_companyCacheFile, json);
                Console.WriteLine($"Company cache saved: {companies.Count} companies");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving company cache: {ex.Message}");
            }
        }

        public async Task<List<Company>?> GetCompanyCacheAsync()
        {
            try
            {
                if (!File.Exists(_companyCacheFile))
                    return null;

                var json = await File.ReadAllTextAsync(_companyCacheFile);
                
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var cacheData = JsonSerializer.Deserialize<CompanyCache>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Check if cache is still valid
                if (cacheData?.ExpiryDate < DateTime.UtcNow)
                {
                    Console.WriteLine("Company cache expired");
                    return null;
                }

                Console.WriteLine($"Company cache loaded: {cacheData?.Companies?.Count ?? 0} companies");
                return cacheData?.Companies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading company cache: {ex.Message}");
                return null;
            }
        }

        public async Task ClearCompanyCacheAsync()
        {
            try
            {
                if (File.Exists(_companyCacheFile))
                {
                    File.Delete(_companyCacheFile);
                    Console.WriteLine("Company cache cleared");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing company cache: {ex.Message}");
            }
        }

        #endregion

        #region Settings Management

        public async Task SaveSettingAsync(string key, string value)
        {
            try
            {
                var settingsFile = Path.Combine(_dataDirectory, "settings.json");
                var settings = new Dictionary<string, string>();

                // Load existing settings
                if (File.Exists(settingsFile))
                {
                    var existingJson = await File.ReadAllTextAsync(settingsFile);
                    if (!string.IsNullOrWhiteSpace(existingJson))
                    {
                        settings = JsonSerializer.Deserialize<Dictionary<string, string>>(existingJson) ?? new Dictionary<string, string>();
                    }
                }

                // Update setting
                settings[key] = value;

                // Save settings
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(settingsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving setting {key}: {ex.Message}");
            }
        }

        public async Task<string?> GetSettingAsync(string key)
        {
            try
            {
                var settingsFile = Path.Combine(_dataDirectory, "settings.json");
                
                if (!File.Exists(settingsFile))
                    return null;

                var json = await File.ReadAllTextAsync(settingsFile);
                
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                
                return settings?.TryGetValue(key, out var value) == true ? value : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading setting {key}: {ex.Message}");
                return null;
            }
        }

        #endregion
    }

    public class CompanyCache
    {
        public List<Company> Companies { get; set; } = new List<Company>();
        public DateTime LastUpdated { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

