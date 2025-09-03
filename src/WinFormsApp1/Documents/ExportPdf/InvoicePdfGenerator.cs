using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.Common;
using WinFormsApp1.Documents;
using WinFormsApp1.Forms.Transaction;

namespace WinFormsApp1.Documents.ExportPdf
{
    public class InvoicePdfGenerator
    {
        public void SaveInvoicePdf(InvoiceModel invoiceModel, string filePath, CopyType copyType = CopyType.Original, InvoiceFormat format = InvoiceFormat.StandardA4)
        {
            try
            {
                // Create the invoice document based on the selected format
                IDocument document = format switch
                {
                    InvoiceFormat.StandardA4 => CreateStandardA4Invoice(invoiceModel, copyType),
                    InvoiceFormat.A5HalfPage => CreateA5HalfPageInvoice(invoiceModel, copyType),
                    InvoiceFormat.Slip3Inch => CreateSlipInvoice(invoiceModel, copyType, 76),
                    InvoiceFormat.Slip4Inch => CreateSlipInvoice(invoiceModel, copyType, 101),
                    _ => CreateStandardA4Invoice(invoiceModel, copyType)
                };

                // Save the PDF
                document.GeneratePdf(filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate invoice PDF: {ex.Message}", ex);
            }
        }

        public void SaveInvoicePdfWithMultipleCopies(InvoiceModel invoiceModel, string filePath, CopyType copyType, int numberOfCopies, InvoiceFormat format = InvoiceFormat.StandardA4)
        {
            try
            {
                // Create a composite document with multiple pages
                var document = Document.Create(container =>
                {
                    for (int i = 0; i < numberOfCopies; i++)
                    {
                        var currentCopyType = GetCopyTypeForPage(copyType, i);
                        var currentInvoiceModel = CloneInvoiceModel(invoiceModel, currentCopyType);
                        
                        container.Page(page =>
                        {
                            page.Size(GetPageSize(format));
                            page.Margin(20);
                            page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                            // For Standard A4, use InvoiceDocument methods
                            if (format == InvoiceFormat.StandardA4)
                            {
                                var invoiceDoc = new InvoiceDocument(currentInvoiceModel, currentCopyType);
                                page.Header().Element(invoiceDoc.ComposeHeader);
                                page.Content().Element(invoiceDoc.ComposeContent);
                                page.Footer().Element(invoiceDoc.ComposeFooter);
                            }
                            else
                            {
                                // For other formats, use compact methods
                                page.Header().Element(container => ComposeHeader(container, currentInvoiceModel, copyType));
                                page.Content().Element(container => ComposeContent(container, currentInvoiceModel));
                                page.Footer().Element(ComposeFooter);
                            }
                        });
                    }
                });

                // Save the PDF only once
                document.GeneratePdf(filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate invoice PDF with multiple copies: {ex.Message}", ex);
            }
        }

        private IDocument CreateStandardA4Invoice(InvoiceModel invoiceModel, CopyType copyType)
        {
            return new InvoiceDocument(invoiceModel, copyType.ToString());
        }

        private IDocument CreateA5HalfPageInvoice(InvoiceModel invoiceModel, CopyType copyType)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(15);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));

