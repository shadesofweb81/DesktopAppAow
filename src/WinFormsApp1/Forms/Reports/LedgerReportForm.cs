using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private Label lblSummary = null!;
        private TextBox txtSummary = null!;
        
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
            reportGroupBox.Size = new Size(1150, 580);
            reportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Controls.Add(reportGroupBox);
            
            // Data Grid View
            dgvReport = new DataGridView();
            dgvReport.Location = new Point(20, 30);
            dgvReport.Size = new Size(1110, 450);
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.MultiSelect = false;
            reportGroupBox.Controls.Add(dgvReport);
            
            // Summary
            lblSummary = new Label();
            lblSummary.Text = "Summary:";
            lblSummary.Location = new Point(20, 500);
            lblSummary.Size = new Size(80, 25);
            lblSummary.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            reportGroupBox.Controls.Add(lblSummary);
            
            txtSummary = new TextBox();
            txtSummary.Location = new Point(110, 500);
            txtSummary.Size = new Size(1020, 60);
            txtSummary.Multiline = true;
            txtSummary.ReadOnly = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            reportGroupBox.Controls.Add(txtSummary);
            
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

            foreach (var monthlyData in _reportData.MonthlyData)
            {
                foreach (var transaction in monthlyData.Transactions)
                {
                    allTransactions.Add(new
                    {
                        Month = monthlyData.MonthName,
                        Year = monthlyData.Year,
                        TransactionNumber = transaction.TransactionNumber,
                        InvoiceNumber = transaction.InvoiceNumber,
                        Date = transaction.TransactionDate.ToString("dd/MM/yyyy"),
                        Type = transaction.Type,
                        Status = transaction.Status,
                        SubTotal = transaction.SubTotal,
                        TaxAmount = transaction.TaxAmount,
                        Total = transaction.Total,
                        PaidAmount = transaction.PaidAmount,
                        BalanceDue = transaction.BalanceDue
                    });
                }
            }

            // Bind data to grid
            dgvReport.DataSource = allTransactions;

            // Format columns
            if (dgvReport.Columns.Count > 0)
            {
                if (dgvReport.Columns["SubTotal"] != null)
                    dgvReport.Columns["SubTotal"].DefaultCellStyle.Format = "N2";
                if (dgvReport.Columns["TaxAmount"] != null)
                    dgvReport.Columns["TaxAmount"].DefaultCellStyle.Format = "N2";
                if (dgvReport.Columns["Total"] != null)
                    dgvReport.Columns["Total"].DefaultCellStyle.Format = "N2";
                if (dgvReport.Columns["PaidAmount"] != null)
                    dgvReport.Columns["PaidAmount"].DefaultCellStyle.Format = "N2";
                if (dgvReport.Columns["BalanceDue"] != null)
                    dgvReport.Columns["BalanceDue"].DefaultCellStyle.Format = "N2";
            }

            // Display summary
            var summary = _reportData.Summary;
            var summaryText = $"Party: {_reportData.PartyName} | " +
                             $"Total Amount: {summary.TotalAmount:N2} | " +
                             $"Total Paid: {summary.TotalPaid:N2} | " +
                             $"Total Balance: {summary.TotalBalance:N2} | " +
                             $"Total Transactions: {summary.TotalTransactions} | " +
                             $"Average Monthly Amount: {summary.AverageMonthlyAmount:N2}";

            if (!string.IsNullOrEmpty(summary.HighestMonth))
            {
                summaryText += $" | Highest Month: {summary.HighestMonth}";
            }

            if (!string.IsNullOrEmpty(summary.LowestMonth))
            {
                summaryText += $" | Lowest Month: {summary.LowestMonth}";
            }

            txtSummary.Text = summaryText;
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            _selectedLedger = null;
            txtLedger.Clear();
            dgvReport.DataSource = null;
            dgvReport.Columns.Clear();
            txtSummary.Clear();
            _reportData = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterLoadingPanel();
        }
    }
}
