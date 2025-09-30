using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Reports
{
    public partial class StockReportForm : Form
    {
        private readonly StockReportService _stockReportService;
        private readonly LocalStorageService _localStorageService;
        
        // Controls
        private GroupBox criteriaGroupBox = null!;
        private Label lblProductFilter = null!;
        private TextBox txtProductFilter = null!;
        private CheckBox chkShowOnlyLowStock = null!;
        private CheckBox chkShowOnlyOutOfStock = null!;
        private CheckBox chkIncludeVariants = null!;
        private Label lblAsOfDate = null!;
        private DateTimePicker dtpAsOfDate = null!;
        private Button btnGenerateReport = null!;
        private Button btnClear = null!;
        
        private GroupBox reportGroupBox = null!;
        private DataGridView dgvReport = null!;
        private GroupBox summaryGroupBox = null!;
        private Label lblTotalProducts = null!;
        private Label lblTotalProductsValue = null!;
        private Label lblInStockProducts = null!;
        private Label lblInStockProductsValue = null!;
        private Label lblLowStockProducts = null!;
        private Label lblLowStockProductsValue = null!;
        private Label lblOutOfStockProducts = null!;
        private Label lblOutOfStockProductsValue = null!;
        private Label lblTotalStockValue = null!;
        private Label lblTotalStockValueValue = null!;
        private Label lblProductsNeedingReorder = null!;
        private Label lblProductsNeedingReorderValue = null!;
        
        private Panel loadingPanel = null!;
        private ProgressBar loadingProgressBar = null!;
        private Label loadingLabel = null!;
        
        // Data
        private StockReportResponse? _reportData;

        public StockReportForm(StockReportService stockReportService, LocalStorageService localStorageService)
        {
            _stockReportService = stockReportService;
            _localStorageService = localStorageService;
            
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Stock Report";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            
            // Criteria Group Box
            criteriaGroupBox = new GroupBox();
            criteriaGroupBox.Text = "Report Criteria";
            criteriaGroupBox.Location = new Point(20, 20);
            criteriaGroupBox.Size = new Size(1350, 120);
            criteriaGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(criteriaGroupBox);
            
            // Product Filter
            lblProductFilter = new Label();
            lblProductFilter.Text = "Product Filter:";
            lblProductFilter.Location = new Point(20, 30);
            lblProductFilter.Size = new Size(100, 25);
            lblProductFilter.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblProductFilter);
            
            txtProductFilter = new TextBox();
            txtProductFilter.Location = new Point(130, 30);
            txtProductFilter.Size = new Size(200, 25);
            txtProductFilter.PlaceholderText = "Enter product name or code";
            criteriaGroupBox.Controls.Add(txtProductFilter);
            
            // Checkboxes
            chkShowOnlyLowStock = new CheckBox();
            chkShowOnlyLowStock.Text = "Show Only Low Stock";
            chkShowOnlyLowStock.Location = new Point(350, 30);
            chkShowOnlyLowStock.Size = new Size(150, 25);
            chkShowOnlyLowStock.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(chkShowOnlyLowStock);
            
            chkShowOnlyOutOfStock = new CheckBox();
            chkShowOnlyOutOfStock.Text = "Show Only Out of Stock";
            chkShowOnlyOutOfStock.Location = new Point(520, 30);
            chkShowOnlyOutOfStock.Size = new Size(150, 25);
            chkShowOnlyOutOfStock.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(chkShowOnlyOutOfStock);
            
            chkIncludeVariants = new CheckBox();
            chkIncludeVariants.Text = "Include Variants";
            chkIncludeVariants.Location = new Point(690, 30);
            chkIncludeVariants.Size = new Size(120, 25);
            chkIncludeVariants.Font = new Font("Segoe UI", 9F);
            chkIncludeVariants.Checked = true; // Default to true
            criteriaGroupBox.Controls.Add(chkIncludeVariants);
            
            // As Of Date
            lblAsOfDate = new Label();
            lblAsOfDate.Text = "As Of Date:";
            lblAsOfDate.Location = new Point(20, 70);
            lblAsOfDate.Size = new Size(80, 25);
            lblAsOfDate.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblAsOfDate);
            
            dtpAsOfDate = new DateTimePicker();
            dtpAsOfDate.Location = new Point(110, 70);
            dtpAsOfDate.Size = new Size(150, 25);
            dtpAsOfDate.Format = DateTimePickerFormat.Short;
            dtpAsOfDate.Value = DateTime.Now; // Default to today
            criteriaGroupBox.Controls.Add(dtpAsOfDate);
            
            // Buttons
            btnGenerateReport = new Button();
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.Location = new Point(280, 70);
            btnGenerateReport.Size = new Size(120, 30);
            btnGenerateReport.BackColor = Color.FromArgb(0, 120, 215);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.Click += BtnGenerateReport_Click;
            criteriaGroupBox.Controls.Add(btnGenerateReport);
            
            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(420, 70);
            btnClear.Size = new Size(80, 30);
            btnClear.Click += BtnClear_Click;
            criteriaGroupBox.Controls.Add(btnClear);
            
            // Report Group Box
            reportGroupBox = new GroupBox();
            reportGroupBox.Text = "Stock Report Results";
            reportGroupBox.Location = new Point(20, 160);
            reportGroupBox.Size = new Size(1350, 600);
            reportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(reportGroupBox);
            
            // Data Grid View
            dgvReport = new DataGridView();
            dgvReport.Location = new Point(20, 30);
            dgvReport.Size = new Size(1310, 400);
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.MultiSelect = false;
            dgvReport.BackgroundColor = Color.White;
            dgvReport.BorderStyle = BorderStyle.Fixed3D;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvReport.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvReport.RowHeadersVisible = false;
            dgvReport.ScrollBars = ScrollBars.Both;
            dgvReport.GridColor = Color.FromArgb(200, 200, 200);
            reportGroupBox.Controls.Add(dgvReport);
            
            // Summary Group Box
            summaryGroupBox = new GroupBox();
            summaryGroupBox.Text = "Summary";
            summaryGroupBox.Location = new Point(20, 450);
            summaryGroupBox.Size = new Size(1310, 120);
            summaryGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            reportGroupBox.Controls.Add(summaryGroupBox);
            
            // Summary Labels - First Row
            lblTotalProducts = new Label();
            lblTotalProducts.Text = "Total Products:";
            lblTotalProducts.Location = new Point(20, 30);
            lblTotalProducts.Size = new Size(100, 25);
            lblTotalProducts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalProducts);
            
            lblTotalProductsValue = new Label();
            lblTotalProductsValue.Text = "0";
            lblTotalProductsValue.Location = new Point(130, 30);
            lblTotalProductsValue.Size = new Size(50, 25);
            lblTotalProductsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalProductsValue);
            
            lblInStockProducts = new Label();
            lblInStockProducts.Text = "In Stock:";
            lblInStockProducts.Location = new Point(200, 30);
            lblInStockProducts.Size = new Size(80, 25);
            lblInStockProducts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblInStockProducts.ForeColor = Color.Green;
            summaryGroupBox.Controls.Add(lblInStockProducts);
            
            lblInStockProductsValue = new Label();
            lblInStockProductsValue.Text = "0";
            lblInStockProductsValue.Location = new Point(290, 30);
            lblInStockProductsValue.Size = new Size(50, 25);
            lblInStockProductsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblInStockProductsValue.ForeColor = Color.Green;
            summaryGroupBox.Controls.Add(lblInStockProductsValue);
            
            lblLowStockProducts = new Label();
            lblLowStockProducts.Text = "Low Stock:";
            lblLowStockProducts.Location = new Point(360, 30);
            lblLowStockProducts.Size = new Size(80, 25);
            lblLowStockProducts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLowStockProducts.ForeColor = Color.Orange;
            summaryGroupBox.Controls.Add(lblLowStockProducts);
            
            lblLowStockProductsValue = new Label();
            lblLowStockProductsValue.Text = "0";
            lblLowStockProductsValue.Location = new Point(450, 30);
            lblLowStockProductsValue.Size = new Size(50, 25);
            lblLowStockProductsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLowStockProductsValue.ForeColor = Color.Orange;
            summaryGroupBox.Controls.Add(lblLowStockProductsValue);
            
            lblOutOfStockProducts = new Label();
            lblOutOfStockProducts.Text = "Out of Stock:";
            lblOutOfStockProducts.Location = new Point(520, 30);
            lblOutOfStockProducts.Size = new Size(90, 25);
            lblOutOfStockProducts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOutOfStockProducts.ForeColor = Color.Red;
            summaryGroupBox.Controls.Add(lblOutOfStockProducts);
            
            lblOutOfStockProductsValue = new Label();
            lblOutOfStockProductsValue.Text = "0";
            lblOutOfStockProductsValue.Location = new Point(620, 30);
            lblOutOfStockProductsValue.Size = new Size(50, 25);
            lblOutOfStockProductsValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOutOfStockProductsValue.ForeColor = Color.Red;
            summaryGroupBox.Controls.Add(lblOutOfStockProductsValue);
            
            // Summary Labels - Second Row
            lblTotalStockValue = new Label();
            lblTotalStockValue.Text = "Total Stock Value:";
            lblTotalStockValue.Location = new Point(20, 60);
            lblTotalStockValue.Size = new Size(120, 25);
            lblTotalStockValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTotalStockValue);
            
            lblTotalStockValueValue = new Label();
            lblTotalStockValueValue.Text = "0.00";
            lblTotalStockValueValue.Location = new Point(150, 60);
            lblTotalStockValueValue.Size = new Size(100, 25);
            lblTotalStockValueValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalStockValueValue.ForeColor = Color.Blue;
            summaryGroupBox.Controls.Add(lblTotalStockValueValue);
            
            lblProductsNeedingReorder = new Label();
            lblProductsNeedingReorder.Text = "Need Reorder:";
            lblProductsNeedingReorder.Location = new Point(270, 60);
            lblProductsNeedingReorder.Size = new Size(100, 25);
            lblProductsNeedingReorder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductsNeedingReorder.ForeColor = Color.DarkRed;
            summaryGroupBox.Controls.Add(lblProductsNeedingReorder);
            
            lblProductsNeedingReorderValue = new Label();
            lblProductsNeedingReorderValue.Text = "0";
            lblProductsNeedingReorderValue.Location = new Point(380, 60);
            lblProductsNeedingReorderValue.Size = new Size(50, 25);
            lblProductsNeedingReorderValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProductsNeedingReorderValue.ForeColor = Color.DarkRed;
            summaryGroupBox.Controls.Add(lblProductsNeedingReorderValue);
            
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
            // Set default date
            dtpAsOfDate.Value = DateTime.Now;
            
            // Center loading panel
            CenterLoadingPanel();
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
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany == null)
                {
                    MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowLoadingPanel("Generating stock report...");

                var request = new StockReportRequest
                {
                    CompanyId = Guid.Parse(selectedCompany.Id),
                    ProductFilter = string.IsNullOrWhiteSpace(txtProductFilter.Text) ? null : txtProductFilter.Text.Trim(),
                    ShowOnlyLowStock = chkShowOnlyLowStock.Checked,
                    ShowOnlyOutOfStock = chkShowOnlyOutOfStock.Checked,
                    IncludeVariants = chkIncludeVariants.Checked,
                    AsOfDate = dtpAsOfDate.Value.Date
                };

                _reportData = await _stockReportService.GetStockReportAsync(request);

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

            // Clear existing data
            dgvReport.DataSource = null;
            dgvReport.Columns.Clear();

            // Create a list to hold all stock items for display
            var allStockItems = new List<object>();

            foreach (var stockItem in _reportData.Products)
            {
                allStockItems.Add(new
                {
                    ProductCode = stockItem.ProductCode,
                    ProductName = stockItem.ProductName,
                    Unit = stockItem.Unit,
                    CurrentStock = stockItem.CurrentStock,
                    UnitCost = stockItem.UnitCost,
                    StockStatus = stockItem.StockStatus,
                    SellingPrice = stockItem.SellingPrice,
                    TotalPurchased = stockItem.TotalPurchased,
                    TotalSold = stockItem.TotalSold,
                    TotalValue = stockItem.TotalValue
                });
            }

            // Bind data to grid
            dgvReport.DataSource = allStockItems;

            // Format columns
            if (dgvReport.Columns.Count > 0)
            {
                // Format numeric columns
                if (dgvReport.Columns["CurrentStock"] != null)
                {
                    dgvReport.Columns["CurrentStock"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["CurrentStock"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvReport.Columns["UnitCost"] != null)
                {
                    dgvReport.Columns["UnitCost"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["UnitCost"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvReport.Columns["SellingPrice"] != null)
                {
                    dgvReport.Columns["SellingPrice"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["SellingPrice"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvReport.Columns["TotalPurchased"] != null)
                {
                    dgvReport.Columns["TotalPurchased"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["TotalPurchased"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvReport.Columns["TotalSold"] != null)
                {
                    dgvReport.Columns["TotalSold"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["TotalSold"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvReport.Columns["TotalValue"] != null)
                {
                    dgvReport.Columns["TotalValue"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["TotalValue"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["TotalValue"]!.DefaultCellStyle.ForeColor = Color.Blue;
                    dgvReport.Columns["TotalValue"]!.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }

                // Color code stock status
                if (dgvReport.Columns["StockStatus"] != null)
                {
                    dgvReport.Columns["StockStatus"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Set column widths
                if (dgvReport.Columns["ProductCode"] != null)
                    dgvReport.Columns["ProductCode"]!.Width = 120;
                if (dgvReport.Columns["ProductName"] != null)
                    dgvReport.Columns["ProductName"]!.Width = 200;
                if (dgvReport.Columns["Unit"] != null)
                    dgvReport.Columns["Unit"]!.Width = 80;
                if (dgvReport.Columns["CurrentStock"] != null)
                    dgvReport.Columns["CurrentStock"]!.Width = 120;
                if (dgvReport.Columns["UnitCost"] != null)
                    dgvReport.Columns["UnitCost"]!.Width = 100;
                if (dgvReport.Columns["StockStatus"] != null)
                    dgvReport.Columns["StockStatus"]!.Width = 100;
                if (dgvReport.Columns["SellingPrice"] != null)
                    dgvReport.Columns["SellingPrice"]!.Width = 100;
                if (dgvReport.Columns["TotalPurchased"] != null)
                    dgvReport.Columns["TotalPurchased"]!.Width = 120;
                if (dgvReport.Columns["TotalSold"] != null)
                    dgvReport.Columns["TotalSold"]!.Width = 120;
                if (dgvReport.Columns["TotalValue"] != null)
                    dgvReport.Columns["TotalValue"]!.Width = 120;
            }

            // Color code rows based on stock status
            dgvReport.CellFormatting += DgvReport_CellFormatting;

            // Update summary labels
            UpdateSummaryLabels();
        }

        private void DgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvReport.Columns["StockStatus"] != null)
            {
                var stockStatus = dgvReport.Rows[e.RowIndex].Cells["StockStatus"].Value?.ToString();
                
                switch (stockStatus)
                {
                    case "Out of Stock":
                        e.CellStyle.BackColor = Color.FromArgb(255, 240, 240);
                        e.CellStyle.ForeColor = Color.Red;
                        break;
                    case "Low Stock":
                        e.CellStyle.BackColor = Color.FromArgb(255, 248, 220);
                        e.CellStyle.ForeColor = Color.Orange;
                        break;
                    case "Over Stock":
                        e.CellStyle.BackColor = Color.FromArgb(240, 240, 255);
                        e.CellStyle.ForeColor = Color.Purple;
                        break;
                    case "In Stock":
                        e.CellStyle.BackColor = Color.FromArgb(240, 255, 240);
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                }
            }
        }

        private void UpdateSummaryLabels()
        {
            if (_reportData != null)
            {
                lblTotalProductsValue.Text = _reportData.TotalProducts.ToString();
                lblLowStockProductsValue.Text = _reportData.LowStockProducts.ToString();
                lblOutOfStockProductsValue.Text = _reportData.OutOfStockProducts.ToString();
                lblTotalStockValueValue.Text = _reportData.TotalStockValue.ToString("N2");
                
                // Calculate in stock products (total - low stock - out of stock)
                int inStockProducts = _reportData.TotalProducts - _reportData.LowStockProducts - _reportData.OutOfStockProducts;
                lblInStockProductsValue.Text = inStockProducts.ToString();
                
                // Products needing reorder = low stock + out of stock
                int productsNeedingReorder = _reportData.LowStockProducts + _reportData.OutOfStockProducts;
                lblProductsNeedingReorderValue.Text = productsNeedingReorder.ToString();
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtProductFilter.Clear();
            chkShowOnlyLowStock.Checked = false;
            chkShowOnlyOutOfStock.Checked = false;
            chkIncludeVariants.Checked = true;
            dtpAsOfDate.Value = DateTime.Now;
            
            dgvReport.DataSource = null;
            dgvReport.Columns.Clear();
            
            // Clear summary labels
            lblTotalProductsValue.Text = "0";
            lblInStockProductsValue.Text = "0";
            lblLowStockProductsValue.Text = "0";
            lblOutOfStockProductsValue.Text = "0";
            lblTotalStockValueValue.Text = "0.00";
            lblProductsNeedingReorderValue.Text = "0";
            
            _reportData = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterLoadingPanel();
        }
    }
}
