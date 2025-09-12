using WinFormsApp1.Models;
using WinFormsApp1.Models.request;
using WinFormsApp1.Services;
using WinFormsApp1.Forms;
using WinFormsApp1.Forms.Transaction;
using System.Linq;

namespace WinFormsApp1.Forms.Payment
{
    public partial class PaymentEditForm : Form
    {
        private readonly PaymentService _paymentService;
        private readonly TransactionService _transactionService;
        private readonly LedgerService _ledgerService;
        private readonly LocalStorageService _localStorageService;
        private readonly Models.Company? _selectedCompany;
        private readonly FinancialYearModel? _selectedFinancialYear;
        private readonly PaymentListDto? _existingPayment;
        private readonly string _viewMode;

        // Form controls
        private GroupBox grpTransactionType = null!;
        private RadioButton rdoPayment = null!;
        private RadioButton rdoReceipt = null!;
        
        private GroupBox grpPaymentDetails = null!;
        private Label lblPayFrom = null!;
        private ComboBox cmbPayFromLedger = null!;
        private Button btnSelectPayFromLedger = null!;
        private Label lblPayTo = null!;
        private ComboBox cmbPayToLedger = null!;
        private Button btnSelectPayToLedger = null!;
        private Label lblAmount = null!;
        private TextBox txtAmount = null!;
        private Label lblTransactionDate = null!;
        private DateTimePicker dtpTransactionDate = null!;
        private Label lblDescription = null!;
        private TextBox txtDescription = null!;
        private Label lblTransactionNumber = null!;
        private TextBox txtTransactionNumber = null!;
        private Label lblInvoiceNumber = null!;
        private TextBox txtInvoiceNumber = null!;
        private Button btnAutoClear = null!;
        
        private GroupBox grpPaymentMethod = null!;
        private RadioButton rdoBank = null!;
        private RadioButton rdoCash = null!;
        private RadioButton rdoCheque = null!;
        private Label lblReferenceNumber = null!;
        private TextBox txtReferenceNumber = null!;
        
        private GroupBox grpInvoices = null!;
        private DataGridView dgvInvoices = null!;
        private CheckBox chkSelectAll = null!;
        private Label lblTotalSelected = null!;
        
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;

        // Data
        private List<LedgerModel> _ledgers = new List<LedgerModel>();
        private List<TransactionListDto> _unpaidTransactions = new List<TransactionListDto>();
        private List<TransactionListDto> _selectedTransactions = new List<TransactionListDto>();
        private decimal _totalSelectedAmount = 0;
        private bool _isUpdatingAmount = false;
        private bool _isLoadingExistingPayment = false;

