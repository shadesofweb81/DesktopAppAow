
using WinFormsApp1.Models;

namespace WinFormsApp1.Documents.ExportPdf
{
    public class PdfExportService
    {
        private readonly TransactionPdfGenerator _pdfGenerator;

        public PdfExportService()
        {
            _pdfGenerator = new TransactionPdfGenerator();
        }

        public Task<bool> ExportTransactionListToPdfAsync(
            List<TransactionListDto> transactions,
            Models.Company? company,
            FinancialYearModel? financialYear,
            string? transactionType,
            string? filterApplied)
        {
            try
            {
                Console.WriteLine($"Starting PDF export for {transactions.Count} transactions");
                Console.WriteLine($"Company: {company?.DisplayName ?? "None"}");
                Console.WriteLine($"Financial Year: {financialYear?.YearLabel ?? "None"}");
                Console.WriteLine($"Transaction Type: {transactionType ?? "All"}");
                Console.WriteLine($"Filter Applied: {filterApplied ?? "All"}");

                // Validate input data
                if (transactions == null || !transactions.Any())
                {
                    MessageBox.Show("No transactions to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return Task.FromResult(false);
                }

                if (company == null)
                {
                    MessageBox.Show("Company information is required for PDF export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return Task.FromResult(false);
                }

                // Create the PDF model
                var pdfModel = CreateTransactionPdfModel(transactions, company, financialYear, transactionType, filterApplied);
                Console.WriteLine($"PDF model created successfully with {pdfModel.Transactions.Count} transactions");

                // Show save file dialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;
                    
                    // Set default filename
                    var defaultFileName = GenerateDefaultFileName(company, transactionType, filterApplied);
                    saveFileDialog.FileName = defaultFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Console.WriteLine($"Saving PDF to: {saveFileDialog.FileName}");
                        
                        try
                        {
                            // Generate and save the PDF
                            _pdfGenerator.SaveTransactionListPdf(pdfModel, saveFileDialog.FileName);
                            Console.WriteLine("PDF generated and saved successfully");
                            
                            // Show success message
                            var result = MessageBox.Show(
                                $"PDF exported successfully to:\n{saveFileDialog.FileName}\n\nWould you like to open the file?",
                                "Export Successful",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                            {
                                // Open the PDF file
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = saveFileDialog.FileName,
                                    UseShellExecute = true
                                });
                            }

                            return Task.FromResult(true);
                        }
                        catch (Exception pdfEx)
                        {
                            Console.WriteLine($"Error during PDF generation: {pdfEx.Message}");
                            Console.WriteLine($"Stack trace: {pdfEx.StackTrace}");
                            
                            MessageBox.Show(
                                $"Error generating PDF: {pdfEx.Message}\n\nPlease check the console for more details.",
                                "PDF Generation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            
                            return Task.FromResult(false);
                        }
                    }
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PDF export service: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                MessageBox.Show(
                    $"Error exporting PDF: {ex.Message}\n\nPlease check the console for more details.",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return Task.FromResult(false);
            }
        }

        private TransactionPdfModel CreateTransactionPdfModel(
            List<TransactionListDto> transactions,
            Models.Company? company,
            FinancialYearModel? financialYear,
            string? transactionType,
            string? filterApplied)
        {
            try
            {
                var model = new TransactionPdfModel
                {
                    CompanyName = company?.DisplayName ?? "Unknown Company",
                    FinancialYear = financialYear?.YearLabel ?? "Unknown Financial Year",
                    ReportTitle = GenerateReportTitle(transactionType),
                    GeneratedDate = DateTime.Now,
                    FilterApplied = filterApplied ?? "All",
                    TotalCount = transactions.Count,
                    Transactions = new List<TransactionPdfItem>()
                };

                Console.WriteLine($"Creating PDF model with company: {model.CompanyName}, financial year: {model.FinancialYear}");

                // Convert transactions to PDF items
                foreach (var transaction in transactions)
                {
                    var pdfItem = new TransactionPdfItem
                    {
                        TransactionNumber = transaction.TransactionNumber ?? "",
                        InvoiceNumber = transaction.InvoiceNumber ?? "",
                        Type = transaction.Type.ToString(),
                        TransactionDate = transaction.TransactionDate,
                        PartyName = transaction.PartyName ?? "",
                        SubTotal = transaction.SubTotal,
                        TaxAmount = transaction.TaxAmount,
                        Total = transaction.Total,
                        Status = transaction.Status ?? "",
                        IsPaid = transaction.IsPaid,
                        DueDate = transaction.DueDate
                    };

                    model.Transactions.Add(pdfItem);
                }

                // Calculate totals
                model.TotalAmount = transactions.Sum(t => t.SubTotal);
                model.TotalTaxAmount = transactions.Sum(t => t.TaxAmount);
                model.GrandTotal = transactions.Sum(t => t.Total);

                Console.WriteLine($"PDF model created: {model.Transactions.Count} transactions, Total: {model.GrandTotal:N2}");

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating PDF model: {ex.Message}");
                throw new InvalidOperationException($"Failed to create PDF model: {ex.Message}", ex);
            }
        }

        private string GenerateReportTitle(string? transactionType)
        {
            if (string.IsNullOrEmpty(transactionType))
                return "Transaction List Report";

            return $"{transactionType} Transactions Report";
        }

        private string GenerateDefaultFileName(Models.Company? company, string? transactionType, string? filterApplied)
        {
            var companyName = company?.DisplayName?.Replace(" ", "_") ?? "Company";
            var typeText = !string.IsNullOrEmpty(transactionType) ? $"{transactionType}_" : "";
            var filterText = !string.IsNullOrEmpty(filterApplied) && filterApplied != "All" ? $"{filterApplied}_" : "";
            var dateText = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            return $"{companyName}_{typeText}{filterText}Transactions_{dateText}.pdf";
        }
    }
}
