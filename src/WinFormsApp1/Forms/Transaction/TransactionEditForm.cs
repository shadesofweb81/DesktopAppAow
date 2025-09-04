using AccountingERP.WebApi.Models.Requests;
using WinFormsApp1.Documents;
using WinFormsApp1.Models;
using WinFormsApp1.Services;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Transaction
{
    public class TaxSelectionResult
    {
        public TaxListDto Tax { get; set; } = null!;
        public List<TaxComponentDto> SelectedComponents { get; set; } = new List<TaxComponentDto>();
        public string ComponentsDisplay { get; set; } = string.Empty;
    }

    public class TransactionItemDisplay
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
            ProductId = item.ProductId;
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
                ProductId = ProductId,
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

    public class TransactionTaxDisplay
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid TaxId { get; set; }
        public string TaxName { get; set; } = string.Empty;
        public string TaxComponents { get; set; } = string.Empty;
        public string ComponentRates { get; set; } = string.Empty;
        public decimal TotalComponentRate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string CalculationMethod { get; set; } = string.Empty;
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
        public List<TaxComponentDto> SelectedComponents { get; set; } = new List<TaxComponentDto>();

        public TransactionTaxDisplay(TransactionTax tax, string taxName, string taxComponents, List<TaxComponentDto>? selectedComponents = null)
        {
            Id = tax.Id;
            TransactionId = tax.TransactionId;
            TaxId = tax.TaxId;
            TaxName = taxName;
            TaxComponents = taxComponents;
            SelectedComponents = selectedComponents ?? new List<TaxComponentDto>();
            TotalComponentRate = SelectedComponents.Sum(c => c.Rate);
            ComponentRates = string.Join(", ", SelectedComponents.Select(c => $"{c.DisplayName}: {c.Rate:N2}%"));
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
                TaxId = TaxId,
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

    public partial class TransactionEditForm : BaseForm
    {
        private readonly TransactionService _transactionService;
        private readonly LocalStorageService _localStorageService;
        private readonly ProductService _productService;
        private readonly TaxService _taxService;
        private readonly LedgerService _ledgerService;
        private Models.Transaction? _transaction;
        private Models.TransactionByIdDto? _transactionDto;
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
        
        // Ledger Controls
        private TextBox txtPartyLedger = null!;
        private TextBox txtAccountLedger = null!;

        // Items Section
        private DataGridView dgvItems = null!;
        private Button btnAddItem = null!;
        private Button btnEditItem = null!;
        private Button btnDeleteItem = null!;
        private Button btnSelectProducts = null!;
        
        // Ledger Selection Buttons
        private Button btnSelectPartyLedger = null!;
        private Button btnSelectAccountLedger = null!;

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
        private Button btnPrint = null!;
        
        // Group Boxes
        private GroupBox itemsGroupBox = null!;
        private GroupBox taxGroupBox = null!;
        private GroupBox summaryGroupBox = null!;

        // Context Menus
        private ContextMenuStrip _itemsContextMenu = null!;
        private ContextMenuStrip _taxesContextMenu = null!;

        private List<Models.ProductListDto> _availableProducts = new List<Models.ProductListDto>();
        private List<Models.TaxListDto> _availableTaxes = new List<Models.TaxListDto>();
        private List<Models.LedgerModel> _availableLedgers = new List<Models.LedgerModel>();
        
        // Selected ledgers (since we're not using ComboBox anymore)
        private Models.LedgerModel? _selectedPartyLedger;
        private Models.LedgerModel? _selectedAccountLedger;

            private class TransactionTaxDisplay
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid TaxId { get; set; }
        public string TaxName { get; set; } = string.Empty;
        public string TaxComponents { get; set; } = string.Empty;
        public string ComponentRates { get; set; } = string.Empty;
        public decimal TotalComponentRate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string CalculationMethod { get; set; } = string.Empty;
        public bool IsApplied { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
        public int SerialNumber { get; set; }
        public List<Models.TaxComponentDto> SelectedComponents { get; set; } = new List<Models.TaxComponentDto>();

            public TransactionTaxDisplay(TransactionTax tax, string taxName, string taxComponents, List<Models.TaxComponentDto>? selectedComponents = null)
            {
                Id = tax.Id;
                TransactionId = tax.TransactionId;
                TaxId = tax.TaxId;
                TaxName = taxName;
                TaxComponents = taxComponents;
                SelectedComponents = selectedComponents ?? new List<Models.TaxComponentDto>();
                TotalComponentRate = SelectedComponents.Sum(c => c.Rate);
                ComponentRates = string.Join(", ", SelectedComponents.Select(c => $"{c.DisplayName}: {c.Rate:N2}%"));
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
                    TaxId = TaxId, // Convert Guid to string
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
            Models.TransactionByIdDto? transactionDto, Models.Company selectedCompany, FinancialYearModel selectedFinancialYear,
            ProductService? productService = null, TaxService? taxService = null, LedgerService? ledgerService = null)
        {
            _transactionService = transactionService;
            _localStorageService = localStorageService;
            _transactionDto = transactionDto;
            _transaction = transactionDto != null ? ConvertDtoToTransaction(transactionDto) : null;
            _selectedCompany = selectedCompany;
            _selectedFinancialYear = selectedFinancialYear;

            // Use provided services or create new ones (for now, create basic services)
            _productService = productService ?? new ProductService(new AuthService());
            _taxService = taxService ?? new TaxService(new AuthService());
            _ledgerService = ledgerService ?? new LedgerService(new AuthService());

            InitializeComponent();
            SetupForm();
            _ = LoadData();
        }

        private Models.Transaction ConvertDtoToTransaction(Models.TransactionByIdDto dto)
        {
            var transaction = new Models.Transaction
            {
                Id = Guid.TryParse(dto.Id, out var id) ? id : Guid.NewGuid(),
                TransactionNumber = dto.TransactionNumber,
                InvoiceNumber = dto.InvoiceNumber,
                TransactionDate = dto.TransactionDate,
                DueDate = dto.DueDate,
                Status = dto.Status,
                Notes = dto.Notes,
                ReferenceNumber = dto.ReferenceNumber,
                PartyName = dto.PartyName,
                SubTotal = dto.SubTotal,
                TaxAmount = dto.TaxAmount,
                Total = dto.Total,
                Discount = dto.Discount,
                Freight = dto.Freight,
                IsFreightIncluded = dto.IsFreightIncluded,
                RoundOff = dto.RoundOff
            };

            //// Parse the type string
            //if (Enum.TryParse<TransactionType>(dto.Type, out var transactionType))
            //{
            //    transaction.Type = transactionType;
            //}

            // Convert items
            transaction.Items = dto.Items.Select(itemDto => new TransactionItem
            {
                Id = Guid.TryParse(itemDto.Id, out var itemId) ? itemId : Guid.NewGuid(),
                ProductId = Guid.TryParse(itemDto.ProductId, out var productId) ? productId : Guid.Empty,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TaxRate = itemDto.TaxRate,
                TaxAmount = itemDto.TaxAmount,
                DiscountRate = itemDto.DiscountRate,
                DiscountAmount = itemDto.DiscountAmount,
                LineTotal = itemDto.LineTotal,
                CurrentQuantity = itemDto.CurrentQuantity,
                SerialNumber = itemDto.SerialNumber
            }).ToList();

            // Convert taxes
            transaction.Taxes = dto.Taxes.Select(taxDto => new TransactionTax
            {
                Id = Guid.TryParse(taxDto.Id, out var taxId) ? taxId : Guid.NewGuid(),
                TaxId = Guid.TryParse(taxDto.TaxId, out var taxTypeId) ? taxTypeId : Guid.Empty,
                TaxableAmount = taxDto.TaxableAmount,
                TaxAmount = taxDto.TaxAmount,
                CalculationMethod = taxDto.CalculationMethod ?? "",
                IsApplied = taxDto.IsApplied,
                AppliedDate = taxDto.AppliedDate,
                ReferenceNumber = taxDto.ReferenceNumber,
                Description = taxDto.Description,
                SerialNumber = taxDto.SerialNumber
            }).ToList();

            return transaction;
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
            
            txtPartyLedger = new TextBox();
            txtAccountLedger = new TextBox();
            
            btnSelectPartyLedger = new Button();
            btnSelectAccountLedger = new Button();

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
            btnPrint = new Button();
            
            itemsGroupBox = new GroupBox();
            taxGroupBox = new GroupBox();
            summaryGroupBox = new GroupBox();

            SuspendLayout();

            // Form properties
            Text = _transaction == null ? "Add Transaction" : "Edit Transaction";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            KeyPreview = true; // Ensure form can handle key events

            SetupLayout();

            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            var yPosition = 10;
            var leftMargin = 20;

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
            yPosition += spaceBetweenRows;

            // Fourth Row - Ledger Selection Section (Improved Layout)
            var ledgerSectionHeight = 80; // More space for ledger section
            
            // Create a group box for better organization
            var ledgerGroupBox = new GroupBox
            {
                Text = "Ledger Selection",
                Location = new Point(leftMargin, yPosition),
                Size = new Size(1140, ledgerSectionHeight),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            
            // Party Ledger Row
            var lblPartyLedger = new Label
            {
                Text = "Party Ledger:",
                Location = new Point(20, 25),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            txtPartyLedger.Location = new Point(20 + labelWidth + 5, 22);
            txtPartyLedger.Size = new Size(400, 25); // Much wider
            txtPartyLedger.Font = new Font("Segoe UI", 9F);
            txtPartyLedger.TabIndex = 100; // Set tab order
            
            // Party Ledger Button
            btnSelectPartyLedger.Location = new Point(20 + labelWidth + 410, 22);
            btnSelectPartyLedger.Size = new Size(80, 25);
            btnSelectPartyLedger.Text = "Select (F4)";
            btnSelectPartyLedger.Font = new Font("Segoe UI", 8F);
            btnSelectPartyLedger.UseVisualStyleBackColor = true;
            btnSelectPartyLedger.TabIndex = 101;
            btnSelectPartyLedger.Click += BtnSelectPartyLedger_Click;
            
            // Party Ledger Hint
            var lblPartyHint = new Label
            {
                Text = "Click or press Enter to search party ledgers (customers/suppliers)",
                Location = new Point(20 + labelWidth + 495, 25),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Account Ledger Row
            var lblAccountLedger = new Label
            {
                Text = "Account Ledger:",
                Location = new Point(20, 55),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            txtAccountLedger.Location = new Point(20 + labelWidth + 5, 52);
            txtAccountLedger.Size = new Size(400, 25); // Much wider
            txtAccountLedger.Font = new Font("Segoe UI", 9F);
            txtAccountLedger.TabIndex = 102; // Set tab order
            
            // Account Ledger Button
            btnSelectAccountLedger.Location = new Point(20 + labelWidth + 410, 52);
            btnSelectAccountLedger.Size = new Size(80, 25);
            btnSelectAccountLedger.Text = "Select (F5)";
            btnSelectAccountLedger.Font = new Font("Segoe UI", 8F);
            btnSelectAccountLedger.UseVisualStyleBackColor = true;
            btnSelectAccountLedger.TabIndex = 103;
            btnSelectAccountLedger.Click += BtnSelectAccountLedger_Click;
            
            // Account Ledger Hint
            var lblAccountHint = new Label
            {
                Text = "Click or press Enter to search account ledgers (income/expense/assets)",
                Location = new Point(20 + labelWidth + 495, 55),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Add all ledger controls to the group box
            ledgerGroupBox.Controls.AddRange(new Control[] { 
                lblPartyLedger, txtPartyLedger, btnSelectPartyLedger, lblPartyHint,
                lblAccountLedger, txtAccountLedger, btnSelectAccountLedger, lblAccountHint
            });
            
            // Add the group box to the form
            Controls.Add(ledgerGroupBox);
            
            yPosition += ledgerSectionHeight + 15;

            // Items Section
            itemsGroupBox.Text = "Transaction Items";
            itemsGroupBox.Location = new Point(leftMargin, yPosition);
            itemsGroupBox.Size = new Size(1140, 250);
            itemsGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            dgvItems.Location = new Point(10, 25);
            dgvItems.Size = new Size(1020, 150);
            dgvItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Add Item button - positioned prominently at the bottom of the items group box
            var lblAddItemHint = new Label
            {
                Text = "After selecting Party and Account Ledgers, click below to add items:",
                Location = new Point(10, 185),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.DarkBlue
            };
            
            btnAddItem.Location = new Point(10, 210);
            btnAddItem.Size = new Size(140, 35);
            btnAddItem.Text = "&Add Item (F2)";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Visible = true;
            btnAddItem.Enabled = true;
            btnAddItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddItem.BackColor = Color.LightGreen;
            btnAddItem.ForeColor = Color.DarkGreen;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Cursor = Cursors.Hand;
            btnAddItem.Click += BtnAddItem_Click;

            btnSelectProducts.Location = new Point(1040, 25);
            btnSelectProducts.Size = new Size(90, 30);
            btnSelectProducts.Text = "Select Products";
            btnSelectProducts.Click += BtnSelectProducts_Click;

            btnEditItem.Location = new Point(1040, 65);
            btnEditItem.Size = new Size(90, 30);
            btnEditItem.Text = "Edit Item";
            btnEditItem.Click += BtnEditItem_Click;

            btnDeleteItem.Location = new Point(1040, 105);
            btnDeleteItem.Size = new Size(90, 30);
            btnDeleteItem.Text = "Delete Item";
            btnDeleteItem.Click += BtnDeleteItem_Click;

            itemsGroupBox.Controls.AddRange(new Control[] { dgvItems, btnSelectProducts, btnEditItem, btnDeleteItem, lblAddItemHint, btnAddItem });
            yPosition += 260;

            // Tax Section
            taxGroupBox.Text = "Transaction Taxes";
            taxGroupBox.Location = new Point(leftMargin, yPosition);
            taxGroupBox.Size = new Size(1140, 180);
            taxGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

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

            // Add test add tax button prominently in the tax group box
            var testAddTaxBtn = new Button
            {
                Location = new Point(10, 145),
                Size = new Size(140, 35),
                Text = "Test Add Tax (F3)",
                UseVisualStyleBackColor = true,
                BackColor = Color.LightBlue,
                ForeColor = Color.DarkBlue,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            testAddTaxBtn.Click += (s, e) => {
                Console.WriteLine("Test Add Tax button clicked!");
                BtnAddTax_Click(s, e);
            };

            taxGroupBox.Controls.AddRange(new Control[] { dgvTaxes, btnSelectTaxes, btnAddTax, btnEditTax, btnDeleteTax, testAddTaxBtn });
            yPosition += 190;

            // Summary Section - Improved Layout
            summaryGroupBox.Text = "Transaction Summary";
            summaryGroupBox.Location = new Point(leftMargin, yPosition);
            summaryGroupBox.Size = new Size(1140, 140);
            summaryGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            summaryGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

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
            btnSave.Location = new Point(900, yPosition);
            btnSave.Size = new Size(80, 35);
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;

            // Add a download PDF button (without printing)
            var btnDownloadPdf = new Button
            {
                Location = new Point(1080 + 100, yPosition),
                Size = new Size(80, 35),
                Text = "Download",
                UseVisualStyleBackColor = true,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkGreen
            };
            btnDownloadPdf.Click += BtnDownloadPdf_Click;
            Controls.Add(btnDownloadPdf);

            btnCancel.Location = new Point(1080, yPosition);
            btnCancel.Size = new Size(80, 35);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            

            


            // Debug: Verify controls are properly set up before adding
            Console.WriteLine($"Adding controls to form:");
            Console.WriteLine($"  lblCompanyInfo: {lblCompanyInfo != null}");
            Console.WriteLine($"  itemsGroupBox: {itemsGroupBox != null}, Controls count: {itemsGroupBox?.Controls.Count}");
            Console.WriteLine($"  taxGroupBox: {taxGroupBox != null}");
            Console.WriteLine($"  summaryGroupBox: {summaryGroupBox != null}");
            Console.WriteLine($"  btnSave: {btnSave != null}");
            Console.WriteLine($"  btnCancel: {btnCancel != null}");
            
            if (itemsGroupBox != null)
            {
                Console.WriteLine($"  Items GroupBox children:");
                foreach (Control ctrl in itemsGroupBox.Controls)
                {
                    Console.WriteLine($"    - {ctrl.GetType().Name}: '{ctrl.Text}' at {ctrl.Location}");
                }
            }

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
            
            // Debug: Check if buttons are properly set up
            Console.WriteLine($"Button setup debug:");
            Console.WriteLine($"btnAddItem: Visible={btnAddItem.Visible}, Enabled={btnAddItem.Enabled}, Text='{btnAddItem.Text}'");
            Console.WriteLine($"btnAddItem.Parent: {btnAddItem.Parent?.GetType().Name}");
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

            // Ledger TextBoxes - Enhanced styling and behavior
            SetupLedgerTextBox(txtPartyLedger, "Click or press Enter to select party ledger...");
            SetupLedgerTextBox(txtAccountLedger, "Click or press Enter to select account ledger...");
            
            // Event handlers
            txtPartyLedger.Click += TxtPartyLedger_Click;
            txtPartyLedger.KeyDown += TxtPartyLedger_KeyDown;
            txtPartyLedger.Enter += TxtPartyLedger_Enter;
            txtPartyLedger.Leave += TxtPartyLedger_Leave;

            txtAccountLedger.Click += TxtAccountLedger_Click;
            txtAccountLedger.KeyDown += TxtAccountLedger_KeyDown;
            txtAccountLedger.Enter += TxtAccountLedger_Enter;
            txtAccountLedger.Leave += TxtAccountLedger_Leave;
        }

        private void SetupDataGridViews()
        {
            SetupItemsDataGridView();
            SetupTaxesDataGridView();
            SetupContextMenus();
        }

        private void SetupItemsDataGridView()
        {
            dgvItems.AutoGenerateColumns = false;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.MultiSelect = false;
            dgvItems.RowHeadersVisible = false;
            dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;
            
            // Add event handlers for keyboard navigation and editing
            dgvItems.KeyDown += DgvItems_KeyDown;
            dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;
            dgvItems.CellEndEdit += DgvItems_CellEndEdit;
            dgvItems.MouseClick += DgvItems_MouseClick; // Add mouse click handler for context menu

            // Serial Number Column (First Column)
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "S.No",
                DataPropertyName = "SerialNumber",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                ReadOnly = true
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Product",
                DataPropertyName = "ProductName",
                Width = 200,
                ReadOnly = true // Product selection via dialog only
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200,
                ReadOnly = false // Editable
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Qty",
                DataPropertyName = "Quantity",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = false // Editable
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Unit Price",
                DataPropertyName = "UnitPrice",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = false // Editable
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiscountRate",
                HeaderText = "Disc %",
                DataPropertyName = "DiscountRate",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = false // Editable
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiscountAmount",
                HeaderText = "Disc Amt",
                DataPropertyName = "DiscountAmount",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = true // Calculated field
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxRate",
                HeaderText = "Tax %",
                DataPropertyName = "TaxRate",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = false // Editable
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaxAmount",
                HeaderText = "Tax Amt",
                DataPropertyName = "TaxAmount",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = true // Calculated field
            });

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineTotal",
                HeaderText = "Line Total",
                DataPropertyName = "LineTotal",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = true // Calculated field
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
            
            // Add event handlers for tax grid interaction
            dgvTaxes.KeyDown += DgvTaxes_KeyDown;
            dgvTaxes.CellDoubleClick += DgvTaxes_CellDoubleClick;
            dgvTaxes.MouseClick += DgvTaxes_MouseClick; // Add mouse click handler for context menu

            // Serial Number Column (First Column)
            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "S.No",
                DataPropertyName = "SerialNumber",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                ReadOnly = true
            });

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
                HeaderText = "Components",
                DataPropertyName = "TaxComponents",
                Width = 150
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ComponentRates",
                HeaderText = "Component Rates",
                DataPropertyName = "ComponentRates",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalComponentRate",
                HeaderText = "Total Rate %",
                DataPropertyName = "TotalComponentRate",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
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

            dgvTaxes.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "CalculationMethod",
                HeaderText = "Calculation Method",
                DataPropertyName = "CalculationMethod",
                Width = 150,
                Items = { "ItemSubtotal", "AboveRowAmount", "Total" },
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            });

            dgvTaxes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsApplied",
                HeaderText = "Applied",
                DataPropertyName = "IsApplied",
                Width = 80
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
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

            // Add event handler for transaction date validation
            dtpTransactionDate.ValueChanged += DtpTransactionDate_ValueChanged;

            // Add event handler for taxes DataGridView
            dgvTaxes.CellValueChanged += DgvTaxes_CellValueChanged;
        }

        private void SetupContextMenus()
        {
            // Create context menu for items DataGridView
            var itemsContextMenu = new ContextMenuStrip();
            itemsContextMenu.Font = new Font("Segoe UI", 9F);
            
            var addItemMenuItem = new ToolStripMenuItem("Add New Item (F2)", null, (s, e) => BtnAddItem_Click(s, e));
            addItemMenuItem.Image = SystemIcons.Application.ToBitmap();
            itemsContextMenu.Items.Add(addItemMenuItem);
            
            itemsContextMenu.Items.Add(new ToolStripSeparator());
            
            var editItemMenuItem = new ToolStripMenuItem("Edit Item (Enter)", null, (s, e) => BtnEditItem_Click(s, e));
            editItemMenuItem.Image = SystemIcons.Information.ToBitmap();
            itemsContextMenu.Items.Add(editItemMenuItem);
            
            itemsContextMenu.Items.Add(new ToolStripSeparator());
            
            var deleteItemMenuItem = new ToolStripMenuItem("Delete Row (Delete)", null, (s, e) => BtnDeleteItem_Click(s, e));
            deleteItemMenuItem.Image = SystemIcons.Error.ToBitmap();
            deleteItemMenuItem.ForeColor = Color.Red;
            itemsContextMenu.Items.Add(deleteItemMenuItem);
            
            itemsContextMenu.Items.Add(new ToolStripSeparator());
            
            var copyItemMenuItem = new ToolStripMenuItem("Copy Item Details", null, (s, e) => CopySelectedItemDetails());
            copyItemMenuItem.Image = SystemIcons.Exclamation.ToBitmap();
            itemsContextMenu.Items.Add(copyItemMenuItem);

            // Create context menu for taxes DataGridView
            var taxesContextMenu = new ContextMenuStrip();
            taxesContextMenu.Font = new Font("Segoe UI", 9F);
            
            var addTaxMenuItem = new ToolStripMenuItem("Add New Tax (F3)", null, (s, e) => BtnAddTax_Click(s, e));
            addTaxMenuItem.Image = SystemIcons.Application.ToBitmap();
            taxesContextMenu.Items.Add(addTaxMenuItem);
            
            taxesContextMenu.Items.Add(new ToolStripSeparator());
            
            var editTaxMenuItem = new ToolStripMenuItem("Edit Tax Components (Enter)", null, (s, e) => EditSelectedTaxComponents());
            editTaxMenuItem.Image = SystemIcons.Information.ToBitmap();
            taxesContextMenu.Items.Add(editTaxMenuItem);
            
            taxesContextMenu.Items.Add(new ToolStripSeparator());
            
            var deleteTaxMenuItem = new ToolStripMenuItem("Delete Row (Delete)", null, (s, e) => BtnDeleteTax_Click(s, e));
            deleteTaxMenuItem.Image = SystemIcons.Error.ToBitmap();
            deleteTaxMenuItem.ForeColor = Color.Red;
            taxesContextMenu.Items.Add(deleteTaxMenuItem);
            
            taxesContextMenu.Items.Add(new ToolStripSeparator());
            
            var copyTaxMenuItem = new ToolStripMenuItem("Copy Tax Details", null, (s, e) => CopySelectedTaxDetails());
            copyTaxMenuItem.Image = SystemIcons.Exclamation.ToBitmap();
            taxesContextMenu.Items.Add(copyTaxMenuItem);

            // Store context menus as class fields for access in mouse click handlers
            _itemsContextMenu = itemsContextMenu;
            _taxesContextMenu = taxesContextMenu;
        }

        private void DtpTransactionDate_ValueChanged(object? sender, EventArgs e)
        {
            var transactionDate = dtpTransactionDate.Value.Date;
            var financialYearStart = _selectedFinancialYear.StartDate.Date;
            var financialYearEnd = _selectedFinancialYear.EndDate.Date;

            if (transactionDate < financialYearStart || transactionDate > financialYearEnd)
            {
                MessageBox.Show(
                    $"Warning: Selected date {transactionDate:dd-MMM-yyyy} is outside the current financial year.\n\n" +
                    $"Financial Year: {_selectedFinancialYear.YearLabel}\n" +
                    $"Valid Date Range: {financialYearStart:dd-MMM-yyyy} to {financialYearEnd:dd-MMM-yyyy}",
                    "Date Out of Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void DgvTaxes_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            // Handle changes in the taxes DataGridView
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dgvTaxes.Columns[e.ColumnIndex];
                if (column.Name == "CalculationMethod")
                {
                    // Recalculate totals when calculation method changes
                    CalculateTotals();
                }
            }
        }

        private void TxtPartyLedger_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("Party Ledger TextBox clicked - opening modal");
            ShowPartyLedgerSelectionDialog();
        }

        private void TxtPartyLedger_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space || e.KeyCode == Keys.F4)
            {
                Console.WriteLine($"Party Ledger TextBox key pressed: {e.KeyCode} - opening modal");
                ShowPartyLedgerSelectionDialog();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                // Tab to account ledger
                txtAccountLedger.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                // Arrow down to account ledger
                txtAccountLedger.Focus();
                e.Handled = true;
            }
        }

        private void TxtPartyLedger_Enter(object? sender, EventArgs e)
        {
            // Enhanced focus handling
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Visual feedback for focus
                textBox.BackColor = Color.LightYellow;
                
                if (!string.IsNullOrEmpty(textBox.Text) && _selectedPartyLedger != null)
                {
                    textBox.SelectAll();
                }
                
                Console.WriteLine("Party Ledger TextBox focused - ready for Enter key or click");
            }
        }

        private void TxtPartyLedger_Leave(object? sender, EventArgs e)
        {
            // Reset background when focus leaves
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = SystemColors.Window;
            }
        }

        private void TxtAccountLedger_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("Account Ledger TextBox clicked - opening modal");
            ShowAccountLedgerSelectionDialog();
        }

        private void TxtAccountLedger_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space || e.KeyCode == Keys.F5)
            {
                Console.WriteLine($"Account Ledger TextBox key pressed: {e.KeyCode} - opening modal");
                ShowAccountLedgerSelectionDialog();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                // Tab to Add Item button if both ledgers are selected
                if (_selectedPartyLedger != null && _selectedAccountLedger != null)
                {
                    btnAddItem.Focus();
                }
                else
                {
                    // Move to next control in tab order
                    SelectNextControl(txtAccountLedger, true, true, true, true);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                // Arrow up to party ledger
                txtPartyLedger.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && _selectedPartyLedger != null && _selectedAccountLedger != null)
            {
                // Arrow down to Add Item button if ready
                btnAddItem.Focus();
                e.Handled = true;
            }
        }

        private void TxtAccountLedger_Enter(object? sender, EventArgs e)
        {
            // Enhanced focus handling
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Visual feedback for focus
                textBox.BackColor = Color.LightYellow;
                
                if (!string.IsNullOrEmpty(textBox.Text) && _selectedAccountLedger != null)
                {
                    textBox.SelectAll();
                }
                
                Console.WriteLine("Account Ledger TextBox focused - ready for Enter key or click");
            }
        }

        private void TxtAccountLedger_Leave(object? sender, EventArgs e)
        {
            // Reset background when focus leaves
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = SystemColors.Window;
            }
        }

        private void AutoSelectAccountLedger()
        {
            if (_selectedPartyLedger == null)
                return;

            Console.WriteLine($"Auto-selecting account ledger for party ledger: {_selectedPartyLedger.DisplayName}");
            Console.WriteLine($"Party ledger category: {_selectedPartyLedger.Category}");
            Console.WriteLine($"Party ledger parent: {_selectedPartyLedger.Parent?.DisplayName ?? "None"}");

            // Check if the selected party ledger is a supplier or has a parent that is "Sundry Creditor"
            bool isSupplier = IsSupplierLedger(_selectedPartyLedger);
            
            // Check if the selected party ledger is a customer or has a parent that is "Sundry Debtor"
            bool isCustomer = IsCustomerLedger(_selectedPartyLedger);

            Console.WriteLine($"Is Supplier: {isSupplier}, Is Customer: {isCustomer}");

            if (isSupplier)
            {
                // For suppliers, select Purchase account
                Console.WriteLine("Auto-selecting Purchase account for supplier");
                SelectAccountLedgerByCategory("Purchases");
            }
            else if (isCustomer)
            {
                // For customers, select Sales account
                Console.WriteLine("Auto-selecting Sales account for customer");
                SelectAccountLedgerByCategory("Sales");
            }
            else
            {
                Console.WriteLine("No auto-selection - party ledger is neither supplier nor customer");
            }
        }

        private bool IsSupplierLedger(LedgerModel ledger)
        {
            // Check if the ledger itself is a supplier
            if (ledger.Category.Equals("Supplier", StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if the parent ledger is "Sundry Creditor"
            if (ledger.Parent != null && 
                ledger.Parent.Category.Equals("Sundry Creditor", StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if the parent name contains "Sundry Creditor"
            if (ledger.Parent != null && 
                ledger.Parent.Name.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase))
                return true;

            // If parent is not loaded, try to find it in the available ledgers
            if (ledger.ParentId.HasValue)
            {
                var parentLedger = _availableLedgers.FirstOrDefault(l => l.Id == ledger.ParentId.Value);
                if (parentLedger != null)
                {
                    if (parentLedger.Category.Equals("Sundry Creditor", StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (parentLedger.Name.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private bool IsCustomerLedger(LedgerModel ledger)
        {
            // Check if the ledger itself is a customer
            if (ledger.Category.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if the parent ledger is "Sundry Debtor"
            if (ledger.Parent != null && 
                ledger.Parent.Category.Equals("Sundry Debtor", StringComparison.OrdinalIgnoreCase))
                return true;

            // Check if the parent name contains "Sundry Debtor"
            if (ledger.Parent != null && 
                ledger.Parent.Name.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase))
                return true;

            // If parent is not loaded, try to find it in the available ledgers
            if (ledger.ParentId.HasValue)
            {
                var parentLedger = _availableLedgers.FirstOrDefault(l => l.Id == ledger.ParentId.Value);
                if (parentLedger != null)
                {
                    if (parentLedger.Category.Equals("Sundry Debtor", StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (parentLedger.Name.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private void SelectAccountLedgerByCategory(string category)
        {
            if (_availableLedgers == null || _availableLedgers.Count == 0)
                return;

            // Find the first ledger with the specified category
            var targetLedger = _availableLedgers.FirstOrDefault(l => 
                l.Name.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (targetLedger != null)
            {
                SetSelectedAccountLedger(targetLedger);
                Console.WriteLine($"Auto-selected {category} account: {targetLedger.DisplayName}");
            }
            else
            {
                Console.WriteLine($"No {category} account found in available ledgers");
            }
        }

        private void SetSelectedPartyLedger(LedgerModel ledger)
        {
            _selectedPartyLedger = ledger;
            txtPartyLedger.Text = $"✓ {ledger.DisplayName}";
            
            // Style for selected ledger
            txtPartyLedger.ForeColor = Color.DarkGreen;
            txtPartyLedger.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void SetSelectedAccountLedger(LedgerModel ledger)
        {
            _selectedAccountLedger = ledger;
            txtAccountLedger.Text = $"✓ {ledger.DisplayName}";
            
            // Style for selected ledger
            txtAccountLedger.ForeColor = Color.DarkGreen;
            txtAccountLedger.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void SetupLedgerTextBox(TextBox textBox, string placeholderText)
        {
            textBox.ReadOnly = true;
            textBox.BackColor = SystemColors.Window;
            textBox.Cursor = Cursors.Hand;
            textBox.PlaceholderText = placeholderText;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 9F);
            
            // Style for empty/placeholder state
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            }
        }

        private void UpdateAddItemButtonState()
        {
            // Enable Add Item button only when both Party Ledger and Account Ledger are selected
            bool canAddItems = _selectedPartyLedger != null && _selectedAccountLedger != null;
            
            btnAddItem.Enabled = canAddItems;
            
            if (canAddItems)
            {
                btnAddItem.BackColor = Color.LightGreen;
                btnAddItem.ForeColor = Color.DarkGreen;
                btnAddItem.Text = "&Add Item (F2)";
            }
            else
            {
                btnAddItem.BackColor = Color.LightGray;
                btnAddItem.ForeColor = Color.Gray;
                btnAddItem.Text = "Select Ledgers First";
            }
            
            Console.WriteLine($"Add Item button state updated - Enabled: {btnAddItem.Enabled}, Can Add Items: {canAddItems}");
        }

        private async Task LoadData()
        {
            Console.WriteLine("LoadData called - starting to load data...");
            
            // Load product, tax, and ledger lists first
            await LoadProductTaxAndLedgerLists();

            if (_transaction != null)
            {
                Console.WriteLine("Loading existing transaction data...");
                await LoadExistingTransactionData();
            }
            else
            {
                Console.WriteLine("Setting up new transaction defaults...");
                // New transaction defaults
                cmbTransactionType.SelectedIndex = 0;
                dtpTransactionDate.Value = DateTime.Now;
                dtpDueDate.Value = DateTime.Now.AddDays(30);
                txtTransactionNumber.Text = GenerateTransactionNumber();

                dgvItems.DataSource = new List<TransactionItemDisplay>();
                dgvTaxes.DataSource = new List<TransactionTaxDisplay>();
            }

            Console.WriteLine("LoadData completed - setting focus...");
            // Set focus to the first field (Transaction Type dropdown)
            cmbTransactionType.Focus();
            
            // Update Add Item button state after loading data
            UpdateAddItemButtonState();
            
            // Final debug: Check if Add Item button is visible after everything is loaded
            Console.WriteLine($"Final check - btnAddItem: Visible={btnAddItem.Visible}, Enabled={btnAddItem.Enabled}, Parent={btnAddItem.Parent?.GetType().Name}");
        }

        private async Task LoadExistingTransactionData()
        {
            if (_transaction == null || _transactionDto == null) return;

            try
            {
                // Use the existing DTO that was passed to the constructor
                if (Enum.TryParse<TransactionType>(_transactionDto.TransactionType, out var transactionType))
                {
                    cmbTransactionType.SelectedItem = transactionType;
                }
                
                txtTransactionNumber.Text = _transactionDto.TransactionNumber;
                txtInvoiceNumber.Text = _transactionDto.InvoiceNumber ?? "";
                dtpTransactionDate.Value = _transactionDto.TransactionDate;
                dtpDueDate.Value = _transactionDto.DueDate;
                cmbStatus.SelectedItem = _transactionDto.Status;
                txtReferenceNumber.Text = _transactionDto.ReferenceNumber ?? "";
                txtNotes.Text = _transactionDto.Notes ?? "";

                // Set ledger selections
                if (!string.IsNullOrEmpty(_transactionDto.PartyLedgerId) && Guid.TryParse(_transactionDto.PartyLedgerId, out var partyLedgerId))
                {
                    var partyLedger = _availableLedgers.FirstOrDefault(l => l.Id == partyLedgerId);
                    if (partyLedger != null)
                    {
                        SetSelectedPartyLedger(partyLedger);
                        // Auto-select account ledger based on party ledger type
                        AutoSelectAccountLedger();
                    }
                }
                
                if (!string.IsNullOrEmpty(_transactionDto.AccountLedgerId) && Guid.TryParse(_transactionDto.AccountLedgerId, out var accountLedgerId))
                {
                    var accountLedger = _availableLedgers.FirstOrDefault(l => l.Id == accountLedgerId);
                    if (accountLedger != null)
                    {
                        SetSelectedAccountLedger(accountLedger);
                    }
                }

                // Convert items to display format using DTO and sort by serial number
                var itemDisplays = _transactionDto.Items
                    .Select(itemDto => CreateTransactionItemDisplay(itemDto))
                    .OrderBy(item => item.SerialNumber)
                    .ToList();

                // Convert taxes to display format using DTO and sort by serial number
                Console.WriteLine($"Processing {_transactionDto.Taxes.Count()} taxes from DTO");
                var taxDisplays = _transactionDto.Taxes
                    .Select(tax => CreateTransactionTaxDisplay(tax))
                    .OrderBy(tax => tax.SerialNumber)
                    .ToList();
                Console.WriteLine($"Created {taxDisplays.Count} tax displays");

                dgvItems.DataSource = itemDisplays;
                dgvTaxes.DataSource = taxDisplays;

                txtDiscountPercent.Text = _transactionDto.Discount.ToString("N2");
                txtDiscountAmount.Text = "0.00"; // Calculated field
                txtFreight.Text = _transactionDto.Freight.ToString("N2");
                chkFreightIncluded.Checked = _transactionDto.IsFreightIncluded;
                txtRoundOff.Text = _transactionDto.RoundOff.ToString("N2");

                CalculateTotals();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transaction details: {ex.Message}");
                LoadFallbackTransactionData();
            }
        }

        private void LoadFallbackTransactionData()
        {
            if (_transaction == null) return;

            cmbTransactionType.SelectedItem = _transaction.Type;
            txtTransactionNumber.Text = _transaction.TransactionNumber;
            txtInvoiceNumber.Text = _transaction.InvoiceNumber ?? "";
            dtpTransactionDate.Value = _transaction.TransactionDate;
            dtpDueDate.Value = _transaction.DueDate;
            cmbStatus.SelectedItem = _transaction.Status;
            txtReferenceNumber.Text = _transaction.ReferenceNumber ?? "";
            txtNotes.Text = _transaction.Notes ?? "";
            
            // Note: Party ledger selection and auto-selection will be handled in LoadProductTaxAndLedgerLists

                         // Convert items to display format and sort by serial number
             var itemDisplays = _transaction.Items
                 .Select(item => 
                 {
                     var product = _availableProducts.FirstOrDefault(p => p.Id == item.ProductId.ToString());
                     var productName = product?.DisplayName ?? "Unknown Product";
                     return new TransactionItemDisplay(item, productName);
                 })
                 .OrderBy(item => item.SerialNumber)
                 .ToList();

             // Convert taxes to display format and sort by serial number
             var taxDisplays = _transaction.Taxes
                 .Select(tax => 
                 {
                                         var taxInfo = _availableTaxes.FirstOrDefault(t => t.Id == tax.TaxId.ToString());
                    var taxName = taxInfo?.DisplayName ?? "Unknown Tax";
                    var selectedComponents = taxInfo?.Components?.ToList() ?? new List<Models.TaxComponentDto>();
                    var taxComponents = selectedComponents.Any() 
                        ? string.Join(", ", selectedComponents.Select(c => c.DisplayName))
                        : "No Components";
                    return new TransactionTaxDisplay(tax, taxName, taxComponents, selectedComponents);
                 })
                 .OrderBy(tax => tax.SerialNumber)
                 .ToList();

            dgvItems.DataSource = itemDisplays;
            dgvTaxes.DataSource = taxDisplays;

            txtDiscountPercent.Text = _transaction.Discount.ToString("N2");
            txtDiscountAmount.Text = "0.00";
            txtFreight.Text = _transaction.Freight.ToString("N2");
            chkFreightIncluded.Checked = _transaction.IsFreightIncluded;
            txtRoundOff.Text = _transaction.RoundOff.ToString("N2");

            CalculateTotals();
        }

        private TransactionItemDisplay CreateTransactionItemDisplay(TransactionItemDto itemDto)
        {
            var productName = itemDto.ProductName ?? "Unknown Product";
            
            // Convert DTO to TransactionItem for the display wrapper
            var transactionItem = new TransactionItem
            {
                Id = Guid.TryParse(itemDto.Id, out var itemId) ? itemId : Guid.NewGuid(),
                TransactionId = Guid.Empty, // Will be set when saving
                ProductId = Guid.TryParse(itemDto.ProductId, out var productId) ? productId : Guid.Empty,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TaxRate = itemDto.TaxRate,
                TaxAmount = itemDto.TaxAmount,
                DiscountRate = itemDto.DiscountRate,
                DiscountAmount = itemDto.DiscountAmount,
                LineTotal = itemDto.LineTotal,
                CurrentQuantity = itemDto.CurrentQuantity,
                SerialNumber = itemDto.SerialNumber
            };

            return new TransactionItemDisplay(transactionItem, productName);
        }

                 private TransactionTaxDisplay CreateTransactionTaxDisplay(TransactionTaxDto taxDto)
         {
             // Get tax name from DTO first, fallback to available tax info
             string taxName = taxDto.TaxName ?? "Unknown Tax";
             
             Console.WriteLine($"Processing tax DTO - TaxId: {taxDto.TaxId}, TaxName: {taxDto.TaxName}");
             Console.WriteLine($"Components in DTO: {taxDto.Components?.Count ?? 0}");
             
             // Create selected components from DTO data with actual rates and amounts
             var selectedComponents = new List<Models.TaxComponentDto>();
             string taxComponents = "No Components";
             string componentRates = "";
             
             if (taxDto.Components != null && taxDto.Components.Any())
             {
                 // Build components from DTO data (actual transaction data)
                 var componentDisplays = new List<string>();
                 var componentRateDisplays = new List<string>();
                 
                 foreach (var dtoComponent in taxDto.Components)
                 {
                     // Calculate rate from amount and taxable amount
                     var calculatedRate = taxDto.TaxableAmount > 0 ? (dtoComponent.Amount / taxDto.TaxableAmount) * 100 : 0;
                     
                     // Create TaxComponentDto for display
                     var componentDto = new Models.TaxComponentDto
                     {
                         Id = dtoComponent.TaxComponentId,
                         Name = dtoComponent.ComponentName ?? "Unknown Component",
                         Rate = calculatedRate,
                         Description = dtoComponent.Description ?? "",
                         IsActive = true
                     };
                     
                     selectedComponents.Add(componentDto);
                     componentDisplays.Add(dtoComponent.ComponentName ?? "Unknown Component");
                     componentRateDisplays.Add($"{dtoComponent.ComponentName}: {calculatedRate:N2}%");
                     
                     Console.WriteLine($"  Component: {dtoComponent.ComponentName}, Amount: {dtoComponent.Amount}, Rate: {calculatedRate:N2}%");
                 }
                 
                 taxComponents = string.Join(", ", componentDisplays);
                 componentRates = string.Join(", ", componentRateDisplays);
             }
             
             // If no components in DTO, try to get from available tax info
             if (!selectedComponents.Any() && Guid.TryParse(taxDto.TaxId, out var taxId))
             {
                 var availableTax = _availableTaxes.FirstOrDefault(t => t.Id == taxId.ToString());
                 if (availableTax != null)
                 {
                     taxName = availableTax.DisplayName; // Use available tax name if found
                     
                     if (availableTax.Components != null && availableTax.Components.Any())
                     {
                             selectedComponents = availableTax.Components.ToList();
                         taxComponents = string.Join(", ", availableTax.Components.Select(c => c.Name));
                         componentRates = string.Join(", ", availableTax.Components.Select(c => $"{c.Name}: {c.Rate:N2}%"));
                     }
                 }
             }
             
             // Convert DTO to TransactionTax for the display wrapper
             var transactionTax = new TransactionTax
             {
                 Id = Guid.TryParse(taxDto.Id, out var itemId) ? itemId : Guid.NewGuid(),
                 TransactionId = Guid.Empty, // Will be set when saving
                 TaxId = Guid.TryParse(taxDto.TaxId, out var taxTypeId) ? taxTypeId : Guid.Empty,
                 TaxableAmount = taxDto.TaxableAmount,
                 TaxAmount = taxDto.TaxAmount,
                 CalculationMethod = taxDto.CalculationMethod ?? "",
                 IsApplied = taxDto.IsApplied,
                 AppliedDate = taxDto.AppliedDate,
                 ReferenceNumber = taxDto.ReferenceNumber,
                 Description = taxDto.Description,
                 SerialNumber = taxDto.SerialNumber
             };

             Console.WriteLine($"Created TransactionTaxDisplay - TaxName: {taxName}, Components: {taxComponents}, ComponentRates: {componentRates}");
             
             return new TransactionTaxDisplay(transactionTax, taxName, taxComponents, selectedComponents);
         }

        private async Task LoadProductTaxAndLedgerLists()
        {
            try
            {
                var companyId = Guid.Parse(_selectedCompany.Id);
                
                // Load products
                var products = await _productService.GetProductsByCompanyAsync(companyId);
                _availableProducts = products.Select(p => new ProductListDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Name = p.Name,
                    Category = p.Category,
                    Unit = p.Unit,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive
                }).ToList();
                Console.WriteLine($"Loaded {_availableProducts.Count} products for transaction");

                // Load taxes
                _availableTaxes = await _taxService.GetTaxListForTransactionAsync(companyId);
                Console.WriteLine($"Loaded {_availableTaxes.Count} taxes for transaction");
                
                // Debug: List the loaded taxes
                if (_availableTaxes.Any())
                {
                    Console.WriteLine("Loaded taxes:");
                    foreach (var tax in _availableTaxes.Take(5))
                    {
                        Console.WriteLine($"  - {tax.DisplayName} (ID: {tax.Id}, Rate: {tax.DefaultRate}%)");
                        if (tax.Components?.Any() == true)
                        {
                            Console.WriteLine($"    Components: {string.Join(", ", tax.Components.Select(c => c.DisplayName))}");
                        }
                    }
                    if (_availableTaxes.Count > 5)
                    {
                        Console.WriteLine($"  ... and {_availableTaxes.Count - 5} more taxes");
                    }
                }
                else
                {
                    Console.WriteLine("WARNING: No taxes were loaded!");
                }

                // Load ledgers
                _availableLedgers = await _ledgerService.GetAllLedgersAsync(companyId);
                Console.WriteLine($"Loaded {_availableLedgers.Count} ledgers for transaction");

                // Debug: Log some ledger details to verify structure
                if (_availableLedgers.Any())
                {
                    var sampleLedger = _availableLedgers.First();
                    Console.WriteLine($"Sample ledger: {sampleLedger.DisplayName}");
                    Console.WriteLine($"  Category: {sampleLedger.Category}");
                    Console.WriteLine($"  Parent: {sampleLedger.Parent?.DisplayName ?? "None"}");
                    Console.WriteLine($"  ParentId: {sampleLedger.ParentId}");
                }

                // Note: Ledgers are now selected via modal dialog, not populated in dropdowns
                // TextBoxes will display selected ledger names
                
                // Auto-select account ledger if party ledger is already selected
                if (_selectedPartyLedger != null)
                {
                    AutoSelectAccountLedger();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data lists: {ex.Message}");
                MessageBox.Show($"Error loading data lists: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Enter on party ledger textbox - open ledger selection
            if (ActiveControl == txtPartyLedger)
            {
                Console.WriteLine("Enter pressed on Party Ledger TextBox - opening modal");
                ShowPartyLedgerSelectionDialog();
                return true;
            }

            // Enter on account ledger textbox - open ledger selection
            if (ActiveControl == txtAccountLedger)
            {
                Console.WriteLine("Enter pressed on Account Ledger TextBox - opening modal");
                ShowAccountLedgerSelectionDialog();
                return true;
            }

            // Enter on Add Item button - open item selection dialog
            if (ActiveControl == btnAddItem)
            {
                Console.WriteLine("Enter pressed on Add Item button - opening item selection dialog");
                ShowItemSelectionDialog();
                return true;
            }

            // Enter on items grid
            if (ActiveControl == dgvItems)
            {
                if (dgvItems.CurrentCell != null && !dgvItems.IsCurrentCellInEditMode)
                {
                    // If not in edit mode, start editing current cell
                    if (!dgvItems.Columns[dgvItems.CurrentCell.ColumnIndex].ReadOnly)
                    {
                        dgvItems.BeginEdit(true);
                        return true;
                    }
                }
                else if (dgvItems.IsCurrentCellInEditMode)
                {
                    // If in edit mode, end editing and move to next field
                    dgvItems.EndEdit();
                    return true;
                }
                else if (dgvItems.SelectedRows.Count > 0)
                {
                    // If no current cell but rows selected, start editing first editable cell
                    var selectedRow = dgvItems.SelectedRows[0];
                    for (int i = 0; i < dgvItems.Columns.Count; i++)
                    {
                        if (!dgvItems.Columns[i].ReadOnly)
                        {
                            dgvItems.CurrentCell = selectedRow.Cells[i];
                            dgvItems.BeginEdit(true);
                            return true;
                        }
                    }
                }
                return true; // Handle Enter key for grid
            }

            return false; // Let BaseForm handle navigation
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Console.WriteLine($"ProcessCmdKey called with key: {keyData}, ActiveControl: {ActiveControl?.GetType().Name}");
            
            switch (keyData)
            {
                case Keys.F2:
                    Console.WriteLine("F2 key pressed - calling ShowItemSelectionDialog");
                    ShowItemSelectionDialog();
                    return true;
                case Keys.F3:
                    Console.WriteLine("F3 key pressed - calling ShowTaxSelectionDialog");
                    ShowTaxSelectionDialog();
                    return true;
                case Keys.F4:
                    Console.WriteLine("F4 key pressed - calling ShowPartyLedgerSelectionDialog");
                    ShowPartyLedgerSelectionDialog();
                    return true;
                case Keys.F5:
                    Console.WriteLine("F5 key pressed - calling ShowAccountLedgerSelectionDialog");
                    ShowAccountLedgerSelectionDialog();
                    return true;
                case Keys.F10:
                    Console.WriteLine("F10 key pressed - calling BtnSave_Click");
                    BtnSave_Click(null, EventArgs.Empty);
                    return true;
                case Keys.Delete:
                    if (ActiveControl == dgvItems && dgvItems.SelectedRows.Count > 0)
                    {
                        Console.WriteLine("Delete key pressed on items grid");
                        BtnDeleteItem_Click(null, EventArgs.Empty);
                        return true;
                    }
                    if (ActiveControl == dgvTaxes && dgvTaxes.SelectedRows.Count > 0)
                    {
                        Console.WriteLine("Delete key pressed on taxes grid");
                        BtnDeleteTax_Click(null, EventArgs.Empty);
                        return true;
                    }
                    break;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Console.WriteLine($"OnKeyDown called with key: {e.KeyCode}");
            
            if (e.KeyCode == Keys.F2)
            {
                Console.WriteLine("F2 detected in OnKeyDown - calling ShowItemSelectionDialog");
                ShowItemSelectionDialog();
                e.Handled = true;
                return;
            }
            
            base.OnKeyDown(e);
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
• F2 - Add/Select Items
• F3 - Add/Select Taxes with Components  
• F4 - Select Party Ledger (Customer/Supplier)
• F5 - Select Account Ledger (Income/Expense)
• F10 - Save Transaction
• Delete - Remove selected item/tax
• Enter - Edit selected item/tax in grid

Item Grid Navigation:
• F2 - Add new item from product list
• Enter - Edit selected item in grid
• Delete - Remove selected item
• Double-click - Edit item in grid
• Tab/Arrow keys - Navigate between cells
• Right-click - Context menu with options (Add, Edit, Delete, Copy)

Tax Grid Navigation:
• F3 - Add new tax with component selection
• Enter - Edit tax components for selected tax
• Delete - Remove selected tax
• Double-click - Edit tax components
• Right-click - Context menu with options (Add, Edit, Delete, Copy)

Debug Info:
• Button Visible: " + btnAddItem.Visible + @"
• Button Enabled: " + btnAddItem.Enabled + @"
• Available Products: " + _availableProducts.Count + @"

Field Order:
1. Transaction Type (dropdown)
2. Transaction Number
3. Invoice Number
4. Transaction Date
5. Due Date
6. Status (dropdown)
7. Reference Number
8. Notes
9. Party Ledger (TextBox - click or Enter to open selection modal)
10. Account Ledger (TextBox - click or Enter to open selection modal)
11. Transaction Items (DataGrid) - S.No, Product, Description, Qty, Unit Price, Disc %, Disc Amt, Tax %, Tax Amt, Line Total
12. Transaction Taxes (DataGrid) - S.No, Tax Name, Tax Components, Description, Taxable Amount, Tax Amount, Calculation Method
13. Subtotal (read-only)
14. Discount Percent
15. Discount Amount
16. Freight
17. Freight Included (checkbox)
18. Tax Amount (read-only)
19. Round Off
20. Total (read-only)
21. Save Button
22. Cancel Button

Tips:
• Use Tab/Backspace for fast field navigation
• Ctrl+Enter to save from any field
• F2 for quick item selection and addition
• F3 for quick tax selection
• F4 for party ledger selection (customers/suppliers)
• F5 for account ledger selection (income/expense)
• Direct editing in item grid for fast data entry
• Right-click on grid rows for context menu options
• C key to copy selected item/tax details
• Escape to cancel and close
• All fields are highlighted when focused

Ledger Selection:
• Click on Party/Account Ledger TextBoxes to open selection modal
• Press Enter/Space on ledger TextBoxes to open selection dialog
• Use F4/F5 or click F4/F5 buttons for quick access
• Ledger selection dialog supports:
  - Search by name, code, or category
  - Filter by ledger type (Party/Account/Assets/etc.)
  - Show/hide group ledgers
  - Keyboard navigation and selection
• Selected ledger names are displayed in the TextBoxes
• TextBoxes are read-only - selection only via modal";

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

                // Calculate tax amount from all taxes
                var totalTaxAmount = 0m;
                foreach (var tax in taxesSource)
                {
                    // For loaded transactions, use the stored tax amount
                    // For new calculations, recalculate based on components
                    if (tax.TaxAmount > 0)
                    {
                        totalTaxAmount += tax.TaxAmount;
                    }
                    else if (tax.SelectedComponents.Any())
                    {
                        // Recalculate based on components
                        var componentBasedAmount = tax.TaxableAmount * (tax.TotalComponentRate / 100);
                        totalTaxAmount += componentBasedAmount;
                        
                        // Update the tax amount in the display object
                        tax.TaxAmount = componentBasedAmount;
                    }
                }
                txtTaxAmount.Text = totalTaxAmount.ToString("N2");

                // Add freight if not included
                decimal freight = 0;
                decimal.TryParse(txtFreight.Text, out freight);

                var beforeRounding = discountedSubtotal + totalTaxAmount;
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
            Console.WriteLine("BtnAddItem_Click called");
            ShowItemSelectionDialog();
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

        private void ShowTaxSelectionDialog()
        {
            try
            {
                Console.WriteLine($"ShowTaxSelectionDialog called. Available taxes: {_availableTaxes?.Count ?? 0}");
                
                if (_availableTaxes == null || _availableTaxes.Count == 0)
                {
                    MessageBox.Show("No taxes available. Please add taxes first.", "No Taxes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Console.WriteLine("Creating TaxSelectionDialog...");
                var dialog = new TaxSelectionDialog(_availableTaxes);
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                Console.WriteLine("Opening tax selection dialog...");
                var dialogResult = dialog.ShowDialog(this);
                Console.WriteLine($"Dialog result: {dialogResult}");
                
                if (dialogResult == DialogResult.OK)
                {
                    Console.WriteLine($"Dialog OK - Selected taxes count: {dialog.SelectedTaxes?.Count ?? 0}");
                    if (dialog.SelectedTaxes != null && dialog.SelectedTaxes.Any())
                    {
                        Console.WriteLine($"Adding {dialog.SelectedTaxes.Count} taxes to grid");
                        foreach (var selectedTax in dialog.SelectedTaxes)
                        {
                            Console.WriteLine($"Adding tax: {selectedTax.Tax.DisplayName}");
                            AddTaxToGrid(selectedTax);
                        }
                        Console.WriteLine("Finished adding taxes to grid");
                    }
                    else
                    {
                        Console.WriteLine("No taxes were selected");
                    }
                }
                else
                {
                    Console.WriteLine("Dialog was cancelled");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowTaxSelectionDialog: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error opening tax selection: {ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddTax_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("BtnAddTax_Click called");
            Console.WriteLine($"Available taxes count: {_availableTaxes?.Count ?? 0}");
            
            // Debug: List some available taxes
            if (_availableTaxes != null && _availableTaxes.Any())
            {
                Console.WriteLine("First few available taxes:");
                foreach (var tax in _availableTaxes.Take(3))
                {
                    Console.WriteLine($"  - {tax.DisplayName} ({tax.DefaultRate}%)");
                }
            }
            else
            {
                Console.WriteLine("No taxes available!");
                MessageBox.Show("No taxes available. Please check if taxes are loaded properly.", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            ShowTaxSelectionDialog();
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
                        var createRequest = CreateTransactionDtoFromForm();
                        var createdDto = await _transactionService.CreateTransactionAsync(createRequest);
                        result = createdDto != null ? ConvertDtoToTransaction(createdDto) : null;
                    }
                    else
                    {
                        var updateRequest = CreateUpdateRequestFromForm();
                        var success = await _transactionService.UpdateTransactionAsync(_transaction.Id, updateRequest);
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



        private Models.TransactionByIdDto CreateTransactionDtoFromForm()
        {
            var companyId = Guid.Parse(_selectedCompany.Id);
            var financialYearId = _selectedFinancialYear.Id;
            
            var partyLedgerId = _selectedPartyLedger?.Id.ToString() ?? string.Empty;
            var accountLedgerId = _selectedAccountLedger?.Id.ToString() ?? string.Empty;

            var dueDate = dtpDueDate.Value;

            decimal.TryParse(txtSubTotal.Text, out var subtotal);
            decimal.TryParse(txtDiscountPercent.Text, out var discountPercent);
            decimal.TryParse(txtFreight.Text, out var freight);
            decimal.TryParse(txtTaxAmount.Text, out var taxAmount);
            decimal.TryParse(txtRoundOff.Text, out var roundOff);
            decimal.TryParse(txtTotal.Text, out var total);

            var itemDisplays = dgvItems.DataSource as List<TransactionItemDisplay> ?? new List<TransactionItemDisplay>();
            var taxDisplays = dgvTaxes.DataSource as List<TransactionTaxDisplay> ?? new List<TransactionTaxDisplay>();

            var transactionTypeString = cmbTransactionType.SelectedItem?.ToString()
                ?? TransactionType.SaleInvoice.ToString();

            var items = itemDisplays
                .OrderBy(i => i.SerialNumber)
                .Select(i => new TransactionItemDto
                {
                    Id = (i.Id == Guid.Empty ? Guid.NewGuid() : i.Id).ToString(),
                    ProductId = i.ProductId.ToString(),
                    ProductName = i.ProductName,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    DiscountRate = i.DiscountRate,
                    DiscountAmount = i.DiscountAmount,
                    TaxRate = i.TaxRate,
                    TaxAmount = i.TaxAmount,
                    LineTotal = i.LineTotal,
                    CurrentQuantity = i.CurrentQuantity,
                    SerialNumber = i.SerialNumber,
                    Variants = new List<ProductVariantDto>()
                })
                .ToList();

            var taxes = taxDisplays
                .OrderBy(t => t.SerialNumber)
                .Select(t => new TransactionTaxDto
                {
                    Id = (t.Id == Guid.Empty ? Guid.NewGuid() : t.Id).ToString(),
                    TaxId = t.TaxId.ToString(),
                    TaxName = t.TaxName,
                    TaxableAmount = t.TaxableAmount,
                    TaxAmount = t.TaxAmount,
                    CalculationMethod = string.IsNullOrWhiteSpace(t.CalculationMethod) ? "ItemSubtotal" : t.CalculationMethod,
                    IsApplied = t.IsApplied,
                    AppliedDate = t.AppliedDate,
                    ReferenceNumber = t.ReferenceNumber,
                    Description = t.Description,
                    SerialNumber = t.SerialNumber,
                    Components = t.SelectedComponents.Select(c => new TransactionTaxComponentDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaxComponentId = c.Id,
                        ComponentName = c.DisplayName,

                        Amount = t.TaxableAmount * (c.Rate / 100),
                        Description = c.Description,
                        IsApplied = true,
                        AppliedDate = DateTime.Now,
                        ReferenceNumber = null
                    }).ToList()
                })
                .ToList();

            return new Models.TransactionByIdDto
            {
                Id = Guid.NewGuid().ToString(), // Add ID for new transactions
                CompanyId = companyId.ToString(), // Add CompanyId
                FinancialYearId = financialYearId.ToString(), // Add FinancialYearId
                TransactionNumber = txtTransactionNumber.Text.Trim(),
                InvoiceNumber = string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ? null : txtInvoiceNumber.Text.Trim(),
                TransactionDate = dtpTransactionDate.Value,
                DueDate = dueDate,
                TransactionType = transactionTypeString,
                Status = cmbStatus.SelectedItem?.ToString() ?? "Draft",
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? string.Empty : txtNotes.Text.Trim(),
                ReferenceNumber = string.IsNullOrWhiteSpace(txtReferenceNumber.Text) ? null : txtReferenceNumber.Text.Trim(),
                PartyLedgerId = partyLedgerId,
                AccountLedgerId = accountLedgerId,
                SubTotal = subtotal,
                Discount = discountPercent,
                Freight = freight,
                IsFreightIncluded = chkFreightIncluded.Checked,
                TaxAmount = taxAmount,
                RoundOff = roundOff,
                Total = total,
                Items = items,
                Taxes = taxes,
                LedgerEntries = new List<TransactionLedgerEntryDto>()
            };
        }

        private UpdateTransactionRequest CreateUpdateRequestFromForm()
        {
            var companyId = Guid.Parse(_selectedCompany.Id);

            var partyLedgerId = _selectedPartyLedger?.Id ?? Guid.Empty;
            var accountLedgerId = _selectedAccountLedger?.Id ?? Guid.Empty;

            if (partyLedgerId == Guid.Empty && !string.IsNullOrWhiteSpace(_transactionDto?.PartyLedgerId) && Guid.TryParse(_transactionDto.PartyLedgerId, out var dtoParty))
            {
                partyLedgerId = dtoParty;
            }
            if (accountLedgerId == Guid.Empty && !string.IsNullOrWhiteSpace(_transactionDto?.AccountLedgerId) && Guid.TryParse(_transactionDto.AccountLedgerId, out var dtoAccount))
            {
                accountLedgerId = dtoAccount;
            }

            var paymentTermDays = Math.Max(0, (dtpDueDate.Value.Date - dtpTransactionDate.Value.Date).Days);

            decimal.TryParse(txtDiscountPercent.Text, out var discountPercent);
            decimal.TryParse(txtFreight.Text, out var freight);
            decimal.TryParse(txtRoundOff.Text, out var roundOff);

            var itemDisplays = dgvItems.DataSource as List<TransactionItemDisplay> ?? new List<TransactionItemDisplay>();
            var taxDisplays = dgvTaxes.DataSource as List<TransactionTaxDisplay> ?? new List<TransactionTaxDisplay>();

            var dtoItemsById = (_transactionDto?.Items ?? new List<TransactionItemDto>())
                .Select(d => new { D = d, Id = Guid.TryParse(d.Id, out var gid) ? gid : Guid.Empty })
                .Where(x => x.Id != Guid.Empty)
                .ToDictionary(x => x.Id, x => x.D);

            var items = itemDisplays
                .OrderBy(i => i.SerialNumber)
                .Select(i =>
                {
                    var req = new UpdateTransactionItemRequest
                    {
                        Id = i.Id == Guid.Empty ? (Guid?)null : i.Id,
                        ProductId = i.ProductId,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        DiscountRate = i.DiscountRate,
                        DiscountAmount = i.DiscountAmount,
                        CurrentQuantity = i.CurrentQuantity,
                        SerialNumber = i.SerialNumber,
                        IsDeleted = false,
                        Variants = new List<UpdateTransactionItemVariantRequest>()
                    };

                    if (dtoItemsById.TryGetValue(i.Id, out var dtoItem) && dtoItem.Variants != null && dtoItem.Variants.Any())
                    {
                        req.Variants = dtoItem.Variants
                            .OrderBy(v => v.SerialNumber)
                            .Select(v => new UpdateTransactionItemVariantRequest
                            {
                                Id = Guid.TryParse(v.Id, out var vid) ? (Guid?)vid : null,
                                ProductVariantId = Guid.TryParse(v.ProductVariantId, out var pvid) ? pvid : Guid.Empty,
                                VariantCode = v.VariantCode ?? string.Empty,
                                VariantName = v.VariantName ?? string.Empty,
                                Quantity = v.Quantity,
                                UnitPrice = v.UnitPrice,
                                SellingPrice = v.SellingPrice,
                                CurrentQuantity = v.CurrentQuantity,
                                SerialNumber = v.SerialNumber,
                                Description = v.Description,
                                IsDeleted = false
                            })
                            .ToList();
                    }

                    return req;
                })
                .ToList();

            var dtoTaxesById = (_transactionDto?.Taxes ?? new List<TransactionTaxDto>())
                .Select(d => new { D = d, Id = Guid.TryParse(d.Id, out var gid) ? gid : Guid.Empty })
                .Where(x => x.Id != Guid.Empty)
                .ToDictionary(x => x.Id, x => x.D);

            var taxes = taxDisplays
                .OrderBy(t => t.SerialNumber)
                .Select(t =>
                {
                    var req = new UpdateTransactionTaxRequest
                    {
                        Id = t.Id == Guid.Empty ? (Guid?)null : t.Id,
                        TaxId = t.TaxId,
                        TaxableAmount = t.TaxableAmount,
                        Amount = t.TaxAmount,
                        CalculationMethod = string.IsNullOrWhiteSpace(t.CalculationMethod) ? "ItemSubtotal" : t.CalculationMethod,
                        IsApplied = t.IsApplied,
                        AppliedDate = t.AppliedDate,
                        ReferenceNumber = t.ReferenceNumber,
                        Description = t.Description,
                        IsDeleted = false,
                        SerialNumber = t.SerialNumber,
                        Components = null
                    };

                    if (dtoTaxesById.TryGetValue(t.Id, out var dtoTax) && dtoTax.Components != null && dtoTax.Components.Any())
                    {
                        // Use selected components from the tax display
                    var selectedComponents = t.SelectedComponents;
                    req.Components = selectedComponents.Select(c => new UpdateTransactionTaxComponentRequest
                    {
                        Id = null, // New component
                        TaxComponentId = Guid.TryParse(c.Id, out var tcid) ? tcid : Guid.Empty,
                        Name = c.DisplayName,
                        Amount = t.TaxableAmount * (c.Rate / 100),
                        Description = c.Description,
                        IsApplied = true,
                        AppliedDate = DateTime.Now,
                        ReferenceNumber = null,
                        PayableLedgerId = null,
                        IsDeleted = false
                    }).ToList();
                    }

                    return req;
                })
                .ToList();

            var transactionTypeString = cmbTransactionType.SelectedItem?.ToString()
                ?? _transactionDto?.TransactionType
                ?? TransactionType.SaleInvoice.ToString();

            return new UpdateTransactionRequest
            {
                CompanyId = companyId,
                PartyLedgerId = partyLedgerId,
                AccountLedgerId = accountLedgerId,
                InvoiceNumber = string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ? null : txtInvoiceNumber.Text.Trim(),
                TransactionType = transactionTypeString,
                TransactionDate = dtpTransactionDate.Value,
                PaymentTermDays = paymentTermDays,
                Status = cmbStatus.SelectedItem?.ToString(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? string.Empty : txtNotes.Text.Trim(),
                Discount = discountPercent,
                Freight = freight,
                IsFreightIncluded = chkFreightIncluded.Checked,
                RoundOff = roundOff,
                Items = items,
                Taxes = taxes
            };
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

            // Validate transaction date is within financial year range
            var transactionDate = dtpTransactionDate.Value.Date;
            var financialYearStart = _selectedFinancialYear.StartDate.Date;
            var financialYearEnd = _selectedFinancialYear.EndDate.Date;

            if (transactionDate < financialYearStart || transactionDate > financialYearEnd)
            {
                MessageBox.Show(
                    $"Transaction date must be between {financialYearStart:dd-MMM-yyyy} and {financialYearEnd:dd-MMM-yyyy}.\n\n" +
                    $"Selected Financial Year: {_selectedFinancialYear.YearLabel}",
                    "Invalid Transaction Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                dtpTransactionDate.Focus();
                return false;
            }

            return true;
        }

        private Models.Transaction CreateTransactionFromForm()
        {
            var transaction = _transaction ?? new Models.Transaction();

            var transactionTypeString = cmbTransactionType.SelectedItem?.ToString() ?? string.Empty;
            if (Enum.TryParse<TransactionType>(transactionTypeString, out var transactionType))
            {
                transaction.Type = transactionType;
            }
            else
            {
                transaction.Type = TransactionType.SaleInvoice; // Default fallback
            }
            transaction.TransactionNumber = txtTransactionNumber.Text.Trim();
            transaction.InvoiceNumber = string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) ? null : txtInvoiceNumber.Text.Trim();
            transaction.TransactionDate = dtpTransactionDate.Value;
            transaction.DueDate = dtpDueDate.Value;
            transaction.Status = cmbStatus.SelectedItem?.ToString() ?? "Draft";
            transaction.ReferenceNumber = string.IsNullOrWhiteSpace(txtReferenceNumber.Text) ? null : txtReferenceNumber.Text.Trim();
            transaction.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            transaction.CompanyId = Guid.Parse(_selectedCompany.Id);
            transaction.FinancialYearId = _selectedFinancialYear.Id;

            // Set party name from selected ledger
            if (_selectedPartyLedger != null)
            {
                transaction.PartyName = _selectedPartyLedger.Name;
            }

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

        private decimal GetTaxRateById(Guid taxId)
        {
            var tax = _availableTaxes.FirstOrDefault(t => t.Id == taxId.ToString());
            return tax?.DefaultRate ?? 0;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }





        private async void BtnDownloadPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Download PDF button clicked");
                
                // Show print options dialog to get format and copy type preferences
                var printDialog = new PrintOptionsDialog();
                var result = printDialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    // Get the selected options
                    var copyType = printDialog.SelectedCopyType;
                    var numberOfCopies = printDialog.NumberOfCopies;
                    var invoiceFormat = printDialog.SelectedInvoiceFormat;

                    Console.WriteLine($"Download options - Copy Type: {copyType}, Copies: {numberOfCopies}, Format: {invoiceFormat}");
                    
                    // Generate and download PDFs based on options (without printing)
                    await GenerateAndDownloadPdfs(copyType, numberOfCopies, invoiceFormat);
                }
                else
                {
                    Console.WriteLine("Download PDF dialog was cancelled");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in download PDF: {ex.Message}");
                MessageBox.Show($"Error opening download dialog: {ex.Message}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task GenerateAndDownloadPdfs(CopyType copyType, int numberOfCopies, InvoiceFormat invoiceFormat)
        {
            try
            {
                Console.WriteLine($"GenerateAndDownloadPdfs called with:");
                Console.WriteLine($"  Copy Type: {copyType}");
                Console.WriteLine($"  Number of Copies: {numberOfCopies}");
                Console.WriteLine($"  Invoice Format: {invoiceFormat}");

                var pdfGenerator = new WinFormsApp1.Documents.ExportPdf.InvoicePdfGenerator();
                var copyTypes = GetCopyTypesToPrint(copyType);

                Console.WriteLine($"Copy types to generate: {string.Join(", ", copyTypes)}");
                Console.WriteLine($"Total PDFs to generate: {copyTypes.Count * numberOfCopies}");

                var successCount = 0;
                var savedFiles = new List<string>();

                foreach (var copy in copyTypes)
                {
                    for (int i = 0; i < numberOfCopies; i++)
                    {
                        try
                        {
                            Console.WriteLine($"Generating PDF for copy: {copy} (copy #{i + 1})");

                            byte[] pdfBytes;
                            
                            if (copy == "All")
                            {
                                // Generate combined PDF with all three copies
                                Console.WriteLine("Generating combined PDF with Original, Duplicate, and Triplicate");
                                pdfBytes = await GenerateCombinedPdf(copyType, invoiceFormat);
                            }
                            else
                            {
                                // Generate individual PDF
                                var invoiceModel = CreateInvoiceModel(copy, invoiceFormat);
                                Console.WriteLine($"Invoice model created with format: {invoiceModel.InvoiceFormat}");
                                // Generate PDF using the new InvoicePdfGenerator
                                var tempFilePath = Path.GetTempFileName() + ".pdf";
                                pdfGenerator.SaveInvoicePdf(invoiceModel, tempFilePath, GetCopyTypeFromString(copy), invoiceFormat);
                                pdfBytes = await File.ReadAllBytesAsync(tempFilePath);
                                File.Delete(tempFilePath); // Clean up temp file
                            }
                            
                            Console.WriteLine($"PDF generated successfully. Size: {pdfBytes.Length} bytes");

                            // Ask user where to save the PDF
                            var savedFilePath = await SavePdfWithDialog(pdfBytes, copy, i + 1, invoiceFormat);
                            if (!string.IsNullOrEmpty(savedFilePath))
                            {
                                savedFiles.Add(savedFilePath);
                                successCount++;
                                Console.WriteLine($"PDF saved to: {savedFilePath}");
                            }
                            else
                            {
                                Console.WriteLine($"User cancelled saving PDF for copy {copy} #{i + 1}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing copy {copy} #{i + 1}: {ex.Message}");
                            MessageBox.Show($"Error generating PDF for {copy} copy #{i + 1}: {ex.Message}",
                                "PDF Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                if (successCount > 0)
                {
                    var message = $"Successfully generated and downloaded {successCount} PDF(s)!\n\n" +
                        $"Copy Types: {string.Join(", ", copyTypes)}\n" +
                        $"Format: {GetInvoiceFormatDescription(invoiceFormat)}\n" +
                        $"Copies per type: {numberOfCopies}\n\n" +
                        $"Saved files:\n{string.Join("\n", savedFiles)}";

                    MessageBox.Show(message, "PDF Download Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No PDFs were generated. Please check the console for errors.",
                        "PDF Generation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GenerateAndDownloadPdfs: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error generating PDFs: {ex.Message}\n\nStack trace:\n{ex.StackTrace}",
                    "PDF Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> GetCopyTypesToPrint(CopyType copyType)
        {
            return copyType switch
            {
                CopyType.All => new List<string> { "All" }, // Special case for combined PDF
                CopyType.OriginalDuplicate => new List<string> { "Original", "Duplicate" },
                CopyType.Original => new List<string> { "Original" },
                CopyType.Duplicate => new List<string> { "Duplicate" },
                CopyType.Triplicate => new List<string> { "Triplicate" },
                _ => new List<string> { "Original" }
            };
        }

        private InvoiceModel CreateInvoiceModel(string copyType, InvoiceFormat invoiceFormat)
        {
            try
            {
                Console.WriteLine($"Creating invoice model for copy: {copyType}, format: {invoiceFormat}");
                
                // Validate required data before creating invoice
                if (string.IsNullOrEmpty(txtTransactionNumber.Text))
                {
                    throw new InvalidOperationException("Transaction number is required");
                }
                
                if (_selectedPartyLedger == null)
                {
                    throw new InvalidOperationException("Party ledger must be selected");
                }
                
                var items = dgvItems.DataSource as List<TransactionItemDisplay> ?? new List<TransactionItemDisplay>();
                if (!items.Any())
                {
                    throw new InvalidOperationException("At least one item must be added to the transaction");
                }
                
                Console.WriteLine($"Creating invoice with {items.Count} items");
                
                var invoiceModel = new InvoiceModel
                {
                    InvoiceNumber = txtTransactionNumber.Text,
                    InvoiceDate = dtpTransactionDate.Value,
                    CustomerName = _selectedPartyLedger?.Name ?? "Unknown Customer",
                    CustomerAddress = "Customer Address", // You can add address field to your form
                    CustomerGSTIN = "Customer GSTIN", // You can add GSTIN field to your form
                    CustomerMobile = "Customer Mobile", // You can add mobile field to your form
                    CompanyName = _selectedCompany.DisplayName,
                    CompanyGSTIN = _selectedCompany.TaxId ?? "Company GSTIN",
                    CompanyPhone = "Company Phone", // You can add phone field to company
                    CompanyEmail = "Company Email", // You can add email field to company
                    PlaceOfSupply = "Place of Supply", // You can add this field
                    CopyType = copyType,
                    InvoiceFormat = GetInvoiceFormatString(invoiceFormat),
                    TransactionType = cmbTransactionType.SelectedItem?.ToString() ?? "Sale Invoice",
                    ReferenceNumber = txtReferenceNumber.Text,
                    Notes = txtNotes.Text,
                    SubTotal = decimal.Parse(txtSubTotal.Text),
                    DiscountAmount = decimal.Parse(txtDiscountAmount.Text),
                    TaxAmount = decimal.Parse(txtTaxAmount.Text),
                    TotalAmount = decimal.Parse(txtTotal.Text),
                    RoundOff = decimal.Parse(txtRoundOff.Text),
                    NetPayable = decimal.Parse(txtTotal.Text),
                    AmountInWords = ConvertToWords(decimal.Parse(txtTotal.Text))
                };

                // Add items
                foreach (var item in items)
                {
                    invoiceModel.Items.Add(new InvoiceItemModel
                    {
                        SerialNumber = item.SerialNumber,
                        ProductName = item.ProductName,
                        HSNCode = "HSN Code", // You can add HSN code field to products
                        Quantity = item.Quantity,
                        Unit = "Pcs", // You can add unit field to products
                        UnitPrice = item.UnitPrice,
                        DiscountPercentage = item.DiscountRate,
                        DiscountAmount = item.DiscountAmount,
                        TaxRate = item.TaxRate,
                        TaxAmount = item.TaxAmount,
                        LineTotal = item.LineTotal
                    });
                }

                Console.WriteLine($"Invoice model created successfully with {invoiceModel.Items.Count} items");
                return invoiceModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating invoice model: {ex.Message}");
                throw new InvalidOperationException($"Failed to create invoice model: {ex.Message}", ex);
            }
        }

        private async Task<byte[]> GenerateCombinedPdf(CopyType copyType, InvoiceFormat invoiceFormat)
        {
            try
            {
                Console.WriteLine($"Generating combined PDF for copy type: {copyType}");
                
                if (copyType != CopyType.All)
                {
                    throw new InvalidOperationException("GenerateCombinedPdf should only be called for CopyType.All");
                }

                var pdfGenerator = new WinFormsApp1.Documents.ExportPdf.InvoicePdfGenerator();
                
                // Create a special invoice model for combined PDF
                var combinedInvoiceModel = CreateInvoiceModel("Combined", invoiceFormat);
                
                // Generate the combined PDF with all three copies using the new method
                var tempFilePath = Path.GetTempFileName() + ".pdf";
                pdfGenerator.SaveInvoicePdfWithMultipleCopies(combinedInvoiceModel, tempFilePath, CopyType.All, 3, invoiceFormat);
                var pdfBytes = await File.ReadAllBytesAsync(tempFilePath);
                File.Delete(tempFilePath); // Clean up temp file
                
                Console.WriteLine($"Combined PDF generated successfully. Size: {pdfBytes.Length} bytes");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating combined PDF: {ex.Message}");
                throw new InvalidOperationException($"Failed to generate combined PDF: {ex.Message}", ex);
            }
        }

        private CopyType GetCopyTypeFromString(string copyTypeString)
        {
            return copyTypeString switch
            {
                "Original" => CopyType.Original,
                "Duplicate" => CopyType.Duplicate,
                "Triplicate" => CopyType.Triplicate,
                "OriginalDuplicate" => CopyType.OriginalDuplicate,
                "All" => CopyType.All,
                _ => CopyType.Original
            };
        }

        private string GetInvoiceFormatString(InvoiceFormat format)
        {
            return format switch
            {
                InvoiceFormat.StandardA4 => "Standard A4",
                InvoiceFormat.A5HalfPage => "A5 Half Page",
                InvoiceFormat.Slip3Inch => "Slip 3 inch",
                InvoiceFormat.Slip4Inch => "Slip 4 inch",
                _ => "Standard A4"
            };
        }

        private void CleanupTempFiles(List<string> tempFiles)
        {
            try
            {
                foreach (var file in tempFiles)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        Console.WriteLine($"Cleaned up temp file: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning up temp files: {ex.Message}");
            }
        }

        private async Task<string> SavePdfWithDialog(byte[] pdfBytes, string copyType, int copyNumber, InvoiceFormat invoiceFormat)
        {
            try
            {
                // Create a default filename based on transaction details
                string defaultFileName;
                string dialogTitle;
                
                if (copyType == "All")
                {
                    defaultFileName = $"Invoice_{txtTransactionNumber.Text}_Combined_AllCopies_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    dialogTitle = $"Save Combined PDF (Original + Duplicate + Triplicate)";
                }
                else
                {
                    defaultFileName = $"Invoice_{txtTransactionNumber.Text}_{copyType}_{copyNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    dialogTitle = $"Save {copyType} PDF - Copy #{copyNumber}";
                }
                
                // Create save file dialog
                using var saveDialog = new SaveFileDialog
                {
                    Title = dialogTitle,
                    Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                    FileName = defaultFileName,
                    DefaultExt = "pdf",
                    AddExtension = true,
                    OverwritePrompt = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                // Show the save dialog
                var result = saveDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                {
                    var filePath = saveDialog.FileName;
                    
                    // Ensure the file has .pdf extension
                    if (!filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        filePath += ".pdf";
                    }
                    
                    // Save the PDF bytes to the selected location
                    await File.WriteAllBytesAsync(filePath, pdfBytes);
                    
                    Console.WriteLine($"PDF saved successfully to: {filePath}");
                    return filePath;
                }
                else
                {
                    Console.WriteLine("User cancelled PDF save dialog");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PDF: {ex.Message}");
                MessageBox.Show($"Error saving PDF: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private string GetInvoiceFormatDescription(InvoiceFormat format)
        {
            return format switch
            {
                InvoiceFormat.StandardA4 => "Standard A4 (210 x 297 mm)",
                InvoiceFormat.A5HalfPage => "A5 Half Page (148 x 210 mm)",
                InvoiceFormat.Slip3Inch => "Slip 3 inch (76 x 210 mm)",
                InvoiceFormat.Slip4Inch => "Slip 4 inch (101 x 210 mm)",
                _ => "Unknown Format"
            };
        }



        private string ConvertToWords(decimal amount)
        {
            // Simple number to words conversion (you can enhance this)
            var wholePart = (int)Math.Floor(amount);
            var decimalPart = (int)Math.Round((amount - wholePart) * 100);

            var words = NumberToWords(wholePart) + " Rupees";
            if (decimalPart > 0)
            {
                words += " and " + NumberToWords(decimalPart) + " Paise";
            }

            return words + " Only";
        }

        private string NumberToWords(int number)
        {
            if (number == 0) return "Zero";

            var units = new[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            var teens = new[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tens = new[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 10) return units[number];
            if (number < 20) return teens[number - 10];
            if (number < 100) return tens[number / 10] + (number % 10 > 0 ? " " + units[number % 10] : "");
            if (number < 1000) return units[number / 100] + " Hundred" + (number % 100 > 0 ? " " + NumberToWords(number % 100) : "");
            if (number < 100000) return NumberToWords(number / 1000) + " Thousand" + (number % 1000 > 0 ? " " + NumberToWords(number % 1000) : "");
            if (number < 10000000) return NumberToWords(number / 100000) + " Lakh" + (number % 100000 > 0 ? " " + NumberToWords(number % 100000) : "");

            return NumberToWords(number / 10000000) + " Crore" + (number % 10000000 > 0 ? " " + NumberToWords(number % 10000000) : "");
        }

        #region Item Management and Grid Events

        private void ShowItemSelectionDialog()
        {
            try
            {
                Console.WriteLine($"ShowItemSelectionDialog called. Available products: {_availableProducts.Count}");
                
                if (_availableProducts.Count == 0)
                {
                    MessageBox.Show("No products available. Please add products first.", "No Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialog = new ItemSelectionDialog(_availableProducts);
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                Console.WriteLine("Opening item selection dialog...");
                
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedProducts.Any())
                {
                    Console.WriteLine($"Selected {dialog.SelectedProducts.Count} products");
                    foreach (var selectedProduct in dialog.SelectedProducts)
                    {
                        AddItemToGrid(selectedProduct);
                    }
                }
                else
                {
                    Console.WriteLine("Dialog cancelled or no products selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowItemSelectionDialog: {ex.Message}");
                MessageBox.Show($"Error opening item selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddItemToGrid(ProductListDto product)
        {
            var items = dgvItems.DataSource as List<TransactionItemDisplay> ?? new List<TransactionItemDisplay>();
            
            var newSerialNumber = items.Any() ? items.Max(i => i.SerialNumber) + 1 : 1;
            
            var newItem = new TransactionItemDisplay(new TransactionItem
            {
                Id = Guid.Empty, // Don't assign ID for new items - let API generate it
                TransactionId = _transaction?.Id ?? Guid.Empty,
                ProductId = Guid.Parse(product.Id),
                Description = product.Name,
                Quantity = 1,
                UnitPrice = product.SellingPrice,
                TaxRate = 0,
                TaxAmount = 0,
                DiscountRate = 0,
                DiscountAmount = 0,
                LineTotal = product.SellingPrice,
                CurrentQuantity = product.StockQuantity,
                SerialNumber = newSerialNumber
            }, product.DisplayName);

            items.Add(newItem);
            
            // Refresh the grid
            dgvItems.DataSource = null;
            dgvItems.DataSource = items.OrderBy(i => i.SerialNumber).ToList();
            
            // Select the new item and start editing description
            if (dgvItems.Rows.Count > 0)
            {
                var newRowIndex = dgvItems.Rows.Count - 1;
                dgvItems.Rows[newRowIndex].Selected = true;
                // Focus on description column (index 2) and start editing immediately
                dgvItems.CurrentCell = dgvItems.Rows[newRowIndex].Cells[2]; // Description column
                // Begin edit mode for keyboard-only workflow
                if (!dgvItems.IsCurrentCellInEditMode)
                {
                    dgvItems.BeginEdit(true);
                }
            }
            
            CalculateTotals();
        }

        private void AddTaxToGrid(TaxSelectionResult taxResult)
        {
            Console.WriteLine($"AddTaxToGrid called for tax: {taxResult.Tax.DisplayName}");
            
            var taxes = dgvTaxes.DataSource as List<TransactionTaxDisplay> ?? new List<TransactionTaxDisplay>();
            Console.WriteLine($"Current taxes in grid: {taxes.Count}");
            
            var newSerialNumber = taxes.Any() ? taxes.Max(t => t.SerialNumber) + 1 : 1;
            Console.WriteLine($"New tax serial number: {newSerialNumber}");
            
            // Calculate taxable amount based on current subtotal
            decimal.TryParse(txtSubTotal.Text, out var subtotal);
            var taxableAmount = subtotal; // Default to subtotal, can be modified based on calculation method
            
            // Calculate tax amount based on selected components
            var totalComponentRate = taxResult.SelectedComponents.Sum(c => c.Rate);
            var componentRates = string.Join(", ", taxResult.SelectedComponents.Select(c => $"{c.DisplayName}: {c.Rate:N2}%"));
            
            Console.WriteLine($"Taxable amount: {taxableAmount}");
            Console.WriteLine($"Selected components: {taxResult.ComponentsDisplay}");
            Console.WriteLine($"Component rates: {componentRates}");
            Console.WriteLine($"Total component rate: {totalComponentRate}%");
            
            var taxAmount = taxableAmount * (totalComponentRate / 100);
            Console.WriteLine($"Calculated tax amount: {taxAmount}");
            
            var newTax = new TransactionTaxDisplay(new TransactionTax
            {
                Id = Guid.Empty, // Don't assign ID for new taxes - let API generate it
                TransactionId = _transaction?.Id ?? Guid.Empty,
                TaxId = Guid.Parse(taxResult.Tax.Id),
                TaxableAmount = taxableAmount,
                TaxAmount = taxAmount,
                CalculationMethod = "ItemSubtotal", // Default calculation method
                IsApplied = true,
                AppliedDate = DateTime.Now,
                ReferenceNumber = null,
                Description = taxResult.Tax.Description,
                SerialNumber = newSerialNumber
            }, taxResult.Tax.DisplayName, taxResult.ComponentsDisplay, taxResult.SelectedComponents);

            taxes.Add(newTax);
            Console.WriteLine($"Added tax to list. New count: {taxes.Count}");
            
            // Refresh the grid
            dgvTaxes.DataSource = null;
            dgvTaxes.DataSource = taxes.OrderBy(t => t.SerialNumber).ToList();
            Console.WriteLine($"Refreshed grid. Grid rows count: {dgvTaxes.Rows.Count}");
            
            // Select the new tax
            if (dgvTaxes.Rows.Count > 0)
            {
                dgvTaxes.Rows[dgvTaxes.Rows.Count - 1].Selected = true;
                dgvTaxes.CurrentCell = dgvTaxes.Rows[dgvTaxes.Rows.Count - 1].Cells[1]; // Focus on tax name
                Console.WriteLine("Selected new tax row");
            }
            
            CalculateTotals();
            Console.WriteLine("AddTaxToGrid completed");
        }

        private void DgvItems_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                ShowItemSelectionDialog();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && dgvItems.SelectedRows.Count > 0)
            {
                BtnDeleteItem_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (dgvItems.CurrentCell != null && !dgvItems.IsCurrentCellInEditMode && !dgvItems.Columns[dgvItems.CurrentCell.ColumnIndex].ReadOnly)
                {
                    // Start editing current editable cell
                    dgvItems.BeginEdit(true);
                    e.Handled = true;
                }
                else if (dgvItems.IsCurrentCellInEditMode)
                {
                    // End editing current cell (will trigger CellEndEdit and auto-advance)
                    dgvItems.EndEdit();
                    e.Handled = true;
                }
                else if (dgvItems.SelectedRows.Count > 0)
                {
                    // Start editing the first editable cell in selected row
                    var selectedRow = dgvItems.SelectedRows[0];
                    for (int i = 0; i < dgvItems.Columns.Count; i++)
                    {
                        if (!dgvItems.Columns[i].ReadOnly)
                        {
                            dgvItems.CurrentCell = selectedRow.Cells[i];
                            dgvItems.BeginEdit(true);
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }
        }

        private void DgvItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dgvItems.Columns[e.ColumnIndex];
                if (!column.ReadOnly)
                {
                    dgvItems.BeginEdit(true);
                }
            }
        }

        private void DgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var item = dgvItems.Rows[e.RowIndex].DataBoundItem as TransactionItemDisplay;
                if (item != null)
                {
                    // Recalculate line totals when quantity, unit price, or discount changes
                    var columnName = dgvItems.Columns[e.ColumnIndex].Name;
                    if (columnName == "Quantity" || columnName == "UnitPrice" || columnName == "DiscountRate" || columnName == "TaxRate")
                    {
                        RecalculateItemTotals(item);

                        // Refresh the grid to show updated calculated fields
                        dgvItems.RefreshEdit();
                        dgvItems.Refresh();

                        CalculateTotals();
                    }
                }

                // Auto-advance to next editable field for keyboard-only workflow
                MoveToNextEditableField(e.RowIndex, e.ColumnIndex);
            }
        }

        private void MoveToNextEditableField(int rowIndex, int currentColumnIndex)
        {
            // Define the editable columns in order: Description(2), Quantity(3), UnitPrice(4), DiscountRate(5), TaxRate(7)
            int[] editableColumnIndexes = { 2, 3, 4, 5, 7 };

            // Find current position in editable columns
            int currentEditableIndex = Array.IndexOf(editableColumnIndexes, currentColumnIndex);

            if (currentEditableIndex >= 0 && currentEditableIndex < editableColumnIndexes.Length - 1)
            {
                // Move to next editable column
                int nextColumnIndex = editableColumnIndexes[currentEditableIndex + 1];
                dgvItems.CurrentCell = dgvItems.Rows[rowIndex].Cells[nextColumnIndex];
                // Start editing the next field
                if (!dgvItems.IsCurrentCellInEditMode)
                {
                    dgvItems.BeginEdit(true);
                }
            }
            else if (currentEditableIndex == editableColumnIndexes.Length - 1)
            {
                // We've reached the last editable field, move focus to Add Item button
                Console.WriteLine("Reached last editable field, moving focus to Add Item button");
                btnAddItem.Focus();
            }
        }

        private void RecalculateItemTotals(TransactionItemDisplay item)
        {
            var subtotal = item.Quantity * item.UnitPrice;
            
            // Calculate discount
            item.DiscountAmount = subtotal * (item.DiscountRate / 100);
            var afterDiscount = subtotal - item.DiscountAmount;
            
            // Calculate tax
            item.TaxAmount = afterDiscount * (item.TaxRate / 100);
            
            // Calculate line total
            item.LineTotal = afterDiscount + item.TaxAmount;
        }

        private void DgvTaxes_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvTaxes.SelectedRows.Count > 0)
            {
                EditSelectedTaxComponents();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && dgvTaxes.SelectedRows.Count > 0)
            {
                BtnDeleteTax_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvTaxes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedTaxComponents();
            }
        }

        private void EditSelectedTaxComponents()
        {
            if (dgvTaxes.SelectedRows.Count == 0) return;

            var selectedTaxDisplay = dgvTaxes.SelectedRows[0].DataBoundItem as TransactionTaxDisplay;
            if (selectedTaxDisplay == null) return;

            Console.WriteLine($"Editing tax components for: {selectedTaxDisplay.TaxName}");

            // Find the tax in available taxes to get its components
            var availableTax = _availableTaxes.FirstOrDefault(t => t.Id == selectedTaxDisplay.TaxId.ToString());
            if (availableTax == null)
            {
                MessageBox.Show("Tax information not found. Cannot edit components.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (availableTax.Components?.Any() != true)
            {
                MessageBox.Show("This tax has no components to edit.", "No Components", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a new tax selection dialog with the current tax pre-selected
            var dialog = new TaxSelectionDialog(new List<Models.TaxListDto> { availableTax });
            dialog.Text = $"Edit Tax Components - {availableTax.DisplayName}";
            dialog.StartPosition = FormStartPosition.CenterParent;

            Console.WriteLine("Opening tax component edit dialog...");

            if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedTaxes.Any())
            {
                var updatedTaxResult = dialog.SelectedTaxes.First();
                Console.WriteLine($"Updated components: {updatedTaxResult.ComponentsDisplay}");

                // Update the existing tax with new component selection
                selectedTaxDisplay.TaxComponents = updatedTaxResult.ComponentsDisplay;

                // Recalculate tax amount based on selected components
                var totalComponentRate = updatedTaxResult.SelectedComponents.Sum(c => c.Rate);
                selectedTaxDisplay.TaxAmount = selectedTaxDisplay.TaxableAmount * (totalComponentRate / 100);

                // Refresh the grid
                dgvTaxes.Refresh();
                CalculateTotals();

                Console.WriteLine("Tax components updated successfully");
            }
            else
            {
                Console.WriteLine("Tax component edit cancelled");
            }
        }

        #endregion

        #region Ledger Selection Methods

        private void BtnSelectPartyLedger_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("BtnSelectPartyLedger_Click called");
            ShowPartyLedgerSelectionDialog();
        }

        private void BtnSelectAccountLedger_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("BtnSelectAccountLedger_Click called");
            ShowAccountLedgerSelectionDialog();
        }

        private void ShowPartyLedgerSelectionDialog()
        {
            try
            {
                Console.WriteLine($"ShowPartyLedgerSelectionDialog called. Available ledgers: {_availableLedgers?.Count ?? 0}");
                
                if (_availableLedgers == null || _availableLedgers.Count == 0)
                {
                    MessageBox.Show("No ledgers available. Please add ledgers first.", "No Ledgers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Console.WriteLine("Creating Party Ledger Selection Dialog...");
                var dialog = new LedgerSelectionDialog(
                    _availableLedgers, 
                    "Select Party Ledger - Click or Enter to Select", 
                    "Party Ledgers (Customers/Suppliers)",
                    "Party Ledgers" // Default filter to Party Ledgers
                );
                
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                Console.WriteLine("Opening party ledger selection dialog...");
                var dialogResult = dialog.ShowDialog(this);
                Console.WriteLine($"Dialog result: {dialogResult}");
                
                if (dialogResult == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    Console.WriteLine($"Selected party ledger: {dialog.SelectedLedger.DisplayName}");
                    
                    // Set the selected ledger
                    SetSelectedPartyLedger(dialog.SelectedLedger);
                    
                    // Trigger auto-selection of account ledger
                    AutoSelectAccountLedger();
                    
                    // Update button states
                    UpdateAddItemButtonState();
                    
                    // Focus on account ledger for next selection
                    txtAccountLedger.Focus();
                }
                else
                {
                    Console.WriteLine("Party ledger selection was cancelled or no ledger selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowPartyLedgerSelectionDialog: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error opening party ledger selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAccountLedgerSelectionDialog()
        {
            try
            {
                Console.WriteLine($"ShowAccountLedgerSelectionDialog called. Available ledgers: {_availableLedgers?.Count ?? 0}");
                
                if (_availableLedgers == null || _availableLedgers.Count == 0)
                {
                    MessageBox.Show("No ledgers available. Please add ledgers first.", "No Ledgers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Console.WriteLine("Creating Account Ledger Selection Dialog...");
                var dialog = new LedgerSelectionDialog(
                    _availableLedgers, 
                    "Select Account Ledger - Click or Enter to Select", 
                    "Account Ledgers (Income/Expense/Asset/Liability)",
                    "Account Ledgers" // Default filter to Account Ledgers
                );
                
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                Console.WriteLine("Opening account ledger selection dialog...");
                var dialogResult = dialog.ShowDialog(this);
                Console.WriteLine($"Dialog result: {dialogResult}");
                
                if (dialogResult == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    Console.WriteLine($"Selected account ledger: {dialog.SelectedLedger.DisplayName}");
                    
                    // Set the selected ledger
                    SetSelectedAccountLedger(dialog.SelectedLedger);
                    
                    // Update button states
                    UpdateAddItemButtonState();
                    
                    // Focus on Add Item button if both ledgers are now selected
                    if (_selectedPartyLedger != null && _selectedAccountLedger != null)
                    {
                        btnAddItem.Focus();
                    }
                }
                else
                {
                    Console.WriteLine("Account ledger selection was cancelled or no ledger selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAccountLedgerSelectionDialog: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error opening account ledger selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Context Menu Event Handlers

        private void DgvItems_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Get the row under the mouse cursor
                var hitTest = dgvItems.HitTest(e.X, e.Y);
                if (hitTest.Type == DataGridViewHitTestType.Cell)
                {
                    // Select the row under the cursor
                    dgvItems.ClearSelection();
                    dgvItems.Rows[hitTest.RowIndex].Selected = true;
                    
                    // Update context menu items based on selection
                    UpdateItemsContextMenu();
                    
                    // Show context menu at cursor position
                    _itemsContextMenu.Show(dgvItems, e.Location);
                }
            }
        }

        private void DgvTaxes_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Get the row under the mouse cursor
                var hitTest = dgvTaxes.HitTest(e.X, e.Y);
                if (hitTest.Type == DataGridViewHitTestType.Cell)
                {
                    // Select the row under the cursor
                    dgvTaxes.ClearSelection();
                    dgvTaxes.Rows[hitTest.RowIndex].Selected = true;
                    
                    // Update context menu items based on selection
                    UpdateTaxesContextMenu();
                    
                    // Show context menu at cursor position
                    _taxesContextMenu.Show(dgvTaxes, e.Location);
                }
            }
        }

        private void UpdateItemsContextMenu()
        {
            // Enable/disable menu items based on current selection
            var hasSelection = dgvItems.SelectedRows.Count > 0;
            
            foreach (ToolStripItem item in _itemsContextMenu.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Enabled = hasSelection;
                }
            }
        }

        private void UpdateTaxesContextMenu()
        {
            // Enable/disable menu items based on current selection
            var hasSelection = dgvTaxes.SelectedRows.Count > 0;
            
            foreach (ToolStripItem item in _taxesContextMenu.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Enabled = hasSelection;
                }
            }
        }

        private void CopySelectedItemDetails()
        {
            if (dgvItems.SelectedRows.Count == 0) return;

            var selectedItem = dgvItems.SelectedRows[0].DataBoundItem as TransactionItemDisplay;
            if (selectedItem != null)
            {
                var details = $"Product: {selectedItem.ProductName}\n" +
                             $"Description: {selectedItem.Description}\n" +
                             $"Quantity: {selectedItem.Quantity}\n" +
                             $"Unit Price: {selectedItem.UnitPrice:C}\n" +
                             $"Tax Rate: {selectedItem.TaxRate:N2}%\n" +
                             $"Discount Rate: {selectedItem.DiscountRate:N2}%\n" +
                             $"Line Total: {selectedItem.LineTotal:C}";

                try
                {
                    Clipboard.SetText(details);
                    // Show a brief success message
                    var toolTip = new ToolTip();
                    toolTip.Show("Item details copied to clipboard!", dgvItems, 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying to clipboard: {ex.Message}");
                }
            }
        }

        private void CopySelectedTaxDetails()
        {
            if (dgvTaxes.SelectedRows.Count == 0) return;

            var selectedTax = dgvTaxes.SelectedRows[0].DataBoundItem as TransactionTaxDisplay;
            if (selectedTax != null)
            {
                var details = $"Tax: {selectedTax.TaxName}\n" +
                             $"Components: {selectedTax.TaxComponents}\n" +
                             $"Component Rates: {selectedTax.ComponentRates}\n" +
                             $"Total Rate: {selectedTax.TotalComponentRate:N2}%\n" +
                             $"Taxable Amount: {selectedTax.TaxableAmount:C}\n" +
                             $"Tax Amount: {selectedTax.TaxAmount:C}\n" +
                             $"Calculation Method: {selectedTax.CalculationMethod}";

                try
                {
                    Clipboard.SetText(details);
                    // Show a brief success message
                    var toolTip = new ToolTip();
                    toolTip.Show("Tax details copied to clipboard!", dgvTaxes, 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying to clipboard: {ex.Message}");
                }
            }
        }

        #endregion
    }
}


