using WinFormsApp1.Models;
using WinFormsApp1.Services;
using WinFormsApp1.Forms;

namespace WinFormsApp1.Forms.Payment
{
    public partial class PaymentListForm : Form
    {
        private readonly PaymentService _paymentService;
        private readonly LocalStorageService _localStorageService;
        private List<PaymentListDto> _payments = new List<PaymentListDto>();
        private List<PaymentListDto> _allPayments = new List<PaymentListDto>(); // Store all payments
        private PaymentListDto? _selectedPayment;

        // Form controls
        private DataGridView dgvPayments = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Button btnExportPdf = null!;
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
        
        // Payment type filter
        private string? _filterPaymentType;
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string? FilterPaymentType 
        { 
            get => _filterPaymentType; 
            set => _filterPaymentType = value; 
        }

        public PaymentListForm(PaymentService paymentService, LocalStorageService localStorageService,
            string? paymentType = null)
        {
            _paymentService = paymentService;
            _localStorageService = localStorageService;
            FilterPaymentType = paymentType;
            
            InitializeComponent();
            SetupForm();
            LoadCompanyAndPayments();
        }

        private void InitializeComponent()
        {
            dgvPayments = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnView = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            btnExportPdf = new Button();
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
            lblCompanyInfo.Size = new Size(700, 25);
            lblCompanyInfo.Text = _selectedCompany != null ? $"Payments for: {_selectedCompany.DisplayName}" : "No company selected";
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            lblCompanyInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 40);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(700, 40);
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
            // dgvPayments
            // 
            dgvPayments.Location = new Point(12, 115);
            dgvPayments.Name = "dgvPayments";
            dgvPayments.Size = new Size(900, 350);
            dgvPayments.TabIndex = 0;
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.AllowUserToDeleteRows = false;
            dgvPayments.ReadOnly = true;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.MultiSelect = false;
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.AutoGenerateColumns = false; // Prevent auto-generation of columns
            dgvPayments.SelectionChanged += new EventHandler(dgvPayments_SelectionChanged);
            dgvPayments.DoubleClick += new EventHandler(dgvPayments_DoubleClick);
            dgvPayments.KeyDown += new KeyEventHandler(dgvPayments_KeyDown);
            
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
            // btnExportPdf
            // 
            btnExportPdf.Location = new Point(420, 82);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(100, 30);
            btnExportPdf.TabIndex = 6;
            btnExportPdf.Text = "&Export PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += new EventHandler(btnExportPdf_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 450);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // PaymentListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnExportPdf);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(dgvPayments);
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
            Name = "PaymentListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = GetFormTitle();
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(PaymentListForm_KeyDown);
            Load += new EventHandler(PaymentListForm_Load);
            Resize += new EventHandler(PaymentListForm_Resize);
            Activated += new EventHandler(PaymentListForm_Activated);
            FormClosing += new FormClosingEventHandler(PaymentListForm_FormClosing);
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
            
            // Focus on payment grid
            dgvPayments.Focus();
        }

