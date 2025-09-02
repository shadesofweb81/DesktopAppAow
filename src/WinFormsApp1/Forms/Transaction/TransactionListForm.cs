using WinFormsApp1.Models;
using WinFormsApp1.Services;
using WinFormsApp1.Forms;

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
        private List<TransactionListDto> _allTransactions = new List<TransactionListDto>(); // Store all transactions
        private TransactionListDto? _selectedTransaction;

        // Form controls
        private DataGridView dgvTransactions = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private Label lblCompanyInfo = null!;
        
        // Filter controls
        private Label lblFilter = null!;
        private ComboBox cmbFilter = null!;
        private Button btnTestFilter = null!;

        // Company and Financial Year info
        private Models.Company? _selectedCompany;
        private FinancialYearModel? _selectedFinancialYear;
        
        // Transaction type filter
        private string? _transactionType;
        public string? TransactionType 
        { 
            get => _transactionType; 
            set => _transactionType = value; 
        }

        public TransactionListForm(TransactionService transactionService, LocalStorageService localStorageService,
            string? transactionType = null, ProductService? productService = null, TaxService? taxService = null, LedgerService? ledgerService = null)
        {
            _transactionService = transactionService;
            _localStorageService = localStorageService;
            TransactionType = transactionType;
            
            // Create services if not provided
            var authService = new AuthService();
            _productService = productService ?? new ProductService(authService);
            _taxService = taxService ?? new TaxService(authService);
            _ledgerService = ledgerService ?? new LedgerService(authService);
            
            InitializeComponent();
            SetupForm();
            LoadCompanyAndTransactions();
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
            lblInstructions = new Label();
            lblCompanyInfo = new Label();
            lblFilter = new Label();
            cmbFilter = new ComboBox();
            btnTestFilter = new Button();
            SuspendLayout();
            
            // 
            // lblCompanyInfo
            // 
            lblCompanyInfo.Location = new Point(12, 9);
            lblCompanyInfo.Name = "lblCompanyInfo";
            lblCompanyInfo.Size = new Size(600, 25);
            lblCompanyInfo.Text = _selectedCompany != null ? $"Transactions for: {_selectedCompany.DisplayName}" : "No company selected";
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            lblCompanyInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 40);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(600, 40);
            lblInstructions.Text = GetInstructionsText();
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // lblFilter
            // 
            lblFilter.Location = new Point(12, 85);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(100, 20);
            lblFilter.Text = "Filter by:";
            lblFilter.ForeColor = Color.DarkBlue;
            lblFilter.Font = new Font("Arial", 9, FontStyle.Bold);
            
            // 
            // cmbFilter
            // 
            cmbFilter.Location = new Point(120, 82);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(200, 25);
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.Font = new Font("Arial", 9, FontStyle.Regular);
            cmbFilter.SelectedIndexChanged += new EventHandler(cmbFilter_SelectedIndexChanged);
            
            // 
            // btnTestFilter
            // 
            btnTestFilter.Location = new Point(330, 82);
            btnTestFilter.Name = "btnTestFilter";
            btnTestFilter.Size = new Size(80, 25);
            btnTestFilter.Text = "Test Filter";
            btnTestFilter.UseVisualStyleBackColor = true;
            btnTestFilter.Click += new EventHandler(btnTestFilter_Click);
            
            // 
            // dgvTransactions
            // 
            dgvTransactions.Location = new Point(12, 115);
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.Size = new Size(800, 350);
            dgvTransactions.TabIndex = 0;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.AutoGenerateColumns = false; // Prevent auto-generation of columns
            dgvTransactions.SelectionChanged += new EventHandler(dgvTransactions_SelectionChanged);
            dgvTransactions.DoubleClick += new EventHandler(dgvTransactions_DoubleClick);
            dgvTransactions.KeyDown += new KeyEventHandler(dgvTransactions_KeyDown);
            
            // 
            // btnNew
            // 
            btnNew.Location = new Point(530, 85);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(100, 30);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (Insert)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);
            
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(530, 125);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "&Edit (Enter)";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Enabled = false;
            
            // 
            // btnView
            // 
            btnView.Location = new Point(530, 165);
            btnView.Name = "btnView";
            btnView.Size = new Size(100, 30);
            btnView.TabIndex = 3;
            btnView.Text = "&View (V)";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += new EventHandler(btnView_Click);
            btnView.Enabled = false;
            
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(530, 205);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "&Delete (Del)";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDelete.Enabled = false;
            
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(530, 245);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "&Refresh (F5)";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 450);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // TransactionListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(dgvTransactions);
            Controls.Add(lblFilter);
            Controls.Add(cmbFilter);
            Controls.Add(btnTestFilter);
            Controls.Add(lblInstructions);
            Controls.Add(lblCompanyInfo);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            CancelButton = null; // Ensure no default cancel button interferes
            Name = "TransactionListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = GetFormTitle();
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(TransactionListForm_KeyDown);
            Load += new EventHandler(TransactionListForm_Load);
            Resize += new EventHandler(TransactionListForm_Resize);
            Activated += new EventHandler(TransactionListForm_Activated);
            FormClosing += new FormClosingEventHandler(TransactionListForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnEdit;
            CancelButton = null; // Remove default cancel button to prevent conflicts
            
            // Setup DataGridView columns
            SetupDataGridViewColumns();
            
            // Focus on transaction grid
            dgvTransactions.Focus();
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
                Name = "DueDate",
                HeaderText = "Due Date",
                DataPropertyName = "DueDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private async void LoadCompanyAndTransactions()
        {
            try
            {
                // Load selected company from local storage
                _selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                _selectedFinancialYear = await _localStorageService.GetSelectedFinancialYearAsync();
                
                if (_selectedCompany == null)
                {
                    lblStatus.Text = "No company selected. Please select a company first.";
                    lblStatus.ForeColor = Color.Orange;
                    lblCompanyInfo.Text = "No company selected";
                    Text = "Transactions - No Company Selected";
                    UpdateButtonStates();
                    return;
                }
                
                // Validate company ID
                if (string.IsNullOrWhiteSpace(_selectedCompany.Id))
                {
                    lblStatus.Text = "Invalid company data. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company data";
                    Text = "Transactions - Invalid Company Data";
                    UpdateButtonStates();
                    return;
                }
                
                // Try to parse the company ID as GUID
                if (!Guid.TryParse(_selectedCompany.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company ID format";
                    Text = "Transactions - Invalid Company ID";
                    UpdateButtonStates();
                    return;
                }
                
                // Update UI with company info
                lblCompanyInfo.Text = GetCompanyInfoText();
                Text = GetFormTitle();
                
                // Setup filter dropdown after company is loaded
                SetupFilterDropdown();
                
                // Load transactions
                await LoadTransactions();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading company: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load company exception: {ex.Message}");
            }
        }

        private async Task LoadTransactions()
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
                
                Console.WriteLine($"Loading transactions for company: {companyId}, financial year: {financialYearId}, type: {TransactionType ?? "All"}");
                
                // Load all transactions for the company and financial year
                _allTransactions = await _transactionService.GetTransactionListAsync(companyId, financialYearId, 1, 50, TransactionType);
                Console.WriteLine($"Loaded {_allTransactions.Count} total transactions");
                
                // Apply current filter to the loaded transactions
                ApplyCurrentFilter();

                // Note: Row selection and button states are now handled in ApplyCurrentFilter()
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
                    _ = LoadTransactions(); // Refresh the list
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
                var transactionDto = await LoadFullTransaction(_selectedTransaction.Id);
                if (transactionDto != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transactionDto, 
                        _selectedCompany!, 
                        _selectedFinancialYear!,
                        _productService,
                        _taxService,
                        _ledgerService
                    );

                    if (transactionEditForm.ShowDialog() == DialogResult.OK)
                    {
                        _ = LoadTransactions(); // Refresh the list
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
                var transactionDto = await LoadFullTransaction(_selectedTransaction.Id);
                if (transactionDto != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transactionDto, 
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
                        _ = LoadTransactions(); // Refresh the list
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
                                await LoadTransactions();
        }

        private async Task<Models.TransactionByIdDto?> LoadFullTransaction(Guid transactionId)
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

        // Event Handlers
        private void TransactionListForm_Load(object? sender, EventArgs e)
        {
            // Open as maximized child form within MDI parent
            WindowState = FormWindowState.Maximized;
            
            // Resize controls to fit the maximized form
            ResizeControls();
            
            // Focus on the data grid
            dgvTransactions.Focus();
            
            // Hide MDI navigation panel when this form is maximized
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
            // Ensure the form stays maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void ResizeControls()
        {
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            // Reserve space for buttons on the right side
            int buttonAreaWidth = 150;
            int availableWidth = clientWidth - buttonAreaWidth - 30; // 30px margin
            int availableHeight = clientHeight - 200; // Increased to accommodate filter controls
            
            // Ensure minimum grid width
            if (availableWidth < 600)
            {
                availableWidth = 600;
            }
            
            // Resize the data grid to use most of the available space
            dgvTransactions.Size = new Size(availableWidth, availableHeight);
            
            // Reposition buttons on the right side with 30px margin
            int buttonX = availableWidth + 30;
            btnNew.Location = new Point(buttonX, 115);
            btnEdit.Location = new Point(buttonX, 155);
            btnView.Location = new Point(buttonX, 195);
            btnDelete.Location = new Point(buttonX, 235);
            btnRefresh.Location = new Point(buttonX, 275);
            
            // Reposition status label at the bottom
            lblStatus.Location = new Point(12, clientHeight - 30);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            // Resize company info and instructions labels
            lblCompanyInfo.Size = new Size(clientWidth - 24, 25);
            lblInstructions.Size = new Size(clientWidth - 24, 40);
            
            // Reposition filter controls
            lblFilter.Location = new Point(12, 85);
            cmbFilter.Location = new Point(120, 82);
            btnTestFilter.Location = new Point(330, 82);
        }

        private void TransactionListForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void TransactionListForm_Activated(object? sender, EventArgs e)
        {
            // When TransactionListForm is activated, ensure it's maximized and navigation is hidden
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            // Hide navigation panel when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
            // Ensure the form takes focus and maintains its state
            this.BringToFront();
            this.Activate();
            
            // Refresh filter dropdown to ensure it's up to date
            SetupFilterDropdown();
            
            // Force the form to stay maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void TransactionListForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When TransactionListForm is closing, ensure navigation panel is shown again
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
        }

        private void TransactionListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"TransactionListForm_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in TransactionListForm_KeyDown");
                    // When closing with Escape, ensure navigation panel is shown
                    if (MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.BeginInvoke(new Action(() =>
                        {
                            mdiForm.ShowNavigationPanel();
                            mdiForm.SetFocusToNavigation();
                        }));
                    }
                    Close();
                    e.Handled = true;
                    break;
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadTransactions());
                    e.Handled = true;
                    break;
                    
                case Keys.Insert:
                    NewTransaction();
                    e.Handled = true;
                    break;
                    
                case Keys.V:
                    if (_selectedTransaction != null)
                    {
                        ViewTransaction();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void dgvTransactions_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvTransactions.CurrentRow != null && dgvTransactions.CurrentRow.Index >= 0 && dgvTransactions.CurrentRow.Index < _transactions.Count)
            {
                _selectedTransaction = _transactions[dgvTransactions.CurrentRow.Index];
            }
            else
            {
                _selectedTransaction = null;
            }
            UpdateButtonStates();
        }

        private void dgvTransactions_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedTransaction != null)
            {
                EditTransaction();
            }
        }

        private void dgvTransactions_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"dgvTransactions_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedTransaction != null)
                    {
                        EditTransaction();
                        e.Handled = true;
                        
                        // Ensure focus stays in the DataGridView after handling Enter
                        dgvTransactions.Focus();
                    }
                    break;
                    
                case Keys.Insert:
                    NewTransaction();
                    e.Handled = true;
                    break;
                    
                case Keys.Delete:
                    if (_selectedTransaction != null)
                    {
                        DeleteTransaction();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.V:
                    if (_selectedTransaction != null)
                    {
                        ViewTransaction();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadTransactions());
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in dgvTransactions_KeyDown");
                    // When closing with Escape, ensure navigation panel is shown
                    if (MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.BeginInvoke(new Action(() =>
                        {
                            mdiForm.ShowNavigationPanel();
                            mdiForm.SetFocusToNavigation();
                        }));
                    }
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            NewTransaction();
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction != null)
            {
                try
                {
                    await EditTransaction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing transaction: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction != null)
            {
                try
                {
                    await ViewTransaction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error viewing transaction: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedTransaction != null)
            {
                DeleteTransaction();
            }
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadTransactions();
        }

        private void NewTransaction()
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
                    _ = LoadTransactions(); // Refresh the list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening transaction edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EditTransaction()
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedTransaction == null)
            {
                MessageBox.Show("Please select a transaction to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Load the full transaction details
                var transactionDto = await LoadFullTransaction(_selectedTransaction.Id);
                if (transactionDto != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transactionDto, 
                        _selectedCompany!, 
                        _selectedFinancialYear!,
                        _productService,
                        _taxService,
                        _ledgerService
                    );

                    if (transactionEditForm.ShowDialog() == DialogResult.OK)
                    {
                        _ = LoadTransactions(); // Refresh the list
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening transaction edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ViewTransaction()
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedTransaction == null)
            {
                MessageBox.Show("Please select a transaction to view.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Load the full transaction details
                var transactionDto = await LoadFullTransaction(_selectedTransaction.Id);
                if (transactionDto != null)
                {
                    var transactionEditForm = new TransactionEditForm(
                        _transactionService, 
                        _localStorageService, 
                        transactionDto, 
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

        private async void DeleteTransaction()
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
                        _ = LoadTransactions(); // Refresh the list
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

        private string GetFormTitle()
        {
            var typeText = !string.IsNullOrEmpty(TransactionType) ? $"{TransactionType} " : "";
            if (_selectedCompany != null)
            {
                return $"{typeText}Transactions - {_selectedCompany.DisplayName}";
            }
            return $"{typeText}Transactions - No Company Selected";
        }

        private string GetCompanyInfoText()
        {
            var typeText = !string.IsNullOrEmpty(TransactionType) ? $"{TransactionType} " : "";
            return $"{typeText}Transactions for: {_selectedCompany?.DisplayName ?? "No company selected"}";
        }

        private string GetInstructionsText()
        {
            var typeText = !string.IsNullOrEmpty(TransactionType) ? $"{TransactionType} " : "";
            return $"Keyboard Navigation: ↑↓ to navigate rows, Enter to edit, V to view details, Insert for new, Delete to remove, F5 to refresh, Esc to close | {typeText}Transactions | Use filter dropdown to refine results | Uses selected company from local storage";
        }

        private void SetupFilterDropdown()
        {
            cmbFilter.Items.Clear();
            
            Console.WriteLine($"Setting up filter dropdown. TransactionType: '{TransactionType}'");
            
            if (string.IsNullOrEmpty(TransactionType))
            {
                // Default filter options for all transactions - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Invoice", "Order", "Quotation", "Returns", "Receipt", "Payment", "Journal" });
                Console.WriteLine("Using default filter options (no transaction type)");
            }
            else if (TransactionType.Equals("Sale", StringComparison.OrdinalIgnoreCase))
            {
                // Sales filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Invoice", "Order", "Dispatched", "Returns", "Quotation" });
                Console.WriteLine("Using Sales filter options");
            }
            else if (TransactionType.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                // Purchase filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Bill", "Order", "Received", "Returns", "Quotation" });
                Console.WriteLine("Using Purchase filter options");
            }
            else if (TransactionType.Equals("Receipt", StringComparison.OrdinalIgnoreCase))
            {
                // Receipt filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Payment", "Advance", "Refund" });
                Console.WriteLine("Using Receipt filter options");
            }
            else if (TransactionType.Equals("Payment", StringComparison.OrdinalIgnoreCase))
            {
                // Payment filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Payment", "Advance", "Refund" });
                Console.WriteLine("Using Payment filter options");
            }
            else if (TransactionType.Equals("Journal", StringComparison.OrdinalIgnoreCase))
            {
                // Journal filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Opening", "Adjustment", "Transfer", "Closing" });
                Console.WriteLine("Using Journal filter options");
            }
            else
            {
                // Generic filter options - use direct values for API
                cmbFilter.Items.AddRange(new object[] { "All", "Invoice", "Order", "Quotation", "Returns", "Receipt", "Payment", "Journal" });
                Console.WriteLine($"Using generic filter options for unknown type: {TransactionType}");
            }
            
            // Set default selection to "All"
            cmbFilter.SelectedIndex = 0;
            
            // Log the filter options for debugging
            Console.WriteLine($"Filter dropdown setup complete. Options: {string.Join(", ", cmbFilter.Items.Cast<object>())}");
            Console.WriteLine($"Selected index: {cmbFilter.SelectedIndex}, Selected item: {cmbFilter.SelectedItem}");
        }

        private void cmbFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbFilter.SelectedItem != null && _allTransactions.Count > 0)
            {
                var selectedFilter = cmbFilter.SelectedItem.ToString();
                Console.WriteLine($"Filter changed to: {selectedFilter}");
                
                // Apply the filter to the current loaded transactions
                ApplyCurrentFilter();
            }
        }

        private void ApplyCurrentFilter()
        {
            try
            {
                var selectedFilter = cmbFilter.SelectedItem?.ToString() ?? "All";
                Console.WriteLine($"Applying filter: {selectedFilter} to {_allTransactions.Count} transactions");
                
                if (selectedFilter == "All")
                {
                    // Show all transactions
                    _transactions = new List<TransactionListDto>(_allTransactions);
                }
                else
                {
                    // Filter transactions based on the selected filter
                    _transactions = _allTransactions.Where(t => 
                    {
                        var transactionType = t.Type.ToString();
                        return transactionType.Contains(selectedFilter, StringComparison.OrdinalIgnoreCase);
                    }).ToList();
                }
                
                Console.WriteLine($"Filter applied. Showing {_transactions.Count} transactions out of {_allTransactions.Count} total");
                
                // Update the DataGridView
                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = _transactions;
                
                // Update status
                lblStatus.Text = $"Showing {_transactions.Count} transactions (filtered from {_allTransactions.Count} total)";
                
                // Select first row if transactions exist
                if (_transactions.Count > 0)
                {
                    dgvTransactions.Rows[0].Selected = true;
                    _selectedTransaction = _transactions[0];
                    dgvTransactions.Focus();
                }
                else
                {
                    _selectedTransaction = null;
                }
                
                // Update button states
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying filter: {ex.Message}");
                lblStatus.Text = $"Error applying filter: {ex.Message}";
            }
        }

        private void btnTestFilter_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("=== TEST FILTER BUTTON CLICKED ===");
            Console.WriteLine($"TransactionType: '{TransactionType}'");
            Console.WriteLine($"Selected Filter: '{cmbFilter.SelectedItem}'");
            Console.WriteLine($"Company: {_selectedCompany?.DisplayName ?? "None"}");
            Console.WriteLine($"Financial Year: {_selectedFinancialYear?.YearLabel ?? "None"}");
            Console.WriteLine($"Total Transactions Loaded: {_allTransactions.Count}");
            Console.WriteLine($"Currently Filtered: {_transactions.Count}");
            
            // Show a message box with the current filter state
            MessageBox.Show(
                $"Current Filter State:\n\n" +
                $"Transaction Type: {TransactionType ?? "None"}\n" +
                $"Selected Filter: {cmbFilter.SelectedItem}\n" +
                $"Total Transactions: {_allTransactions.Count}\n" +
                $"Currently Showing: {_transactions.Count}\n\n" +
                $"Check console for detailed logging.",
                "Filter Test Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}