        public PaymentEditForm(PaymentService paymentService, TransactionService transactionService, 
            LedgerService ledgerService, LocalStorageService localStorageService,
            Models.Company? selectedCompany, FinancialYearModel? selectedFinancialYear,
            PaymentListDto? existingPayment = null, string viewMode = "Payment")
        {
            _paymentService = paymentService;
            _transactionService = transactionService;
            _ledgerService = ledgerService;
            _localStorageService = localStorageService;
            _selectedCompany = selectedCompany;
            _selectedFinancialYear = selectedFinancialYear;
            _existingPayment = existingPayment;
            _viewMode = viewMode;

            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void InitializeComponent()
        {
            grpTransactionType = new GroupBox();
            rdoPayment = new RadioButton();
            rdoReceipt = new RadioButton();
            
            grpPaymentDetails = new GroupBox();
            lblPayFrom = new Label();
            cmbPayFromLedger = new ComboBox();
            btnSelectPayFromLedger = new Button();
            lblPayTo = new Label();
            cmbPayToLedger = new ComboBox();
            btnSelectPayToLedger = new Button();
            lblAmount = new Label();
            txtAmount = new TextBox();
            lblTransactionDate = new Label();
            dtpTransactionDate = new DateTimePicker();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblTransactionNumber = new Label();
            txtTransactionNumber = new TextBox();
            lblInvoiceNumber = new Label();
            txtInvoiceNumber = new TextBox();
            btnAutoClear = new Button();
            
            grpPaymentMethod = new GroupBox();
            rdoBank = new RadioButton();
            rdoCash = new RadioButton();
            rdoCheque = new RadioButton();
            lblReferenceNumber = new Label();
            txtReferenceNumber = new TextBox();
            
            grpInvoices = new GroupBox();
            dgvInvoices = new DataGridView();
            chkSelectAll = new CheckBox();
            lblTotalSelected = new Label();
            
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            
            SuspendLayout();
            
            // 
            // grpTransactionType
            // 
            grpTransactionType.Location = new Point(12, 12);
            grpTransactionType.Name = "grpTransactionType";
            grpTransactionType.Size = new Size(400, 60);
            grpTransactionType.TabIndex = 0;
            grpTransactionType.TabStop = false;
            grpTransactionType.Text = "Transaction Type";
            
            rdoPayment.Location = new Point(20, 25);
            rdoPayment.Name = "rdoPayment";
            rdoPayment.Size = new Size(150, 20);
            rdoPayment.TabIndex = 0;
            rdoPayment.Text = "Payment (Pay Money)";
            rdoPayment.UseVisualStyleBackColor = true;
            rdoPayment.CheckedChanged += rdoPayment_CheckedChanged;
            
            rdoReceipt.Location = new Point(200, 25);
            rdoReceipt.Name = "rdoReceipt";
            rdoReceipt.Size = new Size(150, 20);
            rdoReceipt.TabIndex = 1;
            rdoReceipt.Text = "Receipt (Receive Money)";
            rdoReceipt.UseVisualStyleBackColor = true;
            rdoReceipt.CheckedChanged += rdoReceipt_CheckedChanged;
            
            // 
            // grpPaymentDetails
            // 
            grpPaymentDetails.Location = new Point(12, 85);
            grpPaymentDetails.Name = "grpPaymentDetails";
            grpPaymentDetails.Size = new Size(400, 270);
            grpPaymentDetails.TabIndex = 1;
            grpPaymentDetails.TabStop = false;
            grpPaymentDetails.Text = "Payment Details";
            
            lblPayFrom.Location = new Point(20, 30);
            lblPayFrom.Name = "lblPayFrom";
            lblPayFrom.Size = new Size(100, 20);
            lblPayFrom.Text = "Pay From:";
            
            cmbPayFromLedger.Location = new Point(130, 27);
            cmbPayFromLedger.Name = "cmbPayFromLedger";
            cmbPayFromLedger.Size = new Size(200, 25);
            cmbPayFromLedger.TabIndex = 0;
            cmbPayFromLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayFromLedger.DisplayMember = "Name";
            
            btnSelectPayFromLedger.Location = new Point(340, 27);
            btnSelectPayFromLedger.Name = "btnSelectPayFromLedger";
            btnSelectPayFromLedger.Size = new Size(40, 25);
            btnSelectPayFromLedger.TabIndex = 1;
            btnSelectPayFromLedger.Text = "...";
            btnSelectPayFromLedger.UseVisualStyleBackColor = true;
            btnSelectPayFromLedger.Click += btnSelectPayFromLedger_Click;
            
            lblPayTo.Location = new Point(20, 65);
            lblPayTo.Name = "lblPayTo";
            lblPayTo.Size = new Size(100, 20);
            lblPayTo.Text = "Pay To:";
            
            cmbPayToLedger.Location = new Point(130, 62);
            cmbPayToLedger.Name = "cmbPayToLedger";
            cmbPayToLedger.Size = new Size(200, 25);
            cmbPayToLedger.TabIndex = 2;
            cmbPayToLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayToLedger.DisplayMember = "Name";
            
            btnSelectPayToLedger.Location = new Point(340, 62);
            btnSelectPayToLedger.Name = "btnSelectPayToLedger";
            btnSelectPayToLedger.Size = new Size(40, 25);
            btnSelectPayToLedger.TabIndex = 3;
            btnSelectPayToLedger.Text = "...";
            btnSelectPayToLedger.UseVisualStyleBackColor = true;
            btnSelectPayToLedger.Click += btnSelectPayToLedger_Click;
            
            lblAmount.Location = new Point(20, 100);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(100, 20);
            lblAmount.Text = "Amount:";
            
            txtAmount.Location = new Point(130, 97);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(120, 25);
            txtAmount.TabIndex = 4;
            txtAmount.TextChanged += txtAmount_TextChanged;
            txtAmount.Text = "0.00";
            
            btnAutoClear.Location = new Point(260, 97);
            btnAutoClear.Name = "btnAutoClear";
            btnAutoClear.Size = new Size(80, 25);
            btnAutoClear.TabIndex = 5;
            btnAutoClear.Text = "Auto Clear";
            btnAutoClear.UseVisualStyleBackColor = true;
            btnAutoClear.Click += btnAutoClear_Click;
            
            lblTransactionDate.Location = new Point(20, 135);
            lblTransactionDate.Name = "lblTransactionDate";
            lblTransactionDate.Size = new Size(100, 20);
            lblTransactionDate.Text = "Date:";
            
            dtpTransactionDate.Location = new Point(130, 132);
            dtpTransactionDate.Name = "dtpTransactionDate";
            dtpTransactionDate.Size = new Size(150, 25);
            dtpTransactionDate.TabIndex = 6;
            dtpTransactionDate.Value = DateTime.Today;
            
            lblDescription.Location = new Point(20, 170);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(100, 20);
            lblDescription.Text = "Description:";
            
            txtDescription.Location = new Point(130, 167);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(250, 25);
            txtDescription.TabIndex = 7;
            
            lblTransactionNumber.Location = new Point(20, 202);
            lblTransactionNumber.Name = "lblTransactionNumber";
            lblTransactionNumber.Size = new Size(100, 20);
            lblTransactionNumber.Text = "Transaction #:";
            
            txtTransactionNumber.Location = new Point(130, 199);
            txtTransactionNumber.Name = "txtTransactionNumber";
            txtTransactionNumber.Size = new Size(150, 25);
            txtTransactionNumber.TabIndex = 8;
            txtTransactionNumber.ReadOnly = true;
            txtTransactionNumber.BackColor = Color.LightGray;
            
            lblInvoiceNumber.Location = new Point(20, 237);
            lblInvoiceNumber.Name = "lblInvoiceNumber";
            lblInvoiceNumber.Size = new Size(100, 20);
            lblInvoiceNumber.Text = "Invoice #:";
            
            txtInvoiceNumber.Location = new Point(130, 234);
            txtInvoiceNumber.Name = "txtInvoiceNumber";
            txtInvoiceNumber.Size = new Size(150, 25);
            txtInvoiceNumber.TabIndex = 9;
            
            // Add tooltip for invoice number field
            var toolTip = new ToolTip();
            toolTip.SetToolTip(txtInvoiceNumber, "Enter invoice number manually or leave empty to auto-populate from selected transactions");
            
            // 
            // grpPaymentMethod
            // 
            grpPaymentMethod.Location = new Point(12, 365);
            grpPaymentMethod.Name = "grpPaymentMethod";
            grpPaymentMethod.Size = new Size(400, 100);
            grpPaymentMethod.TabIndex = 2;
            grpPaymentMethod.TabStop = false;
            grpPaymentMethod.Text = "Payment Method";
            
            rdoBank.Location = new Point(20, 25);
            rdoBank.Name = "rdoBank";
            rdoBank.Size = new Size(80, 20);
            rdoBank.TabIndex = 0;
            rdoBank.Text = "Bank";
            rdoBank.UseVisualStyleBackColor = true;
            rdoBank.CheckedChanged += PaymentMethod_CheckedChanged;
            
            rdoCash.Location = new Point(120, 25);
            rdoCash.Name = "rdoCash";
            rdoCash.Size = new Size(80, 20);
            rdoCash.TabIndex = 1;
            rdoCash.Text = "Cash";
            rdoCash.UseVisualStyleBackColor = true;
            rdoCash.CheckedChanged += PaymentMethod_CheckedChanged;
            
            rdoCheque.Location = new Point(220, 25);
            rdoCheque.Name = "rdoCheque";
            rdoCheque.Size = new Size(80, 20);
            rdoCheque.TabIndex = 2;
            rdoCheque.Text = "Cheque";
            rdoCheque.UseVisualStyleBackColor = true;
            rdoCheque.CheckedChanged += PaymentMethod_CheckedChanged;
            
            lblReferenceNumber.Location = new Point(20, 55);
            lblReferenceNumber.Name = "lblReferenceNumber";
            lblReferenceNumber.Size = new Size(100, 20);
            lblReferenceNumber.Text = "UTR Number:";
            
            txtReferenceNumber.Location = new Point(130, 52);
            txtReferenceNumber.Name = "txtReferenceNumber";
            txtReferenceNumber.Size = new Size(250, 25);
            txtReferenceNumber.TabIndex = 3;
            
            // 
            // grpInvoices
            // 
            grpInvoices.Location = new Point(430, 12);
            grpInvoices.Name = "grpInvoices";
            grpInvoices.Size = new Size(750, 500);
            grpInvoices.TabIndex = 3;
            grpInvoices.TabStop = false;
            grpInvoices.Text = "Select Invoices/Bills to Pay";
            
            dgvInvoices.Location = new Point(20, 50);
            dgvInvoices.Name = "dgvInvoices";
            dgvInvoices.Size = new Size(710, 400);
            dgvInvoices.TabIndex = 0;
            dgvInvoices.AllowUserToAddRows = false;
            dgvInvoices.AllowUserToDeleteRows = false;
            dgvInvoices.ReadOnly = false;
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.MultiSelect = true;
            dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.AutoGenerateColumns = false;
            dgvInvoices.CellValueChanged += dgvInvoices_CellValueChanged;
            
            chkSelectAll.Location = new Point(20, 25);
            chkSelectAll.Name = "chkSelectAll";
            chkSelectAll.Size = new Size(100, 20);
            chkSelectAll.TabIndex = 1;
            chkSelectAll.Text = "Select All";
            chkSelectAll.UseVisualStyleBackColor = true;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
            
            lblTotalSelected.Location = new Point(20, 460);
            lblTotalSelected.Name = "lblTotalSelected";
            lblTotalSelected.Size = new Size(710, 20);
            lblTotalSelected.Text = "Total Selected: $0.00";
            lblTotalSelected.ForeColor = Color.Blue;
            lblTotalSelected.Font = new Font("Arial", 9, FontStyle.Bold);
            
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1000, 550);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(1110, 550);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 550);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(800, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // Add controls to their parent containers
            grpTransactionType.Controls.Add(rdoPayment);
            grpTransactionType.Controls.Add(rdoReceipt);
            
            grpPaymentDetails.Controls.Add(lblPayFrom);
            grpPaymentDetails.Controls.Add(cmbPayFromLedger);
            grpPaymentDetails.Controls.Add(btnSelectPayFromLedger);
            grpPaymentDetails.Controls.Add(lblPayTo);
            grpPaymentDetails.Controls.Add(cmbPayToLedger);
            grpPaymentDetails.Controls.Add(btnSelectPayToLedger);
            grpPaymentDetails.Controls.Add(lblAmount);
            grpPaymentDetails.Controls.Add(txtAmount);
            grpPaymentDetails.Controls.Add(btnAutoClear);
            grpPaymentDetails.Controls.Add(lblTransactionDate);
            grpPaymentDetails.Controls.Add(dtpTransactionDate);
            grpPaymentDetails.Controls.Add(lblDescription);
            grpPaymentDetails.Controls.Add(txtDescription);
            grpPaymentDetails.Controls.Add(lblTransactionNumber);
            grpPaymentDetails.Controls.Add(txtTransactionNumber);
            grpPaymentDetails.Controls.Add(lblInvoiceNumber);
            grpPaymentDetails.Controls.Add(txtInvoiceNumber);
            
            grpPaymentMethod.Controls.Add(rdoBank);
            grpPaymentMethod.Controls.Add(rdoCash);
            grpPaymentMethod.Controls.Add(rdoCheque);
            grpPaymentMethod.Controls.Add(lblReferenceNumber);
            grpPaymentMethod.Controls.Add(txtReferenceNumber);
            
            grpInvoices.Controls.Add(chkSelectAll);
            grpInvoices.Controls.Add(dgvInvoices);
            grpInvoices.Controls.Add(lblTotalSelected);
            
            // 
            // PaymentEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1250, 650);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpInvoices);
            Controls.Add(grpPaymentMethod);
            Controls.Add(grpPaymentDetails);
            Controls.Add(grpTransactionType);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "PaymentEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = GetFormTitle();
            WindowState = FormWindowState.Maximized;
            KeyDown += PaymentEditForm_KeyDown;
            Load += PaymentEditForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default transaction type based on view mode
            if (_viewMode.Equals("Receipt", StringComparison.OrdinalIgnoreCase))
            {
                rdoReceipt.Checked = true;
            }
            else
            {
                rdoPayment.Checked = true;
            }
            
            // Set default payment method
            rdoBank.Checked = true;
            
            // Setup DataGridView columns
            SetupInvoiceDataGridView();
            
            // Update UI based on initial settings
            UpdateTransactionTypeUI();
            UpdatePaymentMethodUI();
            UpdateTransactionNumber();
        }

