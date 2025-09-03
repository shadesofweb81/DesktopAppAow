using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

namespace WinFormsApp1.Documents
{
    public class InvoicePdfGenerator
    {
        public byte[] GenerateInvoicePdf(InvoiceModel invoice)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(GetPageSize(invoice.InvoiceFormat));
                    page.Margin(GetPageMargin(invoice.InvoiceFormat));
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(GetBaseFontSize(invoice.InvoiceFormat)));
                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeContent(x, invoice));
                    page.Footer().Element(ComposeFooter);

                    void ComposeHeader(IContainer container)
                    {
                        if (invoice.InvoiceFormat.Contains("Slip"))
                        {
                            ComposeSlipHeader(container, invoice);
                        }
                        else
                        {
                            ComposeStandardHeader(container, invoice);
                        }
                    }

                    void ComposeContent(IContainer container, InvoiceModel invoice)
                    {
                        if (invoice.InvoiceFormat.Contains("Slip"))
                        {
                            ComposeSlipContent(container, invoice);
                        }
                        else
                        {
                            ComposeStandardContent(container, invoice);
                        }
                    }

                    void ComposeFooter(IContainer container)
                    {
                        if (invoice.InvoiceFormat.Contains("Slip"))
                        {
                            ComposeSlipFooter(container, invoice);
                        }
                        else
                        {
                            ComposeStandardFooter(container, invoice);
                        }
                    }
                });
            }).GeneratePdf();
        }

        public byte[] GenerateCombinedInvoicePdf(InvoiceModel invoice)
        {
            return Document.Create(container =>
            {
                // First page - Original
                container.Page(page =>
                {
                    page.Size(GetPageSize(invoice.InvoiceFormat));
                    page.Margin(GetPageMargin(invoice.InvoiceFormat));
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(GetBaseFontSize(invoice.InvoiceFormat)));
                    page.Header().Element(x => ComposeHeaderWithCopyType(x, invoice, "ORIGINAL"));
                    page.Content().Element(x => ComposeContentForCombined(x, invoice));
                    page.Footer().Element(x => ComposeFooterForCombined(x, invoice));
                });

                // Second page - Duplicate
                container.Page(page =>
                {
                    page.Size(GetPageSize(invoice.InvoiceFormat));
                    page.Margin(GetPageMargin(invoice.InvoiceFormat));
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(GetBaseFontSize(invoice.InvoiceFormat)));
                    page.Header().Element(x => ComposeHeaderWithCopyType(x, invoice, "DUPLICATE"));
                    page.Content().Element(x => ComposeContentForCombined(x, invoice));
                    page.Footer().Element(x => ComposeFooterForCombined(x, invoice));
                });

                // Third page - Triplicate
                container.Page(page =>
                {
                    page.Size(GetPageSize(invoice.InvoiceFormat));
                    page.Margin(GetPageMargin(invoice.InvoiceFormat));
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(GetBaseFontSize(invoice.InvoiceFormat)));
                    page.Header().Element(x => ComposeHeaderWithCopyType(x, invoice, "TRIPLICATE"));
                    page.Content().Element(x => ComposeContentForCombined(x, invoice));
                    page.Footer().Element(x => ComposeFooterForCombined(x, invoice));
                });
            }).GeneratePdf();
        }

        private PageSize GetPageSize(string format)
        {
            return format switch
            {
                "Standard A4" => PageSizes.A4,
                "A5 Half Page" => PageSizes.A5,
                "Slip 3 inch" => new PageSize(76, 210), // 3 inch width
                "Slip 4 inch" => new PageSize(101, 210), // 4 inch width
                _ => PageSizes.A4
            };
        }

        private float GetPageMargin(string format)
        {
            return format switch
            {
                "Standard A4" => 20,
                "A5 Half Page" => 15,
                "Slip 3 inch" => 5,
                "Slip 4 inch" => 8,
                _ => 20
            };
        }

        private float GetBaseFontSize(string format)
        {
            return format switch
            {
                "Standard A4" => 10,
                "A5 Half Page" => 9,
                "Slip 3 inch" => 6,
                "Slip 4 inch" => 7,
                _ => 10
            };
        }

        private void ComposeStandardHeader(IContainer container, InvoiceModel invoice)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().Text(invoice.CompanyName).Bold().FontSize(16);
                    col.Item().Text($"GSTIN: {invoice.CompanyGSTIN}").FontSize(8);
                    col.Item().Text($"Phone: {invoice.CompanyPhone}").FontSize(8);
                    col.Item().Text($"Email: {invoice.CompanyEmail}").FontSize(8);
                });

                row.RelativeItem(1).Column(col =>
                {
                    col.Item().AlignRight().Text("TAX INVOICE").Bold().FontSize(14);
                    col.Item().AlignRight().Text($"Invoice No: {invoice.InvoiceNumber}").FontSize(10);
                    col.Item().AlignRight().Text($"Date: {invoice.InvoiceDate:dd-MM-yyyy}").FontSize(10);
                    col.Item().AlignRight().Text($"Copy: {invoice.CopyType}").FontSize(10);
                });
            });
        }

        private void ComposeSlipHeader(IContainer container, InvoiceModel invoice)
        {
            container.Column(col =>
            {
                col.Item().AlignCenter().Text(invoice.CompanyName).Bold().FontSize(12);
                col.Item().AlignCenter().Text($"GSTIN: {invoice.CompanyGSTIN}").FontSize(6);
                col.Item().AlignCenter().Text("TAX INVOICE").Bold().FontSize(10);
                col.Item().AlignCenter().Text($"Invoice: {invoice.InvoiceNumber} / {invoice.InvoiceDate:dd-MM-yyyy}").FontSize(6);
                col.Item().AlignCenter().Text($"Copy: {invoice.CopyType}").FontSize(6);
            });
        }

        private void ComposeStandardContent(IContainer container, InvoiceModel invoice)
        {
            container.Column(col =>
            {
                // Customer Information
                col.Item().PaddingBottom(10).Element(x => ComposeCustomerInfo(x, invoice));

                // Items Table
                col.Item().PaddingBottom(10).Element(x => ComposeItemsTable(x, invoice));

                // Summary
                col.Item().Element(x => ComposeSummary(x, invoice));
            });
        }

        private void ComposeSlipContent(IContainer container, InvoiceModel invoice)
        {
            container.Column(col =>
            {
                // Customer Info (compact)
                col.Item().Text($"Customer: {invoice.CustomerName}").FontSize(6);
                col.Item().Text($"GSTIN: {invoice.CustomerGSTIN}").FontSize(6);

                // Items (compact)
                col.Item().PaddingTop(5).Element(x => ComposeSlipItemsTable(x, invoice));

                // Summary (compact)
                col.Item().PaddingTop(5).Element(x => ComposeSlipSummary(x, invoice));
            });
        }

        private void ComposeCustomerInfo(IContainer container, InvoiceModel invoice)
        {
            container.Border(1).Padding(10).Column(col =>
            {
                col.Item().Text("Bill To:").Bold();
                col.Item().Text(invoice.CustomerName);
                col.Item().Text(invoice.CustomerAddress);
                col.Item().Text($"GSTIN: {invoice.CustomerGSTIN}");
                col.Item().Text($"Mobile: {invoice.CustomerMobile}");
                col.Item().Text($"Place of Supply: {invoice.PlaceOfSupply}");
            });
        }

        private void ComposeItemsTable(IContainer container, InvoiceModel invoice)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.5f); // S.No
                    columns.RelativeColumn(2f);   // Product
                    columns.RelativeColumn(1f);   // HSN
                    columns.RelativeColumn(0.8f); // Qty
                    columns.RelativeColumn(0.8f); // Unit
                    columns.RelativeColumn(1f);   // Price
                    columns.RelativeColumn(1f);   // Amount
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Text("S.No").Bold();
                    header.Cell().Text("Product").Bold();
                    header.Cell().Text("HSN").Bold();
                    header.Cell().Text("Qty").Bold();
                    header.Cell().Text("Unit").Bold();
                    header.Cell().Text("Price").Bold();
                    header.Cell().Text("Amount").Bold();
                });

                // Items
                foreach (var item in invoice.Items)
                {
                    table.Cell().Text(item.SerialNumber);
                    table.Cell().Text(item.ProductName);
                    table.Cell().Text(item.HSNCode);
                    table.Cell().Text(item.Quantity.ToString("N2"));
                    table.Cell().Text(item.Unit);
                    table.Cell().Text(item.UnitPrice.ToString("N2"));
                    table.Cell().Text(item.LineTotal.ToString("N2"));
                }
            });
        }

        private void ComposeSlipItemsTable(IContainer container, InvoiceModel invoice)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.3f); // S.No
                    columns.RelativeColumn(1.5f); // Product
                    columns.RelativeColumn(0.8f); // Qty
                    columns.RelativeColumn(0.8f); // Price
                    columns.RelativeColumn(0.8f); // Amount
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Text("S.No").Bold().FontSize(6);
                    header.Cell().Text("Product").Bold().FontSize(6);
                    header.Cell().Text("Qty").Bold().FontSize(6);
                    header.Cell().Text("Price").Bold().FontSize(6);
                    header.Cell().Text("Amount").Bold().FontSize(6);
                });

                // Items
                foreach (var item in invoice.Items)
                {
                    table.Cell().Text(item.SerialNumber).FontSize(6);
                    table.Cell().Text(item.ProductName).FontSize(6);
                    table.Cell().Text(item.Quantity.ToString("N2")).FontSize(6);
                    table.Cell().Text(item.UnitPrice.ToString("N2")).FontSize(6);
                    table.Cell().Text(item.LineTotal.ToString("N2")).FontSize(6);
                }
            });
        }

        private void ComposeSummary(IContainer container, InvoiceModel invoice)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().Text($"Amount in Words: {invoice.AmountInWords}").Italic();
                    col.Item().Text($"Tax Rate: {invoice.Items.FirstOrDefault()?.TaxRate:N2}%");
                    col.Item().Text($"Taxable Amount: {invoice.SubTotal:N2}");
                });

                row.RelativeItem(1).Column(col =>
                {
                    col.Item().Row(r => { r.RelativeItem().Text("Sub Total:"); r.RelativeItem().Text(invoice.SubTotal.ToString("N2")); });
                    col.Item().Row(r => { r.RelativeItem().Text("Discount:"); r.RelativeItem().Text(invoice.DiscountAmount.ToString("N2")); });
                    col.Item().Row(r => { r.RelativeItem().Text("Tax Amount:"); r.RelativeItem().Text(invoice.TaxAmount.ToString("N2")); });
                    col.Item().Row(r => { r.RelativeItem().Text("Round Off:"); r.RelativeItem().Text(invoice.RoundOff.ToString("N2")); });
                    col.Item().Row(r => { r.RelativeItem().Text("Net Payable:").Bold(); r.RelativeItem().Text(invoice.NetPayable.ToString("N2")).Bold(); });
                });
            });
        }

        private void ComposeSlipSummary(IContainer container, InvoiceModel invoice)
        {
            container.Column(col =>
            {
                col.Item().Row(r => { r.RelativeItem().Text("Sub Total:").FontSize(6); r.RelativeItem().Text(invoice.SubTotal.ToString("N2")).FontSize(6); });
                col.Item().Row(r => { r.RelativeItem().Text("Tax Amount:").FontSize(6); r.RelativeItem().Text(invoice.TaxAmount.ToString("N2")).FontSize(6); });
                col.Item().Row(r => { r.RelativeItem().Text("Net Payable:").Bold().FontSize(6); r.RelativeItem().Text(invoice.NetPayable.ToString("N2")).Bold().FontSize(6); });
                col.Item().Text($"Amount: {invoice.AmountInWords}").FontSize(6).Italic();
            });
        }

        private void ComposeStandardFooter(IContainer container, InvoiceModel invoice)
        {
            container.Row(row =>
            {
                row.RelativeItem(1).Text("Receiver's Signature").Italic();
                row.RelativeItem(1).AlignRight().Text("Authorised Signatory").Italic();
            });
        }

        private void ComposeSlipFooter(IContainer container, InvoiceModel invoice)
        {
            container.Column(col =>
            {
                col.Item().AlignCenter().Text("THANK YOU. VISIT US AGAIN.").Bold().FontSize(8);
                col.Item().Row(r => { r.RelativeItem().Text("Signature").FontSize(6); r.RelativeItem().AlignRight().Text("Authorised").FontSize(6); });
            });
        }

        private void ComposeHeaderWithCopyType(IContainer container, InvoiceModel invoice, string copyType)
        {
            if (invoice.InvoiceFormat.Contains("Slip"))
            {
                ComposeSlipHeaderWithCopyType(container, invoice, copyType);
            }
            else
            {
                ComposeStandardHeaderWithCopyType(container, invoice, copyType);
            }
        }

        private void ComposeStandardHeaderWithCopyType(IContainer container, InvoiceModel invoice, string copyType)
        {
            container.Column(column =>
            {
                column.Item().Element(ComposeCompanyHeader);
                column.Item().Element(ComposeBillingAndInvoiceDetails);
                column.Item().Element(ComposeInvoiceTitle);
                column.Item().Element(ComposePartyAndInvoiceDetails);
                column.Item().Element(x => ComposeCopyTypeIndicator(x, copyType));
            });
        }

        private void ComposeSlipHeaderWithCopyType(IContainer container, InvoiceModel invoice, string copyType)
        {
            container.Column(col =>
            {
                col.Item().AlignCenter().Text(invoice.CompanyName).Bold().FontSize(12);
                col.Item().AlignCenter().Text($"GSTIN: {invoice.CompanyGSTIN}").FontSize(6);
                col.Item().AlignCenter().Text("").FontSize(10); // Empty title
                col.Item().AlignCenter().Text($"Invoice: {invoice.InvoiceNumber} / {invoice.InvoiceDate:dd-MM-yyyy}").FontSize(6);
                col.Item().AlignCenter().Text($"Copy: {copyType}").FontSize(6).Bold().FontColor(Colors.Red.Medium);
            });
        }

        private void ComposeContentForCombined(IContainer container, InvoiceModel invoice)
        {
            if (invoice.InvoiceFormat.Contains("Slip"))
            {
                ComposeSlipContent(container, invoice);
            }
            else
            {
                ComposeStandardContent(container, invoice);
            }
        }

        private void ComposeFooterForCombined(IContainer container, InvoiceModel invoice)
        {
            if (invoice.InvoiceFormat.Contains("Slip"))
            {
                ComposeSlipFooter(container, invoice);
            }
            else
            {
                ComposeStandardFooter(container, invoice);
            }
        }

        private void ComposeCompanyHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Company Logo/Name (Left side) - Empty grid structure
                row.RelativeItem(2).Element(ComposeCompanyDetailsGrid);

                // Company Tax Details (Right side) - Empty grid structure
                row.RelativeItem(1).Element(ComposeCompanyTaxGrid);
            });
        }

        private void ComposeCompanyDetailsGrid(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(5).Column(column =>
            {
                // Header
                column.Item().Text("Company Details").FontSize(11).Bold().Italic();

                // Grid rows
                for (int i = 0; i < 5; i++)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                        row.RelativeItem(2).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                    });
                }
            });
        }

        private void ComposeCompanyTaxGrid(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(5).Column(column =>
            {
                // Header
                column.Item().Text("Company Tax Details").FontSize(11).Bold().Italic();

                // Grid rows
                for (int i = 0; i < 5; i++)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                        row.RelativeItem(2).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                    });
                }
            });
        }

        private void ComposeInvoiceTitle(IContainer container)
        {
            container.Row(row =>
            {
                row.ConstantItem(0).Text(""); // Spacer
                row.RelativeItem(1).AlignCenter().Text("").FontSize(16).Bold().Underline(); // Empty title
                row.ConstantItem(0).Text(""); // Spacer
            });
        }

        private void ComposePartyAndInvoiceDetails(IContainer container)
        {
            container.Row(row =>
            {
                // Party Details (Left side) - Empty grid structure
                row.RelativeItem(2).Element(ComposePartyDetailsGrid);

                // Invoice Details (Right side) - Empty grid structure
                row.RelativeItem(1).Element(ComposeInvoiceDetailsGrid);
            });
        }

        private void ComposeBillingAndInvoiceDetails(IContainer container)
        {
            container.Row(row =>
            {
                // Left side - Party Details Grid
                row.RelativeItem(1).Element(ComposePartyDetailsGrid);

                // Spacer between columns
                row.ConstantItem(50);

                // Right side - Invoice Details Grid
                row.RelativeItem(1).Element(ComposeInvoiceDetailsGrid);
            });
        }

        private void ComposePartyDetailsGrid(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(5).Column(column =>
            {
                // Header
                column.Item().Text("Party Details").FontSize(11).Bold().Italic();

                // Grid rows
                for (int i = 0; i < 6; i++)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                        row.RelativeItem(2).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                    });
                }
            });
        }

        private void ComposeInvoiceDetailsGrid(IContainer container)
        {
            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(5).Column(column =>
            {
                // Header
                column.Item().Text("Invoice Details").FontSize(11).Bold().Italic();

                // Grid rows
                for (int i = 0; i < 7; i++)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem(1).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                        row.RelativeItem(2).Border(1).BorderColor(Colors.Grey.Medium).Padding(3).Text("").FontSize(10);
                    });
                }
            });
        }

        private void ComposeCopyTypeIndicator(IContainer container, string copyType)
        {
            container.Row(row =>
            {
                row.RelativeItem(1).Text(""); // Spacer
                row.RelativeItem(1).AlignRight().Text(copyType).FontSize(12).Bold().Italic().FontColor(Colors.Red.Medium);
            });
        }
    }
}