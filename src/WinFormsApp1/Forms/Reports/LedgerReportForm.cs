
using WinFormsApp1.Models;
using WinFormsApp1.Services;
using WinFormsApp1.Forms.Transaction;

namespace WinFormsApp1.Forms.Reports
{
    public partial class LedgerReportForm : Form
    {
        private readonly LedgerService _ledgerService;
        private readonly LocalStorageService _localStorageService;
        
        // Controls
        private GroupBox criteriaGroupBox = null!;
        private Label lblLedger = null!;
        private TextBox txtLedger = null!;
        private Button btnSelectLedger = null!;
        private Label lblFromDate = null!;
        private DateTimePicker dtpFromDate = null!;
        private Label lblToDate = null!;
        private DateTimePicker dtpToDate = null!;
        private Button btnGenerateReport = null!;
        private Button btnClear = null!;
        
        private GroupBox reportGroupBox = null!;
        private DataGridView dgvReport = null!;
        private GroupBox summaryGroupBox = null!;
        private Label lblTotalCredit = null!;
        private Label lblTotalCreditValue = null!;
        private Label lblTotalDebit = null!;
        private Label lblTotalDebitValue = null!;
        private Label lblBalance = null!;
        private Label lblBalanceValue = null!;
        private Label lblTransactionCount = null!;
        private Label lblTransactionCountValue = null!;
        
        private Panel loadingPanel = null!;
        private ProgressBar loadingProgressBar = null!;
        private Label loadingLabel = null!;
        
        // Data
        private List<LedgerModel> _availableLedgers = new List<LedgerModel>();
        private LedgerModel? _selectedLedger;
        private LedgerReportResponse? _reportData;

        public LedgerReportForm(LedgerService ledgerService, LocalStorageService localStorageService)
        {
            _ledgerService = ledgerService;
            _localStorageService = localStorageService;
            
            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Ledger Report";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            
            // Criteria Group Box
            criteriaGroupBox = new GroupBox();
            criteriaGroupBox.Text = "Report Criteria";
            criteriaGroupBox.Location = new Point(20, 20);
            criteriaGroupBox.Size = new Size(1150, 120);
            criteriaGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(criteriaGroupBox);
            
            // Ledger Selection
            lblLedger = new Label();
            lblLedger.Text = "Select Ledger:";
            lblLedger.Location = new Point(20, 30);
            lblLedger.Size = new Size(100, 25);
            lblLedger.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblLedger);
            
            txtLedger = new TextBox();
            txtLedger.Location = new Point(130, 30);
            txtLedger.Size = new Size(300, 25);
            txtLedger.ReadOnly = true;
            txtLedger.PlaceholderText = "Click 'Select Ledger' to choose a ledger";
            criteriaGroupBox.Controls.Add(txtLedger);
            
            btnSelectLedger = new Button();
            btnSelectLedger.Text = "Select Ledger";
            btnSelectLedger.Location = new Point(450, 30);
            btnSelectLedger.Size = new Size(120, 30);
            btnSelectLedger.Click += BtnSelectLedger_Click;
            criteriaGroupBox.Controls.Add(btnSelectLedger);
            
            // From Date
            lblFromDate = new Label();
            lblFromDate.Text = "From Date:";
            lblFromDate.Location = new Point(20, 70);
            lblFromDate.Size = new Size(80, 25);
            lblFromDate.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblFromDate);
            
            dtpFromDate = new DateTimePicker();
            dtpFromDate.Location = new Point(110, 70);
            dtpFromDate.Size = new Size(150, 25);
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Value = DateTime.Now.AddMonths(-6); // Default to 6 months ago
            criteriaGroupBox.Controls.Add(dtpFromDate);
            
            // To Date
            lblToDate = new Label();
            lblToDate.Text = "To Date:";
            lblToDate.Location = new Point(280, 70);
            lblToDate.Size = new Size(60, 25);
            lblToDate.Font = new Font("Segoe UI", 9F);
            criteriaGroupBox.Controls.Add(lblToDate);
            
