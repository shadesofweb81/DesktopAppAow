using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WinFormsApp1.Documents;

namespace WinFormsApp1.Documents.ExportPdf
{
    public class InvoiceDocument : IDocument
    {
        private readonly InvoiceModel _invoiceModel;
        private readonly string _copyType;

        public InvoiceDocument(InvoiceModel invoiceModel, string copyType = "Original")
        {
            _invoiceModel = invoiceModel;
            _copyType = copyType;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        public void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                // Company Information
                column.Item().Text(_invoiceModel.CompanyName)
                    .FontSize(18).Bold().FontColor(Colors.Blue.Medium);

                column.Item().Text($"GSTIN: {_invoiceModel.CompanyGSTIN}")
                    .FontSize(10).FontColor(Colors.Grey.Medium);

                column.Item().Text($"Phone: {_invoiceModel.CompanyPhone}")
                    .FontSize(10).FontColor(Colors.Grey.Medium);

                column.Item().Text($"Email: {_invoiceModel.CompanyEmail}")
                    .FontSize(10).FontColor(Colors.Grey.Medium);

                // Invoice Title
                column.Item().PaddingTop(20).AlignCenter().Text("INVOICE")
                    .FontSize(24).Bold().FontColor(Colors.Blue.Medium);

                column.Item().AlignCenter().Text($"Copy: {_copyType}")
                    .FontSize(12).Bold().FontColor(Colors.Grey.Medium);

                // Simple line separator
                column.Item().PaddingTop(20).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });
        }

        public void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                // Party Details and Invoice Details Row
                column.Item().PaddingTop(30).Row(row =>
                {
                    // Left Column - Party Details
                    row.RelativeItem(2).Column(partyColumn =>
                    {
                        partyColumn.Item().Text("Bill To:")
                            .FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                        
                        partyColumn.Item().Text(_invoiceModel.CustomerName)
                            .FontSize(11).FontColor(Colors.Black);
                        
                        partyColumn.Item().Text(_invoiceModel.CustomerAddress)
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        
                        partyColumn.Item().Text($"GSTIN: {_invoiceModel.CustomerGSTIN}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        
                        partyColumn.Item().Text($"Mobile: {_invoiceModel.CustomerMobile}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                    // Spacer column
                    row.ConstantItem(50);

                    // Right Column - Invoice Details
                    row.RelativeItem(1).Column(invoiceColumn =>
                    {
                        invoiceColumn.Item().Text("Invoice Details:")
                            .FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                        
                        invoiceColumn.Item().Text($"Invoice #: {_invoiceModel.InvoiceNumber}")
                            .FontSize(10).FontColor(Colors.Black);
                        
                        invoiceColumn.Item().Text($"Date: {_invoiceModel.InvoiceDate:dd/MM/yyyy}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        
                        invoiceColumn.Item().Text($"Type: {_invoiceModel.TransactionType}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        
                        invoiceColumn.Item().Text($"Reference: {_invoiceModel.ReferenceNumber}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                        
                        invoiceColumn.Item().Text($"Place: {_invoiceModel.PlaceOfSupply}")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                    });
                });

                // Separator line
                column.Item().PaddingTop(20).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                // Items Table
                column.Item().PaddingTop(30).Element(ComposeTable);

                // Totals Section
                column.Item().PaddingTop(30).Element(ComposeTotals);
            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);  // S.No
                    columns.RelativeColumn(3);   // Description
                    columns.RelativeColumn();    // HSN Code
                    columns.RelativeColumn();    // Qty
                    columns.RelativeColumn();    // Unit
                    columns.RelativeColumn();    // Rate
                    columns.RelativeColumn();    // Amount
                });
                
                table.Header(header =>
                {
                    header.Cell().Element(CellHeaderStyle).Text("S.No");
                    header.Cell().Element(CellHeaderStyle).Text("Description");
                    header.Cell().Element(CellHeaderStyle).Text("HSN Code");
                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("Qty");
                    header.Cell().Element(CellHeaderStyle).AlignCenter().Text("Unit");
                    header.Cell().Element(CellHeaderStyle).AlignRight().Text("Rate");
                    header.Cell().Element(CellHeaderStyle).AlignRight().Text("Amount");
                    
                    static IContainer CellHeaderStyle(IContainer container)
                    {
                        return container
                            .DefaultTextStyle(x => x.SemiBold())
                            .PaddingVertical(8)
                            .PaddingHorizontal(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black)
                            .Background(Colors.Grey.Lighten3);
                    }
                });
                
                foreach (var item in _invoiceModel.Items)
                {
                    table.Cell().Element(CellStyle).Text(item.SerialNumber);
                    table.Cell().Element(CellStyle).Text(item.ProductName);
                    table.Cell().Element(CellStyle).Text(item.HSNCode);
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString("N2"));
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Unit);
                    table.Cell().Element(CellStyle).AlignRight().Text(item.UnitPrice.ToString("N2"));
                    table.Cell().Element(CellStyle).AlignRight().Text(item.LineTotal.ToString("N2"));
                    
                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingVertical(6)
                            .PaddingHorizontal(5);
                    }
                }
            });
        }

        private void ComposeTotals(IContainer container)
        {
            container.AlignRight().Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Sub Total:").FontSize(10).Bold();
                    row.ConstantItem(80).Text(_invoiceModel.SubTotal.ToString("N2")).FontSize(10);
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Discount:").FontSize(10);
                    row.ConstantItem(80).Text(_invoiceModel.DiscountAmount.ToString("N2")).FontSize(10);
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Tax Amount:").FontSize(10);
                    row.ConstantItem(80).Text(_invoiceModel.TaxAmount.ToString("N2")).FontSize(10);
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Round Off:").FontSize(10);
                    row.ConstantItem(80).Text(_invoiceModel.RoundOff.ToString("N2")).FontSize(10);
                });

                // Separator line
                column.Item().PaddingTop(5).PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Total Amount:").FontSize(12).Bold();
                    row.ConstantItem(80).Text(_invoiceModel.NetPayable.ToString("N2")).FontSize(12).Bold();
                });

                column.Item().PaddingTop(5).Text($"Amount in Words: {_invoiceModel.AmountInWords}")
                    .FontSize(9).FontColor(Colors.Grey.Medium);
            });
        }

        public void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(8).FontColor(Colors.Grey.Medium);
                
                column.Item().Text($"Copy: {_copyType}")
                    .FontSize(8).FontColor(Colors.Grey.Medium);
            });
        }
    }
}