        private void SetupInvoiceDataGridView()
        {
            dgvInvoices.Columns.Clear();
            
            // Checkbox column for selection
            dgvInvoices.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = "Select",
                Width = 50
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionNumber",
                HeaderText = "Transaction #",
                DataPropertyName = "TransactionNumber",
                Width = 120,
                ReadOnly = true
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionDate",
                HeaderText = "Date",
                DataPropertyName = "TransactionDate",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PartyName",
                HeaderText = "Party",
                DataPropertyName = "PartyName",
                Width = 150,
                ReadOnly = true
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PaidAmount",
                HeaderText = "Paid",
                DataPropertyName = "PaidAmount",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BalanceDue",
                HeaderText = "Balance",
                DataPropertyName = "BalanceDue",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PaymentAmount",
                HeaderText = "Pay Amount",
                DataPropertyName = "PaymentAmount",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async void LoadData()
        {
            try
            {
                if (_selectedCompany == null || _selectedFinancialYear == null)
                {
                    lblStatus.Text = "No company or financial year selected";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                lblStatus.Text = "Loading data...";
                Application.DoEvents();

                // Load ledgers
                var companyId = Guid.Parse(_selectedCompany.Id);
                var financialYearId = _selectedFinancialYear.Id;
                
                _ledgers = await _ledgerService.GetAllLedgersAsync(companyId);
                
                // Clear combo boxes - they will be populated when user selects ledgers
                cmbPayFromLedger.DataSource = null;
                cmbPayFromLedger.Items.Clear();
                cmbPayFromLedger.Text = "Click ... to select ledger";
                
                cmbPayToLedger.DataSource = null;
                cmbPayToLedger.Items.Clear();
                cmbPayToLedger.Text = "Click ... to select ledger";
                
                // Load existing payment details if editing
                if (_existingPayment != null)
                {
                    lblStatus.Text = "Loading payment details...";
                    Application.DoEvents();
                    
                    var paymentDetails = await _paymentService.GetPaymentByIdAsync(Guid.Parse(_existingPayment.Id));
                    if (paymentDetails != null)
                    {
                        await LoadExistingPaymentData(paymentDetails);
                    }
                    else
                    {
                        MessageBox.Show("Failed to load payment details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // Generate transaction number for new payments
                    string prefix;
                    if (rdoPayment.Checked)
                    {
                        prefix = rdoCash.Checked ? "CP" : "BP"; // Cash Payment or Bank Payment
                    }
                    else
                    {
                        prefix = rdoCash.Checked ? "CR" : "BR"; // Cash Receipt or Bank Receipt
                    }
                    txtTransactionNumber.Text = $"{prefix}-{DateTime.Now:yyyyMMdd-HHmmss}";
                    
                    // Load unpaid transactions (will be filtered by selected ledger later)
                  
                }
                
                lblStatus.Text = "Ready - Please select ledgers to view outstanding bills";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExistingPaymentData(PaymentByIdDto paymentDetails)
        {
            try
            {
                _isLoadingExistingPayment = true; // Prevent event handlers from clearing the list
                
                // Populate form fields with existing payment data
                txtTransactionNumber.Text = paymentDetails.TransactionNumber;
                dtpTransactionDate.Value = paymentDetails.TransactionDate;
                txtReferenceNumber.Text = paymentDetails.ReferenceNumber;
                txtDescription.Text = paymentDetails.Notes;
                txtAmount.Text = paymentDetails.Total.ToString("N2");
                txtInvoiceNumber.Text = paymentDetails.InvoiceNumber ?? "";
                
                // Set payment type based on transaction type
                if (paymentDetails.PaymentType == PaymentType.PaymentReceived)
                {
                    rdoReceipt.Checked = true;
                }
                else
                {
                    rdoPayment.Checked = true;
                }
                
                // Set payment method based on PaymentMethod field
                if (paymentDetails.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                {
                    rdoCash.Checked = true;
                }
                else if (paymentDetails.PaymentMethod.Equals("Bank", StringComparison.OrdinalIgnoreCase))
                {
                    rdoBank.Checked = true;
                }
                else
                {
                    rdoBank.Checked = true; // Default to bank
                }
                
                // Load ledgers from the API response
                if (!string.IsNullOrEmpty(paymentDetails.PayFromLedgerId) && !string.IsNullOrEmpty(paymentDetails.PayToLedgerId))
                {
                    // Find the ledgers by ID
                    var payFromLedger = _ledgers.FirstOrDefault(l => l.Id.ToString() == paymentDetails.PayFromLedgerId);
                    var payToLedger = _ledgers.FirstOrDefault(l => l.Id.ToString() == paymentDetails.PayToLedgerId);
                    
                    // If not found in loaded ledgers, create a temporary ledger object
                    if (payFromLedger == null && !string.IsNullOrEmpty(paymentDetails.PayFromLedgerName))
                    {
                        payFromLedger = new LedgerModel
                        {
                            Id = Guid.Parse(paymentDetails.PayFromLedgerId),
                            Name = paymentDetails.PayFromLedgerName,
                            Code = paymentDetails.PayFromLedgerId.Substring(0, 8) // Use first 8 chars of ID as code
                        };
                    }
                    
                    if (payToLedger == null && !string.IsNullOrEmpty(paymentDetails.PayToLedgerName))
                    {
                        payToLedger = new LedgerModel
                        {
                            Id = Guid.Parse(paymentDetails.PayToLedgerId),
                            Name = paymentDetails.PayToLedgerName,
                            Code = paymentDetails.PayToLedgerId.Substring(0, 8) // Use first 8 chars of ID as code
                        };
                    }
                    
                    // Set the combo boxes
                    if (payFromLedger != null)
                    {
                        cmbPayFromLedger.Items.Clear();
                        cmbPayFromLedger.Items.Add(payFromLedger);
                        cmbPayFromLedger.SelectedItem = payFromLedger;
                        cmbPayFromLedger.Tag = payFromLedger;
                    }
                    
                    if (payToLedger != null)
                    {
                        cmbPayToLedger.Items.Clear();
                        cmbPayToLedger.Items.Add(payToLedger);
                        cmbPayToLedger.SelectedItem = payToLedger;
                        cmbPayToLedger.Tag = payToLedger;
                    }
                }
                
                // Update UI based on loaded data
                UpdateTransactionTypeUI();
                UpdatePaymentMethodUI();
                
                // Load associated transactions that were paid by this payment
                await LoadAssociatedTransactions(paymentDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading existing payment data: {ex.Message}");
                MessageBox.Show($"Error loading payment data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingExistingPayment = false; // Reset flag
            }
        }

        private async Task LoadAssociatedTransactions(PaymentByIdDto paymentDetails)
        {
            try
            {
                // First, load unpaid invoices for the selected party ledger
                var partyLedgerId = Guid.Parse(paymentDetails.PayToLedgerId);
                var unpaidInvoices = await _transactionService.GetUnpaidInvoicesAsync(partyLedgerId);

                // Convert unpaid invoices to TransactionListDto for display
                var allTransactions = new List<TransactionListDto>();

                foreach (var invoice in unpaidInvoices)
                {
                    var transaction = new TransactionListDto
                    {
                        Id = invoice.Id,
                        TransactionNumber = invoice.TransactionNumber,
                        InvoiceNumber = invoice.InvoiceNumber,
                        TransactionDate = invoice.TransactionDate,
                        DueDate = invoice.DueDate,
                        Type = invoice.Type,
                        Status = invoice.Status,
                        SubTotal = invoice.SubTotal,
                        TaxAmount = invoice.TaxAmount,
                        Total = invoice.Total,
                        PaidAmount = invoice.PaidAmount,
                        BalanceDue = invoice.BalanceDue,
                        PartyName = invoice.PartyName,
                        Notes = invoice.Notes,
                        PaymentAmount = 0m // Initialize to 0, will be set for paid invoices
                    };

                    allTransactions.Add(transaction);
                }

                // Add any paid invoices that are not already in the list
                var existingInvoiceIds = allTransactions.Select(t => t.Id).ToHashSet();
                foreach (var paidInvoice in paymentDetails.PaidInvoices)
                {
                    var invoiceId = Guid.Parse(paidInvoice.InvoiceId);
                    if (!existingInvoiceIds.Contains(invoiceId))
                    {
                        // This paid invoice is not in the unpaid list, so we need to add it
                        var transaction = new TransactionListDto
                        {
                            Id = invoiceId,
                            TransactionNumber = paidInvoice.InvoiceNumber,
                            InvoiceNumber = paidInvoice.InvoiceNumber,
                            TransactionDate = paidInvoice.InvoiceDate,
                            DueDate = paidInvoice.InvoiceDate, // Use invoice date as due date
                            Type = GetTransactionTypeFromString(paidInvoice.InvoiceType),
                            Status = "Paid",
                            SubTotal = paidInvoice.InvoiceTotal,
                            TaxAmount = 0m, // We don't have this info from paidInvoice
                            Total = paidInvoice.InvoiceTotal,
                            PaidAmount = paidInvoice.PaidAmount,
                            BalanceDue = paidInvoice.InvoiceTotal - paidInvoice.PaidAmount,
                            PartyName = paymentDetails.PayToLedgerName,
                            Notes = paidInvoice.Notes,
                            PaymentAmount = paidInvoice.PaidAmount // Set the payment amount
                        };

                        allTransactions.Add(transaction);
                    }
                }
                
                // Now mark the invoices that were actually paid by this payment
                var paidInvoiceIds = paymentDetails.PaidInvoices.Select(pi => Guid.Parse(pi.InvoiceId)).ToList();
                
                foreach (var transaction in allTransactions)
                {
                    if (paidInvoiceIds.Contains(transaction.Id))
                    {
                        // Find the corresponding paid invoice details
                        var paidInvoice = paymentDetails.PaidInvoices.FirstOrDefault(pi => Guid.Parse(pi.InvoiceId) == transaction.Id);
                        if (paidInvoice != null)
                        {
                            // Update the transaction with paid amount and mark as selected
                            transaction.PaymentAmount = paidInvoice.PaidAmount;
                            transaction.PaidAmount = paidInvoice.PaidAmount;
                            transaction.BalanceDue = paidInvoice.InvoiceTotal - paidInvoice.PaidAmount;
                        }
                    }
                }

                _unpaidTransactions = allTransactions;
                dgvInvoices.DataSource = _unpaidTransactions;
                
                // Mark only the paid invoices as selected and set their payment amounts
                foreach (DataGridViewRow row in dgvInvoices.Rows)
                {
                    if (row.DataBoundItem is TransactionListDto transaction)
                    {
                        if (paidInvoiceIds.Contains(transaction.Id))
                        {
                            row.Cells["Selected"].Value = true;
                            row.Cells["PaymentAmount"].Value = transaction.PaymentAmount;
                        }
                        else
                        {
                            row.Cells["Selected"].Value = false;
                            row.Cells["PaymentAmount"].Value = 0m; // Use decimal literal
                        }
                    }
                }
                
                UpdateTotalSelected();
                
                var totalInvoices = allTransactions.Count;
                var paidCount = paidInvoiceIds.Count;
                lblStatus.Text = $"Loaded {totalInvoices} invoices for {paymentDetails.PayToLedgerName}. {paidCount} invoice{(paidCount == 1 ? "" : "s")} selected as paid by this transaction.";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading associated transactions: {ex.Message}");
                _unpaidTransactions = new List<TransactionListDto>();
                dgvInvoices.DataSource = _unpaidTransactions;
                UpdateTotalSelected();
                lblStatus.Text = $"Error loading associated transactions: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

  
        private void UpdateTransactionTypeUI()
        {
            if (rdoPayment.Checked)
            {
                lblPayFrom.Text = "Pay To:";
                lblPayTo.Text = "Pay From:";
                grpInvoices.Text = "Select Bills to Pay";
            }
            else
            {
                lblPayFrom.Text = "Receive From:";
                lblPayTo.Text = "Receive To:";
                grpInvoices.Text = "Select Invoices to Receive Payment";
            }
        }

        private void UpdatePaymentMethodUI()
        {
            if (rdoBank.Checked)
            {
                lblReferenceNumber.Text = "UTR Number:";
                // txtReferenceNumber.PlaceholderText = "Enter UTR/Transaction ID"; // PlaceholderText not available in older .NET versions
            }
            else if (rdoCash.Checked)
            {
                lblReferenceNumber.Text = "Reference:";
                // txtReferenceNumber.PlaceholderText = "Enter reference number (optional)"; // PlaceholderText not available in older .NET versions
            }
            else if (rdoCheque.Checked)
            {
                lblReferenceNumber.Text = "Cheque Number:";
                // txtReferenceNumber.PlaceholderText = "Enter cheque number"; // PlaceholderText not available in older .NET versions
            }
        }

        private void UpdateTotalSelected()
        {
            _totalSelectedAmount = 0;
            _selectedTransactions.Clear();
            
            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells["Selected"].Value ?? false);
                if (selected && row.DataBoundItem is TransactionListDto transaction)
                {
                    var paymentAmount = Convert.ToDecimal(row.Cells["PaymentAmount"].Value ?? 0);
                    _totalSelectedAmount += paymentAmount;
                    _selectedTransactions.Add(transaction);
                }
            }
            
            lblTotalSelected.Text = $"Total Selected: ${_totalSelectedAmount:N2}";
            
            // Update main amount field only if we're not already updating it
            if (!_isUpdatingAmount)
            {
                _isUpdatingAmount = true;
                txtAmount.Text = _totalSelectedAmount.ToString("N2");
                _isUpdatingAmount = false;
            }
        }

        private string GetFormTitle()
        {
            var action = _existingPayment != null ? "Edit" : "New";
            var type = _viewMode.Equals("Receipt", StringComparison.OrdinalIgnoreCase) ? "Receipt" : "Payment";
            return $"{action} {type}";
        }

        private PaymentType GetPaymentTypeFromTransactionType(string transactionType)
        {
            return transactionType switch
            {
                "CashPayment" or "BankPayment" => PaymentType.PaymentMade,
                "CashReceipt" or "BankReceipt" => PaymentType.PaymentReceived,
                _ => PaymentType.PaymentMade
            };
        }

        private TransactionType GetTransactionTypeFromString(string transactionType)
        {
            return transactionType switch
            {
                "CashPayment" => TransactionType.CashPayment,
                "CashReceipt" => TransactionType.CashReceipt,
                "BankPayment" => TransactionType.BankPayment,
                "BankReceipt" => TransactionType.BankReceipt,
                "SaleInvoice" => TransactionType.SaleInvoice,
                "PurchaseBill" => TransactionType.PurchaseBill,
                "JournalEntry" => TransactionType.JournalEntry,
                _ => TransactionType.CashPayment
            };
        }

        private void UpdateTransactionNumber()
        {
            // Only update transaction number for new payments
            if (_existingPayment == null)
            {
                string prefix;
                if (rdoPayment.Checked)
                {
                    prefix = rdoCash.Checked ? "CP" : "BP"; // Cash Payment or Bank Payment
                }
                else
                {
                    prefix = rdoCash.Checked ? "CR" : "BR"; // Cash Receipt or Bank Receipt
                }
                txtTransactionNumber.Text = $"{prefix}-{DateTime.Now:yyyyMMdd-HHmmss}";
            }
        }

        // Event Handlers
        private void rdoPayment_CheckedChanged(object? sender, EventArgs e)
        {
            if (rdoPayment.Checked && !_isLoadingExistingPayment)
            {
                UpdateTransactionTypeUI();
                ClearLedgerSelections();               
                UpdateTransactionNumber();
            }
        }

        private void rdoReceipt_CheckedChanged(object? sender, EventArgs e)
        {
            if (rdoReceipt.Checked && !_isLoadingExistingPayment)
            {
                UpdateTransactionTypeUI();
                ClearLedgerSelections();               
                UpdateTransactionNumber();
            }
        }

        private void ClearLedgerSelections()
        {
            cmbPayFromLedger.Items.Clear();
            cmbPayFromLedger.Text = "Click ... to select ledger";
            cmbPayFromLedger.Tag = null;
            cmbPayToLedger.Items.Clear();
            cmbPayToLedger.Text = "Click ... to select ledger";
            cmbPayToLedger.Tag = null;
            // Don't clear invoice number as it's now user-editable
            _unpaidTransactions.Clear();
            dgvInvoices.DataSource = _unpaidTransactions;
            UpdateTotalSelected();
        }

        private void PaymentMethod_CheckedChanged(object? sender, EventArgs e)
        {
            UpdatePaymentMethodUI();
            UpdateTransactionNumber();
        }

        private void txtAmount_TextChanged(object? sender, EventArgs e)
        {
            // Skip if we're programmatically updating the amount
            if (_isUpdatingAmount) return;
            
            // Validate numeric input
            if (!string.IsNullOrEmpty(txtAmount.Text) && !decimal.TryParse(txtAmount.Text, out _))
            {
                // Remove non-numeric characters except decimal point
                var cleanText = new string(txtAmount.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
                if (cleanText != txtAmount.Text)
                {
                    var selectionStart = txtAmount.SelectionStart;
                    txtAmount.Text = cleanText;
                    txtAmount.SelectionStart = Math.Min(selectionStart, cleanText.Length);
                }
            }
        }

        private void btnAutoClear_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount to auto-clear.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_unpaidTransactions.Count == 0)
                {
                    MessageBox.Show("No outstanding bills available to clear.", "No Bills", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Sort transactions by date (oldest first)
                var sortedTransactions = _unpaidTransactions.OrderBy(t => t.TransactionDate).ToList();
                
                decimal remainingAmount = amount;
                
                // Clear all checkboxes first
                foreach (DataGridViewRow row in dgvInvoices.Rows)
                {
                    row.Cells["Selected"].Value = false;
                    row.Cells["PaymentAmount"].Value = 0m; // Use decimal literal
                }
                
                // Distribute amount across oldest bills first
                foreach (var transaction in sortedTransactions)
                {
                    if (remainingAmount <= 0) break;
                    
                    var row = dgvInvoices.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.DataBoundItem == transaction);
                    
                    if (row != null)
                    {
                        decimal billBalance = transaction.BalanceDue ?? 0;
                        decimal paymentAmount = Math.Min(remainingAmount, billBalance);
                        
                        row.Cells["Selected"].Value = true;
                        row.Cells["PaymentAmount"].Value = paymentAmount;
                        
                        remainingAmount -= paymentAmount;
                    }
                }
                
                UpdateTotalSelected();
                
                if (remainingAmount > 0)
                {
                    MessageBox.Show($"Auto-cleared {amount - remainingAmount:C2} from outstanding bills. Remaining amount: {remainingAmount:C2}", 
                        "Auto-Clear Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Successfully auto-cleared {amount:C2} from outstanding bills.", 
                        "Auto-Clear Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during auto-clear: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkSelectAll_CheckedChanged(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                row.Cells["Selected"].Value = chkSelectAll.Checked;
            }
            UpdateTotalSelected();
        }

        private void dgvInvoices_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvInvoices.Columns[e.ColumnIndex].Name;
                
                if (columnName == "Selected" || columnName == "PaymentAmount")
                {
                    UpdateTotalSelected();
                }
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                lblStatus.Text = "Saving payment...";
                Application.DoEvents();

                var paymentRequest = CreatePaymentRequest();
                
                if (_existingPayment != null)
                {
                    // Update existing payment
                    var updateRequest = new UpdatePaymentRequest
                    {
                        PaymentNumber = $"PAY-{DateTime.Now:yyyyMMdd-HHmmss}",
                        PaymentDate = paymentRequest.TransactionDate,
                        ReferenceNumber = paymentRequest.ReferenceNumber,
                        PaymentType = GetPaymentTypeFromTransactionType(paymentRequest.TransactionType),
                        Notes = paymentRequest.Description,
                        PaymentDetails = paymentRequest.Invoices.Select(i => new UpdatePaymentDetailRequest
                        {
                            Id = Guid.NewGuid().ToString(), // This should be the existing detail ID
                            LedgerId = paymentRequest.PayToLedgerId.ToString(),
                            DetailType = PaymentDetailType.Credit,
                            Amount = i.Amount,
                            Description = i.Notes,
                            SerialNumber = 1
                        }).ToList()
                    };
                    
                    var result = await _paymentService.UpdatePaymentAsync(Guid.Parse(_existingPayment.Id), updateRequest);
                    if (result != null)
                    {
                        MessageBox.Show("Payment updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Create new payment
                    var createRequest = new CreatePaymentRequest
                    {                      
                        TransactionDate = paymentRequest.TransactionDate,
                        ReferenceNumber = paymentRequest.ReferenceNumber,
                        TransactionType = paymentRequest.TransactionType, // Use the correct transaction type (CashPayment, CashReceipt, BankPayment, BankReceipt)
                        Description = paymentRequest.Description,
                        CompanyId = paymentRequest.CompanyId,
                        FinancialYearId = paymentRequest.FinancialYearId,
                        PayFromLedgerId = paymentRequest.PayFromLedgerId,
                        PayToLedgerId = paymentRequest.PayToLedgerId,
                        Amount = paymentRequest.Amount,
                        PaymentMethod = paymentRequest.PaymentMethod,
                        Invoices = paymentRequest.Invoices
                    };
                    
                    var result = await _paymentService.CreatePaymentAsync(createRequest);
                    if (result != null)
                    {
                        MessageBox.Show("Payment created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PaymentEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    btnCancel_Click(sender, e);
                    e.Handled = true;
                    break;
                case Keys.F2:
                    btnSave_Click(sender, e);
                    e.Handled = true;
                    break;
            }
        }

        private void PaymentEditForm_Load(object? sender, EventArgs e)
        {
            // Focus on first control
            rdoPayment.Focus();
        }

        private bool ValidateForm()
        {
            if (cmbPayFromLedger.Tag == null)
            {
                MessageBox.Show("Please select a ledger to pay from.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSelectPayFromLedger.Focus();
                return false;
            }

            if (cmbPayToLedger.Tag == null)
            {
                MessageBox.Show("Please select a ledger to pay to.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSelectPayToLedger.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            if (_selectedTransactions.Count == 0)
            {
                MessageBox.Show("Please select at least one invoice/bill to pay.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvInvoices.Focus();
                return false;
            }

            return true;
        }

        private CreatePaymentRequest CreatePaymentRequest()
        {
            var paymentMethod = rdoBank.Checked ? "Bank" : rdoCash.Checked ? "Cash" : "Cheque";
            
            // Determine transaction type based on payment method and receipt/payment selection
            // Four possible combinations:
            // 1. Payment + Cash = CashPayment (money going out via cash)
            // 2. Payment + Bank = BankPayment (money going out via bank)
            // 3. Receipt + Cash = CashReceipt (money coming in via cash)
            // 4. Receipt + Bank = BankReceipt (money coming in via bank)
            string transactionType;
            if (rdoPayment.Checked)
            {
                // Payment (money going out)
                transactionType = paymentMethod == "Cash" ? "CashPayment" : "BankPayment";
            }
            else
            {
                // Receipt (money coming in)
                transactionType = paymentMethod == "Cash" ? "CashReceipt" : "BankReceipt";
            }
            
            var payFromLedger = cmbPayFromLedger.Tag as LedgerModel;
            var payToLedger = cmbPayToLedger.Tag as LedgerModel;
            
            var request = new CreatePaymentRequest
            {
                CompanyId = Guid.Parse(_selectedCompany!.Id),
                FinancialYearId = _selectedFinancialYear!.Id,
                TransactionType = transactionType,
                TransactionNumber = txtTransactionNumber.Text,
                InvoiceNumber = txtInvoiceNumber.Text,
                PayFromLedgerId = payFromLedger!.Id,
                PayToLedgerId = payToLedger!.Id,
                Amount = decimal.Parse(txtAmount.Text),
                TransactionDate = dtpTransactionDate.Value,
                Description = txtDescription.Text,
                PaymentMethod = paymentMethod,
                ReferenceNumber = txtReferenceNumber.Text,
                Invoices = _selectedTransactions.Select(t => new InvoicePaymentItem
                {
                    TransactionId = t.Id,
                    Amount = Convert.ToDecimal(dgvInvoices.Rows.Cast<DataGridViewRow>()
                        .First(r => r.DataBoundItem == t).Cells["PaymentAmount"].Value),
                    Notes = txtDescription.Text
                }).ToList()
            };

            return request;
        }

        private void btnSelectPayFromLedger_Click(object? sender, EventArgs e)
        {
            try
            {
                var dialog = new LedgerSelectionDialog(
                    _ledgers, 
                    rdoPayment.Checked ? "Select Supplier to Pay To" : "Select Customer to Receive From",
                    rdoPayment.Checked ? "Select the supplier to pay to" : "Select the customer to receive from",
                    "Party Ledgers"
                );
                
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    // Clear and repopulate the combo box to ensure proper display
                    cmbPayFromLedger.Items.Clear();
                    cmbPayFromLedger.Items.Add(dialog.SelectedLedger);
                    cmbPayFromLedger.SelectedItem = dialog.SelectedLedger;
                    cmbPayFromLedger.Tag = dialog.SelectedLedger;
                    
                    // Load outstanding bills for the selected ledger
                    _ = LoadOutstandingBillsForLedger(dialog.SelectedLedger);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectPayToLedger_Click(object? sender, EventArgs e)
        {
            try
            {
                var dialog = new LedgerSelectionDialog(
                    _ledgers, 
                    rdoPayment.Checked ? "Select Account to Pay From" : "Select Account to Receive To",
                    rdoPayment.Checked ? "Select the account to pay from (e.g., Bank, Cash)" : "Select the account to receive to (e.g., Bank, Cash)",
                    rdoPayment.Checked ? "Bank Accounts" : "Cash Accounts"
                );
                
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    // Clear and repopulate the combo box to ensure proper display
                    cmbPayToLedger.Items.Clear();
                    cmbPayToLedger.Items.Add(dialog.SelectedLedger);
                    cmbPayToLedger.SelectedItem = dialog.SelectedLedger;
                    cmbPayToLedger.Tag = dialog.SelectedLedger;
                    
                    // Second ledger is just the account - don't load bills, preserve existing bills from first ledger
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadOutstandingBillsForLedger(LedgerModel selectedLedger)
        {
            try
            {
                if (_selectedCompany == null || _selectedFinancialYear == null)
                    return;

                lblStatus.Text = $"Loading outstanding bills for {selectedLedger.Name}...";
                Application.DoEvents();

                var companyId = Guid.Parse(_selectedCompany.Id);
                var financialYearId = _selectedFinancialYear.Id;
                
                // Determine transaction type based on current selection
                var transactionType = rdoPayment.Checked ? "Purchase" : "Sale";
                
                // Load unpaid invoices/bills for the selected ledger directly
                _unpaidTransactions = await _transactionService.GetUnpaidInvoicesAsync(selectedLedger.Id);
                
                // Bind to DataGridView
                dgvInvoices.DataSource = _unpaidTransactions;
                
                // Initialize payment amounts to balance due
                foreach (DataGridViewRow row in dgvInvoices.Rows)
                {
                    if (row.DataBoundItem is TransactionListDto transaction)
                    {
                        row.Cells["PaymentAmount"].Value = transaction.BalanceDue;
                    }
                }
                
                UpdateTotalSelected();
                
                // Set invoice number if only one transaction is found and field is empty
                if (string.IsNullOrEmpty(txtInvoiceNumber.Text))
                {
                    if (_unpaidTransactions.Count == 1)
                    {
                        txtInvoiceNumber.Text = _unpaidTransactions[0].InvoiceNumber ?? "";
                    }
                    else if (_unpaidTransactions.Count > 1)
                    {
                        txtInvoiceNumber.Text = "Multiple invoices";
                    }
                    else
                    {
                        txtInvoiceNumber.Text = "";
                    }
                }
                
                lblStatus.Text = $"Found {_unpaidTransactions.Count} outstanding bills for {selectedLedger.Name}";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading outstanding bills: {ex.Message}");
                _unpaidTransactions = new List<TransactionListDto>();
                dgvInvoices.DataSource = _unpaidTransactions;
                lblStatus.Text = $"Error loading bills: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }
    }
}
