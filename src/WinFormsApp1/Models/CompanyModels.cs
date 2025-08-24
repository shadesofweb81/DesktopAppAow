using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class Company
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        // Display property for UI
        public string DisplayName => $"{Code} - {Name}";
    }

    public class CompanyListResponse
    {
        [JsonPropertyName("companies")]
        public List<Company> Companies { get; set; } = new List<Company>();

        [JsonPropertyName("data")]
        public List<Company> Data { get; set; } = new List<Company>();

        [JsonPropertyName("items")]
        public List<Company> Items { get; set; } = new List<Company>();

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        // Helper method to get companies from any of the possible arrays
        public List<Company> GetCompanies()
        {
            if (Companies.Any()) return Companies;
            if (Data.Any()) return Data;
            if (Items.Any()) return Items;
            return new List<Company>();
        }
    }

    public class CompanyCreateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class CompanyResponse
    {
        [JsonPropertyName("company")]
        public Company? Company { get; set; }

        [JsonPropertyName("data")]
        public Company? Data { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        // Helper method to get company from response
        public Company? GetCompany()
        {
            return Company ?? Data;
        }
    }

    public class SelectCompanyModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonPropertyName("companyCode")]
        public string CompanyCode { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;

        // Computed property for display
        public string DisplayName => $"{CompanyCode} - {CompanyName}";

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class EditCompanyModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; } = string.Empty;

        [JsonPropertyName("logoUrl")]
        public string LogoUrl { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("userRole")]
        public string UserRole { get; set; } = string.Empty;

        [JsonPropertyName("startingFinancialYearDate")]
        public DateTime? StartingFinancialYearDate { get; set; }

        // Display property for UI
        public string DisplayName => $"{Name} ({Currency})";

        // Convert to Company model for compatibility
        public Company ToCompany()
        {
            return new Company
            {
                Id = Guid.TryParse(Id, out var guid) ? guid : Guid.Empty,
                Name = Name,
                Code = "", // Code not available in EditCompanyModel
                Address = Address,
                City = City,
                State = State,
                ZipCode = ZipCode,
                Country = Country,
                Phone = Phone,
                Email = Email,
                Website = Website,
                TaxId = TaxId,
                IsActive = true // Default to active since not provided in response
            };
        }
    }
}
