using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinFormsApp1.Models
{
    public class TrialBalanceRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ReportType { get; set; } = "AllAccounts"; // AllAccounts, Grouped, Hierarchical
    }

    public class TrialBalanceResponse
    {
        [JsonPropertyName("companyId")]
        public string CompanyId { get; set; } = string.Empty;

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonPropertyName("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonPropertyName("toDate")]
        public DateTime ToDate { get; set; }

        [JsonPropertyName("generatedOn")]
        public DateTime GeneratedOn { get; set; }

        [JsonPropertyName("accounts")]
        public List<TrialBalanceAccount> Accounts { get; set; } = new List<TrialBalanceAccount>();

        [JsonPropertyName("summary")]
        public TrialBalanceSummary Summary { get; set; } = new TrialBalanceSummary();
    }

    public class TrialBalanceAccount
    {
        [JsonPropertyName("ledgerId")]
        public string LedgerId { get; set; } = string.Empty;

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; } = string.Empty;

        [JsonPropertyName("accountCode")]
        public string AccountCode { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("accountType")]
        public string AccountType { get; set; } = string.Empty;

        [JsonPropertyName("rootCategory")]
        public string RootCategory { get; set; } = string.Empty;

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }

        [JsonPropertyName("parentName")]
        public string ParentName { get; set; } = string.Empty;

        [JsonPropertyName("hierarchyPath")]
        public string HierarchyPath { get; set; } = string.Empty;

        [JsonPropertyName("hierarchyLevel")]
        public int HierarchyLevel { get; set; }

        [JsonPropertyName("openingBalance")]
        public decimal OpeningBalance { get; set; }

        [JsonPropertyName("openingBalanceType")]
        public string OpeningBalanceType { get; set; } = string.Empty; // "Dr" or "Cr"

        [JsonPropertyName("debitAmount")]
        public decimal DebitAmount { get; set; }

        [JsonPropertyName("creditAmount")]
        public decimal CreditAmount { get; set; }

        [JsonPropertyName("closingBalance")]
        public decimal ClosingBalance { get; set; }

        [JsonPropertyName("closingBalanceType")]
        public string ClosingBalanceType { get; set; } = string.Empty; // "Dr" or "Cr"

        [JsonPropertyName("openingBalanceFormatted")]
        public string OpeningBalanceFormatted { get; set; } = string.Empty;

        [JsonPropertyName("closingBalanceFormatted")]
        public string ClosingBalanceFormatted { get; set; } = string.Empty;
    }

    public class TrialBalanceSummary
    {
        [JsonPropertyName("totalDebitOpeningBalance")]
        public decimal TotalDebitOpeningBalance { get; set; }

        [JsonPropertyName("totalCreditOpeningBalance")]
        public decimal TotalCreditOpeningBalance { get; set; }

        [JsonPropertyName("totalDebitAmount")]
        public decimal TotalDebitAmount { get; set; }

        [JsonPropertyName("totalCreditAmount")]
        public decimal TotalCreditAmount { get; set; }

        [JsonPropertyName("totalDebitClosingBalance")]
        public decimal TotalDebitClosingBalance { get; set; }

        [JsonPropertyName("totalCreditClosingBalance")]
        public decimal TotalCreditClosingBalance { get; set; }

        [JsonPropertyName("totalAccounts")]
        public int TotalAccounts { get; set; }

        [JsonPropertyName("accountsWithActivity")]
        public int AccountsWithActivity { get; set; }

        [JsonPropertyName("accountsWithNoActivity")]
        public int AccountsWithNoActivity { get; set; }

        [JsonPropertyName("isBalanced")]
        public bool IsBalanced { get; set; }

        [JsonPropertyName("balanceDifference")]
        public decimal BalanceDifference { get; set; }
    }

    public class TrialBalanceGroup
    {
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public decimal GroupDebitBalance { get; set; }
        public decimal GroupCreditBalance { get; set; }
        public List<TrialBalanceAccount> Accounts { get; set; } = new List<TrialBalanceAccount>();
    }

    public class TrialBalanceHierarchicalResponse
    {
        public string CompanyId { get; set; } = string.Empty;
        public string FinancialYearId { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<TrialBalanceAccount> RootAccounts { get; set; } = new List<TrialBalanceAccount>();
        public TrialBalanceSummary Summary { get; set; } = new TrialBalanceSummary();
    }
}
