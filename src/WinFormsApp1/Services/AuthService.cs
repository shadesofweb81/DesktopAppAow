using System.Text;
using System.Text.Json;
using System.Security.Claims;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class AuthService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _authBaseUrl = "https://auth.accountingonweb.com";
        //private readonly string _erpBaseUrl = "https://erp.accountingonweb.com";
        private readonly string _erpBaseUrl = "https://localhost:7046";
        private readonly string _tokenFilePath;
        private string? _jwtToken;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_authBaseUrl);

            // Add headers that might be required
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "WinFormsApp1/1.0");

            // Set up token file path
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "WinFormsApp1");
            Directory.CreateDirectory(appFolder);
            _tokenFilePath = Path.Combine(appFolder, "auth_token.json");

            // Try to load saved token on startup
            LoadSavedToken();
        }

        public string? JwtToken => _jwtToken;
        public string AuthBaseUrl => _authBaseUrl;
        public string ErpBaseUrl => _erpBaseUrl;

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Password = password,
                    Email = username     // Some APIs expect email instead of username
                };

                var json = JsonSerializer.Serialize(loginRequest, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending login request to: {_authBaseUrl}/api/auth/User/login");
                Console.WriteLine($"Request JSON: {json}");

                var response = await _httpClient.PostAsync("/api/auth/User/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (loginResponse != null)
                        {
                            var token = loginResponse.GetToken();
                            if (!string.IsNullOrEmpty(token))
                            {
                                _jwtToken = token;
                                SaveToken(); // Save token to file
                                loginResponse.Success = true;
                                return loginResponse;
                            }
                            else
                            {
                                return new LoginResponse
                                {
                                    Success = false,
                                    Message = loginResponse.Message ?? loginResponse.Error ?? "Login failed: No token received"
                                };
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        // Try to parse as a simple response
                        return new LoginResponse
                        {
                            Success = false,
                            Message = $"Response parsing error: {ex.Message}. Raw response: {responseContent}"
                        };
                    }
                }

                // If we get here, the login failed
                return new LoginResponse
                {
                    Success = false,
                    Message = $"HTTP {(int)response.StatusCode}: {responseContent}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login exception: {ex.Message}");
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(registerModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending register request to: {_authBaseUrl}/api/auth/User/register");
                Console.WriteLine($"Request JSON: {json}");

                var response = await _httpClient.PostAsync("/api/auth/User/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var registerResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (registerResponse != null)
                        {
                            registerResponse.Success = true;
                            return registerResponse;
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return new RegisterResponse
                        {
                            Success = false,
                            Message = $"Response parsing error: {ex.Message}. Raw response: {responseContent}"
                        };
                    }
                }

                // If we get here, the registration failed
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"HTTP {(int)response.StatusCode}: {responseContent}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration exception: {ex.Message}");
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<RegisterResponse> ResendVerificationAsync(string email)
        {
            try
            {
                var resendModel = new ResendVerificationModel { Email = email };
                var json = JsonSerializer.Serialize(resendModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending resend verification request to: {_authBaseUrl}/api/auth/User/resend-verification");
                Console.WriteLine($"Request JSON: {json}");

                var response = await _httpClient.PostAsync("/api/auth/User/resend-verification", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var resendResponse = JsonSerializer.Deserialize<RegisterResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (resendResponse != null)
                        {
                            resendResponse.Success = true;
                            return resendResponse;
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON parsing error: {ex.Message}");
                        return new RegisterResponse
                        {
                            Success = false,
                            Message = $"Response parsing error: {ex.Message}. Raw response: {responseContent}"
                        };
                    }
                }

                // If we get here, the resend failed
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"HTTP {(int)response.StatusCode}: {responseContent}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Resend verification exception: {ex.Message}");
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Resend verification failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> TestApiConnection()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/auth/health");
                Console.WriteLine($"API Health Check Status: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Health Check failed: {ex.Message}");
                return false;
            }
        }

        public void Logout()
        {
            _jwtToken = null;
            ClearSavedToken();
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_jwtToken) && IsTokenValid();

        public bool HasValidSavedToken => IsAuthenticated;

        private void LoadSavedToken()
        {
            try
            {
                if (File.Exists(_tokenFilePath))
                {
                    var json = File.ReadAllText(_tokenFilePath);
                    var tokenData = JsonSerializer.Deserialize<TokenData>(json);

                    if (tokenData != null && !string.IsNullOrEmpty(tokenData.Token))
                    {
                        _jwtToken = tokenData.Token;

                        // Check if token is still valid
                        if (!IsTokenValid())
                        {
                            Console.WriteLine("Saved token has expired, clearing it");
                            _jwtToken = null;
                            ClearSavedToken();
                        }
                        else
                        {
                            Console.WriteLine("Valid saved token loaded successfully");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading saved token: {ex.Message}");
                _jwtToken = null;
            }
        }

        private void SaveToken()
        {
            try
            {
                if (!string.IsNullOrEmpty(_jwtToken))
                {
                    var tokenData = new TokenData
                    {
                        Token = _jwtToken,
                        SavedAt = DateTime.UtcNow
                    };

                    var json = JsonSerializer.Serialize(tokenData, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    File.WriteAllText(_tokenFilePath, json);
                    Console.WriteLine("Token saved successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving token: {ex.Message}");
            }
        }

        private void ClearSavedToken()
        {
            try
            {
                if (File.Exists(_tokenFilePath))
                {
                    File.Delete(_tokenFilePath);
                    Console.WriteLine("Saved token cleared");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing saved token: {ex.Message}");
            }
        }

        private bool IsTokenValid()
        {
            if (string.IsNullOrEmpty(_jwtToken))
                return false;

            try
            {
                // Simple JWT token validation - check if it's not expired
                var parts = _jwtToken.Split('.');
                if (parts.Length != 3)
                    return false;

                // Decode the payload (second part)
                var payload = parts[1];
                // Add padding if needed
                payload = payload.PadRight(4 * ((payload.Length + 3) / 4), '=');
                payload = payload.Replace('-', '+').Replace('_', '/');

                var payloadBytes = Convert.FromBase64String(payload);
                var payloadJson = Encoding.UTF8.GetString(payloadBytes);

                var payloadData = JsonSerializer.Deserialize<JsonElement>(payloadJson);

                // Check for expiration
                if (payloadData.TryGetProperty("exp", out var expElement))
                {
                    var expTimestamp = expElement.GetInt64();
                    var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expTimestamp);

                    // Add 5 minute buffer for safety
                    if (DateTimeOffset.UtcNow >= expDateTime.AddMinutes(-5))
                    {
                        Console.WriteLine($"Token expired at {expDateTime}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating token: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    public class TokenData
    {
        public string Token { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
    }
}
