using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class TransactionEditForm : BaseForm
    {
        private readonly TransactionService _transactionService;
        private readonly LocalStorageService _localStorageService;
        private readonly ProductService _productService;
        private readonly TaxService _taxService;
        private readonly LedgerService _ledgerService;
        private Models.Transaction? _transaction;
        private readonly Models.Company _selectedCompany;
        private readonly FinancialYearModel _selectedFinancialYear;

        // Header Controls
        private Label lblCompanyInfo = null!;
        private ComboBox cmbTransactionType = null!;
        private TextBox txtTransactionNumber = null!;
        private TextBox txtInvoiceNumber = null!;
        private DateTimePicker dtpTransactionDate = null!;
        private DateTimePicker dtpDueDate = null!;
        private ComboBox cmbStatus = null!;
        private TextBox txtReferenceNumber = null!;
        private TextBox txtNotes = null!;

        // Items Section
        private DataGridView dgvItems = null!;
        private Button btnAddItem = null!;
        private Button btnEditItem = null!;
        private Button btnDeleteItem = null!;
        private Button btnSelectProducts = null!;

        // Tax Section
        private DataGridView dgvTaxes = null!;
        private Button btnAddTax = null!;
        private Button btnEditTax = null!;
        private Button btnDeleteTax = null!;
        private Button btnSelectTaxes = null!;

        // Summary Section
        private TextBox txtSubTotal = null!;
        private TextBox txtDiscountPercent = null!;
        private TextBox txtDiscountAmount = null!;
        private TextBox txtFreight = null!;
        private CheckBox chkFreightIncluded = null!;
        private TextBox txtTaxAmount = null!;
        private TextBox txtRoundOff = null!;
        private TextBox txtTotal = null!;

        // Action Buttons
        private Button btnSave = null!;
        private Button btnCancel = null!;

        private List<Models.ProductListDto> _availableProducts = new List<Models.ProductListDto>();
        private List<Models.TaxListDto> _availableTaxes = new List<Models.TaxListDto>();

        // Wrapper classes for DataGridView display
        private class TransactionItemDisplay
        {
            public Guid Id { get; set; }
            public Guid TransactionId { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TaxRate { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal DiscountRate { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal LineTotal { get; set; }
            public decimal CurrentQuantity { get; set; }
            public int SerialNumber { get; set; }

            public TransactionItemDisplay(TransactionItem item, string productName)
            {
                Id = item.Id;
                TransactionId = item.TransactionId;
                ProductId = Guid.Parse(item.ProductId); // Convert string to Guid
                ProductName = productName;
                Description = item.Description;
                Quantity = item.Quantity;
                UnitPrice = item.UnitPrice;
                TaxRate = item.TaxRate;
                TaxAmount = item.TaxAmount;
                DiscountRate = item.DiscountRate;
                DiscountAmount = item.DiscountAmount;
                LineTotal = item.LineTotal;
                CurrentQuantity = item.CurrentQuantity;
                SerialNumber = item.SerialNumber;
            }

            public TransactionItem ToTransactionItem()
            {
                return new TransactionItem
                {
                    Id = Id,
                    TransactionId = TransactionId,
                    ProductId = ProductId.ToString(), // Convert Guid to string
                    Description = Description,
                    Quantity = Quantity,
                    UnitPrice = UnitPrice,
                    TaxRate = TaxRate,
                    TaxAmount = TaxAmount,
                    DiscountRate = DiscountRate,
                    DiscountAmount = DiscountAmount,
                    LineTotal = LineTotal,
                    CurrentQuantity = CurrentQuantity,
                    SerialNumber = SerialNumber
                };
            }
        }

        private class TransactionTaxDisplay
        {
            public Guid Id { get; set; }
            public Guid TransactionId { get; set; }
            public Guid TaxId { get; set; }
            public string TaxName { get; set; } = string.Empty;
            public string TaxComponents { get; set; } = string.Empty;
            public decimal TaxableAmount { get; set; }
            public decimal TaxAmount { get; set; }
            public string CalculationMethod { get; set; } = string.Empty;
            public bool IsApplied { get; set; }
            public DateTime? AppliedDate { get; set; }
            public string? ReferenceNumber { get; set; }
            public string? Description { get; set; }
            public int SerialNumber { get; set; }

            public TransactionTaxDisplay(TransactionTax tax, string taxName, string taxComponents)
            {
                Id = tax.Id;
                TransactionId = tax.TransactionId;
                TaxId = Guid.Parse(tax.TaxId); // Convert string to Guid
                TaxName = taxName;
                TaxComponents = taxComponents;
                TaxableAmount = tax.TaxableAmount;
                TaxAmount = tax.TaxAmount;
                CalculationMethod = tax.CalculationMethod;
                IsApplied = tax.IsApplied;
                AppliedDate = tax.AppliedDate;
                ReferenceNumber = tax.ReferenceNumber;
                Description = tax.Description;
                SerialNumber = tax.SerialNumber;
            }

            public TransactionTax ToTransactionTax()
            {
                return new TransactionTax
                {
                    Id = Id,
                    TransactionId = TransactionId,
                    TaxId = TaxId.ToString(), // Convert Guid to string
                    TaxableAmount = TaxableAmount,
                    TaxAmount = TaxAmount,
                    CalculationMethod = CalculationMethod,
                    IsApplied = IsApplied,
                    AppliedDate = AppliedDate,
                    ReferenceNumber = ReferenceNumber,
                    Description = Description,
                    SerialNumber = SerialNumber
                };
            }
        }

        public TransactionEditForm(TransactionService transactionService, LocalStorageService localStorageService,
            Models.Transaction? transaction, Models.Company selectedCompany, FinancialYearModel selectedFinancialYear,
            ProductService? productService = null, TaxService? taxService = null, LedgerService? ledgerService = null)
        {
            _transactionService = transactionService;
            _localStorageService = localStorageService;
            _transaction = transaction;
            _selectedCompany = selectedCompany;
            _selectedFinancialYear = selectedFinancialYear;

            // Use provided services or create new ones (for now, create basic services)
            _productService = productService ?? new ProductService(new AuthService());
            _taxService = taxService ?? new TaxService(new AuthService());
            _ledgerService = ledgerService ?? new LedgerService(new AuthService());

            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void InitializeComponent()
        {
            // Initialize all controls
            lblCompanyInfo = new Label();
            cmbTransactionType = new ComboBox();
            txtTransactionNumber = new TextBox();
            txtInvoiceNumber = new TextBox();
            dtpTransactionDate = new DateTimePicker();
            dtpDueDate = new DateTimePicker();
            cmbStatus = new ComboBox();
            txtReferenceNumber = new TextBox();
            txtNotes = new TextBox();

            dgvItems = new DataGridView();
            btnAddItem = new Button();
            btnEditItem = new Button();
            btnDeleteItem = new Button();
            btnSelectProducts = new Button();

            dgvTaxes = new DataGridView();
            btnAddTax = new Button();
            btnEditTax = new Button();
            btnDeleteTax = new Button();
            btnSelectTaxes = new Button();

            txtSubTotal = new TextBox();
            txtDiscountPercent = new TextBox();
            txtDiscountAmount = new TextBox();
            txtFreight = new TextBox();
            chkFreightIncluded = new CheckBox();
            txtTaxAmount = new TextBox();
            txtRoundOff = new TextBox();
            txtTotal = new TextBox();

            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Form properties
            Text = _transaction == null ? "Add Transaction" : "Edit Transaction";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            // KeyPreview is handled by BaseForm

            SetupLayout();

            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            var yPosition = 10;
            var leftMargin = 20;
            var rightMargin = 20;
            var controlHeight = 25;
            var spaceBetweenRows = 30;
            var labelWidth = 120;
            var controlWidth = 200;

            // Company Header
            lblCompanyInfo.Location = new Point(leftMargin, yPosition);
            lblCompanyInfo.Size = new Size(1100, 25);
            lblCompanyInfo.Text = $"Company: {_selectedCompany.DisplayName} | Financial Year: {_selectedFinancialYear.YearLabel}";
            lblCompanyInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            yPosition += 40;

            // First Row - Transaction Type, Number, Invoice Number
            AddLabelAndControl("Transaction Type:", cmbTransactionType, leftMargin, yPosition, labelWidth, controlWidth);
            AddLabelAndControl("Transaction #:", txtTransactionNumber, leftMargin + 350, yPosition, labelWidth, controlWidth);
            AddLabelAndControl("Invoice #:", txtInvoiceNumber, leftMargin + 700, yPosition, labelWidth, controlWidth);
            yPosition += spaceBetweenRows;

            // Second Row - Dates, Status
            AddLabelAndControl("Transaction Date:", dtpTransactionDate, leftMargin, yPosition, labelWidth, controlWidth);
            AddLabelAndControl("Due Date:", dtpDueDate, leftMargin + 350, yPosition, labelWidth, controlWidth);
            AddLabelAndControl("Status:", cmbStatus, leftMargin + 700, yPosition, labelWidth, controlWidth);
            yPosition += spaceBetweenRows;

            // Third Row - Reference Number, Notes
            AddLabelAndControl("Reference #:", txtReferenceNumber, leftMargin, yPosition, labelWidth, controlWidth);
            AddLabelAndControl("Notes:", txtNotes, leftMargin + 350, yPosition, labelWidth, 550);
            yPosition += spaceBetweenRows + 10;

            // Items Section
            var itemsGroupBox = new GroupBox
            {
                Text = "Transaction Items",
                Location = new Point(leftMargin, yPosition),
                Size = new Size(1140, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvItems.Location = new Point(10, 25);
            dgvItems.Size = new Size(1020, 180);
            dgvItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnSelectProducts.Location = new Point(1040, 25);
            btnSelectProducts.Size = new Size(90, 30);
            btnSelectProducts.Text = "Select Products";
            btnSelectProducts.Click += BtnSelectProducts_Click;

            btnAddItem.Location = new Point(1040, 65);
            btnAddItem.Size = new Size(90, 30);
            btnAddItem.Text = "Add Item";
            btnAddItem.Click += BtnAddItem_Click;

            btnEditItem.Location = new Point(1040, 105);
            btnEditItem.Size = new Size(90, 30);
            btnEditItem.Text = "Edit Item";
            btnEditItem.Click += BtnEditItem_Click;

            btnDeleteItem.Location = new Point(1040, 145);
            btnDeleteItem.Size = new Size(90, 30);
            btnDeleteItem.Text = "Delete Item";
            btnDeleteItem.Click += BtnDeleteItem_Click;

            itemsGroupBox.Controls.AddRange(new Control[] { dgvItems, btnSelectProducts, btnAddItem, btnEditItem, btnDeleteItem });
            yPosition += 260;

            // Tax Section
            var taxGroupBox = new GroupBox
            {
                Text = "Transaction Taxes",
                Location = new Point(leftMargin, yPosition),
                Size = new Size(1140, 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvTaxes.Location = new Point(10, 25);
            dgvTaxes.Size = new Size(1020, 110);
            dgvTaxes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnSelectTaxes.Location = new Point(1040, 25);
            btnSelectTaxes.Size = new Size(90, 30);
            btnSelectTaxes.Text = "Select Taxes";
            btnSelectTaxes.Click += BtnSelectTaxes_Click;

            btnAddTax.Location = new Point(1040, 65);
            btnAddTax.Size = new Size(90, 30);
            btnAddTax.Text = "Add Tax";
            btnAddTax.Click += BtnAddTax_Click;

            btnEditTax.Location = new Point(1040, 105);
            btnEditTax.Size = new Size(90, 30);
            btnEditTax.Text = "Edit Tax";
            btnEditTax.Click += BtnEditTax_Click;

            btnDeleteTax.Location = new Point(1040, 145);
            btnDeleteTax.Size = new Size(90, 30);
            btnDeleteTax.Text = "Delete Tax";
            btnDeleteTax.Click += BtnDeleteTax_Click;

            taxGroupBox.Controls.AddRange(new Control[] { dgvTaxes, btnSelectTaxes, btnAddTax, btnEditTax, btnDeleteTax });
            yPosition += 190;

            // Summary Section - Improved Layout
            var summaryGroupBox = new GroupBox
            {
                Text = "Transaction Summary",
                Location = new Point(leftMargin, yPosition),
                Size = new Size(1140, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            // First Row - Subtotal and Discount
            var summaryYPos = 30;
            var summaryLabelWidth = 120;
            var summaryControlWidth = 140;
            var summarySpacing = 20;

            // Subtotal (Left side)
            AddLabelAndControlToParent("Subtotal:", txtSubTotal, summaryGroupBox, 20, summaryYPos, summaryLabelWidth, summaryControlWidth, true);
            
            // Discount % (Center left)
            AddLabelAndControlToParent("Discount %:", txtDiscountPercent, summaryGroupBox, 20 + summaryLabelWidth + summaryControlWidth + summarySpacing, summaryYPos, 90, 80);
            
            // Discount Amount (Center right)
            AddLabelAndControlToParent("Discount Amt:", txtDiscountAmount, summaryGroupBox, 20 + (summaryLabelWidth + summaryControlWidth + summarySpacing) * 2 + 90, summaryYPos, 100, summaryControlWidth, true);
            
            // Freight (Right side)
            AddLabelAndControlToParent("Freight:", txtFreight, summaryGroupBox, 20 + (summaryLabelWidth + summaryControlWidth + summarySpacing) * 3 + 190, summaryYPos, 80, summaryControlWidth);

            // Freight Included checkbox
            chkFreightIncluded.Location = new Point(20 + (summaryLabelWidth + summaryControlWidth + summarySpacing) * 4 + 270, summaryYPos);
            chkFreightIncluded.Size = new Size(130, 25);
            chkFreightIncluded.Text = "Freight Included";
            chkFreightIncluded.Font = new Font("Segoe UI", 9F);
            summaryGroupBox.Controls.Add(chkFreightIncluded);

            // Second Row - Tax, Round Off, and Total
            summaryYPos += 40;
            
            // Tax Amount (Left side)
            AddLabelAndControlToParent("Tax Amount:", txtTaxAmount, summaryGroupBox, 20, summaryYPos, summaryLabelWidth, summaryControlWidth, true);
            
            // Round Off (Center)
            AddLabelAndControlToParent("Round Off:", txtRoundOff, summaryGroupBox, 20 + summaryLabelWidth + summaryControlWidth + summarySpacing, summaryYPos, 90, summaryControlWidth);
            
            // Total (Right side) - Make it prominent
            AddLabelAndControlToParent("Total:", txtTotal, summaryGroupBox, 20 + (summaryLabelWidth + summaryControlWidth + summarySpacing) * 2 + 90, summaryYPos, 100, summaryControlWidth + 20, true);

            // Add a separator line
            var separatorLine = new Panel
            {
                Location = new Point(20, summaryYPos - 15),
                Size = new Size(1100, 1),
                BackColor = Color.LightGray
            };
            summaryGroupBox.Controls.Add(separatorLine);

            yPosition += 150;

            // Action Buttons
            btnSave.Location = new Point(1000, yPosition);
            btnSave.Size = new Size(80, 35);
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            btnCancel.Location = new Point(1090, yPosition);
            btnCancel.Size = new Size(80, 35);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            Controls.AddRange(new Control[] { lblCompanyInfo, itemsGroupBox, taxGroupBox, summaryGroupBox, btnSave, btnCancel });
        }

        private void AddLabelAndControl(string labelText, Control control, int x, int y, int labelWidth, int controlWidth)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 3),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            control.Location = new Point(x + labelWidth + 5, y);
            control.Size = new Size(controlWidth, 25);

            Controls.Add(label);
            Controls.Add(control);
        }

        private void AddLabelAndControlToParent(string labelText, Control control, Control parent, int x, int y, int labelWidth, int controlWidth, bool readOnly = false)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 3),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            control.Location = new Point(x + labelWidth + 5, y);
            control.Size = new Size(controlWidth, 25);

            if (control is TextBox textBox && readOnly)
            {
                textBox.ReadOnly = true;
                textBox.BackColor = SystemColors.Control;
            }

            parent.Controls.Add(label);
            parent.Controls.Add(control);
        }

        private void SetupForm()
        {
            // KeyDown is handled by BaseForm
            SetupComboBoxes();
            SetupDataGridViews();
            SetupEventHandlers();
        }

        private void SetupComboBoxes()
        {
            // Transaction Type
            cmbTransactionType.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (TransactionType type in Enum.GetValues<TransactionType>())
            {
                cmbTransactionType.Items.Add(type);
            }

            // Status
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new[] { "Draft", "Pending", "Approved", "Rejected", "Completed", "Cancelled" });
            cmbStatus.SelectedIndex = 0; // Default to Draft
        }

        private void SetupDataGridViews()
        {
            SetupItemsDataGridView();
            SetupTaxesDataGridView();
        }

        private void SetupItemsDataGridView()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.MultiSelect = false;
            dgvItems.RowHeadersVisible = false;

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Product",
                DataPropertyName = "ProductName",
                Width = 200
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Qty",
                DataPropertyName = "Quantity",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Unit Price",
                DataPropertyName = "UnitPrice",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiscountRate",
                HeaderText = "Disc %",
                DataPropertyName = "DiscountRate",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiscountAmount",
                HeaderText = "Disc Amt",
                DataPropertyName = "DiscountAmount",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxRate",
                HeaderText = "Tax %",
                DataPropertyName = "TaxRate",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxAmount",
                HeaderText = "Tax Amt",
                DataPropertyName = "TaxAmount",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineTotal",
                HeaderText = "Line Total",
                DataPropertyName = "LineTotal",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private void SetupTaxesDataGridView()
        {
            dgvTaxes.AutoGenerateColumns = false;
            dgvTaxes.AllowUserToAddRows = false;
            dgvTaxes.AllowUserToDeleteRows = false;
            dgvTaxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaxes.MultiSelect = false;
            dgvTaxes.RowHeadersVisible = false;

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxName",
                HeaderText = "Tax Name",
                DataPropertyName = "TaxName",
                Width = 200
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxComponents",
                HeaderText = "Tax Components",
                DataPropertyName = "TaxComponents",
                Width = 200
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxableAmount",
                HeaderText = "Taxable Amount",
                DataPropertyName = "TaxableAmount",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxAmount",
                HeaderText = "Tax Amount",
                DataPropertyName = "TaxAmount",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalculationMethod",
                HeaderText = "Calculation Method",
                DataPropertyName = "CalculationMethod",
                Width = 150
            });
        }

        private void SetupEventHandlers()
        {
            // Add event handlers for calculation
            txtDiscountPercent.TextChanged += CalculationField_Changed;
            txtDiscountAmount.TextChanged += CalculationField_Changed;
            txtFreight.TextChanged += CalculationField_Changed;
            txtRoundOff.TextChanged += CalculationField_Changed;
            chkFreightIncluded.CheckedChanged += CalculationField_Changed;
        }

        private async void LoadData()
        {
            // Load product and tax lists first
            await LoadProductAndTaxLists();

            if (_transaction != null)
            {
                // Load existing transaction data
                cmbTransactionType.SelectedItem = _transaction.Type;
                txtTransactionNumber.Text = _transaction.TransactionNumber;
                txtInvoiceNumber.Text = _transaction.InvoiceNumber ?? "";
                dtpTransactionDate.Value = _transaction.TransactionDate;
                dtpDueDate.Value = _transaction.DueDate;
                cmbStatus.SelectedItem = _transaction.Status;
                txtReferenceNumber.Text = _transaction.ReferenceNumber ?? "";
                txtNotes.Text = _transaction.Notes ?? "";

                // Convert items to display format
                var itemDisplays = _transaction.Items.Select(item => 
                {
                    var product = _availableProducts.FirstOrDefault(p => p.Id == item.ProductId.ToString());
                    var productName = product?.DisplayName ?? "Unknown Product";
                    return new TransactionItemDisplay(item, productName);
                }).ToList();

                // Convert taxes to display format
                var taxDisplays = _transaction.Taxes.Select(tax => 
                {
                    var taxInfo = _availableTaxes.FirstOrDefault(t => t.Id == tax.TaxId.ToString());
                    var taxName = taxInfo?.DisplayName ?? "Unknown Tax";
                    var taxComponents = taxInfo?.Components != null && taxInfo.Components.Any() 
                        ? string.Join(", ", taxInfo.Components.Select(c => c.DisplayName))
                        : "No Components";
                    return new TransactionTaxDisplay(tax, taxName, taxComponents);
                }).ToList();

                dgvItems.DataSource = itemDisplays;
                dgvTaxes.DataSource = taxDisplays;

                txtDiscountPercent.Text = _transaction.Discount.ToString("N2");
                txtDiscountAmount.Text = "0.00"; // DiscountAmount property was removed from model
                txtFreight.Text = _transaction.Freight.ToString("N2");
                chkFreightIncluded.Checked = _transaction.IsFreightIncluded;
                txtRoundOff.Text = _transaction.RoundOff.ToString("N2");

                CalculateTotals();
            }
            else
            {
                // New transaction defaults
                cmbTransactionType.SelectedIndex = 0;
                dtpTransactionDate.Value = DateTime.Now;
                dtpDueDate.Value = DateTime.Now.AddDays(30);
                txtTransactionNumber.Text = GenerateTransactionNumber();

                dgvItems.DataSource = new List<TransactionItemDisplay>();
                dgvTaxes.DataSource = new List<TransactionTaxDisplay>();
            }
        }

        private async Task LoadProductAndTaxLists()
        {
            try
            {
                var companyId = Guid.Parse(_selectedCompany.Id);
                
                // Load products
                _availableProducts = await _productService.GetProductListForTransactionAsync(companyId);
                Console.WriteLine($"Loaded {_availableProducts.Count} products for transaction");

                // Load taxes
                _availableTaxes = await _taxService.GetTaxListForTransactionAsync(companyId);
                Console.WriteLine($"Loaded {_availableTaxes.Count} taxes for transaction");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading product and tax lists: {ex.Message}");
                MessageBox.Show($"Error loading product and tax lists: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateTransactionNumber()
        {
            // Generate a simple transaction number - you might want to make this more sophisticated
            return $"TXN{DateTime.Now:yyyyMMddHHmmss}";
        }

        protected override bool HandleEnterKey()
        {
            // F10 or Ctrl+Enter to save
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                BtnSave_Click(null, EventArgs.Empty);
                return true;
            }
            
            // Enter on save button
            if (ActiveControl == btnSave)
            {
                BtnSave_Click(null, EventArgs.Empty);
                return true;
            }
            
            // Enter on cancel button
            if (ActiveControl == btnCancel)
            {
                BtnCancel_Click(null, EventArgs.Empty);
                return true;
            }
            
            return false; // Let BaseForm handle navigation
        }

        protected override void HandleEscapeKey()
        {
            BtnCancel_Click(null, EventArgs.Empty);
        }

        protected override void ShowHelp()
        {
            var helpMessage = @"Transaction Edit Form - Keyboard Navigation Help:

Navigation:
• Tab - Move to next field
• Backspace - Move to previous field
• Enter - Confirm action or move to next field
• Ctrl+Enter - Save transaction
• Escape - Close form or go back
• F1 - Show this help

Special Keys:
• F2 - Select Products
• F3 - Select Taxes
• F10 - Save Transaction

Field Order:
1. Transaction Type (dropdown)
2. Transaction Number
3. Invoice Number
4. Transaction Date
5. Due Date
6. Status (dropdown)
7. Reference Number
8. Notes
9. Transaction Items (DataGrid)
10. Transaction Taxes (DataGrid)
11. Subtotal (read-only)
12. Discount Percent
13. Discount Amount
14. Freight
15. Freight Included (checkbox)
16. Tax Amount (read-only)
17. Round Off
18. Total (read-only)
19. Save Button
20. Cancel Button

Tips:
• Use Tab/Backspace for fast field navigation
• Ctrl+Enter to save from any field
• F2/F3 for quick product/tax selection
• Escape to cancel and close
• All fields are highlighted when focused";

            MessageBox.Show(helpMessage, "Transaction Edit Form Help", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CalculationField_Changed(object? sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            try
            {
                var itemsSource = dgvItems.DataSource as List<TransactionItemDisplay>;
                var taxesSource = dgvTaxes.DataSource as List<TransactionTaxDisplay>;

                if (itemsSource == null) itemsSource = new List<TransactionItemDisplay>();
                if (taxesSource == null) taxesSource = new List<TransactionTaxDisplay>();

                // Calculate subtotal from items
                var subtotal = itemsSource.Sum(item => item.LineTotal);
                txtSubTotal.Text = subtotal.ToString("N2");

                // Apply discount
                decimal discountPercent = 0;
                decimal discountAmount = 0;
                
                if (decimal.TryParse(txtDiscountPercent.Text, out discountPercent))
                {
                    discountAmount = subtotal * (discountPercent / 100);
                    txtDiscountAmount.Text = discountAmount.ToString("N2");
                }
                else if (decimal.TryParse(txtDiscountAmount.Text, out discountAmount))
                {
                    if (subtotal > 0)
                    {
                        discountPercent = (discountAmount / subtotal) * 100;
                        txtDiscountPercent.Text = discountPercent.ToString("N2");
                    }
                }

                var discountedSubtotal = subtotal - discountAmount;

                // Calculate tax
                var taxAmount = taxesSource.Sum(tax => tax.TaxAmount);
                txtTaxAmount.Text = taxAmount.ToString("N2");

                // Add freight if not included
                decimal freight = 0;
                decimal.TryParse(txtFreight.Text, out freight);

                var beforeRounding = discountedSubtotal + taxAmount;
                if (!chkFreightIncluded.Checked)
                {
                    beforeRounding += freight;
                }

                // Apply round off
                decimal roundOff = 0;
                decimal.TryParse(txtRoundOff.Text, out roundOff);

                var total = beforeRounding + roundOff;
                txtTotal.Text = total.ToString("N2");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating totals: {ex.Message}");
            }
        }

        // Event handlers for buttons (simplified - you'll need to implement these)
        private void BtnSelectProducts_Click(object? sender, EventArgs e)
        {
            var message = $"Available Products ({_availableProducts.Count}):\n\n";
            foreach (var product in _availableProducts.Take(10)) // Show first 10 products
            {
                message += $"• {product.DisplayName} - {product.SellingPrice:C}\n";
            }
            if (_availableProducts.Count > 10)
            {
                message += $"\n... and {_availableProducts.Count - 10} more products";
            }
            MessageBox.Show(message, "Available Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            if (_availableProducts.Count == 0)
            {
                MessageBox.Show("No products available. Please add products first.", "No Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var message = $"Add Item - Available Products ({_availableProducts.Count}):\n\n";
            foreach (var product in _availableProducts.Take(5)) // Show first 5 products
            {
                message += $"• {product.DisplayName} - {product.SellingPrice:C}\n";
            }
            if (_availableProducts.Count > 5)
            {
                message += $"\n... and {_availableProducts.Count - 5} more products";
            }
            MessageBox.Show(message, "Add Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditItem_Click(object? sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvItems.SelectedRows[0].DataBoundItem as TransactionItemDisplay;
            if (selectedItem != null)
            {
                var message = $"Edit Item:\n\nProduct: {selectedItem.ProductName}\nDescription: {selectedItem.Description}\nQuantity: {selectedItem.Quantity}\nUnit Price: {selectedItem.UnitPrice:C}";
                MessageBox.Show(message, "Edit Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteItem_Click(object? sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                var items = dgvItems.DataSource as List<TransactionItemDisplay>;
                var selectedItem = dgvItems.SelectedRows[0].DataBoundItem as TransactionItemDisplay;
                
                if (items != null && selectedItem != null)
                {
                    items.Remove(selectedItem);
                    dgvItems.DataSource = null;
                    dgvItems.DataSource = items;
                    CalculateTotals();
                }
            }
        }

        private void BtnSelectTaxes_Click(object? sender, EventArgs e)
        {
            var message = $"Available Taxes ({_availableTaxes.Count}):\n\n";
            foreach (var tax in _availableTaxes.Take(10)) // Show first 10 taxes
            {
                var components = tax.Components.Any() 
                    ? string.Join(", ", tax.Components.Select(c => c.DisplayName))
                    : "No Components";
                message += $"• {tax.DisplayName} ({tax.DefaultRate}%)\n  Components: {components}\n\n";
            }
            if (_availableTaxes.Count > 10)
            {
                message += $"... and {_availableTaxes.Count - 10} more taxes";
            }
            MessageBox.Show(message, "Available Taxes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAddTax_Click(object? sender, EventArgs e)
        {
            if (_availableTaxes.Count == 0)
            {
                MessageBox.Show("No taxes available. Please add taxes first.", "No Taxes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var message = $"Add Tax - Available Taxes ({_availableTaxes.Count}):\n\n";
            foreach (var tax in _availableTaxes.Take(5)) // Show first 5 taxes
            {
                var components = tax.Components.Any() 
                    ? string.Join(", ", tax.Components.Select(c => c.DisplayName))
                    : "No Components";
                message += $"• {tax.DisplayName} ({tax.DefaultRate}%)\n  Components: {components}\n\n";
            }
            if (_availableTaxes.Count > 5)
            {
                message += $"... and {_availableTaxes.Count - 5} more taxes";
            }
            MessageBox.Show(message, "Add Tax", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditTax_Click(object? sender, EventArgs e)
        {
            if (dgvTaxes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a tax to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedTax = dgvTaxes.SelectedRows[0].DataBoundItem as TransactionTaxDisplay;
            if (selectedTax != null)
            {
                var message = $"Edit Tax:\n\nTax: {selectedTax.TaxName}\nComponents: {selectedTax.TaxComponents}\nTaxable Amount: {selectedTax.TaxableAmount:C}\nTax Amount: {selectedTax.TaxAmount:C}";
                MessageBox.Show(message, "Edit Tax", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteTax_Click(object? sender, EventArgs e)
        {
            if (dgvTaxes.SelectedRows.Count > 0)
            {
                var taxes = dgvTaxes.DataSource as List<TransactionTaxDisplay>;
                var selectedTax = dgvTaxes.SelectedRows[0].DataBoundItem as TransactionTaxDisplay;
                
                if (taxes != null && selectedTax != null)
                {
                    taxes.Remove(selectedTax);
                    dgvTaxes.DataSource = null;
                    dgvTaxes.DataSource = taxes;
                    CalculateTotals();
                }
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    var transaction = CreateTransactionFromForm();
                    
                    Models.Transaction? result;
                    if (_transaction == null)
                    {
                        result = await _transactionService.CreateTransactionAsync(transaction);
                    }
                    else
                    {
                        var success = await _transactionService.UpdateTransactionAsync(_transaction.Id, transaction);
                        result = success ? transaction : null;
                    }

                    if (result != null)
                    {
                        MessageBox.Show("Transaction saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save transaction. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a transaction type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTransactionType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTransactionNumber.Text))
            {
                MessageBox.Show("Please enter a transaction number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTransactionNumber.Focus();
                return false;
            }

            return true;
        }

        private Models.Transaction CreateTransactionFromForm()
        {
            var transaction = _transaction ?? new Models.Transaction();

            transaction.Type = cmbTransactionType.SelectedItem?.ToString() ?? string.Empty;
            transaction.TransactionNumber = txtTransactionNumber.Text.Trim();
            transaction.InvoiceNumber = string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ? null : txtInvoiceNumber.Text.Trim();
            transaction.TransactionDate = dtpTransactionDate.Value;
            transaction.DueDate = dtpDueDate.Value;
            transaction.Status = cmbStatus.SelectedItem?.ToString() ?? "Draft";
            transaction.ReferenceNumber = string.IsNullOrWhiteSpace(txtReferenceNumber.Text) ? null : txtReferenceNumber.Text.Trim();
            transaction.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            transaction.CompanyId = Guid.Parse(_selectedCompany.Id);
            transaction.FinancialYearId = _selectedFinancialYear.Id;

            // Parse summary fields
            decimal.TryParse(txtSubTotal.Text, out var subtotal);
            decimal.TryParse(txtDiscountPercent.Text, out var discountPercent);
            decimal.TryParse(txtDiscountAmount.Text, out var discountAmount);
            decimal.TryParse(txtFreight.Text, out var freight);
            decimal.TryParse(txtTaxAmount.Text, out var taxAmount);
            decimal.TryParse(txtRoundOff.Text, out var roundOff);
            decimal.TryParse(txtTotal.Text, out var total);

            transaction.SubTotal = subtotal;
            transaction.Discount = discountPercent;
            // transaction.DiscountAmount = discountAmount; // Property removed from model
            transaction.Freight = freight;
            transaction.IsFreightIncluded = chkFreightIncluded.Checked;
            transaction.TaxAmount = taxAmount;
            transaction.RoundOff = roundOff;
            transaction.Total = total;

            // Set items and taxes from grids
            var itemDisplays = dgvItems.DataSource as List<TransactionItemDisplay> ?? new List<TransactionItemDisplay>();
            var taxDisplays = dgvTaxes.DataSource as List<TransactionTaxDisplay> ?? new List<TransactionTaxDisplay>();

            // Convert display classes back to original models
            var items = itemDisplays.Select(item => item.ToTransactionItem()).ToList();
            var taxes = taxDisplays.Select(tax => tax.ToTransactionTax()).ToList();

            transaction.Items = items;
            transaction.Taxes = taxes;

            return transaction;
        }

        private string GetProductNameById(Guid productId)
        {
            var product = _availableProducts.FirstOrDefault(p => p.Id == productId.ToString());
            return product?.DisplayName ?? "Unknown Product";
        }

        private string GetTaxNameById(Guid taxId)
        {
            var tax = _availableTaxes.FirstOrDefault(t => t.Id == taxId.ToString());
            return tax?.DisplayName ?? "Unknown Tax";
        }

        private string GetTaxComponentsById(Guid taxId)
        {
            var tax = _availableTaxes.FirstOrDefault(t => t.Id == taxId.ToString());
            if (tax?.Components != null && tax.Components.Any())
            {
                return string.Join(", ", tax.Components.Select(c => c.DisplayName));
            }
            return "No Components";
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
