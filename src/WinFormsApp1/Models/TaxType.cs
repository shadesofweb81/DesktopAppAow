using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaxType
    {
        // GST Related
        CGST,
        SGST,
        IGST,
        UTGST,
        CompensationCess,

        // Income Tax Related
        TDS,
        TCS,
        AdvanceTax,
        SelfAssessmentTax,

        // Customs Related
        BasicCustomsDuty,
        CountervailingDuty,
        AntiDumpingDuty,
        SafeguardDuty,

        // Cess Types
        EducationCess,
        HigherEducationCess,
        SwachhBharatCess,
        KrishiKalyanCess,
        RoadCess,

        // Others
        ProfessionalTax,
        StampDuty,
        Other
    }
} 