                    page.Header().Element(container => ComposeCompactHeader(container, invoiceModel, copyType));
                    page.Content().Element(container => ComposeCompactContent(container, invoiceModel));
                    page.Footer().Element(ComposeCompactFooter);
                });
            });
        }

        // Replace usages of MmToPoints(76) and MmToPoints(210) with the correct calculation.
        // Since Unit is an enum, you should convert millimeters to points manually.
        // 1 inch = 25.4 mm, 1 inch = 72 points, so 1 mm = 72 / 25.4 points ≈ 2.83465 points.

        private float MmToPoints(float mm)
        {
            return mm * 72f / 25.4f;
        }

        private IDocument CreateSlipInvoice(InvoiceModel invoiceModel, CopyType copyType, int widthMm)
        {
            // For slip formats, use A4 height but custom width
            var pageSize = widthMm switch
            {
                76 => new PageSize(MmToPoints(76), MmToPoints(210)), // 3 inch width
                101 => new PageSize(MmToPoints(101), MmToPoints(210)), // 4 inch width
                _ => PageSizes.A4
            };

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(pageSize);
                    page.Margin(10);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(7));

                    page.Header().Element(container => ComposeSlipHeader(container, invoiceModel, copyType));
                    page.Content().Element(container => ComposeSlipContent(container, invoiceModel));
                    page.Footer().Element(ComposeSlipFooter);
                });
            });
        }

        private PageSize GetPageSize(InvoiceFormat format)
        {
            return format switch
            {
                InvoiceFormat.StandardA4 => PageSizes.A4,
                InvoiceFormat.A5HalfPage => PageSizes.A5,
                InvoiceFormat.Slip3Inch => new PageSize(MmToPoints(76), MmToPoints(210)),
                InvoiceFormat.Slip4Inch => new PageSize(MmToPoints(101), MmToPoints(210)),
                _ => PageSizes.A4
            };
        }

   

        private string GetCopyTypeForPage(CopyType copyType, int pageIndex)
        {
            return copyType switch
            {
                CopyType.All => pageIndex switch
                {
                    0 => "Original",
                    1 => "Duplicate",
                    2 => "Triplicate",
                    _ => $"Copy {pageIndex + 1}"
                },
                CopyType.OriginalDuplicate => pageIndex switch
                {
                    0 => "Original",
                    1 => "Duplicate",
                    _ => $"Copy {pageIndex + 1}"
                },
                _ => copyType.ToString()
            };
        }

        private InvoiceModel CloneInvoiceModel(InvoiceModel original, string copyType)
        {
            return new InvoiceModel
            {
                InvoiceNumber = original.InvoiceNumber,
                InvoiceDate = original.InvoiceDate,
                CustomerName = original.CustomerName,
                CustomerAddress = original.CustomerAddress,
                CustomerGSTIN = original.CustomerGSTIN,
                CustomerMobile = original.CustomerMobile,
                CompanyName = original.CompanyName,
                CompanyGSTIN = original.CompanyGSTIN,
                CompanyPhone = original.CompanyPhone,
                CompanyEmail = original.CompanyEmail,
                PlaceOfSupply = original.PlaceOfSupply,
                Items = original.Items,
                SubTotal = original.SubTotal,
                DiscountAmount = original.DiscountAmount,
                TaxAmount = original.TaxAmount,
                TotalAmount = original.TotalAmount,
                RoundOff = original.RoundOff,
                NetPayable = original.NetPayable,
                AmountInWords = original.AmountInWords,
                CopyType = copyType,
                InvoiceFormat = original.InvoiceFormat,
                TransactionType = original.TransactionType,
                ReferenceNumber = original.ReferenceNumber,
                Notes = original.Notes
            };
        }

        // Compact layout methods for A5 and slip formats
        private void ComposeCompactHeader(IContainer container, InvoiceModel invoiceModel, CopyType copyType)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(column =>
                {
                    column.Item().Text(invoiceModel.CompanyName).FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().Text($"GSTIN: {invoiceModel.CompanyGSTIN}").FontSize(7).FontColor(Colors.Grey.Medium);
                });

                row.RelativeItem(1).Column(column =>
                {
                    column.Item().AlignRight().Text("INVOICE").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().AlignRight().Text($"#{invoiceModel.InvoiceNumber}").FontSize(10).Bold();
                    column.Item().AlignRight().Text($"Copy: {copyType}").FontSize(7).FontColor(Colors.Grey.Medium);
                });
            });
        }

        private void ComposeCompactContent(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                // Customer info
                column.Item().Text($"Bill To: {invoiceModel.CustomerName}").FontSize(8);
                
                // Items (simplified)
                column.Item().Element(container => ComposeCompactItems(container, invoiceModel));
                
                // Totals
                column.Item().Element(container => ComposeCompactTotals(container, invoiceModel));
            });
        }

        private void ComposeCompactItems(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                // Header
                column.Item().Background(Colors.Grey.Lighten3).Padding(3).Row(row =>
                {
                    row.RelativeItem(3).Text("Item").FontSize(7).Bold();
                    row.ConstantItem(40).Text("Qty").FontSize(7).Bold();
                    row.ConstantItem(50).Text("Rate").FontSize(7).Bold();
                    row.ConstantItem(50).Text("Amount").FontSize(7).Bold();
                });

                // Items
                foreach (var item in invoiceModel.Items)
                {
                    column.Item().Padding(2).Row(row =>
                    {
                        row.RelativeItem(3).Text(item.ProductName).FontSize(7);
                        row.ConstantItem(40).Text(item.Quantity.ToString("N2")).FontSize(7);
                        row.ConstantItem(50).Text(item.UnitPrice.ToString("N2")).FontSize(7);
                        row.ConstantItem(50).Text(item.LineTotal.ToString("N2")).FontSize(7);
                    });
                }
            });
        }

        private void ComposeCompactTotals(IContainer container, InvoiceModel invoiceModel)
        {
            container.AlignRight().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.ConstantItem(60).Text("Total:").FontSize(8).Bold();
                    row.ConstantItem(50).Text(invoiceModel.NetPayable.ToString("N2")).FontSize(8).Bold();
                });
            });
        }

        private void ComposeCompactFooter(IContainer container)
        {
            container.Text($"Generated: {DateTime.Now:dd/MM/yyyy}").FontSize(6).FontColor(Colors.Grey.Medium);
        }

        // Slip layout methods
        private void ComposeSlipHeader(IContainer container, InvoiceModel invoiceModel, CopyType copyType)
        {
            container.Column(column =>
            {
                column.Item().Text(invoiceModel.CompanyName).FontSize(8).Bold().AlignCenter();
                column.Item().Text($"INVOICE #{invoiceModel.InvoiceNumber}").FontSize(7).Bold().AlignCenter();
                column.Item().Text($"Copy: {copyType}").FontSize(6).FontColor(Colors.Grey.Medium).AlignCenter();
            });
        }

        private void ComposeSlipContent(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                column.Item().Text($"Customer: {invoiceModel.CustomerName}").FontSize(6);
                column.Item().Element(container => ComposeSlipItems(container, invoiceModel));
                column.Item().Text($"Total: {invoiceModel.NetPayable:N2}").FontSize(7).Bold().AlignRight();
            });
        }

        private void ComposeSlipItems(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                foreach (var item in invoiceModel.Items.Take(3)) // Limit items for slip
                {
                    column.Item().Text($"{item.ProductName} x{item.Quantity} @{item.UnitPrice:N2}").FontSize(6);
                }
            });
        }

        private void ComposeSlipFooter(IContainer container)
        {
            container.Text(DateTime.Now.ToString("dd/MM/yyyy")).FontSize(5).FontColor(Colors.Grey.Medium).AlignCenter();
        }

        // Standard layout methods (reused from InvoiceDocument)
        private void ComposeHeader(IContainer container, InvoiceModel invoiceModel, CopyType copyType)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(column =>
                {
                    column.Item().Text(invoiceModel.CompanyName).FontSize(16).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().Text($"GSTIN: {invoiceModel.CompanyGSTIN}").FontSize(9).FontColor(Colors.Grey.Medium);
                    column.Item().Text($"Phone: {invoiceModel.CompanyPhone}").FontSize(9).FontColor(Colors.Grey.Medium);
                    column.Item().Text($"Email: {invoiceModel.CompanyEmail}").FontSize(9).FontColor(Colors.Grey.Medium);
                });

                row.RelativeItem(1).Column(column =>
                {
                    column.Item().AlignRight().Text("INVOICE").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().AlignRight().Text($"#{invoiceModel.InvoiceNumber}").FontSize(14).Bold();
                    column.Item().AlignRight().Text($"Date: {invoiceModel.InvoiceDate:dd/MM/yyyy}").FontSize(9);
                    column.Item().AlignRight().Text($"Copy: {copyType}").FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });
        }

        private void ComposeContent(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                column.Item().Element(container => ComposeCustomerInfo(container, invoiceModel));
                column.Item().Element(container => ComposeItemsTable(container, invoiceModel));
                column.Item().Element(container => ComposeTotals(container, invoiceModel));
            });
        }

        private void ComposeCustomerInfo(IContainer container, InvoiceModel invoiceModel)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(column =>
                {
                    column.Item().Text("Bill To:").FontSize(11).Bold();
                    column.Item().Text(invoiceModel.CustomerName).FontSize(11);
                    column.Item().Text(invoiceModel.CustomerAddress).FontSize(9).FontColor(Colors.Grey.Medium);
                });

                row.RelativeItem(1).Column(column =>
                {
                    column.Item().Text("Invoice Details:").FontSize(11).Bold();
                    column.Item().Text($"Type: {invoiceModel.TransactionType}").FontSize(9);
                    column.Item().Text($"Reference: {invoiceModel.ReferenceNumber}").FontSize(9);
                });
            });
        }

        private void ComposeItemsTable(IContainer container, InvoiceModel invoiceModel)
        {
            container.Column(column =>
            {
                column.Item().Element(container => ComposeTableHeader(container));
                
                foreach (var item in invoiceModel.Items)
                {
                    column.Item().Element(container => ComposeTableRow(container, item));
                }
            });
        }

        private void ComposeTableHeader(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(5).Row(row =>
            {
                row.ConstantItem(40).Text("S.No").FontSize(9).Bold();
                row.RelativeItem(3).Text("Description").FontSize(9).Bold();
                row.ConstantItem(60).Text("HSN Code").FontSize(9).Bold();
                row.ConstantItem(50).Text("Qty").FontSize(9).Bold();
                row.ConstantItem(40).Text("Unit").FontSize(9).Bold();
                row.ConstantItem(70).Text("Rate").FontSize(9).Bold();
                row.ConstantItem(60).Text("Discount").FontSize(9).Bold();
                row.ConstantItem(70).Text("Amount").FontSize(9).Bold();
            });
        }

        private void ComposeTableRow(IContainer container, InvoiceItemModel item)
        {
            container.Padding(5).Row(row =>
            {
                row.ConstantItem(40).Text(item.SerialNumber.ToString()).FontSize(9);
                row.RelativeItem(3).Text(item.ProductName).FontSize(9);
                row.ConstantItem(60).Text(item.HSNCode).FontSize(9);
                row.ConstantItem(50).Text(item.Quantity.ToString("N2")).FontSize(9);
                row.ConstantItem(40).Text(item.Unit).FontSize(9);
                row.ConstantItem(70).Text(item.UnitPrice.ToString("N2")).FontSize(9);
                row.ConstantItem(60).Text(item.DiscountAmount.ToString("N2")).FontSize(9);
                row.ConstantItem(70).Text(item.LineTotal.ToString("N2")).FontSize(9);
            });
        }

        private void ComposeTotals(IContainer container, InvoiceModel invoiceModel)
        {
            container.AlignRight().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Sub Total:").FontSize(10).Bold();
                    row.ConstantItem(80).Text(invoiceModel.SubTotal.ToString("N2")).FontSize(10);
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Tax Amount:").FontSize(10);
                    row.ConstantItem(80).Text(invoiceModel.TaxAmount.ToString("N2")).FontSize(10);
                });

                column.Item().Element(container =>
                    container.PaddingTop(5).PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Medium)
                );

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Total Amount:").FontSize(12).Bold();
                    row.ConstantItem(80).Text(invoiceModel.NetPayable.ToString("N2")).FontSize(12).Bold();
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).FontColor(Colors.Grey.Medium);
                row.RelativeItem().AlignRight().Text($"Page ").FontSize(8).FontColor(Colors.Grey.Medium);
                //row.ConstantItem(20).Text(x => x.CurrentPageNumber()).FontSize(8).FontColor(Colors.Grey.Medium);
                row.ConstantItem(10).Text(" of ").FontSize(8).FontColor(Colors.Grey.Medium);
                // Replace this line:
                // column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium).PaddingTop(5).PaddingBottom(5);

                // With the following block:
                //column.Item().Element(container =>
                //{
                //    container.PaddingTop(5).PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                //});
                //row.ConstantItem(20).Text(x => x.TotalPages()).FontSize(8).FontColor(Colors.Grey.Medium);
            });
        }
    }
}
