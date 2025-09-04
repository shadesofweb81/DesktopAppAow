using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


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
                // TAX INVOICE Title
                column.Item().PaddingTop(0).AlignCenter().Text("TAX INVOICE")
                    .FontSize(13).Bold().FontColor(Colors.Blue.Medium);

                // Page Name (Copy Type)
                column.Item().PaddingTop(0).AlignCenter().Text($"{_copyType}")
                    .FontSize(9).Bold().FontColor(Colors.Red.Medium);
                // Company Details Section
                //column.Item().AlignCenter().Text(_invoiceModel.CompanyName)
                //    .FontSize(18).Bold().FontColor(Colors.Blue.Medium);

                //column.Item().PaddingTop(5).AlignCenter().Text($"GSTIN: {_invoiceModel.CompanyGSTIN}")
                //    .FontSize(11).FontColor(Colors.Grey.Medium);

                //column.Item().AlignCenter().Text($"Phone: {_invoiceModel.CompanyPhone}")
                //    .FontSize(10).FontColor(Colors.Grey.Medium);

                //column.Item().AlignCenter().Text($"Email: {_invoiceModel.CompanyEmail}")
                //    .FontSize(10).FontColor(Colors.Grey.Medium);

                // Separator line
                column.Item().PaddingTop(1).PaddingBottom(1).LineHorizontal(1).LineColor(Colors.Grey.Medium);

            
            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(1).Column(column =>
            {
                column.Spacing(1);
                column.Item().Text(_invoiceModel.CompanyName).FontSize(10);
                column.Item().Text($"Address: {_invoiceModel.CustomerAddress}").FontSize(10);
                column.Item().Text($"GSTIN: {_invoiceModel.CompanyGSTIN}").FontSize(10);
            });
        }

        public void ComposeContent(IContainer container)
        {
          
            container.Column(column =>
            {
                column.Item().Element(ComposeComments);
                // Party Details and Invoice Details Row
                column.Item().PaddingTop(20).Row(row =>
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
                column.Item().PaddingTop(15).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                // Items Table
                column.Item().PaddingTop(0).Element(ComposeTable);

                // Totals Section
                column.Item().PaddingTop(20).Element(ComposeTotals);

                // Lower Section - Terms, Bank Details, and Signature
                column.Item().PaddingTop(25).Element(ComposeLowerSection);
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
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });
                
                foreach (var item in _invoiceModel.Items)
                {
                    table.Cell().Element(CellStyle).Text(item.SerialNumber.ToString());
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
                            .PaddingVertical(4)
                            .PaddingHorizontal(4);
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
                column.Item().PaddingTop(3).PaddingBottom(3).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                column.Item().Row(row =>
                {
                    row.ConstantItem(100).Text("Total Amount:").FontSize(12).Bold();
                    row.ConstantItem(80).Text(_invoiceModel.NetPayable.ToString("N2")).FontSize(12).Bold();
                });

                column.Item().PaddingTop(5).Text($"Amount in Words: {_invoiceModel.AmountInWords}")
                    .FontSize(9).FontColor(Colors.Grey.Medium);
            });
        }

        private void ComposeLowerSection(IContainer container)
        {
            container.Row(row =>
            {
                // Left Column - Terms and Conditions
                row.RelativeItem(2).Column(termsColumn =>
                {
                    termsColumn.Item().Text("Terms & Conditions:")
                        .FontSize(11).Bold().FontColor(Colors.Blue.Medium);
                    
                    termsColumn.Item().PaddingTop(3).Text("1. Payment is due within 30 days of invoice date")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    termsColumn.Item().Text("2. Late payments may incur additional charges")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    termsColumn.Item().Text("3. Goods once sold will not be taken back")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    termsColumn.Item().Text("4. All disputes are subject to local jurisdiction")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    termsColumn.Item().Text("5. This is a computer generated invoice")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                });

                // Spacer column
                row.ConstantItem(30);

                // Right Column - Bank Details and Signature
                row.RelativeItem(1).Column(rightColumn =>
                {
                    // Bank Details
                    rightColumn.Item().Text("Bank Details:")
                        .FontSize(11).Bold().FontColor(Colors.Blue.Medium);
                    
                    rightColumn.Item().PaddingTop(3).Text("Bank Name: Sample Bank Ltd.")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    rightColumn.Item().Text("Account No: 1234567890")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    rightColumn.Item().Text("IFSC Code: SAMPLE000123")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                    
                    rightColumn.Item().Text("Branch: Main Branch")
                        .FontSize(9).FontColor(Colors.Grey.Medium);

                    // Signature Section
                    rightColumn.Item().PaddingTop(15).Text("Authorized Signature:")
                        .FontSize(11).Bold().FontColor(Colors.Blue.Medium);
                    
                    rightColumn.Item().PaddingTop(10).LineHorizontal(80).LineColor(Colors.White);
                });
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
