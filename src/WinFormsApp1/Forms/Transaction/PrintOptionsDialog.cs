using System.ComponentModel;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class PrintOptionsDialog : Form
    {
        // Public properties to access selected options
        public CopyType SelectedCopyType { get; private set; }
        public int NumberOfCopies { get; private set; }
        public InvoiceFormat SelectedInvoiceFormat { get; private set; }

        // Controls
        private GroupBox grpCopyType = null!;
        private RadioButton rbAll = null!;
        private RadioButton rbOriginal = null!;
        private RadioButton rbDuplicate = null!;
        private RadioButton rbTriplicate = null!;
        private RadioButton rbOriginalDuplicate = null!;

        private GroupBox grpNumberOfCopies = null!;
        private NumericUpDown numCopies = null!;
        private Label lblCopiesInfo = null!;

        private GroupBox grpInvoiceFormat = null!;
        private RadioButton rbStandardA4 = null!;
        private RadioButton rbA5HalfPage = null!;
        private RadioButton rbSlip3Inch = null!;
        private RadioButton rbSlip4Inch = null!;

        private Button btnPrint = null!;
        private Button btnCancel = null!;
        private Button btnPreview = null!;

        public PrintOptionsDialog()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            grpCopyType = new GroupBox();
            rbAll = new RadioButton();
            rbOriginal = new RadioButton();
            rbDuplicate = new RadioButton();
            rbTriplicate = new RadioButton();
            rbOriginalDuplicate = new RadioButton();

            grpNumberOfCopies = new GroupBox();
            numCopies = new NumericUpDown();
            lblCopiesInfo = new Label();

            grpInvoiceFormat = new GroupBox();
            rbStandardA4 = new RadioButton();
            rbA5HalfPage = new RadioButton();
            rbSlip3Inch = new RadioButton();
            rbSlip4Inch = new RadioButton();

            btnPrint = new Button();
            btnCancel = new Button();
            btnPreview = new Button();

            ((ISupportInitialize)numCopies).BeginInit();
            SuspendLayout();

            // Form properties
            Text = "Print Options";
            Size = new Size(650, 600); // Professional size
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            KeyPreview = true;
            BackColor = Color.FromArgb(240, 240, 245); // Professional light gray background

            SetupLayout();

            ((ISupportInitialize)numCopies).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            var yPosition = 25; // Top margin
            var leftMargin = 25; // Left margin
            var groupBoxWidth = 580; // Group box width
            var groupBoxHeight = 140; // Group box height

            // Copy Type Group Box
            grpCopyType.Text = "Copy Type";
            grpCopyType.Location = new Point(leftMargin, yPosition);
            grpCopyType.Size = new Size(groupBoxWidth, groupBoxHeight);
            grpCopyType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCopyType.ForeColor = Color.FromArgb(64, 64, 64);
            grpCopyType.BackColor = Color.White;

            // Copy Type Radio Buttons
            rbAll.Location = new Point(25, 30);
            rbAll.Size = new Size(250, 24);
            rbAll.Text = "All (Original + Duplicate + Triplicate)";
            rbAll.Checked = true;
            rbAll.Tag = CopyType.All;
            rbAll.CheckedChanged += CopyType_CheckedChanged;
            rbAll.Font = new Font("Segoe UI", 9F);
            rbAll.ForeColor = Color.FromArgb(64, 64, 64);

            rbOriginal.Location = new Point(25, 60);
            rbOriginal.Size = new Size(250, 24);
            rbOriginal.Text = "Original Only";
            rbOriginal.Tag = CopyType.Original;
            rbOriginal.Font = new Font("Segoe UI", 9F);
            rbOriginal.ForeColor = Color.FromArgb(64, 64, 64);

            rbDuplicate.Location = new Point(25, 90);
            rbDuplicate.Size = new Size(250, 24);
            rbDuplicate.Text = "Duplicate Only";
            rbDuplicate.Tag = CopyType.Duplicate;
            rbDuplicate.Font = new Font("Segoe UI", 9F);
            rbDuplicate.ForeColor = Color.FromArgb(64, 64, 64);

            rbTriplicate.Location = new Point(300, 30);
            rbTriplicate.Size = new Size(250, 24);
            rbTriplicate.Text = "Triplicate Only";
            rbTriplicate.Tag = CopyType.Triplicate;
            rbTriplicate.Font = new Font("Segoe UI", 9F);
            rbTriplicate.ForeColor = Color.FromArgb(64, 64, 64);

            rbOriginalDuplicate.Location = new Point(300, 60);
            rbOriginalDuplicate.Size = new Size(250, 24);
            rbOriginalDuplicate.Text = "Original + Duplicate";
            rbOriginalDuplicate.Tag = CopyType.OriginalDuplicate;
            rbOriginalDuplicate.Font = new Font("Segoe UI", 9F);
            rbOriginalDuplicate.ForeColor = Color.FromArgb(64, 64, 64);

            grpCopyType.Controls.AddRange(new Control[] { rbAll, rbOriginal, rbDuplicate, rbTriplicate, rbOriginalDuplicate });

            yPosition += groupBoxHeight + 25;

            // Number of Copies Group Box
            grpNumberOfCopies.Text = "Number of Copies";
            grpNumberOfCopies.Location = new Point(leftMargin, yPosition);
            grpNumberOfCopies.Size = new Size(groupBoxWidth, 100);
            grpNumberOfCopies.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpNumberOfCopies.ForeColor = Color.FromArgb(64, 64, 64);
            grpNumberOfCopies.BackColor = Color.White;

            var lblCopies = new Label
            {
                Text = "Copies:",
                Location = new Point(25, 30),
                Size = new Size(80, 24),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                TextAlign = ContentAlignment.MiddleLeft
            };

            numCopies.Location = new Point(110, 28);
            numCopies.Size = new Size(100, 28);
            numCopies.Minimum = 1;
            numCopies.Maximum = 10;
            numCopies.Value = 1;
            numCopies.Font = new Font("Segoe UI", 9F);
            numCopies.ValueChanged += NumCopies_ValueChanged;
            numCopies.BorderStyle = BorderStyle.FixedSingle;

            lblCopiesInfo.Location = new Point(25, 65);
            lblCopiesInfo.Size = new Size(530, 24);
            lblCopiesInfo.Text = "Total pages to print: 3 (Original + Duplicate + Triplicate)";
            lblCopiesInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblCopiesInfo.ForeColor = Color.FromArgb(0, 102, 204);

            grpNumberOfCopies.Controls.AddRange(new Control[] { lblCopies, numCopies, lblCopiesInfo });

            yPosition += 125;

            // Invoice Format Group Box
            grpInvoiceFormat.Text = "Invoice Format";
            grpInvoiceFormat.Location = new Point(leftMargin, yPosition);
            grpInvoiceFormat.Size = new Size(groupBoxWidth, groupBoxHeight);
            grpInvoiceFormat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpInvoiceFormat.ForeColor = Color.FromArgb(64, 64, 64);
            grpInvoiceFormat.BackColor = Color.White;

            rbStandardA4.Location = new Point(25, 30);
            rbStandardA4.Size = new Size(250, 24);
            rbStandardA4.Text = "Standard A4 (210 x 297 mm)";
            rbStandardA4.Checked = true;
            rbStandardA4.Tag = InvoiceFormat.StandardA4;
            rbStandardA4.Font = new Font("Segoe UI", 9F);
            rbStandardA4.ForeColor = Color.FromArgb(64, 64, 64);

            rbA5HalfPage.Location = new Point(25, 60);
            rbA5HalfPage.Size = new Size(250, 24);
            rbA5HalfPage.Text = "A5 Half Page (148 x 210 mm)";
            rbA5HalfPage.Tag = InvoiceFormat.A5HalfPage;
            rbA5HalfPage.Font = new Font("Segoe UI", 9F);
            rbA5HalfPage.ForeColor = Color.FromArgb(64, 64, 64);

            rbSlip3Inch.Location = new Point(300, 30);
            rbSlip3Inch.Size = new Size(250, 24);
            rbSlip3Inch.Text = "Slip 3 inch (76 x 210 mm)";
            rbSlip3Inch.Tag = InvoiceFormat.Slip3Inch;
            rbSlip3Inch.Font = new Font("Segoe UI", 9F);
            rbSlip3Inch.ForeColor = Color.FromArgb(64, 64, 64);

            rbSlip4Inch.Location = new Point(300, 60);
            rbSlip4Inch.Size = new Size(250, 24);
            rbSlip4Inch.Text = "Slip 4 inch (101 x 210 mm)";
            rbSlip4Inch.Tag = InvoiceFormat.Slip4Inch;
            rbSlip4Inch.Font = new Font("Segoe UI", 9F);
            rbSlip4Inch.ForeColor = Color.FromArgb(64, 64, 64);

            grpInvoiceFormat.Controls.AddRange(new Control[] { rbStandardA4, rbA5HalfPage, rbSlip3Inch, rbSlip4Inch });

            yPosition += groupBoxHeight + 40;

            // Buttons
            var buttonWidth = 130;
            var buttonHeight = 40;
            var totalButtonWidth = (buttonWidth * 3) + 40;
            var startButtonX = (groupBoxWidth + (leftMargin * 2) - totalButtonWidth) / 2 + leftMargin;

            btnPreview.Location = new Point(startButtonX, yPosition);
            btnPreview.Size = new Size(buttonWidth, buttonHeight);
            btnPreview.Text = "&Preview";
            btnPreview.UseVisualStyleBackColor = true;
            btnPreview.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPreview.BackColor = Color.FromArgb(70, 130, 180);
            btnPreview.ForeColor = Color.White;
            btnPreview.FlatStyle = FlatStyle.Flat;
            btnPreview.FlatAppearance.BorderSize = 0;
            btnPreview.Cursor = Cursors.Hand;
            btnPreview.Click += BtnPreview_Click;

            btnPrint.Location = new Point(startButtonX + buttonWidth + 20, yPosition);
            btnPrint.Size = new Size(buttonWidth, buttonHeight);
            btnPrint.Text = "&Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrint.BackColor = Color.FromArgb(34, 139, 34);
            btnPrint.ForeColor = Color.White;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.Click += BtnPrint_Click;

            btnCancel.Location = new Point(startButtonX + (buttonWidth + 20) * 2, yPosition);
            btnCancel.Size = new Size(buttonWidth, buttonHeight);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            Controls.AddRange(new Control[] { grpCopyType, grpNumberOfCopies, grpInvoiceFormat, btnPreview, btnPrint, btnCancel });

            // Set default button
            AcceptButton = btnPrint;
            CancelButton = btnCancel;
        }

        private void SetupForm()
        {
            // Set initial values
            SelectedCopyType = CopyType.All;
            NumberOfCopies = 1;
            SelectedInvoiceFormat = InvoiceFormat.StandardA4;

            // Update copy info label
            UpdateCopyInfo();
        }

        private void CopyType_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                SelectedCopyType = (CopyType)radioButton.Tag;
                UpdateCopyInfo();
            }
        }

        private void NumCopies_ValueChanged(object? sender, EventArgs e)
        {
            NumberOfCopies = (int)numCopies.Value;
            UpdateCopyInfo();
        }

        private void UpdateCopyInfo()
        {
            var totalPages = GetTotalPages();
            var copyInfo = SelectedCopyType == CopyType.All 
                ? $"Total pages to print: {totalPages} (Original + Duplicate + Triplicate)"
                : $"Total pages to print: {totalPages} ({GetCopyTypeDescription()})";
            lblCopiesInfo.Text = copyInfo;
        }

        private int GetTotalPages()
        {
            // For "All" copy type, always return 3 pages regardless of NumberOfCopies
            if (SelectedCopyType == CopyType.All)
            {
                return 3; // Original + Duplicate + Triplicate
            }
            return NumberOfCopies * GetPagesPerCopy();
        }

        private int GetPagesPerCopy()
        {
            return SelectedCopyType switch
            {
                CopyType.All => 3, // Original + Duplicate + Triplicate
                CopyType.OriginalDuplicate => 2, // Original + Duplicate
                _ => 1
            };
        }

        private string GetCopyTypeDescription()
        {
            return SelectedCopyType switch
            {
                CopyType.All => "Original + Duplicate + Triplicate",
                CopyType.OriginalDuplicate => "Original + Duplicate",
                CopyType.Original => "Original Only",
                CopyType.Duplicate => "Duplicate Only",
                CopyType.Triplicate => "Triplicate Only",
                _ => "Unknown"
            };
        }

        private void BtnPreview_Click(object? sender, EventArgs e)
        {
            try
            {
                // Capture the selected values
                CaptureSelectedValues();
                
                // Show preview with actual invoice generation
                var previewMessage = $"Print Preview:\n\n" +
                    $"Copy Type: {GetCopyTypeDescription()}\n" +
                    $"Number of Copies: {NumberOfCopies}\n" +
                    $"Total Pages: {GetTotalPages()}\n" +
                    $"Format: {GetInvoiceFormatDescription()}\n\n" +
                    $"Invoice will be generated with:\n" +
                    $"- {SelectedInvoiceFormat} format\n" +
                    $"- {SelectedCopyType} copy type\n" +
                    $"- {NumberOfCopies} copies\n" +
                    $"- Professional A4 layout with company branding\n" +
                    $"- Customer details and itemized billing\n" +
                    $"- Tax calculations and totals\n" +
                    $"- Terms and conditions";
                
                MessageBox.Show(
                    previewMessage,
                    "Print Preview",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating preview: {ex.Message}", "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                // Capture the selected values before closing
                CaptureSelectedValues();

                // Check if this dialog is being used as a child dialog
                if (Owner != null)
                {
                    // If it's a child dialog, just close with OK result
                    // The parent form will handle PDF generation
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    // If it's a standalone dialog, generate invoice PDF
                    if (GenerateInvoicePdf())
                    {
                        // Set the selected values and close with OK result
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error preparing print: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool GenerateInvoicePdf()
        {
            try
            {
                // Show save file dialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;
                    
                    // Set default filename
                    var copyTypeForFilename = SelectedCopyType == CopyType.All ? "All_3Copies" : SelectedCopyType.ToString();
                    var defaultFileName = $"Invoice_{SelectedInvoiceFormat}_{copyTypeForFilename}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    saveFileDialog.FileName = defaultFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create sample invoice data for demonstration
                        var invoiceModel = CreateSampleInvoiceModel();
                        
                        // Generate PDF using the new InvoicePdfGenerator
                        var pdfGenerator = new WinFormsApp1.Documents.ExportPdf.InvoicePdfGenerator();
                        
                        // For "All" copy type, always generate multiple copies (Original, Duplicate, Triplicate)
                        if (SelectedCopyType == CopyType.All)
                        {
                            // Generate 3 copies: Original, Duplicate, Triplicate
                            pdfGenerator.SaveInvoicePdfWithMultipleCopies(
                                invoiceModel, 
                                saveFileDialog.FileName, 
                                SelectedCopyType, 
                                3, // Always 3 for All option
                                SelectedInvoiceFormat
                            );
                        }
                        else if (NumberOfCopies > 1)
                        {
                            // Generate multiple copies for other copy types
                            pdfGenerator.SaveInvoicePdfWithMultipleCopies(
                                invoiceModel, 
                                saveFileDialog.FileName, 
                                SelectedCopyType, 
                                NumberOfCopies, 
                                SelectedInvoiceFormat
                            );
                        }
                        else
                        {
                            // Generate single copy
                            pdfGenerator.SaveInvoicePdf(
                                invoiceModel, 
                                saveFileDialog.FileName, 
                                SelectedCopyType, 
                                SelectedInvoiceFormat
                            );
                        }

                        // Show success message
                        var copyInfo = SelectedCopyType == CopyType.All 
                            ? "3 copies (Original + Duplicate + Triplicate)"
                            : $"{NumberOfCopies} copies ({GetCopyTypeDescription()})";
                        
                        var result = MessageBox.Show(
                            $"Invoice PDF generated successfully!\n\n" +
                            $"File: {saveFileDialog.FileName}\n" +
                            $"Format: {GetInvoiceFormatDescription()}\n" +
                            $"Copy Type: {GetCopyTypeDescription()}\n" +
                            $"Copies: {copyInfo}\n\n" +
                            $"Would you like to open the file?",
                            "PDF Generated",
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

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating invoice PDF: {ex.Message}", "PDF Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private WinFormsApp1.Documents.InvoiceModel CreateSampleInvoiceModel()
        {
            // Create sample invoice data for demonstration purposes
            // In a real application, this would come from the actual transaction data
            return new WinFormsApp1.Documents.InvoiceModel
            {
                InvoiceNumber = "INV-2024-001",
                InvoiceDate = DateTime.Now,
                CustomerName = "Sample Customer Ltd.",
                CustomerAddress = "123 Business Street, City, State 12345",
                CustomerGSTIN = "GSTIN123456789",
                CustomerMobile = "+91 98765 43210",
                CompanyName = "Your Company Name",
                CompanyGSTIN = "COMPANY123456789",
                CompanyPhone = "+91 12345 67890",
                CompanyEmail = "info@yourcompany.com",
                PlaceOfSupply = "Mumbai, Maharashtra",
                TransactionType = "Sale Invoice",
                ReferenceNumber = "REF-001",
                Notes = "Thank you for your business!",
                Items = new List<WinFormsApp1.Documents.InvoiceItemModel>
                {
                    new WinFormsApp1.Documents.InvoiceItemModel
                    {
                        SerialNumber = 1,
                        ProductName = "Sample Product 1",
                        HSNCode = "HSN001",
                        Quantity = 2,
                        Unit = "PCS",
                        UnitPrice = 100.00m,
                        DiscountPercentage = 10,
                        DiscountAmount = 20.00m,
                        TaxRate = 18,
                        TaxAmount = 32.40m,
                        LineTotal = 212.40m
                    },
                    new WinFormsApp1.Documents.InvoiceItemModel
                    {
                        SerialNumber = 2,
                        ProductName = "Sample Product 2",
                        HSNCode = "HSN002",
                        Quantity = 1,
                        Unit = "PCS",
                        UnitPrice = 150.00m,
                        DiscountPercentage = 0,
                        DiscountAmount = 0.00m,
                        TaxRate = 18,
                        TaxAmount = 27.00m,
                        LineTotal = 177.00m
                    }
                },
                SubTotal = 250.00m,
                DiscountAmount = 20.00m,
                TaxAmount = 59.40m,
                TotalAmount = 389.40m,
                RoundOff = 0.60m,
                NetPayable = 390.00m,
                AmountInWords = "Three Hundred and Ninety Rupees Only",
                InvoiceFormat = GetInvoiceFormatDescription()
            };
        }

        private void CaptureSelectedValues()
        {
            // Capture selected copy type
            if (rbAll.Checked) SelectedCopyType = CopyType.All;
            else if (rbOriginal.Checked) SelectedCopyType = CopyType.Original;
            else if (rbDuplicate.Checked) SelectedCopyType = CopyType.Duplicate;
            else if (rbTriplicate.Checked) SelectedCopyType = CopyType.Triplicate;
            else if (rbOriginalDuplicate.Checked) SelectedCopyType = CopyType.OriginalDuplicate;

            // Capture number of copies
            NumberOfCopies = (int)numCopies.Value;

            // Capture selected invoice format
            if (rbStandardA4.Checked) SelectedInvoiceFormat = InvoiceFormat.StandardA4;
            else if (rbA5HalfPage.Checked) SelectedInvoiceFormat = InvoiceFormat.A5HalfPage;
            else if (rbSlip3Inch.Checked) SelectedInvoiceFormat = InvoiceFormat.Slip3Inch;
            else if (rbSlip4Inch.Checked) SelectedInvoiceFormat = InvoiceFormat.Slip4Inch;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string GetInvoiceFormatDescription()
        {
            return SelectedInvoiceFormat switch
            {
                InvoiceFormat.StandardA4 => "Standard A4 (210 x 297 mm)",
                InvoiceFormat.A5HalfPage => "A5 Half Page (148 x 210 mm)",
                InvoiceFormat.Slip3Inch => "Slip 3 inch (76 x 210 mm)",
                InvoiceFormat.Slip4Inch => "Slip 4 inch (101 x 210 mm)",
                _ => "Unknown"
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    BtnCancel_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F5:
                    BtnPreview_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    if (ModifierKeys.HasFlag(Keys.Control))
                    {
                        BtnPrint_Click(null, EventArgs.Empty);
                        e.Handled = true;
                    }
                    break;
            }
            base.OnKeyDown(e);
        }
    }

    // Enums for the dialog options
    public enum CopyType
    {
        All,
        Original,
        Duplicate,
        Triplicate,
        OriginalDuplicate
    }

    public enum InvoiceFormat
    {
        StandardA4,
        A5HalfPage,
        Slip3Inch,
        Slip4Inch
    }
}