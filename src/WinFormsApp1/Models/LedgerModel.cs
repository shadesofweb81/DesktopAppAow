using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class LedgerModel 
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;  // e.g., "Customer", "Supplier", "Bank", etc.

     
        public string Code { get; set; } = string.Empty;

      
        public string? Address { get; set; }

      
        public string? City { get; set; }

      
        public string? State { get; set; }

     
        public string? ZipCode { get; set; }

       
        public string? Country { get; set; }

        public string? Phone { get; set; }

      
        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxId { get; set; }

        public bool IsGroup { get; set; }
        public bool? IsRegistered { get; set; }

        // Parent-Child relationship
        public Guid? ParentId { get; set; }
        public LedgerModel? Parent { get; set; }
        public ICollection<LedgerModel> Children { get; set; } = new HashSet<LedgerModel>();

        // Navigation properties
        public Guid CompanyId { get; set; }
        
        // Display property for list
        public string DisplayName => $"{Name} ({Category})";
       
    }

    public class SelectLedgerList
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? TaxId { get; set; }
        public bool IsGroup { get; set; }
        
        [JsonConverter(typeof(FlexibleBooleanConverter))]
        public bool? IsRegistered { get; set; }
        
        public string? ParentId { get; set; }
        public string CompanyId { get; set; } = string.Empty;

        // Display property for list
        public string DisplayName => $"{Name} ({Category})";

        // Convert to LedgerModel
        public LedgerModel ToLedgerModel()
        {
            return new LedgerModel
            {
                Id = Guid.TryParse(Id, out var id) ? id : Guid.Empty,
                Name = Name,
                Category = Category,
                Code = Code,
                Address = Address,
                City = City,
                State = State,
                ZipCode = ZipCode,
                Country = Country,
                Phone = Phone,
                Email = Email,
                Website = Website,
                TaxId = TaxId,
                IsGroup = IsGroup,
                IsRegistered = IsRegistered,
                ParentId = Guid.TryParse(ParentId, out var parentId) ? parentId : null,
                CompanyId = Guid.TryParse(CompanyId, out var companyId) ? companyId : Guid.Empty
            };
        }
    }

    public class UpdateLedgerModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Category { get; set; } = string.Empty;

        public string? Code { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxId { get; set; }

        public bool IsGroup { get; set; }
        public bool? IsRegistered { get; set; }

        // Parent-Child relationship
        public Guid? ParentId { get; set; }

        // Company ID is required for updates
        public Guid CompanyId { get; set; }
    }

    public class FlexibleBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.String:
                    var stringValue = reader.GetString();
                    if (string.IsNullOrEmpty(stringValue))
                        return null;
                    
                    return stringValue.ToLowerInvariant() switch
                    {
                        "true" or "1" or "yes" or "on" => true,
                        "false" or "0" or "no" or "off" => false,
                        _ => null
                    };
                case JsonTokenType.Number:
                    var numberValue = reader.GetInt32();
                    return numberValue switch
                    {
                        1 => true,
                        0 => false,
                        _ => null
                    };
                default:
                    return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteBooleanValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
} 