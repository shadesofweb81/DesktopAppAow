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
            lblCopiesInfo.Text = $"Total pages to print: {totalPages} ({GetCopyTypeDescription()})";
        }

        private int GetTotalPages()
        {
            return NumberOfCopies * GetPagesPerCopy();
        }

        private int GetPagesPerCopy()
        {
            return SelectedCopyType switch
            {
                CopyType.All => 3,
                CopyType.OriginalDuplicate => 2,
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
            // TODO: Implement print preview functionality
            MessageBox.Show(
                $"Print Preview:\n\n" +
                $"Copy Type: {GetCopyTypeDescription()}\n" +
                $"Number of Copies: {NumberOfCopies}\n" +
                $"Total Pages: {GetTotalPages()}\n" +
                $"Format: {GetInvoiceFormatDescription()}\n\n" +
                $"Preview functionality will be implemented here.",
                "Print Preview",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            // Set the selected values and close with OK result
            DialogResult = DialogResult.OK;
            Close();
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