using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Reports
{
    public partial class StockItemReportForm : Form
    {
        private readonly StockReportService _stockReportService;
        private readonly Guid _productId;
        private readonly string _productName;
        private readonly string _productCode;
        
        // Controls
        private GroupBox criteriaGroupBox = null!;
        private Label lblFromDate = null!;
        private DateTimePicker dtpFromDate = null!;
        private Label lblToDate = null!;
        private DateTimePicker dtpToDate = null!;
        private CheckBox chkIncludeTransactionDetails = null!;
        private Button btnGenerateReport = null!;
        private Button btnClear = null!;
        
        private GroupBox productInfoGroupBox = null!;
        private Label lblProductName = null!;
        private Label lblProductCode = null!;
        private Label lblCurrentStock = null!;
        private Label lblUnit = null!;
        
        private GroupBox reportGroupBox = null!;
        private DataGridView dgvTransactions = null!;
        
        private GroupBox summaryGroupBox = null!;
        private Label lblTotalTransactions = null!;
        private Label lblTotalTransactionsValue = null!;
        private Label lblPurchaseTransactions = null!;
        private Label lblPurchaseTransactionsValue = null!;
        private Label lblSaleTransactions = null!;
        private Label lblSaleTransactionsValue = null!;
        private Label lblTotalPurchaseValue = null!;
        private Label lblTotalPurchaseValueValue = null!;
        private Label lblTotalSaleValue = null!;
        private Label lblTotalSaleValueValue = null!;
        private Label lblAveragePurchasePrice = null!;
        private Label lblAveragePurchasePriceValue = null!;
        private Label lblAverageSalePrice = null!;
        private Label lblAverageSalePriceValue = null!;
        
        private Panel loadingPanel = null!;
        private ProgressBar loadingProgressBar = null!;
        private Label loadingLabel = null!;
        
        // Data
        private StockItemReportResponse? _reportData;

        public StockItemReportForm(StockReportService stockReportService, Guid productId, string productName, string productCode)
        {
            _stockReportService = stockReportService;
            _productId = productId;
            _productName = productName;
            _productCode = productCode;
            
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = $"Stock Item Report - {_productName}";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 600);
            
            // Criteria Group Box
            criteriaGroupBox = new GroupBox();
            criteriaGroupBox.Text = "Report Criteria";
            criteriaGroupBox.Location = new Point(20, 20);
            criteriaGroupBox.Size = new Size(1350, 100);
            criteriaGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(criteriaGroupBox);
            
            // From Date
            lblFromDate = new Label();
            lblFromDate.Text = "From Date:";
            lblFromDate.Location = new Point(20, 30);
            lblFromDate.Size = new Size(80, 25);
            lblFromDate.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblFromDate);
            
            dtpFromDate = new DateTimePicker();
            dtpFromDate.Location = new Point(110, 30);
            dtpFromDate.Size = new Size(150, 25);
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Value = DateTime.Now.AddMonths(-3); // Default to 3 months ago
            criteriaGroupBox.Controls.Add(dtpFromDate);
            
            // To Date
            lblToDate = new Label();
            lblToDate.Text = "To Date:";
            lblToDate.Location = new Point(280, 30);
            lblToDate.Size = new Size(60, 25);
            lblToDate.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblToDate);
            
            dtpToDate = new DateTimePicker();
            dtpToDate.Location = new Point(350, 30);
            dtpToDate.Size = new Size(150, 25);
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Value = DateTime.Now; // Default to today
            criteriaGroupBox.Controls.Add(dtpToDate);
            
            // Include Transaction Details
            chkIncludeTransactionDetails = new CheckBox();
            chkIncludeTransactionDetails.Text = "Include Transaction Details";
            chkIncludeTransactionDetails.Location = new Point(520, 30);
            chkIncludeTransactionDetails.Size = new Size(180, 25);
            chkIncludeTransactionDetails.Font = new Font("Segoe UI", 9F);
            chkIncludeTransactionDetails.Checked = true; // Default to true
            criteriaGroupBox.Controls.Add(chkIncludeTransactionDetails);
            
            // Buttons
            btnGenerateReport = new Button();
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.Location = new Point(720, 30);
            btnGenerateReport.Size = new Size(120, 30);
            btnGenerateReport.BackColor = Color.FromArgb(0, 120, 215);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.Click += BtnGenerateReport_Click;
            criteriaGroupBox.Controls.Add(btnGenerateReport);
            
            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(860, 30);
            btnClear.Size = new Size(80, 30);
            btnClear.Click += BtnClear_Click;
            criteriaGroupBox.Controls.Add(btnClear);
            
            // Product Info Group Box
            productInfoGroupBox = new GroupBox();
            productInfoGroupBox.Text = "Product Information";
            productInfoGroupBox.Location = new Point(20, 140);
            productInfoGroupBox.Size = new Size(1350, 80);
            productInfoGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(productInfoGroupBox);
            
            // Product Info Labels
            lblProductName = new Label();
            lblProductName.Text = $"Product: {_productName}";
            lblProductName.Location = new Point(20, 30);
            lblProductName.Size = new Size(400, 25);
            lblProductName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductName.ForeColor = Color.Blue;
            productInfoGroupBox.Controls.Add(lblProductName);
            
            lblProductCode = new Label();
            lblProductCode.Text = $"Code: {_productCode}";
            lblProductCode.Location = new Point(440, 30);
            lblProductCode.Size = new Size(200, 25);
            lblProductCode.Font = new Font("Segoe UI", 9F);
            productInfoGroupBox.Controls.Add(lblProductCode);
            
            lblCurrentStock = new Label();
            lblCurrentStock.Text = "Current Stock: Loading...";
            lblCurrentStock.Location = new Point(660, 30);
            lblCurrentStock.Size = new Size(150, 25);
            lblCurrentStock.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentStock.ForeColor = Color.Green;
            productInfoGroupBox.Controls.Add(lblCurrentStock);
            
            lblUnit = new Label();
            lblUnit.Text = "Unit: Loading...";
            lblUnit.Location = new Point(830, 30);
            lblUnit.Size = new Size(100, 25);
            lblUnit.Font = new Font("Segoe UI", 9F);
            productInfoGroupBox.Controls.Add(lblUnit);
            
            // Report Group Box
            reportGroupBox = new GroupBox();
            reportGroupBox.Text = "Transaction History";
            reportGroupBox.Location = new Point(20, 240);
            reportGroupBox.Size = new Size(1350, 400);
            reportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(reportGroupBox);
            
            // Data Grid View
            dgvTransactions = new DataGridView();
            dgvTransactions.Location = new Point(20, 30);
            dgvTransactions.Size = new Size(1310, 320);
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.Fixed3D;
            dgvTransactions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvTransactions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvTransactions.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
            dgvTransactions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgvTransactions.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.ScrollBars = ScrollBars.Both;
            dgvTransactions.GridColor = Color.FromArgb(200, 200, 200);
            reportGroupBox.Controls.Add(dgvTransactions);
            
            // Summary Group Box
            summaryGroupBox = new GroupBox();
            summaryGroupBox.Text = "Summary";
            summaryGroupBox.Location = new Point(20, 660);
            summaryGroupBox.Size = new Size(1350, 100);
            summaryGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(summaryGroupBox);
            
            // Summary Labels - First Row
            lblTotalTransactions = new Label();
            lblTotalTransactions.Text = "Total Transactions:";
            lblTotalTransactions.Location = new Point(20, 30);
            lblTotalTransactions.Size = new Size(120, 25);
            lblTotalTransactions.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalTransactions);
            
            lblTotalTransactionsValue = new Label();
            lblTotalTransactionsValue.Text = "0";
            lblTotalTransactionsValue.Location = new Point(150, 30);
            lblTotalTransactionsValue.Size = new Size(50, 25);
            lblTotalTransactionsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalTransactionsValue);
            
            lblPurchaseTransactions = new Label();
            lblPurchaseTransactions.Text = "Purchases:";
            lblPurchaseTransactions.Location = new Point(220, 30);
            lblPurchaseTransactions.Size = new Size(80, 25);
            lblPurchaseTransactions.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPurchaseTransactions.ForeColor = Color.Green;
            summaryGroupBox.Controls.Add(lblPurchaseTransactions);
            
            lblPurchaseTransactionsValue = new Label();
            lblPurchaseTransactionsValue.Text = "0";
            lblPurchaseTransactionsValue.Location = new Point(310, 30);
            lblPurchaseTransactionsValue.Size = new Size(50, 25);
            lblPurchaseTransactionsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPurchaseTransactionsValue.ForeColor = Color.Green;
            summaryGroupBox.Controls.Add(lblPurchaseTransactionsValue);
            
            lblSaleTransactions = new Label();
            lblSaleTransactions.Text = "Sales:";
            lblSaleTransactions.Location = new Point(380, 30);
            lblSaleTransactions.Size = new Size(50, 25);
            lblSaleTransactions.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSaleTransactions.ForeColor = Color.Red;
            summaryGroupBox.Controls.Add(lblSaleTransactions);
            
            lblSaleTransactionsValue = new Label();
            lblSaleTransactionsValue.Text = "0";
            lblSaleTransactionsValue.Location = new Point(440, 30);
            lblSaleTransactionsValue.Size = new Size(50, 25);
            lblSaleTransactionsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSaleTransactionsValue.ForeColor = Color.Red;
            summaryGroupBox.Controls.Add(lblSaleTransactionsValue);
            
            // Summary Labels - Second Row
            lblTotalPurchaseValue = new Label();
            lblTotalPurchaseValue.Text = "Total Purchase Value:";
            lblTotalPurchaseValue.Location = new Point(20, 60);
            lblTotalPurchaseValue.Size = new Size(130, 25);
            lblTotalPurchaseValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalPurchaseValue);
            
            lblTotalPurchaseValueValue = new Label();
            lblTotalPurchaseValueValue.Text = "0.00";
            lblTotalPurchaseValueValue.Location = new Point(160, 60);
            lblTotalPurchaseValueValue.Size = new Size(100, 25);
            lblTotalPurchaseValueValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalPurchaseValueValue.ForeColor = Color.Green;
            summaryGroupBox.Controls.Add(lblTotalPurchaseValueValue);
            
            lblTotalSaleValue = new Label();
            lblTotalSaleValue.Text = "Total Sale Value:";
            lblTotalSaleValue.Location = new Point(280, 60);
            lblTotalSaleValue.Size = new Size(110, 25);
            lblTotalSaleValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalSaleValue);
            
            lblTotalSaleValueValue = new Label();
            lblTotalSaleValueValue.Text = "0.00";
            lblTotalSaleValueValue.Location = new Point(400, 60);
            lblTotalSaleValueValue.Size = new Size(100, 25);
            lblTotalSaleValueValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalSaleValueValue.ForeColor = Color.Red;
            summaryGroupBox.Controls.Add(lblTotalSaleValueValue);
            
            lblAveragePurchasePrice = new Label();
            lblAveragePurchasePrice.Text = "Avg Purchase Price:";
            lblAveragePurchasePrice.Location = new Point(520, 60);
            lblAveragePurchasePrice.Size = new Size(120, 25);
            lblAveragePurchasePrice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblAveragePurchasePrice);
            
            lblAveragePurchasePriceValue = new Label();
            lblAveragePurchasePriceValue.Text = "0.00";
            lblAveragePurchasePriceValue.Location = new Point(650, 60);
            lblAveragePurchasePriceValue.Size = new Size(100, 25);
            lblAveragePurchasePriceValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAveragePurchasePriceValue.ForeColor = Color.Blue;
            summaryGroupBox.Controls.Add(lblAveragePurchasePriceValue);
            
            lblAverageSalePrice = new Label();
            lblAverageSalePrice.Text = "Avg Sale Price:";
            lblAverageSalePrice.Location = new Point(770, 60);
            lblAverageSalePrice.Size = new Size(100, 25);
            lblAverageSalePrice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblAverageSalePrice);
            
            lblAverageSalePriceValue = new Label();
            lblAverageSalePriceValue.Text = "0.00";
            lblAverageSalePriceValue.Location = new Point(880, 60);
            lblAverageSalePriceValue.Size = new Size(100, 25);
            lblAverageSalePriceValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAverageSalePriceValue.ForeColor = Color.Blue;
            summaryGroupBox.Controls.Add(lblAverageSalePriceValue);
            
            // Loading Panel
            SetupLoadingPanel();
        }

        private void SetupLoadingPanel()
        {
            loadingPanel = new Panel();
            loadingPanel.Size = new Size(200, 100);
            loadingPanel.BackColor = Color.White;
            loadingPanel.BorderStyle = BorderStyle.FixedSingle;
            loadingPanel.Visible = false;
            
            loadingProgressBar = new ProgressBar();
            loadingProgressBar.Style = ProgressBarStyle.Marquee;
            loadingProgressBar.Location = new Point(20, 20);
            loadingProgressBar.Size = new Size(160, 20);
            loadingPanel.Controls.Add(loadingProgressBar);
            
            loadingLabel = new Label();
            loadingLabel.Text = "Loading...";
            loadingLabel.Location = new Point(20, 50);
            loadingLabel.Size = new Size(160, 20);
            loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            loadingPanel.Controls.Add(loadingLabel);
            
            this.Controls.Add(loadingPanel);
        }

        private void SetupForm()
        {
            // Set default dates
            dtpFromDate.Value = DateTime.Now.AddMonths(-3);
            dtpToDate.Value = DateTime.Now;
            
            // Center loading panel
            CenterLoadingPanel();
            
            // Resize controls for initial layout
            ResizeControls();
            
            // Generate initial report
            BtnGenerateReport_Click(null, EventArgs.Empty);
        }

        private void CenterLoadingPanel()
        {
            if (loadingPanel != null)
            {
                loadingPanel.Location = new Point(
                    (this.ClientSize.Width - loadingPanel.Width) / 2,
                    (this.ClientSize.Height - loadingPanel.Height) / 2
                );
            }
        }

        private void ShowLoadingPanel(string message = "Loading...")
        {
            loadingLabel.Text = message;
            loadingPanel.Visible = true;
            loadingPanel.BringToFront();
            CenterLoadingPanel();
            Application.DoEvents();
        }

        private void HideLoadingPanel()
        {
            loadingPanel.Visible = false;
        }

        private async void BtnGenerateReport_Click(object? sender, EventArgs e)
        {
            try
            {
                ShowLoadingPanel("Loading stock item report...");

                var request = new StockItemReportRequest
                {
                    ProductId = _productId,
                    FromDate = dtpFromDate.Value.Date,
                    ToDate = dtpToDate.Value.Date,
                    IncludeTransactionDetails = chkIncludeTransactionDetails.Checked
                };

                _reportData = await _stockReportService.GetStockItemReportAsync(request);

                HideLoadingPanel();

                if (_reportData != null)
                {
                    DisplayReport();
                }
                else
                {
                    MessageBox.Show("No data found for the selected criteria.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                HideLoadingPanel();
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayReport()
        {
            if (_reportData == null) return;

            // Update product info
            lblCurrentStock.Text = $"Current Stock: {_reportData.CurrentStock:N2}";
            lblUnit.Text = $"Unit: {_reportData.Unit}";

            // Clear existing data
            dgvTransactions.DataSource = null;
            dgvTransactions.Columns.Clear();

            // Create a list to hold all transactions for display
            var allTransactions = new List<object>();

            foreach (var transaction in _reportData.Transactions)
            {
                allTransactions.Add(new
                {
                    TransactionDate = transaction.TransactionDate.ToString("dd/MM/yyyy"),
                    TransactionType = transaction.TransactionType,
                    TransactionNumber = transaction.TransactionNumber,
                    Reference = transaction.Reference,
                    Quantity = transaction.Quantity,
                    UnitPrice = transaction.UnitPrice,
                    TotalAmount = transaction.TotalAmount,
                    Remarks = transaction.Remarks,
                    Status = transaction.Status
                });
            }

            // Bind data to grid
            dgvTransactions.DataSource = allTransactions;

            // Format columns
            if (dgvTransactions.Columns.Count > 0)
            {
                // Format numeric columns
                if (dgvTransactions.Columns["Quantity"] != null)
                {
                    dgvTransactions.Columns["Quantity"]!.DefaultCellStyle.Format = "N2";
                    dgvTransactions.Columns["Quantity"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvTransactions.Columns["UnitPrice"] != null)
                {
                    dgvTransactions.Columns["UnitPrice"]!.DefaultCellStyle.Format = "N2";
                    dgvTransactions.Columns["UnitPrice"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvTransactions.Columns["TotalAmount"] != null)
                {
                    dgvTransactions.Columns["TotalAmount"]!.DefaultCellStyle.Format = "N2";
                    dgvTransactions.Columns["TotalAmount"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Set column widths
                if (dgvTransactions.Columns["TransactionDate"] != null)
                    dgvTransactions.Columns["TransactionDate"]!.Width = 100;
                if (dgvTransactions.Columns["TransactionType"] != null)
                    dgvTransactions.Columns["TransactionType"]!.Width = 120;
                if (dgvTransactions.Columns["TransactionNumber"] != null)
                    dgvTransactions.Columns["TransactionNumber"]!.Width = 150;
                if (dgvTransactions.Columns["Reference"] != null)
                    dgvTransactions.Columns["Reference"]!.Width = 120;
                if (dgvTransactions.Columns["Quantity"] != null)
                    dgvTransactions.Columns["Quantity"]!.Width = 100;
                if (dgvTransactions.Columns["UnitPrice"] != null)
                    dgvTransactions.Columns["UnitPrice"]!.Width = 100;
                if (dgvTransactions.Columns["TotalAmount"] != null)
                    dgvTransactions.Columns["TotalAmount"]!.Width = 120;
                if (dgvTransactions.Columns["Remarks"] != null)
                    dgvTransactions.Columns["Remarks"]!.Width = 200;
                if (dgvTransactions.Columns["Status"] != null)
                    dgvTransactions.Columns["Status"]!.Width = 100;
            }

            // Color code rows based on transaction type
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;

            // Update summary labels
            UpdateSummaryLabels();
        }

        private void DgvTransactions_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvTransactions.Columns["TransactionType"] != null)
            {
                var transactionType = dgvTransactions.Rows[e.RowIndex].Cells["TransactionType"].Value?.ToString();
                
                switch (transactionType?.ToLower())
                {
                    case "purchase":
                        e.CellStyle.BackColor = Color.FromArgb(240, 255, 240);
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                    case "sale":
                        e.CellStyle.BackColor = Color.FromArgb(255, 240, 240);
                        e.CellStyle.ForeColor = Color.Red;
                        break;
                    case "manufacture":
                        e.CellStyle.BackColor = Color.FromArgb(240, 240, 255);
                        e.CellStyle.ForeColor = Color.Blue;
                        break;
                    case "return":
                        e.CellStyle.BackColor = Color.FromArgb(255, 248, 220);
                        e.CellStyle.ForeColor = Color.Orange;
                        break;
                    case "adjustment":
                        e.CellStyle.BackColor = Color.FromArgb(248, 248, 248);
                        e.CellStyle.ForeColor = Color.Gray;
                        break;
                }
            }
        }

        private void UpdateSummaryLabels()
        {
            if (_reportData?.Summary != null)
            {
                lblTotalTransactionsValue.Text = _reportData.Summary.TotalTransactions.ToString();
                lblPurchaseTransactionsValue.Text = _reportData.Summary.PurchaseTransactions.ToString();
                lblSaleTransactionsValue.Text = _reportData.Summary.SaleTransactions.ToString();
                lblTotalPurchaseValueValue.Text = _reportData.Summary.TotalPurchaseValue.ToString("N2");
                lblTotalSaleValueValue.Text = _reportData.Summary.TotalSaleValue.ToString("N2");
                lblAveragePurchasePriceValue.Text = _reportData.Summary.AveragePurchasePrice.ToString("N2");
                lblAverageSalePriceValue.Text = _reportData.Summary.AverageSalePrice.ToString("N2");
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now.AddMonths(-3);
            dtpToDate.Value = DateTime.Now;
            chkIncludeTransactionDetails.Checked = true;
            
            dgvTransactions.DataSource = null;
            dgvTransactions.Columns.Clear();
            
            // Clear summary labels
            lblTotalTransactionsValue.Text = "0";
            lblPurchaseTransactionsValue.Text = "0";
            lblSaleTransactionsValue.Text = "0";
            lblTotalPurchaseValueValue.Text = "0.00";
            lblTotalSaleValueValue.Text = "0.00";
            lblAveragePurchasePriceValue.Text = "0.00";
            lblAverageSalePriceValue.Text = "0.00";
            
            _reportData = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterLoadingPanel();
            ResizeControls();
        }

        private void ResizeControls()
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            var clientWidth = this.ClientSize.Width;
            var clientHeight = this.ClientSize.Height;

            // Resize criteria group box
            if (criteriaGroupBox != null)
            {
                criteriaGroupBox.Width = Math.Max(800, clientWidth - 40);
            }

            // Resize product info group box
            if (productInfoGroupBox != null)
            {
                productInfoGroupBox.Width = Math.Max(800, clientWidth - 40);
            }

            // Resize report group box
            if (reportGroupBox != null)
            {
                reportGroupBox.Width = Math.Max(800, clientWidth - 40);
                reportGroupBox.Height = Math.Max(300, clientHeight - 300);
            }

            // Resize data grid view
            if (dgvTransactions != null)
            {
                dgvTransactions.Width = Math.Max(760, clientWidth - 60);
                dgvTransactions.Height = Math.Max(200, clientHeight - 420);
            }

            // Resize summary group box
            if (summaryGroupBox != null)
            {
                summaryGroupBox.Width = Math.Max(800, clientWidth - 40);
                summaryGroupBox.Top = Math.Max(660, clientHeight - 120);
            }
        }
    }
}
