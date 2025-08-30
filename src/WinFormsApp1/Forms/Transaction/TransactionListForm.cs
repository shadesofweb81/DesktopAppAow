using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class TransactionListForm : Form
    {
        private readonly TransactionService _transactionService;
        private readonly LocalStorageService _localStorageService;
        private readonly ProductService _productService;
        private readonly TaxService _taxService;
        private readonly LedgerService _ledgerService;
        private List<TransactionListDto> _transactions = new List<TransactionListDto>();
        private TransactionListDto? _selectedTransaction;

        // Form controls
        private DataGridView dgvTransactions = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblCompanyInfo = null!;

        // Pagination fields
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalCount = 0;
        private int _totalPages = 0;

        // Company and Financial Year info
        private Models.Company? _selectedCompany;
        private FinancialYearModel? _selectedFinancialYear;

        public TransactionListForm(TransactionService transactionService, LocalStorageService localStorageService,
            ProductService? productService = null, TaxService? taxService = null, LedgerService? ledgerService = null)
        {
            _transactionService = transactionService;
            _localStorageService = localStorageService;
            
            // Create services if not provided
            var authService = new AuthService();
            _productService = productService ?? new ProductService(authService);
            _taxService = taxService ?? new TaxService(authService);
            _ledgerService = ledgerService ?? new LedgerService(authService);
            
            InitializeComponent();
            _ = LoadCompanyAndFinancialYearAsync();
        }

        private void InitializeComponent()
        {
            dgvTransactions = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnView = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            lblCompanyInfo = new Label();

            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 650);
            Name = "TransactionListForm";
            Text = "Transaction Management";
            StartPosition = FormStartPosition.CenterParent;

            // Company Info Label
            lblCompanyInfo.Location = new Point(20, 20);
            lblCompanyInfo.Size = new Size(800, 25);
            lblCompanyInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCompanyInfo.ForeColor = Color.DarkBlue;

            // DataGridView
            dgvTransactions.Location = new Point(20, 60);
            dgvTransactions.Size = new Size(1160, 500);
            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionChanged += DgvTransactions_SelectionChanged;
            dgvTransactions.KeyDown += DgvTransactions_KeyDown;

            // Setup columns
            SetupDataGridViewColumns();

            // Buttons
            btnNew.Location = new Point(20, 580);
            btnNew.Size = new Size(80, 35);
            btnNew.Text = "&New";
            btnNew.Click += BtnNew_Click;

            btnEdit.Location = new Point(110, 580);
            btnEdit.Size = new Size(80, 35);
            btnEdit.Text = "&Edit";
            btnEdit.Click += BtnEdit_Click;

            btnView.Location = new Point(200, 580);
            btnView.Size = new Size(80, 35);
            btnView.Text = "&View";
            btnView.Click += BtnView_Click;

            btnDelete.Location = new Point(290, 580);
            btnDelete.Size = new Size(80, 35);
            btnDelete.Text = "&Delete";
            btnDelete.Click += BtnDelete_Click;

            btnRefresh.Location = new Point(380, 580);
            btnRefresh.Size = new Size(80, 35);
            btnRefresh.Text = "&Refresh";
            btnRefresh.Click += BtnRefresh_Click;

            // Status Label
            lblStatus.Location = new Point(480, 580);
            lblStatus.Size = new Size(400, 35);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            Controls.AddRange(new Control[] { lblCompanyInfo, dgvTransactions, btnNew, btnEdit, btnView, btnDelete, btnRefresh, lblStatus });

            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupDataGridViewColumns()
        {
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionNumber",
                HeaderText = "Transaction #",
                DataPropertyName = "TransactionNumber",
                Width = 120
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InvoiceNumber",
                HeaderText = "Invoice #",
                DataPropertyName = "InvoiceNumber",
                Width = 120
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                HeaderText = "Type",
                DataPropertyName = "Type",
                Width = 100
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionDate",
                HeaderText = "Date",
                DataPropertyName = "TransactionDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PartyName",
                HeaderText = "Party",
                DataPropertyName = "PartyName",
                Width = 150
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubTotal",
                HeaderText = "Subtotal",
                DataPropertyName = "SubTotal",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxAmount",
                HeaderText = "Tax",
                DataPropertyName = "TaxAmount",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 80
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IsPaid",
                HeaderText = "Paid",
                DataPropertyName = "IsPaid",
                Width = 60
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });
        }

        private async Task LoadCompanyAndFinancialYearAsync()
        {
            try
            {
                Console.WriteLine("Loading company and financial year from local storage...");
                
                _selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                _selectedFinancialYear = await _localStorageService.GetSelectedFinancialYearAsync();

                Console.WriteLine($"Selected Company: {_selectedCompany?.DisplayName ?? "null"}");
                Console.WriteLine($"Selected Financial Year: {_selectedFinancialYear?.YearLabel ?? "null"}");

                if (_selectedCompany != null && _selectedFinancialYear != null)
                {
                    lblCompanyInfo.Text = $"Company: {_selectedCompany.DisplayName} | Financial Year: {_selectedFinancialYear.YearLabel}";
                    await LoadTransactionsAsync();
                }
                else
                {
                    lblCompanyInfo.Text = "Please select a company and financial year";
                    lblStatus.Text = "No company or financial year selected";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading company and financial year: {ex.Message}");
                lblCompanyInfo.Text = "Error loading company information";
                lblStatus.Text = "Error loading company information";
            }
        }

        private async Task LoadTransactionsAsync()
        {
            try
            {
                if (_selectedCompany == null)
                {
                    lblStatus.Text = "No company selected";
                    return;
                }

                lblStatus.Text = "Loading transactions...";
                Application.DoEvents();

                var companyId = Guid.Parse(_selectedCompany.Id);
                var financialYearId = _selectedFinancialYear.Id;
                Console.WriteLine($"Loading transactions for company: {companyId}, financial year: {financialYearId}");
                
                _transactions = await _transactionService.GetTransactionListAsync(companyId, financialYearId, _currentPage, _pageSize);
                Console.WriteLine($"Loaded {_transactions.Count} transactions");

                dgvTransactions.DataSource = _transactions;
                lblStatus.Text = $"Loaded {_transactions.Count} transactions";

                // Select first row if transactions exist
                if (_transactions.Count > 0)
                {
                    dgvTransactions.Rows[0].Selected = true;
                    _selectedTransaction = _transactions[0];
                }

                // Update button states
                UpdateButtonStates();

                // Focus edit button if a transaction is selected
                if (_selectedTransaction != null)
                {
                    btnEdit.Focus();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                lblStatus.Text = $"Error loading transactions: {ex.Message}";
                MessageBox.Show($"Error loading transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonStates()
        {
            btnEdit.Enabled = _selectedTransaction != null;
            btnView.Enabled = _selectedTransaction != null;
            btnDelete.Enabled = _selectedTransaction != null;
        }

        private void DgvTransactions_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count > 0)
            {
                _selectedTransaction = dgvTransactions.SelectedRows[0].DataBoundItem as TransactionListDto;
            }
            else
            {
                _selectedTransaction = null;
            }
            UpdateButtonStates();
        }

        private void DgvTransactions_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _selectedTransaction != null)
            {
                e.Handled = true; // Prevent default behavior
                BtnEdit_Click(sender, e);
            }
        }

        private void BtnNew_Click(object? sender, EventArgs e)
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var transactionEditForm = new TransactionEditForm(
                    _transactionService, 
                    _localStorageService, 
                    null, // New transaction
                    _selectedCompany, 
                    _selectedFinancialYear,
                    _productService,
                    _taxService,
                    _ledgerService
                );

                if (transactionEditForm.ShowDialog() == DialogResult.OK)
                {
                    _ = LoadTransactionsAsync(); // Refresh the list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening transaction edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction == null)
            {
                MessageBox.Show("Please select a transaction to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Load the full transaction details
                var transaction = await LoadFullTransaction(_selectedTransaction.Id);
                if (transaction != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transaction, 
                        _selectedCompany!, 
                        _selectedFinancialYear!,
                        _productService,
                        _taxService,
                        _ledgerService
                    );

                    if (transactionEditForm.ShowDialog() == DialogResult.OK)
                    {
                        _ = LoadTransactionsAsync(); // Refresh the list
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening transaction edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnView_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction == null)
            {
                MessageBox.Show("Please select a transaction to view.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Load the full transaction details
                var transaction = await LoadFullTransaction(_selectedTransaction.Id);
                if (transaction != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transaction, 
                        _selectedCompany!, 
                        _selectedFinancialYear!,
                        _productService,
                        _taxService,
                        _ledgerService
                    );

                    // TODO: Make the form read-only for viewing
                    transactionEditForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening transaction view form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction == null)
            {
                MessageBox.Show("Please select a transaction to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete transaction '{_selectedTransaction.TransactionNumber}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = await _transactionService.DeleteTransactionAsync(_selectedTransaction.Id);
                    if (success)
                    {
                        MessageBox.Show("Transaction deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ = LoadTransactionsAsync(); // Refresh the list
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadTransactionsAsync();
        }

        private async Task<Models.Transaction?> LoadFullTransaction(Guid transactionId)
        {
            try
            {
                return await _transactionService.GetTransactionByIdAsync(transactionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading full transaction: {ex.Message}");
                return null;
            }
        }
    }
}