            dtpToDate = new DateTimePicker();
            dtpToDate.Location = new Point(350, 70);
            dtpToDate.Size = new Size(150, 25);
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Value = DateTime.Now; // Default to today
            criteriaGroupBox.Controls.Add(dtpToDate);
            
            // Buttons
            btnGenerateReport = new Button();
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.Location = new Point(520, 70);
            btnGenerateReport.Size = new Size(120, 30);
            btnGenerateReport.BackColor = Color.FromArgb(0, 120, 215);
            btnGenerateReport.ForeColor = Color.White;
            btnGenerateReport.Click += BtnGenerateReport_Click;
            criteriaGroupBox.Controls.Add(btnGenerateReport);
            
            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(660, 70);
            btnClear.Size = new Size(80, 30);
            btnClear.Click += BtnClear_Click;
            criteriaGroupBox.Controls.Add(btnClear);
            
            // Report Group Box
            reportGroupBox = new GroupBox();
            reportGroupBox.Text = "Report Results";
            reportGroupBox.Location = new Point(20, 160);
            reportGroupBox.Size = new Size(1150, 600);
            reportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(reportGroupBox);
            
            // Data Grid View
            dgvReport = new DataGridView();
            dgvReport.Location = new Point(20, 30);
            dgvReport.Size = new Size(1110, 400);
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            dgvReport.GridColor = Color.FromArgb(200, 200, 200);
            reportGroupBox.Controls.Add(dgvReport);
            
            // Summary Group Box
            summaryGroupBox = new GroupBox();
            summaryGroupBox.Text = "Summary";
            summaryGroupBox.Location = new Point(20, 450);
            summaryGroupBox.Size = new Size(1110, 100);
            summaryGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            reportGroupBox.Controls.Add(summaryGroupBox);
            
            // Calculate approximate column positions based on grid layout
            // Grid columns: TransactionNumber, InvoiceNumber, Date, Type, Status, Total, PaidAmount, BalanceDue, Debit, Credit
            // Debit column is approximately at 80% of grid width, Credit at 90%
            int debitColumnX = 800;  // Approximate position of Debit column
            int creditColumnX = 950; // Approximate position of Credit column
            
