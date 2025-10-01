using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Reports.TrialBalance
{
    public partial class TrialBalanceAllAccountsForm : BaseForm
    {
        // Services
        private TrialBalanceService _trialBalanceService = null!;
        private LocalStorageService _localStorageService = null!;

        // Main controls
        private GroupBox mainGroupBox = null!;
        private Label titleLabel = null!;
        private GroupBox filterGroupBox = null!;
        private Label fromDateLabel = null!;
        private DateTimePicker fromDatePicker = null!;
        private Label toDateLabel = null!;
        private DateTimePicker toDatePicker = null!;
        private Button generateButton = null!;
        private Button closeButton = null!;

        // Data display
        private DataGridView trialBalanceDataGrid = null!;
        private GroupBox summaryGroupBox = null!;
        private Label totalDebitsLabel = null!;
        private Label totalCreditsLabel = null!;
        private Label differenceLabel = null!;
        private Label isBalancedLabel = null!;

        // Data
        private TrialBalanceResponse? _trialBalanceData = null;

        public TrialBalanceAllAccountsForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            mainGroupBox = new GroupBox();
            titleLabel = new Label();
            filterGroupBox = new GroupBox();
            fromDateLabel = new Label();
            fromDatePicker = new DateTimePicker();
            toDateLabel = new Label();
            toDatePicker = new DateTimePicker();
            generateButton = new Button();
            closeButton = new Button();
            trialBalanceDataGrid = new DataGridView();
            summaryGroupBox = new GroupBox();
            totalDebitsLabel = new Label();
            totalCreditsLabel = new Label();
            differenceLabel = new Label();
            isBalancedLabel = new Label();

            mainGroupBox.SuspendLayout();
            filterGroupBox.SuspendLayout();
            summaryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trialBalanceDataGrid).BeginInit();
            SuspendLayout();

            // 
            // mainGroupBox
            // 
            mainGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            mainGroupBox.Controls.Add(titleLabel);
            mainGroupBox.Controls.Add(filterGroupBox);
            mainGroupBox.Controls.Add(trialBalanceDataGrid);
            mainGroupBox.Controls.Add(summaryGroupBox);
            mainGroupBox.Controls.Add(closeButton);
            mainGroupBox.FlatStyle = FlatStyle.Flat;
            mainGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mainGroupBox.Location = new Point(10, 10);
            mainGroupBox.Name = "mainGroupBox";
            mainGroupBox.Size = new Size(1000, 600);
            mainGroupBox.TabIndex = 0;
            mainGroupBox.TabStop = false;
            mainGroupBox.Text = "TRIAL BALANCE - ALL ACCOUNTS";

            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 102, 204);
            titleLabel.Location = new Point(10, 25);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(980, 30);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Trial Balance Report - All Accounts";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // filterGroupBox
            // 
            filterGroupBox.BackColor = Color.FromArgb(250, 250, 250);
            filterGroupBox.Controls.Add(fromDateLabel);
            filterGroupBox.Controls.Add(fromDatePicker);
            filterGroupBox.Controls.Add(toDateLabel);
            filterGroupBox.Controls.Add(toDatePicker);
            filterGroupBox.Controls.Add(generateButton);
            filterGroupBox.FlatStyle = FlatStyle.Flat;
            filterGroupBox.Font = new Font("Segoe UI", 9F);
            filterGroupBox.Location = new Point(10, 65);
            filterGroupBox.Name = "filterGroupBox";
            filterGroupBox.Size = new Size(980, 60);
            filterGroupBox.TabIndex = 1;
            filterGroupBox.TabStop = false;
            filterGroupBox.Text = "Report Filters";

            // 
            // fromDateLabel
            // 
            fromDateLabel.AutoSize = true;
            fromDateLabel.Font = new Font("Segoe UI", 9F);
            fromDateLabel.Location = new Point(10, 25);
            fromDateLabel.Name = "fromDateLabel";
            fromDateLabel.Size = new Size(70, 15);
            fromDateLabel.TabIndex = 0;
            fromDateLabel.Text = "From Date:";

            // 
            // fromDatePicker
            // 
            fromDatePicker.Font = new Font("Segoe UI", 9F);
            fromDatePicker.Format = DateTimePickerFormat.Short;
            fromDatePicker.Location = new Point(90, 22);
            fromDatePicker.Name = "fromDatePicker";
            fromDatePicker.Size = new Size(120, 23);
            fromDatePicker.TabIndex = 1;

            // 
            // toDateLabel
            // 
            toDateLabel.AutoSize = true;
            toDateLabel.Font = new Font("Segoe UI", 9F);
            toDateLabel.Location = new Point(230, 25);
            toDateLabel.Name = "toDateLabel";
            toDateLabel.Size = new Size(55, 15);
            toDateLabel.TabIndex = 2;
            toDateLabel.Text = "To Date:";

            // 
            // toDatePicker
            // 
            toDatePicker.Font = new Font("Segoe UI", 9F);
            toDatePicker.Format = DateTimePickerFormat.Short;
            toDatePicker.Location = new Point(290, 22);
            toDatePicker.Name = "toDatePicker";
            toDatePicker.Size = new Size(120, 23);
            toDatePicker.TabIndex = 3;

            // 
            // generateButton
            // 
            generateButton.BackColor = Color.FromArgb(0, 102, 204);
            generateButton.FlatStyle = FlatStyle.Flat;
            generateButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            generateButton.ForeColor = Color.White;
            generateButton.Location = new Point(430, 20);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(100, 30);
            generateButton.TabIndex = 4;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = false;
            generateButton.Click += GenerateButton_Click;

            // 
            // trialBalanceDataGrid
            // 
            trialBalanceDataGrid.AllowUserToAddRows = false;
            trialBalanceDataGrid.AllowUserToDeleteRows = false;
            trialBalanceDataGrid.BackgroundColor = Color.White;
            trialBalanceDataGrid.BorderStyle = BorderStyle.Fixed3D;
            trialBalanceDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            trialBalanceDataGrid.Font = new Font("Segoe UI", 9F);
            trialBalanceDataGrid.Location = new Point(10, 135);
            trialBalanceDataGrid.Name = "trialBalanceDataGrid";
            trialBalanceDataGrid.ReadOnly = true;
            trialBalanceDataGrid.RowHeadersVisible = false;
            trialBalanceDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            trialBalanceDataGrid.Size = new Size(980, 350);
            trialBalanceDataGrid.TabIndex = 2;
            trialBalanceDataGrid.TabStop = false;

            // 
            // summaryGroupBox
            // 
            summaryGroupBox.BackColor = Color.FromArgb(250, 250, 250);
            summaryGroupBox.Controls.Add(totalDebitsLabel);
            summaryGroupBox.Controls.Add(totalCreditsLabel);
            summaryGroupBox.Controls.Add(differenceLabel);
            summaryGroupBox.Controls.Add(isBalancedLabel);
            summaryGroupBox.FlatStyle = FlatStyle.Flat;
            summaryGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            summaryGroupBox.Location = new Point(10, 495);
            summaryGroupBox.Name = "summaryGroupBox";
            summaryGroupBox.Size = new Size(980, 50);
            summaryGroupBox.TabIndex = 3;
            summaryGroupBox.TabStop = false;
            summaryGroupBox.Text = "Summary";

            // 
            // totalDebitsLabel
            // 
            totalDebitsLabel.AutoSize = true;
            totalDebitsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            totalDebitsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            totalDebitsLabel.Location = new Point(10, 20);
            totalDebitsLabel.Name = "totalDebitsLabel";
            totalDebitsLabel.Size = new Size(80, 15);
            totalDebitsLabel.TabIndex = 0;
            totalDebitsLabel.Text = "Total Debits:";

            // 
            // totalCreditsLabel
            // 
            totalCreditsLabel.AutoSize = true;
            totalCreditsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            totalCreditsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            totalCreditsLabel.Location = new Point(200, 20);
            totalCreditsLabel.Name = "totalCreditsLabel";
            totalCreditsLabel.Size = new Size(85, 15);
            totalCreditsLabel.TabIndex = 1;
            totalCreditsLabel.Text = "Total Credits:";

            // 
            // differenceLabel
            // 
            differenceLabel.AutoSize = true;
            differenceLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            differenceLabel.ForeColor = Color.FromArgb(0, 102, 204);
            differenceLabel.Location = new Point(400, 20);
            differenceLabel.Name = "differenceLabel";
            differenceLabel.Size = new Size(70, 15);
            differenceLabel.TabIndex = 2;
            differenceLabel.Text = "Difference:";

            // 
            // isBalancedLabel
            // 
            isBalancedLabel.AutoSize = true;
            isBalancedLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            isBalancedLabel.ForeColor = Color.FromArgb(0, 102, 204);
            isBalancedLabel.Location = new Point(600, 20);
            isBalancedLabel.Name = "isBalancedLabel";
            isBalancedLabel.Size = new Size(80, 15);
            isBalancedLabel.TabIndex = 3;
            isBalancedLabel.Text = "Is Balanced:";

            // 
            // closeButton
            // 
            closeButton.BackColor = Color.FromArgb(220, 53, 69);
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            closeButton.ForeColor = Color.White;
            closeButton.Location = new Point(890, 555);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(100, 30);
            closeButton.TabIndex = 4;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = false;
            closeButton.Click += CloseButton_Click;

            // 
            // TrialBalanceAllAccountsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(1020, 620);
            Controls.Add(mainGroupBox);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "TrialBalanceAllAccountsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Trial Balance - All Accounts";
            WindowState = FormWindowState.Maximized;

            mainGroupBox.ResumeLayout(false);
            filterGroupBox.ResumeLayout(false);
            filterGroupBox.PerformLayout();
            summaryGroupBox.ResumeLayout(false);
            summaryGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trialBalanceDataGrid).EndInit();
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Initialize services
            _localStorageService = new LocalStorageService();
            _trialBalanceService = new TrialBalanceService(new AuthService());

            // Set default dates
            var currentDate = DateTime.Now;
            fromDatePicker.Value = new DateTime(currentDate.Year, 4, 1); // Start of financial year
            toDatePicker.Value = currentDate;

            // Setup DataGridView
            SetupDataGridView();

            // Load initial data
            LoadTrialBalanceData();
        }

        private void SetupDataGridView()
        {
            trialBalanceDataGrid.Columns.Clear();

            // Add columns based on the actual API response structure
            var accountCodeColumn = new DataGridViewTextBoxColumn
            {
                Name = "AccountCode",
                HeaderText = "Code",
                Width = 80,
                ReadOnly = true
            };

            var accountNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "AccountName",
                HeaderText = "Account Name",
                Width = 250,
                ReadOnly = true
            };

            var accountTypeColumn = new DataGridViewTextBoxColumn
            {
                Name = "AccountType",
                HeaderText = "Type",
                Width = 100,
                ReadOnly = true
            };

            var openingBalanceColumn = new DataGridViewTextBoxColumn
            {
                Name = "OpeningBalance",
                HeaderText = "Opening Balance",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };

            var debitAmountColumn = new DataGridViewTextBoxColumn
            {
                Name = "DebitAmount",
                HeaderText = "Debit Amount",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            };

            var creditAmountColumn = new DataGridViewTextBoxColumn
            {
                Name = "CreditAmount",
                HeaderText = "Credit Amount",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            };

            var closingBalanceColumn = new DataGridViewTextBoxColumn
            {
                Name = "ClosingBalance",
                HeaderText = "Closing Balance",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };

            trialBalanceDataGrid.Columns.AddRange(new DataGridViewColumn[]
            {
                accountCodeColumn, accountNameColumn, accountTypeColumn,
                openingBalanceColumn, debitAmountColumn, creditAmountColumn, closingBalanceColumn
            });

            // Set alternating row colors
            trialBalanceDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
        }

        private async void LoadTrialBalanceData()
        {
            try
            {
                // Get company and financial year from local storage
                var company = await _localStorageService.GetSelectedCompanyAsync();
                var financialYear = await _localStorageService.GetSelectedFinancialYearAsync();

                if (company == null || financialYear == null)
                {
                    MessageBox.Show("Please select a company and financial year first.", "Selection Required", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Show loading
                this.Cursor = Cursors.WaitCursor;
                generateButton.Enabled = false;

                // Fetch trial balance data
                if (!Guid.TryParse(company.Id, out var companyGuid))
                {
                    MessageBox.Show("Invalid company ID format.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _trialBalanceData = await _trialBalanceService.GetTrialBalanceAllAccountsAsync(
                    companyGuid, fromDatePicker.Value, toDatePicker.Value, false, null, null);

                if (_trialBalanceData != null)
                {
                    PopulateDataGrid();
                    UpdateSummary();
                }
                else
                {
                    MessageBox.Show("Failed to load trial balance data.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trial balance data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                generateButton.Enabled = true;
            }
        }

        private void PopulateDataGrid()
        {
            if (_trialBalanceData?.Accounts == null) return;

            trialBalanceDataGrid.Rows.Clear();

            foreach (var account in _trialBalanceData.Accounts)
            {
                var row = new DataGridViewRow();
                row.CreateCells(trialBalanceDataGrid);

                // Format opening balance with type
                string openingBalanceText = account.OpeningBalance == 0 ? "-" : 
                    $"{account.OpeningBalance:N2} {account.OpeningBalanceType}";

                // Format closing balance with type
                string closingBalanceText = account.ClosingBalance == 0 ? "-" : 
                    $"{account.ClosingBalance:N2} {account.ClosingBalanceType}";

                row.Cells[0].Value = account.AccountCode;
                row.Cells[1].Value = account.AccountName;
                row.Cells[2].Value = account.AccountType;
                row.Cells[3].Value = openingBalanceText;
                row.Cells[4].Value = account.DebitAmount;
                row.Cells[5].Value = account.CreditAmount;
                row.Cells[6].Value = closingBalanceText;

                // Color code based on account type
                if (account.AccountType == "Asset" || account.AccountType == "Expense")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245); // Light red for assets/expenses
                }
                else if (account.AccountType == "Liability" || account.AccountType == "Equity" || account.AccountType == "Income")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 255, 245); // Light green for liabilities/equity/income
                }

                trialBalanceDataGrid.Rows.Add(row);
            }
        }

        private void UpdateSummary()
        {
            if (_trialBalanceData?.Summary == null) return;

            var summary = _trialBalanceData.Summary;

            totalDebitsLabel.Text = $"Total Debits: {summary.TotalDebitAmount:N2}";
            totalCreditsLabel.Text = $"Total Credits: {summary.TotalCreditAmount:N2}";
            differenceLabel.Text = $"Difference: {summary.BalanceDifference:N2}";
            isBalancedLabel.Text = $"Is Balanced: {(summary.IsBalanced ? "Yes" : "No")}";

            // Color code the balance status
            if (summary.IsBalanced)
            {
                isBalancedLabel.ForeColor = Color.Green;
            }
            else
            {
                isBalancedLabel.ForeColor = Color.Red;
            }
        }

        private void GenerateButton_Click(object? sender, EventArgs e)
        {
            LoadTrialBalanceData();
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void HandleEscapeKey()
        {
            this.Close();
        }

        protected override bool HandleEnterKey()
        {
            // Handle Enter key for button activation
            var focusedControl = this.ActiveControl;
            if (focusedControl is Button button)
            {
                button.PerformClick();
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _trialBalanceService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
