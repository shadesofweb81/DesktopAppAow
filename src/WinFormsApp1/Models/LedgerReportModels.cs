using System;
using System.Collections.Generic;

namespace WinFormsApp1.Models
{
    public class LedgerReportRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string PartyLedgerId { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class LedgerReportResponse
    {
        public string PartyId { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string PartyType { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<MonthlyData> MonthlyData { get; set; } = new List<MonthlyData>();
        public ReportSummary Summary { get; set; } = new ReportSummary();
    }

    public class MonthlyData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public List<TransactionData> Transactions { get; set; } = new List<TransactionData>();
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalBalance { get; set; }
        public int TransactionCount { get; set; }
    }

    public class TransactionData
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionNumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceDue { get; set; }
    }

    public class ReportSummary
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalBalance { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageMonthlyAmount { get; set; }
        public string HighestMonth { get; set; } = string.Empty;
        public string LowestMonth { get; set; } = string.Empty;
    }
}
