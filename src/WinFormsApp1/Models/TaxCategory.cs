using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaxCategory
    {
        GST,            // Goods and Services Tax
        IncomeTax,      // Income Tax, TDS, TCS
        CustomsDuty,    // Import/Export Duties
        Cess,          // Various types of Cess
        ServiceTax,     // Service Tax
        VAT,           // Value Added Tax (for legacy)
        Other          // Other types of taxes
    }
} 