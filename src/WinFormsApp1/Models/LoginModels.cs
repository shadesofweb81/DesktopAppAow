using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        // Alternative field names that might be expected
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("jwtToken")]
        public string JwtToken { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        // Helper property to get the token from any of the possible fields
        public string GetToken()
        {
            if (!string.IsNullOrEmpty(Token)) return Token;
            if (!string.IsNullOrEmpty(AccessToken)) return AccessToken;
            if (!string.IsNullOrEmpty(JwtToken)) return JwtToken;
            return string.Empty;
        }

        // Helper property to check if login was successful
        public bool IsLoginSuccessful()
        {
            return Success || IsSuccess || Status?.ToLower() == "success";
        }
    }
}