            // Total Debit (aligned with grid Debit column) - Wider labels for better visibility
            lblTotalDebit = new Label();
            lblTotalDebit.Text = "Total Debit:";
            lblTotalDebit.Location = new Point(debitColumnX - 100, 30);
            lblTotalDebit.Size = new Size(100, 25);
            lblTotalDebit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalDebit.ForeColor = Color.Green;
            lblTotalDebit.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblTotalDebit);
            
            lblTotalDebitValue = new Label();
            lblTotalDebitValue.Text = "0.00";
            lblTotalDebitValue.Location = new Point(debitColumnX, 30);
            lblTotalDebitValue.Size = new Size(120, 25);
            lblTotalDebitValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalDebitValue.ForeColor = Color.Green;
            lblTotalDebitValue.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblTotalDebitValue);
            
            // Total Credit (aligned with grid Credit column) - Wider labels for better visibility
            lblTotalCredit = new Label();
            lblTotalCredit.Text = "Total Credit:";
            lblTotalCredit.Location = new Point(creditColumnX - 100, 30);
            lblTotalCredit.Size = new Size(100, 25);
            lblTotalCredit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalCredit.ForeColor = Color.Red;
            lblTotalCredit.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblTotalCredit);
            
            lblTotalCreditValue = new Label();
            lblTotalCreditValue.Text = "0.00";
            lblTotalCreditValue.Location = new Point(creditColumnX, 30);
            lblTotalCreditValue.Size = new Size(120, 25);
            lblTotalCreditValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalCreditValue.ForeColor = Color.Red;
            lblTotalCreditValue.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblTotalCreditValue);
            
            // Transaction Count (on left side of summary, fully visible)
            lblTransactionCount = new Label();
            lblTransactionCount.Text = "Total Transactions:";
            lblTransactionCount.Location = new Point(20, 60);
            lblTransactionCount.Size = new Size(120, 25);
            lblTransactionCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTransactionCount);
            
            lblTransactionCountValue = new Label();
            lblTransactionCountValue.Text = "0";
            lblTransactionCountValue.Location = new Point(150, 60);
            lblTransactionCountValue.Size = new Size(50, 25);
            lblTransactionCountValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Controls.Add(lblTransactionCountValue);
            
            // Balance (on next line, aligned with the totals) - Wider labels for better visibility
            lblBalance = new Label();
            lblBalance.Text = "Balance:";
            lblBalance.Location = new Point(debitColumnX - 100, 60);
            lblBalance.Size = new Size(100, 25);
            lblBalance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBalance.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblBalance);
            
            lblBalanceValue = new Label();
            lblBalanceValue.Text = "0.00";
            lblBalanceValue.Location = new Point(debitColumnX, 60);
            lblBalanceValue.Size = new Size(120, 25);
            lblBalanceValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBalanceValue.TextAlign = ContentAlignment.MiddleRight;
            summaryGroupBox.Controls.Add(lblBalanceValue);
            
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
            dtpFromDate.Value = DateTime.Now.AddMonths(-6);
            dtpToDate.Value = DateTime.Now;
            
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

        private async void LoadData()
        {
            try
            {
                ShowLoadingPanel("Loading ledgers...");
                
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany == null)
                {
                    MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    HideLoadingPanel();
                    return;
                }

                var companyId = Guid.Parse(selectedCompany.Id);
                _availableLedgers = await _ledgerService.GetAllLedgersAsync(companyId);
                
                HideLoadingPanel();
            }
            catch (Exception ex)
            {
                HideLoadingPanel();
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void BtnSelectLedger_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_availableLedgers == null || !_availableLedgers.Any())
                {
                    MessageBox.Show("No ledgers available. Please ensure you have ledgers created.", "No Ledgers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialog = new LedgerSelectionDialog(
                    _availableLedgers,
                    _ledgerService,
                    (await _localStorageService.GetSelectedCompanyAsync())?.Id ?? "",
                    "Select Ledger for Report",
                    "All Ledgers",
                    "Party Ledgers"
                );

                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    _selectedLedger = dialog.SelectedLedger;
                    txtLedger.Text = $"{_selectedLedger.Name} ({_selectedLedger.Code})";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnGenerateReport_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (_selectedLedger == null)
                {
                    MessageBox.Show("Please select a ledger first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("From Date cannot be greater than To Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany == null)
                {
                    MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowLoadingPanel("Generating report...");

                var request = new LedgerReportRequest
                {
                    CompanyId = selectedCompany.Id,
                    PartyLedgerId = _selectedLedger.Id.ToString(),
                    FromDate = dtpFromDate.Value.Date,
                    ToDate = dtpToDate.Value.Date
                };

                _reportData = await _ledgerService.GetLedgerReportAsync(request);

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

            // Create a list to hold all transactions for display
            var allTransactions = new List<object>();
            decimal totalCredit = 0;
            decimal totalDebit = 0;
            int totalTransactionCount = _reportData.Transactions.Count;

            // Track running balance starting from opening balance
            decimal runningBalance = _reportData.OpeningBalance;
            bool isOpeningDebit = _reportData.OpeningBalanceType.Equals("Dr", StringComparison.OrdinalIgnoreCase);
            
            foreach (var transaction in _reportData.Transactions)
            {
                // Use the direct debit/credit amounts from the API response
                decimal creditAmount = 0;
                decimal debitAmount = 0;
                
                if (transaction.EntryType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                {
                    debitAmount = transaction.Amount;
                    totalDebit += debitAmount;
                }
                else if (transaction.EntryType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
                {
                    creditAmount = transaction.Amount;
                    totalCredit += creditAmount;
                }

                // Check if this is the opening balance transaction
                bool isOpeningBalanceTransaction = transaction.PartyName.Equals("Opening Balance Adjustment", StringComparison.OrdinalIgnoreCase) || 
                    transaction.Notes.Contains("Opening Balance", StringComparison.OrdinalIgnoreCase) ||
                    (transaction.Type.Equals("JournalEntry", StringComparison.OrdinalIgnoreCase) && 
                     transaction.PartyName.Equals("Opening Balance Adjustment", StringComparison.OrdinalIgnoreCase));

                decimal balanceToShow;
                if (isOpeningBalanceTransaction)
                {
                    // For opening balance transaction, show the opening balance
                    balanceToShow = _reportData.OpeningBalance;
                }
                else
                {
                    // For subsequent transactions, calculate running balance properly
                    if (transaction.EntryType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                    {
                        // Debit increases the balance if opening is debit, decreases if opening is credit
                        if (isOpeningDebit)
                            runningBalance += debitAmount;
                        else
                            runningBalance -= debitAmount;
                    }
                    else if (transaction.EntryType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
                    {
                        // Credit decreases the balance if opening is debit, increases if opening is credit
                        if (isOpeningDebit)
                            runningBalance -= creditAmount;
                        else
                            runningBalance += creditAmount;
                    }
                    balanceToShow = Math.Abs(runningBalance);
                }

                allTransactions.Add(new
                {
                    TransactionNumber = transaction.TransactionNumber,
                    InvoiceNumber = transaction.InvoiceNumber ?? "",
                    Date = transaction.TransactionDate.ToString("dd/MM/yyyy"),
                    Type = transaction.Type,
                    Status = transaction.Status,
                    PartyName = transaction.PartyName,
                    Debit = debitAmount,
                    Credit = creditAmount,
                    Balance = balanceToShow
                });
            }

            // Bind data to grid
            dgvReport.DataSource = allTransactions;

            // Format columns and set widths
            if (dgvReport.Columns.Count > 0)
            {
                if (dgvReport.Columns["Debit"] != null)
                {
                    dgvReport.Columns["Debit"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["Debit"]!.DefaultCellStyle.ForeColor = Color.Green;
                    dgvReport.Columns["Debit"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["Debit"]!.Width = 120; // More spacious
                }
                if (dgvReport.Columns["Credit"] != null)
                {
                    dgvReport.Columns["Credit"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["Credit"]!.DefaultCellStyle.ForeColor = Color.Red;
                    dgvReport.Columns["Credit"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["Credit"]!.Width = 120; // More spacious
                }
                if (dgvReport.Columns["Balance"] != null)
                {
                    dgvReport.Columns["Balance"]!.DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["Balance"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["Balance"]!.Width = 120;
                    dgvReport.Columns["Balance"]!.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                
                // Set other column widths for better layout
                if (dgvReport.Columns["TransactionNumber"] != null)
                    dgvReport.Columns["TransactionNumber"]!.Width = 120;
                if (dgvReport.Columns["InvoiceNumber"] != null)
                    dgvReport.Columns["InvoiceNumber"]!.Width = 120;
                if (dgvReport.Columns["Date"] != null)
                    dgvReport.Columns["Date"]!.Width = 100;
                if (dgvReport.Columns["Type"] != null)
                    dgvReport.Columns["Type"]!.Width = 120;
                if (dgvReport.Columns["Status"] != null)
                    dgvReport.Columns["Status"]!.Width = 100;
                if (dgvReport.Columns["PartyName"] != null)
                    dgvReport.Columns["PartyName"]!.Width = 150;
            }

            // Align summary with grid columns after data is bound
            AlignSummaryWithGridColumns();

            // Use the totals directly from the API response
            decimal apiTotalDebits = _reportData.TotalDebits;
            decimal apiTotalCredits = _reportData.TotalCredits;
            decimal apiClosingBalance = _reportData.ClosingBalance;
            string apiClosingBalanceType = _reportData.ClosingBalanceType;

            // Format closing balance with type
            string balanceText = $"{apiClosingBalance:N2} {apiClosingBalanceType}";
            Color balanceColor = apiClosingBalanceType.Equals("Dr", StringComparison.OrdinalIgnoreCase) ? Color.Green : Color.Red;

            // Update summary labels with API response totals
            lblTotalDebitValue.Text = apiTotalDebits.ToString("N2");
            lblTotalCreditValue.Text = apiTotalCredits.ToString("N2");
            lblBalanceValue.Text = balanceText;
            lblBalanceValue.ForeColor = balanceColor;
            lblTransactionCountValue.Text = totalTransactionCount.ToString();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            _selectedLedger = null;
            txtLedger.Clear();
            dgvReport.DataSource = null;
            dgvReport.Columns.Clear();
            
            // Clear summary labels
            lblTotalDebitValue.Text = "0.00";
            lblTotalCreditValue.Text = "0.00";
            lblBalanceValue.Text = "0.00";
            lblBalanceValue.ForeColor = Color.Black;
            lblTransactionCountValue.Text = "0";
            
            _reportData = null;
        }

        private void AlignSummaryWithGridColumns()
        {
            // Wait for the grid to finish rendering
            Application.DoEvents();
            
            if (dgvReport.Columns.Count > 0)
            {
                // Find the Debit and Credit column positions
                var debitColumn = dgvReport.Columns["Debit"];
                var creditColumn = dgvReport.Columns["Credit"];
                
                if (debitColumn != null && creditColumn != null)
                {
                    // Calculate the actual column positions
                    int gridLeft = dgvReport.Left;
                    int summaryLeft = summaryGroupBox.Left;
                    
                    // Get column positions relative to the grid
                    int debitColumnLeft = 0;
                    int creditColumnLeft = 0;
                    
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        if (dgvReport.Columns[i].Name == "Debit")
                        {
                            debitColumnLeft = GetColumnPosition(i);
                            break;
                        }
                    }
                    
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        if (dgvReport.Columns[i].Name == "Credit")
                        {
                            creditColumnLeft = GetColumnPosition(i);
                            break;
                        }
                    }
                    
                    // Adjust label positions to align with columns
                    int debitX = debitColumnLeft - summaryLeft + gridLeft;
                    int creditX = creditColumnLeft - summaryLeft + gridLeft;
                    
                    // Position the debit labels (wider labels)
                    lblTotalDebitValue.Location = new Point(debitX, lblTotalDebitValue.Location.Y);
                    lblTotalDebit.Location = new Point(debitX - 100, lblTotalDebit.Location.Y);
                    
                    // Position the credit labels (wider labels)
                    lblTotalCreditValue.Location = new Point(creditX, lblTotalCreditValue.Location.Y);
                    lblTotalCredit.Location = new Point(creditX - 100, lblTotalCredit.Location.Y);
                    
                    // Position the balance labels (aligned with debit column, wider labels)
                    lblBalanceValue.Location = new Point(debitX, lblBalanceValue.Location.Y);
                    lblBalance.Location = new Point(debitX - 100, lblBalance.Location.Y);
                    
                    // Transaction count stays on the left side (no need to reposition)
                }
            }
        }
        
        private int GetColumnPosition(int columnIndex)
        {
            int position = 0;
            for (int i = 0; i < columnIndex; i++)
            {
                if (dgvReport.Columns[i].Visible)
                {
                    position += dgvReport.Columns[i].Width;
                }
            }
            return position;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterLoadingPanel();
            
            // Realign summary when form is resized
            if (dgvReport != null && summaryGroupBox != null)
            {
                AlignSummaryWithGridColumns();
            }
        }
    }
}
