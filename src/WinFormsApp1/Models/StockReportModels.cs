using System;
using System.Collections.Generic;

namespace WinFormsApp1.Models
{
    public class StockReportRequest
    {
        public Guid CompanyId { get; set; }
        public string? ProductFilter { get; set; }
        public bool ShowOnlyLowStock { get; set; }
        public bool ShowOnlyOutOfStock { get; set; }
        public bool IncludeVariants { get; set; } = true;
        public DateTime? AsOfDate { get; set; }
    }

    // This matches the actual API response structure
    public class StockReportResponse
    {
        public List<StockItem> Products { get; set; } = new List<StockItem>();
        public decimal TotalStockValue { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
    }

    public class StockItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public decimal MaxStockLevel { get; set; }
        public decimal StockValue { get; set; }
        public decimal TotalPurchased { get; set; }
        public decimal TotalSold { get; set; }
        public decimal TotalReturned { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }
        public bool IsOverStock { get; set; }
        public List<StockVariant> Variants { get; set; } = new List<StockVariant>();
        public bool HasVariants { get; set; }
        
        // Computed properties for display
        public string StockStatus 
        { 
            get 
            {
                if (IsOutOfStock) return "Out of Stock";
                if (IsLowStock) return "Low Stock";
                if (IsOverStock) return "Over Stock";
                return "In Stock";
            } 
        }
        
        public decimal MinimumStock => MinStockLevel;
        public decimal MaximumStock => MaxStockLevel;
        public decimal ReorderLevel => MinStockLevel;
        public decimal UnitCost => PurchasePrice;
        public decimal TotalValue => StockValue;
    }

    public class StockVariant
    {
        public Guid VariantId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public string VariantCode { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string StockStatus { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }
    }

    public class StockItemReportRequest
    {
        public Guid ProductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludeTransactionDetails { get; set; } = true;
    }

    public class StockItemReportResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal TotalPurchased { get; set; }
        public decimal TotalSold { get; set; }
        public decimal TotalManufactured { get; set; }
        public decimal TotalReturned { get; set; }
        public decimal TotalAdjusted { get; set; }
        public List<StockTransaction> Transactions { get; set; } = new List<StockTransaction>();
        public StockItemSummaryDetail Summary { get; set; } = new StockItemSummaryDetail();
    }

    public class StockTransaction
    {
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty; // Purchase, Sale, Manufacture, Return, Adjustment
        public string TransactionNumber { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class StockItemSummaryDetail
    {
        public int TotalTransactions { get; set; }
        public int PurchaseTransactions { get; set; }
        public int SaleTransactions { get; set; }
        public int ManufactureTransactions { get; set; }
        public int ReturnTransactions { get; set; }
        public int AdjustmentTransactions { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalSaleValue { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal AverageSalePrice { get; set; }
    }
}