        private void SetupDataGridViewColumns()
        {
            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionNumber",
                HeaderText = "Transaction #",
                DataPropertyName = "TransactionNumber",
                Width = 120
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InvoiceNumber",
                HeaderText = "Invoice #",
                DataPropertyName = "InvoiceNumber",
                Width = 120
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionDate",
                HeaderText = "Date",
                DataPropertyName = "TransactionDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                HeaderText = "Type",
                DataPropertyName = "Type",
                Width = 120
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PartyName",
                HeaderText = "Party",
                DataPropertyName = "PartyName",
                Width = 150
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubTotal",
                HeaderText = "Subtotal",
                DataPropertyName = "SubTotal",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxAmount",
                HeaderText = "Tax",
                DataPropertyName = "TaxAmount",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PaidAmount",
                HeaderText = "Paid",
                DataPropertyName = "PaidAmount",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BalanceDue",
                HeaderText = "Balance",
                DataPropertyName = "BalanceDue",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 80
            });

            dgvPayments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DueDate",
                HeaderText = "Due Date",
                DataPropertyName = "DueDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private async void LoadCompanyAndPayments()
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
                    Text = "Payments - No Company Selected";
                    UpdateButtonStates();
                    return;
                }
                
                // Validate company ID
                if (string.IsNullOrWhiteSpace(_selectedCompany.Id))
                {
                    lblStatus.Text = "Invalid company data. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company data";
                    Text = "Payments - Invalid Company Data";
                    UpdateButtonStates();
                    return;
                }
                
                // Try to parse the company ID as GUID
                if (!Guid.TryParse(_selectedCompany.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company ID format";
                    Text = "Payments - Invalid Company ID";
                    UpdateButtonStates();
                    return;
                }
                
                // Update UI with company info
                lblCompanyInfo.Text = GetCompanyInfoText();
                Text = GetFormTitle();
                
                // Setup filter dropdown after company is loaded
                SetupFilterDropdown();
                
                // Load payments
                await LoadPayments();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading company: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load company exception: {ex.Message}");
            }
        }

        private async Task LoadPayments()
        {
            try
            {
                if (_selectedCompany == null)
                {
                    lblStatus.Text = "No company selected";
                    return;
                }

                lblStatus.Text = "Loading payments...";
                Application.DoEvents();

                var companyId = Guid.Parse(_selectedCompany.Id);
                var financialYearId = _selectedFinancialYear.Id;
                
                Console.WriteLine($"Loading payments for company: {companyId}, financial year: {financialYearId}, type: {FilterPaymentType ?? "All"}");
                
                // Load all payments for the company and financial year
                _allPayments = await _paymentService.GetPaymentsListAsync(companyId, financialYearId, 1, 50, FilterPaymentType);
                Console.WriteLine($"Loaded {_allPayments.Count} total payments");
                
                // Apply current filter to the loaded payments
                ApplyCurrentFilter();

                // Note: Row selection and button states are now handled in ApplyCurrentFilter()
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payments: {ex.Message}");
                lblStatus.Text = $"Error loading payments: {ex.Message}";
                MessageBox.Show($"Error loading payments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonStates()
        {
            btnEdit.Enabled = _selectedPayment != null;
            btnView.Enabled = _selectedPayment != null;
            btnDelete.Enabled = _selectedPayment != null;
        }

        private void dgvPayments_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPayments.SelectedRows.Count > 0)
            {
                _selectedPayment = dgvPayments.SelectedRows[0].DataBoundItem as PaymentListDto;
            }
            else
            {
                _selectedPayment = null;
            }
            UpdateButtonStates();
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment edit form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment edit form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to view.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment view form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment view form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete payment '{_selectedPayment.TransactionNumber}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = await _paymentService.DeletePaymentAsync(Guid.Parse(_selectedPayment.Id));
                    if (success)
                    {
                        MessageBox.Show("Payment deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ = LoadPayments(); // Refresh the list
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadPayments();
        }

        private async void btnExportPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_selectedCompany == null || _selectedFinancialYear == null)
                {
                    MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_payments.Count == 0)
                {
                    MessageBox.Show("No payments to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // TODO: Implement PDF export for payments
                MessageBox.Show("PDF export for payments not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Export error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        // Event Handlers
        private void PaymentListForm_Load(object? sender, EventArgs e)
        {
            // Open as maximized child form within MDI parent
            WindowState = FormWindowState.Maximized;
            
            // Resize controls to fit the maximized form
            ResizeControls();
            
            // Focus on the data grid
            dgvPayments.Focus();
            
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
            dgvPayments.Size = new Size(availableWidth, availableHeight);
            
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
            btnExportPdf.Location = new Point(440, 82);
        }

        private void PaymentListForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void PaymentListForm_Activated(object? sender, EventArgs e)
        {
            // When PaymentListForm is activated, ensure it's maximized and navigation is hidden
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

        private void PaymentListForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When PaymentListForm is closing, ensure navigation panel is shown again
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
        }

        private void PaymentListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"PaymentListForm_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in PaymentListForm_KeyDown");
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
                    _ = Task.Run(async () => await LoadPayments());
                    e.Handled = true;
                    break;
                    
                case Keys.Insert:
                    NewPayment();
                    e.Handled = true;
                    break;
                    
                case Keys.V:
                    if (_selectedPayment != null)
                    {
                        ViewPayment();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void dgvPayments_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedPayment != null)
            {
                EditPayment();
            }
        }

        private void dgvPayments_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"dgvPayments_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedPayment != null)
                    {
                        EditPayment();
                        e.Handled = true;
                        
                        // Ensure focus stays in the DataGridView after handling Enter
                        dgvPayments.Focus();
                    }
                    break;
                    
                case Keys.Insert:
                    NewPayment();
                    e.Handled = true;
                    break;
                    
                case Keys.Delete:
                    if (_selectedPayment != null)
                    {
                        DeletePayment();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.V:
                    if (_selectedPayment != null)
                    {
                        ViewPayment();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadPayments());
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in dgvPayments_KeyDown");
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

        private void NewPayment()
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment edit form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EditPayment()
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment edit form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ViewPayment()
        {
            if (_selectedCompany == null || _selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a company and financial year first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to view.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: Create PaymentEditForm when available
                MessageBox.Show("Payment view form not yet implemented.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment view form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeletePayment()
        {
            if (_selectedPayment == null)
            {
                MessageBox.Show("Please select a payment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete payment '{_selectedPayment.TransactionNumber}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = await _paymentService.DeletePaymentAsync(Guid.Parse(_selectedPayment.Id));
                    if (success)
                    {
                        MessageBox.Show("Payment deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ = LoadPayments(); // Refresh the list
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetFormTitle()
        {
            var typeText = !string.IsNullOrEmpty(FilterPaymentType) ? $"{FilterPaymentType} " : "";
            if (_selectedCompany != null)
            {
                return $"{typeText}Payments - {_selectedCompany.DisplayName}";
            }
            return $"{typeText}Payments - No Company Selected";
        }

        private string GetCompanyInfoText()
        {
            var typeText = !string.IsNullOrEmpty(FilterPaymentType) ? $"{FilterPaymentType} " : "";
            return $"{typeText}Payments for: {_selectedCompany?.DisplayName ?? "No company selected"}";
        }

        private string GetInstructionsText()
        {
            var typeText = !string.IsNullOrEmpty(FilterPaymentType) ? $"{FilterPaymentType} " : "";
            return $"Keyboard Navigation: ↑↓ to navigate rows, Enter to edit, V to view details, Insert for new, Delete to remove, F5 to refresh, Esc to close | {typeText}Payments | Use filter dropdown to refine results | Export PDF button to save as PDF | Uses selected company from local storage";
        }

        private void SetupFilterDropdown()
        {
            cmbFilter.Items.Clear();
            
            Console.WriteLine($"Setting up filter dropdown. FilterPaymentType: '{FilterPaymentType}'");
            
            if (string.IsNullOrEmpty(FilterPaymentType))
            {
                // Default filter options for all payments - based on actual API response types
                cmbFilter.Items.AddRange(new object[] { "All", "BankPayment", "Payment", "Receipt", "CashPayment" });
                Console.WriteLine("Using default filter options (no payment type)");
            }
            else if (FilterPaymentType.Equals("Payment", StringComparison.OrdinalIgnoreCase))
            {
                // Payment filter options - based on actual API response types
                cmbFilter.Items.AddRange(new object[] { "All", "BankPayment", "Payment", "CashPayment" });
                Console.WriteLine("Using Payment filter options");
            }
            else
            {
                // Generic filter options - based on actual API response types
                cmbFilter.Items.AddRange(new object[] { "All", "BankPayment", "Payment", "Receipt", "CashPayment" });
                Console.WriteLine($"Using generic filter options for unknown type: {FilterPaymentType}");
            }
            
            // Set default selection to "All"
            cmbFilter.SelectedIndex = 0;
            
            // Log the filter options for debugging
            Console.WriteLine($"Filter dropdown setup complete. Options: {string.Join(", ", cmbFilter.Items.Cast<object>())}");
            Console.WriteLine($"Selected index: {cmbFilter.SelectedIndex}, Selected item: {cmbFilter.SelectedItem}");
        }

        private void cmbFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbFilter.SelectedItem != null && _allPayments.Count > 0)
            {
                var selectedFilter = cmbFilter.SelectedItem.ToString();
                Console.WriteLine($"Filter changed to: {selectedFilter}");
                
                // Apply the filter to the current loaded payments
                ApplyCurrentFilter();
            }
        }

        private void ApplyCurrentFilter()
        {
            try
            {
                var selectedFilter = cmbFilter.SelectedItem?.ToString() ?? "All";
                Console.WriteLine($"Applying filter: {selectedFilter} to {_allPayments.Count} payments");
                
                if (selectedFilter == "All")
                {
                    // Show all payments
                    _payments = new List<PaymentListDto>(_allPayments);
                }
                else
                {
                    // Filter payments based on the selected filter
                    _payments = _allPayments.Where(p => 
                    {
                        return p.Type.Contains(selectedFilter, StringComparison.OrdinalIgnoreCase);
                    }).ToList();
                }
                
                Console.WriteLine($"Filter applied. Showing {_payments.Count} payments out of {_allPayments.Count} total");
                
                // Update the DataGridView
                dgvPayments.DataSource = null;
                dgvPayments.DataSource = _payments;
                
                // Update status
                lblStatus.Text = $"Showing {_payments.Count} payments (filtered from {_allPayments.Count} total)";
                
                // Select first row if payments exist
                if (_payments.Count > 0)
                {
                    dgvPayments.Rows[0].Selected = true;
                    _selectedPayment = _payments[0];
                    dgvPayments.Focus();
                }
                else
                {
                    _selectedPayment = null;
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
            Console.WriteLine($"FilterPaymentType: '{FilterPaymentType}'");
            Console.WriteLine($"Selected Filter: '{cmbFilter.SelectedItem}'");
            Console.WriteLine($"Company: {_selectedCompany?.DisplayName ?? "None"}");
            Console.WriteLine($"Financial Year: {_selectedFinancialYear?.YearLabel ?? "None"}");
            Console.WriteLine($"Total Payments Loaded: {_allPayments.Count}");
            Console.WriteLine($"Currently Filtered: {_payments.Count}");
            
            // Show a message box with the current filter state
            MessageBox.Show(
                $"Current Filter State:\n\n" +
                $"Payment Type: {FilterPaymentType ?? "None"}\n" +
                $"Selected Filter: {cmbFilter.SelectedItem}\n" +
                $"Total Payments: {_allPayments.Count}\n" +
                $"Currently Showing: {_payments.Count}\n\n" +
                $"Check console for detailed logging.",
                "Filter Test Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
