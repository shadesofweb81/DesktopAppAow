using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace WinFormsApp1.Documents.ExportPdf
{
    public class TransactionPdfGenerator
    {
        public TransactionPdfGenerator()
        {
            // Set QuestPDF license type to Community (MIT)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateTransactionListPdf(TransactionPdfModel model)
        {
            try
            {
                // Ensure license is set
                QuestPDF.Settings.License = LicenseType.Community;
                
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(20);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        // Header - combined into single call
                        page.Header().Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text(model.CompanyName).FontSize(16).Bold();
                                    col.Item().Text(model.FinancialYear).FontSize(12).FontColor(Colors.Grey.Medium);
                                });

                                row.ConstantItem(100).AlignRight().Text(model.GeneratedDate.ToString("dd/MM/yyyy HH:mm")).FontSize(10);
                            });

                            column.Item().PaddingVertical(10).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                        });

                        // Content
                        page.Content().Column(column =>
                        {
                            // Report Title
                            column.Item().AlignCenter().Text(model.ReportTitle).FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                            column.Item().Height(20);

                            // Filter Information
                            if (!string.IsNullOrEmpty(model.FilterApplied) && model.FilterApplied != "All")
                            {
                                column.Item().Text($"Filter Applied: {model.FilterApplied}").FontSize(11).FontColor(Colors.Grey.Medium);
                                column.Item().Height(10);
                            }

                            // Summary Information
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Total Transactions: {model.TotalCount:N0}").FontSize(11);
                                    col.Item().Text($"Subtotal: {model.TotalAmount:N2}").FontSize(11);
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Tax Amount: {model.TotalTaxAmount:N2}").FontSize(11);
                                    col.Item().Text($"Grand Total: {model.GrandTotal:N2}").FontSize(11).Bold();
                                });
                            });

                            column.Item().Height(20);

                            // Transactions Table
                            if (model.Transactions.Any())
                            {
                                column.Item().Table(table =>
                                {
                                    // Define columns
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1.2f); // Transaction #
                                        columns.RelativeColumn(1.2f); // Invoice #
                                        columns.RelativeColumn(1.0f); // Type
                                        columns.RelativeColumn(1.0f); // Date
                                        columns.RelativeColumn(1.5f); // Party
                                        columns.RelativeColumn(1.0f); // Subtotal
                                        columns.RelativeColumn(0.8f); // Tax
                                        columns.RelativeColumn(1.0f); // Total
                                        columns.RelativeColumn(0.8f); // Status
                                        columns.RelativeColumn(0.6f); // Paid
                                        columns.RelativeColumn(1.0f); // Due Date
                                    });

                                    // Header
                                    table.Header(header =>
                                    {
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Txn #").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Invoice #").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Type").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Date").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Party").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Subtotal").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Tax").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Total").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Status").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Paid").FontSize(9).Bold();
                                        header.Cell().Background(Colors.Grey.Medium).Padding(5).Text("Due Date").FontSize(9).Bold();
                                    });

                                    // Rows
                                    foreach (var transaction in model.Transactions)
                                    {
                                        table.Cell().Padding(3).Text(transaction.TransactionNumber).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.InvoiceNumber).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.Type).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.TransactionDate.ToString("dd/MM/yyyy")).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.PartyName).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.SubTotal.ToString("N2")).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.TaxAmount.ToString("N2")).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.Total.ToString("N2")).FontSize(8).Bold();
                                        table.Cell().Padding(3).Text(transaction.Status).FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.IsPaid ? "Yes" : "No").FontSize(8);
                                        table.Cell().Padding(3).Text(transaction.DueDate?.ToString("dd/MM/yyyy") ?? "-").FontSize(8);
                                    }
                                });
                            }
                            else
                            {
                                column.Item().AlignCenter().Text("No transactions found").FontSize(12).FontColor(Colors.Grey.Medium);
                            }
                        });

                        // Footer
                        page.Footer().Row(row =>
                        {
                            row.RelativeItem().Text($"Generated on {DateTime.Now:dd/MM/yyyy HH:mm:ss}").FontSize(8).FontColor(Colors.Grey.Medium);
                            row.ConstantItem(100).AlignRight().Text("Page 1").FontSize(8).FontColor(Colors.Grey.Medium);
                        });
                    });
                });

                // Generate PDF with proper error handling
                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new InvalidOperationException($"Failed to generate PDF: {ex.Message}", ex);
            }
        }

        public void SaveTransactionListPdf(TransactionPdfModel model, string filePath)
        {
            try
            {
                var pdfBytes = GenerateTransactionListPdf(model);
                File.WriteAllBytes(filePath, pdfBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PDF: {ex.Message}");
                throw new InvalidOperationException($"Failed to save PDF: {ex.Message}", ex);
            }
        }
    }
}
