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

                // Placeholder content
                column.Item().PaddingTop(30).Text("Invoice Content Coming Soon")
                    .FontSize(16).FontColor(Colors.Grey.Medium);
                
                column.Item().PaddingTop(20).Text("This is a test invoice")
                    .FontSize(12).FontColor(Colors.Grey.Medium);
                
                column.Item().PaddingTop(10).Text("Copy Type: " + _copyType)
                    .FontSize(10).FontColor(Colors.Grey.Medium);
            });
        }

        // Placeholder methods - will be implemented later
        // private void ComposeItemsTable(IContainer container) { }
        // private void ComposeTableHeader(IContainer container) { }
        // private void ComposeTableRow(IContainer container, InvoiceItemModel item) { }

        // Placeholder method - will be implemented later
        // private void ComposeTotals(IContainer container) { }

        // Placeholder method - will be implemented later
        // private void ComposeNotes(IContainer container) { }

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
