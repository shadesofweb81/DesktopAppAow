using WinFormsApp1.Forms.Company;
using WinFormsApp1.Forms.Product;
using WinFormsApp1.Forms.FinancialYear;
using WinFormsApp1.Forms.Ledger;
using WinFormsApp1.Forms.Tax;
using WinFormsApp1.Forms.Transaction;
using WinFormsApp1.Models;
using WinFormsApp1.Services;
using WinFormsApp1.Forms.Auth;

namespace WinFormsApp1.Forms
{
    public partial class MainMDIForm : Form
    {
        private readonly AuthService _authService;
        private readonly CompanyService _companyService;
        private readonly LocalStorageService _localStorageService;
        private readonly FinancialYearService _financialYearService;
        private readonly LedgerService _ledgerService;
        private readonly TaxService _taxService;
        private readonly TransactionService _transactionService;
        private MenuStrip menuStrip = null!;
        private ToolStripMenuItem fileMenu = null!;
        private ToolStripMenuItem companyMenuItem = null!;
        private ToolStripMenuItem selectCompanyMenuItem = null!;
        private ToolStripMenuItem debugApiMenuItem = null!;
        private ToolStripMenuItem logoutMenuItem = null!;
        private ToolStripMenuItem exitMenuItem = null!;
        private ToolStripMenuItem transactionsMenu = null!;
        private ToolStripMenuItem transactionListMenuItem = null!;
        private ToolStripMenuItem windowMenu = null!;
        private ToolStripMenuItem cascadeMenuItem = null!;
        private ToolStripMenuItem tileHorizontalMenuItem = null!;
        private ToolStripMenuItem tileVerticalMenuItem = null!;
        private ToolStripMenuItem closeAllMenuItem = null!;
        private Label lblSelectedCompany = null!;
        private Label lblActiveFinancialYear = null!;

        // Navigation Panel Controls
        private Panel navigationPanel = null!;
        private GroupBox mainNavigationGroupBox = null!;
        private Label mastersLabel = null!;
        private Label transactionsLabel = null!;
        private Label reportsLabel = null!;

        // Masters Section Buttons
        private Button productsListButton = null!;
        private Button companyListButton = null!;
        private Button selectCompanyButton = null!;
        private Button financialYearListButton = null!;
        private Button accountsButton = null!;
        private Button taxButton = null!;

        // Transactions Section Buttons
        private Button saleButton = null!;
        private Button purchaseButton = null!;
        private Button receiptButton = null!;
        private Button paymentButton = null!;
        private Button journalButton = null!;

        // Reports Section Buttons
        private Button stockReportButton = null!;
        private Button taxReportButton = null!;
        private Button salesReportButton = null!;
        private Button purchaseReportButton = null!;
        private Button profitLossButton = null!;

        // Navigation state
        private bool isNavigationVisible = true;

        // Button navigation order for arrow key navigation within sections
        private Button[] mastersButtons = null!;
        private Button[] transactionsButtons = null!;
        private Button[] reportsButtons = null!;

        public MainMDIForm(AuthService authService)
        {
            _authService = authService;
            _companyService = new CompanyService(authService);
            _localStorageService = new LocalStorageService();
            _financialYearService = new FinancialYearService(authService);
            _ledgerService = new LedgerService(authService);
            _taxService = new TaxService(authService);
            _transactionService = new TransactionService(authService);
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem();
            selectCompanyMenuItem = new ToolStripMenuItem();
            companyMenuItem = new ToolStripMenuItem();
            debugApiMenuItem = new ToolStripMenuItem();
            logoutMenuItem = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            transactionsMenu = new ToolStripMenuItem();
            transactionListMenuItem = new ToolStripMenuItem();
            windowMenu = new ToolStripMenuItem();
            cascadeMenuItem = new ToolStripMenuItem();
            tileHorizontalMenuItem = new ToolStripMenuItem();
            tileVerticalMenuItem = new ToolStripMenuItem();
            closeAllMenuItem = new ToolStripMenuItem();
            lblSelectedCompany = new Label();
            lblActiveFinancialYear = new Label();
            navigationPanel = new Panel();
            mainNavigationGroupBox = new GroupBox();
            mastersLabel = new Label();
            productsListButton = new Button();
            companyListButton = new Button();
            selectCompanyButton = new Button();
            financialYearListButton = new Button();
            accountsButton = new Button();
            taxButton = new Button();
            transactionsLabel = new Label();
            saleButton = new Button();
            purchaseButton = new Button();
            receiptButton = new Button();
            paymentButton = new Button();
            journalButton = new Button();
            reportsLabel = new Label();
            stockReportButton = new Button();
            taxReportButton = new Button();
            salesReportButton = new Button();
            purchaseReportButton = new Button();
            profitLossButton = new Button();
            menuStrip.SuspendLayout();
            navigationPanel.SuspendLayout();
            mainNavigationGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, transactionsMenu, windowMenu });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 3, 0, 3);
            menuStrip.Size = new Size(914, 30);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // lblSelectedCompany
            // 
            lblSelectedCompany.AutoSize = true;
            lblSelectedCompany.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSelectedCompany.Location = new Point(360, 35);
            lblSelectedCompany.Name = "lblSelectedCompany";
            lblSelectedCompany.Size = new Size(300, 20);
            lblSelectedCompany.TabIndex = 2;
            lblSelectedCompany.Text = "No Company Selected";
            lblSelectedCompany.ForeColor = Color.Orange;
            lblSelectedCompany.BackColor = Color.FromArgb(255, 255, 240);
            lblSelectedCompany.BorderStyle = BorderStyle.FixedSingle;
            lblSelectedCompany.Padding = new Padding(8, 4, 8, 4);
            lblSelectedCompany.TextAlign = ContentAlignment.MiddleCenter;
            
            // 
            // lblActiveFinancialYear
            // 
            lblActiveFinancialYear.AutoSize = true;
            lblActiveFinancialYear.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblActiveFinancialYear.Location = new Point(360, 65);
            lblActiveFinancialYear.Name = "lblActiveFinancialYear";
            lblActiveFinancialYear.Size = new Size(300, 20);
            lblActiveFinancialYear.TabIndex = 3;
            lblActiveFinancialYear.Text = "No Financial Year Selected";
            lblActiveFinancialYear.ForeColor = Color.Gray;
            lblActiveFinancialYear.BackColor = Color.FromArgb(248, 248, 248);
            lblActiveFinancialYear.BorderStyle = BorderStyle.FixedSingle;
            lblActiveFinancialYear.Padding = new Padding(6, 2, 6, 2);
            lblActiveFinancialYear.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { selectCompanyMenuItem, companyMenuItem, debugApiMenuItem, logoutMenuItem, exitMenuItem });
            fileMenu.Name = "fileMenu";
            fileMenu.Size = new Size(46, 24);
            fileMenu.Text = "File";
            // 
            // selectCompanyMenuItem
            // 
            selectCompanyMenuItem.Name = "selectCompanyMenuItem";
            selectCompanyMenuItem.Size = new Size(224, 26);
            selectCompanyMenuItem.Text = "&Select Company";
            selectCompanyMenuItem.Click += selectCompanyMenuItem_Click;
            // 
            // companyMenuItem
            // 
            companyMenuItem.Name = "companyMenuItem";
            companyMenuItem.Size = new Size(224, 26);
            companyMenuItem.Text = "&Manage Companies";
            companyMenuItem.Click += companyMenuItem_Click;
            // 
            // debugApiMenuItem
            // 
            debugApiMenuItem.Name = "debugApiMenuItem";
            debugApiMenuItem.Size = new Size(224, 26);
            // 
            // logoutMenuItem
            // 
            logoutMenuItem.Name = "logoutMenuItem";
            logoutMenuItem.Size = new Size(224, 26);
            logoutMenuItem.Text = "Logout";
            logoutMenuItem.Click += logoutMenuItem_Click;
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(224, 26);
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += exitMenuItem_Click;
            // 
            // transactionsMenu
            // 
            transactionsMenu.DropDownItems.AddRange(new ToolStripItem[] { transactionListMenuItem });
            transactionsMenu.Name = "transactionsMenu";
            transactionsMenu.Size = new Size(106, 24);
            transactionsMenu.Text = "&Transactions";
            // 
            // transactionListMenuItem
            // 
            transactionListMenuItem.Name = "transactionListMenuItem";
            transactionListMenuItem.Size = new Size(224, 26);
            transactionListMenuItem.Text = "&Transaction List";
            transactionListMenuItem.Click += transactionListMenuItem_Click;
            // 
            // windowMenu
            // 
            windowMenu.DropDownItems.AddRange(new ToolStripItem[] { cascadeMenuItem, tileHorizontalMenuItem, tileVerticalMenuItem, closeAllMenuItem });
            windowMenu.Name = "windowMenu";
            windowMenu.Size = new Size(78, 24);
            windowMenu.Text = "Window";
            // 
            // cascadeMenuItem
            // 
            cascadeMenuItem.Name = "cascadeMenuItem";
            cascadeMenuItem.Size = new Size(190, 26);
            cascadeMenuItem.Text = "Cascade";
            cascadeMenuItem.Click += cascadeMenuItem_Click;
            // 
            // tileHorizontalMenuItem
            // 
            tileHorizontalMenuItem.Name = "tileHorizontalMenuItem";
            tileHorizontalMenuItem.Size = new Size(190, 26);
            tileHorizontalMenuItem.Text = "Tile Horizontal";
            tileHorizontalMenuItem.Click += tileHorizontalMenuItem_Click;
            // 
            // tileVerticalMenuItem
            // 
            tileVerticalMenuItem.Name = "tileVerticalMenuItem";
            tileVerticalMenuItem.Size = new Size(190, 26);
            tileVerticalMenuItem.Text = "Tile Vertical";
            tileVerticalMenuItem.Click += tileVerticalMenuItem_Click;
            // 
            // closeAllMenuItem
            // 
            closeAllMenuItem.Name = "closeAllMenuItem";
            closeAllMenuItem.Size = new Size(190, 26);
            closeAllMenuItem.Text = "Close All";
            closeAllMenuItem.Click += closeAllMenuItem_Click;
            // 
            // navigationPanel
            // 
            navigationPanel.BackColor = Color.FromArgb(245, 245, 245);
            navigationPanel.BorderStyle = BorderStyle.FixedSingle;
            navigationPanel.Controls.Add(mainNavigationGroupBox);
            navigationPanel.Dock = DockStyle.Left;
            navigationPanel.Location = new Point(0, 30);
            navigationPanel.Margin = new Padding(3, 4, 3, 4);
            navigationPanel.Name = "navigationPanel";
            navigationPanel.Padding = new Padding(6, 7, 6, 7);
            navigationPanel.Size = new Size(343, 770);
            navigationPanel.TabIndex = 1;
            // 
            // mainNavigationGroupBox
            // 
            mainNavigationGroupBox.Controls.Add(mastersLabel);
            mainNavigationGroupBox.Controls.Add(productsListButton);
            mainNavigationGroupBox.Controls.Add(companyListButton);
            mainNavigationGroupBox.Controls.Add(selectCompanyButton);
            mainNavigationGroupBox.Controls.Add(financialYearListButton);
            mainNavigationGroupBox.Controls.Add(accountsButton);
            mainNavigationGroupBox.Controls.Add(taxButton);
            mainNavigationGroupBox.Controls.Add(transactionsLabel);
            mainNavigationGroupBox.Controls.Add(saleButton);
            mainNavigationGroupBox.Controls.Add(purchaseButton);
            mainNavigationGroupBox.Controls.Add(receiptButton);
            mainNavigationGroupBox.Controls.Add(paymentButton);
            mainNavigationGroupBox.Controls.Add(journalButton);
            mainNavigationGroupBox.Controls.Add(reportsLabel);
            mainNavigationGroupBox.Controls.Add(stockReportButton);
            mainNavigationGroupBox.Controls.Add(taxReportButton);
            mainNavigationGroupBox.Controls.Add(salesReportButton);
            mainNavigationGroupBox.Controls.Add(purchaseReportButton);
            mainNavigationGroupBox.Controls.Add(profitLossButton);
            mainNavigationGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            mainNavigationGroupBox.Location = new Point(11, 13);
            mainNavigationGroupBox.Margin = new Padding(3, 4, 3, 4);
            mainNavigationGroupBox.Name = "mainNavigationGroupBox";
            mainNavigationGroupBox.Padding = new Padding(3, 4, 3, 4);
            mainNavigationGroupBox.Size = new Size(320, 833);
            mainNavigationGroupBox.TabIndex = 0;
            mainNavigationGroupBox.TabStop = false;
            mainNavigationGroupBox.Text = "APPLICATION NAVIGATION";
            mainNavigationGroupBox.Enter += mainNavigationGroupBox_Enter;
            // 
            // mastersLabel
            // 
            mastersLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            mastersLabel.ForeColor = Color.FromArgb(0, 102, 204);
            mastersLabel.Location = new Point(9, 33);
            mastersLabel.Name = "mastersLabel";
            mastersLabel.Size = new Size(171, 27);
            mastersLabel.TabIndex = 0;
            mastersLabel.Text = "MASTERS (Alt+M)";
            // 
            // productsListButton
            // 
            productsListButton.Font = new Font("Segoe UI", 9F);
            productsListButton.Location = new Point(9, 67);
            productsListButton.Margin = new Padding(3, 4, 3, 4);
            productsListButton.Name = "productsListButton";
            productsListButton.Size = new Size(302, 37);
            productsListButton.TabIndex = 1;
            productsListButton.Text = "&Products List (F2)";
            productsListButton.UseVisualStyleBackColor = true;
            productsListButton.Click += productsListButton_Click;
            // 
            // companyListButton
            // 
            companyListButton.Font = new Font("Segoe UI", 9F);
            companyListButton.Location = new Point(9, 112);
            companyListButton.Margin = new Padding(3, 4, 3, 4);
            companyListButton.Name = "companyListButton";
            companyListButton.Size = new Size(302, 37);
            companyListButton.TabIndex = 1;
            companyListButton.Text = "&Company List (F3)";
            companyListButton.UseVisualStyleBackColor = true;
            companyListButton.Click += companyListButton_Click;
            // 
            // selectCompanyButton
            // 
            selectCompanyButton.Font = new Font("Segoe UI", 9F);
            selectCompanyButton.Location = new Point(9, 157);
            selectCompanyButton.Margin = new Padding(3, 4, 3, 4);
            selectCompanyButton.Name = "selectCompanyButton";
            selectCompanyButton.Size = new Size(302, 37);
            selectCompanyButton.TabIndex = 2;
            selectCompanyButton.Text = "&Select Company (F4)";
            selectCompanyButton.UseVisualStyleBackColor = true;
            selectCompanyButton.Click += selectCompanyButton_Click;
            // 
            // financialYearListButton
            // 
            financialYearListButton.Font = new Font("Segoe UI", 9F);
            financialYearListButton.Location = new Point(9, 203);
            financialYearListButton.Margin = new Padding(3, 4, 3, 4);
            financialYearListButton.Name = "financialYearListButton";
            financialYearListButton.Size = new Size(302, 37);
            financialYearListButton.TabIndex = 3;
            financialYearListButton.Text = "&Financial Years (F5)";
            financialYearListButton.UseVisualStyleBackColor = true;
            financialYearListButton.Click += financialYearListButton_Click;
            // 
            // accountsButton
            // 
            accountsButton.Font = new Font("Segoe UI", 9F);
            accountsButton.Location = new Point(9, 248);
            accountsButton.Margin = new Padding(3, 4, 3, 4);
            accountsButton.Name = "accountsButton";
            accountsButton.Size = new Size(302, 37);
            accountsButton.TabIndex = 4;
            accountsButton.Text = "&Accounts (F6)";
            accountsButton.UseVisualStyleBackColor = true;
            accountsButton.Click += accountsButton_Click;
            // 
            // taxButton
            // 
            taxButton.Font = new Font("Segoe UI", 9F);
            taxButton.Location = new Point(9, 293);
            taxButton.Margin = new Padding(3, 4, 3, 4);
            taxButton.Name = "taxButton";
            taxButton.Size = new Size(302, 37);
            taxButton.TabIndex = 5;
            taxButton.Text = "&Tax (F12)";
            taxButton.UseVisualStyleBackColor = true;
            taxButton.Click += taxButton_Click;
            // 
            // transactionsLabel
            // 
            transactionsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            transactionsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            transactionsLabel.Location = new Point(9, 345);
            transactionsLabel.Name = "transactionsLabel";
            transactionsLabel.Size = new Size(229, 27);
            transactionsLabel.TabIndex = 6;
            transactionsLabel.Text = "TRANSACTIONS (Alt+T)";
            // 
            // saleButton
            // 
            saleButton.Font = new Font("Segoe UI", 9F);
            saleButton.Location = new Point(9, 378);
            saleButton.Margin = new Padding(3, 4, 3, 4);
            saleButton.Name = "saleButton";
            saleButton.Size = new Size(302, 37);
            saleButton.TabIndex = 7;
            saleButton.Text = "&Sale (F7)";
            saleButton.UseVisualStyleBackColor = true;
            saleButton.Click += saleButton_Click;
            // 
            // purchaseButton
            // 
            purchaseButton.Font = new Font("Segoe UI", 9F);
            purchaseButton.Location = new Point(9, 424);
            purchaseButton.Margin = new Padding(3, 4, 3, 4);
            purchaseButton.Name = "purchaseButton";
            purchaseButton.Size = new Size(302, 37);
            purchaseButton.TabIndex = 8;
            purchaseButton.Text = "&Purchase (F8)";
            purchaseButton.UseVisualStyleBackColor = true;
            purchaseButton.Click += purchaseButton_Click;
            // 
            // receiptButton
            // 
            receiptButton.Font = new Font("Segoe UI", 9F);
            receiptButton.Location = new Point(9, 469);
            receiptButton.Margin = new Padding(3, 4, 3, 4);
            receiptButton.Name = "receiptButton";
            receiptButton.Size = new Size(302, 37);
            receiptButton.TabIndex = 9;
            receiptButton.Text = "&Receipt (F9)";
            receiptButton.UseVisualStyleBackColor = true;
            receiptButton.Click += receiptButton_Click;
            // 
            // paymentButton
            // 
            paymentButton.Font = new Font("Segoe UI", 9F);
            paymentButton.Location = new Point(9, 514);
            paymentButton.Margin = new Padding(3, 4, 3, 4);
            paymentButton.Name = "paymentButton";
            paymentButton.Size = new Size(302, 37);
            paymentButton.TabIndex = 10;
            paymentButton.Text = "Pa&yment (F10)";
            paymentButton.UseVisualStyleBackColor = true;
            paymentButton.Click += paymentButton_Click;
            // 
            // journalButton
            // 
            journalButton.Font = new Font("Segoe UI", 9F);
            journalButton.Location = new Point(9, 559);
            journalButton.Margin = new Padding(3, 4, 3, 4);
            journalButton.Name = "journalButton";
            journalButton.Size = new Size(302, 37);
            journalButton.TabIndex = 11;
            journalButton.Text = "&Journal (F11)";
            journalButton.UseVisualStyleBackColor = true;
            journalButton.Click += journalButton_Click;
            // 
            // reportsLabel
            // 
            reportsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            reportsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            reportsLabel.Location = new Point(9, 611);
            reportsLabel.Name = "reportsLabel";
            reportsLabel.Size = new Size(171, 27);
            reportsLabel.TabIndex = 12;
            reportsLabel.Text = "REPORTS (Alt+R)";
            // 
            // stockReportButton
            // 
            stockReportButton.Font = new Font("Segoe UI", 9F);
            stockReportButton.Location = new Point(9, 644);
            stockReportButton.Margin = new Padding(3, 4, 3, 4);
            stockReportButton.Name = "stockReportButton";
            stockReportButton.Size = new Size(302, 37);
            stockReportButton.TabIndex = 13;
            stockReportButton.Text = "St&ock Report (Ctrl+F1)";
            stockReportButton.UseVisualStyleBackColor = true;
            stockReportButton.Click += stockReportButton_Click;
            // 
            // taxReportButton
            // 
            taxReportButton.Font = new Font("Segoe UI", 9F);
            taxReportButton.Location = new Point(9, 644);
            taxReportButton.Margin = new Padding(3, 4, 3, 4);
            taxReportButton.Name = "taxReportButton";
            taxReportButton.Size = new Size(302, 37);
            taxReportButton.TabIndex = 13;
            taxReportButton.Text = "Ta&x Report (Ctrl+F2)";
            taxReportButton.UseVisualStyleBackColor = true;
            taxReportButton.Click += taxReportButton_Click;
            // 
            // salesReportButton
            // 
            salesReportButton.Font = new Font("Segoe UI", 9F);
            salesReportButton.Location = new Point(9, 689);
            salesReportButton.Margin = new Padding(3, 4, 3, 4);
            salesReportButton.Name = "salesReportButton";
            salesReportButton.Size = new Size(302, 37);
            salesReportButton.TabIndex = 14;
            salesReportButton.Text = "Sales &Report (Ctrl+F3)";
            salesReportButton.UseVisualStyleBackColor = true;
            salesReportButton.Click += salesReportButton_Click;
            // 
            // purchaseReportButton
            // 
            purchaseReportButton.Font = new Font("Segoe UI", 9F);
            purchaseReportButton.Location = new Point(9, 734);
            purchaseReportButton.Margin = new Padding(3, 4, 3, 4);
            purchaseReportButton.Name = "purchaseReportButton";
            purchaseReportButton.Size = new Size(302, 37);
            purchaseReportButton.TabIndex = 15;
            purchaseReportButton.Text = "Purchase Re&port (Ctrl+F4)";
            purchaseReportButton.UseVisualStyleBackColor = true;
            purchaseReportButton.Click += purchaseReportButton_Click;
            // 
            // profitLossButton
            // 
            profitLossButton.Font = new Font("Segoe UI", 9F);
            profitLossButton.Location = new Point(9, 779);
            profitLossButton.Margin = new Padding(3, 4, 3, 4);
            profitLossButton.Name = "profitLossButton";
            profitLossButton.Size = new Size(302, 37);
            profitLossButton.TabIndex = 16;
            profitLossButton.Text = "Profit && &Loss (Ctrl+F5)";
            profitLossButton.UseVisualStyleBackColor = true;
            profitLossButton.Click += profitLossButton_Click;
            // 
            // MainMDIForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 800);
            Controls.Add(lblActiveFinancialYear);
            Controls.Add(lblSelectedCompany);
            Controls.Add(navigationPanel);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainMDIForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Application - Esc=File Menu | ↑↓=Navigate All | Alt+M/T/R=Jump Sections | F1=Help";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainMDIForm_FormClosing;
            KeyDown += MainMDIForm_KeyDown;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            navigationPanel.ResumeLayout(false);
            mainNavigationGroupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Add dashboard menu item
            var dashboardMenuItem = new ToolStripMenuItem("&Dashboard");
            dashboardMenuItem.Click += new EventHandler(this.dashboardMenuItem_Click);
            fileMenu.DropDownItems.Insert(0, dashboardMenuItem);
            fileMenu.DropDownItems.Insert(1, new ToolStripSeparator());

            // Add a menu item to create new child forms
            var newFormMenuItem = new ToolStripMenuItem("New Form");
            fileMenu.DropDownItems.Insert(2, newFormMenuItem);
            fileMenu.DropDownItems.Insert(3, new ToolStripSeparator());

            // Initialize navigation button order for arrow key navigation
            InitializeNavigationOrder();

            // Update title with selected company if any
            UpdateTitleWithSelectedCompany();

            // Check if company is selected, if not, show company selection form
            CheckAndShowCompanySelection();

            // Set focus to navigation panel after form is loaded
            this.Load += (s, e) => 
            {
                SetFocusToNavigation();
                // Ensure company display is updated after form loads
                UpdateTitleWithSelectedCompany();
            };

            // Monitor child form closing events
            this.MdiChildActivate += (s, e) => CheckAndShowNavigationIfNoChildForms();
            
            // Monitor child form closing to show navigation when CompanyForm is closed
            this.MdiChildActivate += (s, e) => HandleChildFormActivation();
            
            // Update company display when form is activated
            this.Activated += (s, e) => UpdateTitleWithSelectedCompany();
        }

        private void InitializeNavigationOrder()
        {
            // Define a single array for all navigation buttons for easy arrow key navigation
            mastersButtons = new Button[]
            {
                productsListButton,
                companyListButton,
                selectCompanyButton,
                financialYearListButton,
                accountsButton,
                taxButton
            };

            // Keep section-specific arrays for Alt+M/T/R shortcuts
            transactionsButtons = new Button[]
            {
                saleButton,
                purchaseButton,
                receiptButton,
                paymentButton,
                journalButton
            };

            reportsButtons = new Button[]
            {
                stockReportButton,
                taxReportButton,
                salesReportButton,
                purchaseReportButton,
                profitLossButton
            };
        }

        private async void CheckAndShowCompanySelection()
        {
            try
            {
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany == null)
                {
                    // No company selected, show company selection form automatically
                    var companySelectForm = new CompanySelectForm(_companyService, _localStorageService);
                    var result = companySelectForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // Company was selected successfully, update title and show navigation
                        UpdateTitleWithSelectedCompany();
                        ShowNavigationPanel();
                    }
                    else
                    {
                        // User cancelled company selection, show navigation panel
                        ShowNavigationPanel();
                    }
                }
                else
                {
                    // Company is already selected, just update title and show navigation
                    UpdateTitleWithSelectedCompany();
                    ShowNavigationPanel();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking company selection: {ex.Message}");
                // Show navigation panel if there's an error
                ShowNavigationPanel();
            }
        }

        private void CreateDashboardForm()
        {
            // Dashboard form will be implemented later
            MessageBox.Show("Dashboard feature will be implemented in a future update.", "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void logoutMenuItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?\n\nThis will clear your saved login, company data, and require you to log in again.", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await PerformLogout();
            }
        }

        private async Task PerformLogout()
        {
            try
            {
                // Close all child forms first
                foreach (Form childForm in this.MdiChildren.ToList())
                {
                    childForm.Close();
                }

                // Clear all local storage data (company and user data)
                await _localStorageService.ClearAllDataAsync();

                // Logout from auth service (clears JWT token)
                _authService.Logout();

                // Hide the main form
                this.Hide();

                // Show login form
                using var loginForm = new LoginForm(_authService);
                var loginResult = loginForm.ShowDialog();

                // If login was successful, show the main form again
                if (loginResult == DialogResult.OK && _authService.IsAuthenticated)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
                else
                {
                    // If login was cancelled or failed, close the application
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void exitMenuItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void cascadeMenuItem_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalMenuItem_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalMenuItem_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void closeAllMenuItem_Click(object? sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }
        }

        private void transactionListMenuItem_Click(object? sender, EventArgs e)
        {
            OpenTransactionListForm();
        }



        private async void MainMDIForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F when e.Alt: // Alt+F to open File menu
                    fileMenu.ShowDropDown();
                    e.Handled = true;
                    break;

                case Keys.M when e.Alt: // Alt+M to focus Masters section
                    ShowNavigationAndFocusSection("masters");
                    e.Handled = true;
                    break;

                case Keys.T when e.Alt: // Alt+T to focus Transactions section
                    ShowNavigationAndFocusSection("transactions");
                    e.Handled = true;
                    break;

                case Keys.R when e.Alt: // Alt+R to focus Reports section
                    ShowNavigationAndFocusSection("reports");
                    e.Handled = true;
                    break;

                case Keys.Y when e.Alt: // Alt+Y to refresh financial year display
                    await RefreshFinancialYearDisplay();
                    e.Handled = true;
                    break;

                case Keys.S when e.Alt: // Alt+S for Select Company
                    OpenCompanySelectForm();
                    e.Handled = true;
                    break;

                case Keys.D when e.Alt: // Alt+D for Dashboard
                    CreateDashboardForm();
                    e.Handled = true;
                    break;

                case Keys.C when e.Alt: // Alt+C for Manage Companies
                    OpenCompanyFormMaximized();
                    e.Handled = true;
                    break;

                // Function keys for Masters section (only when no modifiers)
                case Keys.F2 when !e.Control && !e.Alt:
                    productsListButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F3 when !e.Control && !e.Alt:
                    companyListButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F4 when !e.Control && !e.Alt:
                    selectCompanyButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F5 when !e.Control && !e.Alt:
                    financialYearListButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F6 when !e.Control && !e.Alt:
                    accountsButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;

                // Function keys for Transactions section (only when no modifiers)
                case Keys.F7 when !e.Control && !e.Alt:
                    saleButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F8 when !e.Control && !e.Alt:
                    purchaseButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F9 when !e.Control && !e.Alt:
                    receiptButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F10 when !e.Control && !e.Alt:
                    paymentButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F11 when !e.Control && !e.Alt:
                    journalButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;

                // Ctrl+Function keys for Reports section
                case Keys.F1 when e.Control && !e.Alt:
                    stockReportButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F2 when e.Control && !e.Alt:
                    taxReportButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F3 when e.Control && !e.Alt:
                    salesReportButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F4 when e.Control && !e.Alt:
                    purchaseReportButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F5 when e.Control && !e.Alt:
                    profitLossButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;

                case Keys.F1 when !e.Control && !e.Alt: // F1 for Help (only when no modifiers)
                    ShowKeyboardHelp();
                    e.Handled = true;
                    break;

                case Keys.F4 when e.Alt: // Alt+F4 to exit
                    this.Close();
                    e.Handled = true;
                    break;

                case Keys.Escape: // ESC to close active child form or open file menu
                    if (this.ActiveMdiChild != null)
                    {
                        this.ActiveMdiChild.Close();
                        e.Handled = true;

                        // If this was the last child form, show navigation panel
                        if (this.MdiChildren.Length == 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                // Clear all highlights and show navigation panel with first button focused
                                ClearAllButtonHighlights();
                                ShowNavigationPanel();
                                Console.WriteLine("Last MDI form closed, Navigation panel shown");
                            }));
                        }
                    }
                    else
                    {
                        // If no child forms, open file menu
                        fileMenu.ShowDropDown();
                        e.Handled = true;
                    }
                    break;

                case Keys.Tab when e.Control: // Ctrl+Tab to cycle through child forms
                    CycleChildForms();
                    e.Handled = true;
                    break;

                case Keys.W when e.Control: // Ctrl+W to close active child form
                    if (this.ActiveMdiChild != null)
                    {
                        this.ActiveMdiChild.Close();
                        e.Handled = true;

                        // If this was the last child form, show navigation panel
                        if (this.MdiChildren.Length == 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                // Clear all highlights and show navigation panel with first button focused
                                ClearAllButtonHighlights();
                                ShowNavigationPanel();
                                Console.WriteLine("Last MDI form closed with Ctrl+W, Navigation panel shown");
                            }));
                        }
                    }
                    break;

                case Keys.Up: // Up arrow to navigate to previous button
                    if (isNavigationVisible && NavigateToPreviousButton())
                    {
                        e.Handled = true;
                    }
                    break;

                case Keys.Down: // Down arrow to navigate to next button
                    if (isNavigationVisible && NavigateToNextButton())
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void selectCompanyMenuItem_Click(object? sender, EventArgs e)
        {
            OpenCompanySelectForm();
        }

        private void dashboardMenuItem_Click(object? sender, EventArgs e)
        {
            CreateDashboardForm();
        }

        private void companyMenuItem_Click(object? sender, EventArgs e)
        {
            OpenCompanyFormMaximized();
        }

        private void OpenCompanyForm()
        {
            // Check if Company form is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is CompanyForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new company form
            var companyForm = new CompanyForm(_companyService)
            {
                MdiParent = this,
                Text = "Company Management"
            };

            companyForm.Show();
            
            // Ensure navigation panel remains visible
            ShowNavigationPanel();
        }

        private void OpenCompanyFormMaximized()
        {
            // Check if Company form is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is CompanyForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    childForm.WindowState = FormWindowState.Maximized;
                    HideNavigationPanel();
                    return;
                }
            }

            // Create new company form as maximized child form
            var companyForm = new CompanyForm(_companyService)
            {
                MdiParent = this,
                Text = "Company Management",
                WindowState = FormWindowState.Maximized
            };

            companyForm.Show();
            
            // Hide navigation panel when company form is maximized
            HideNavigationPanel();
        }

        private void OpenProductForm()
        {
            // Check if Product form is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is ProductForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new product form
            var productService = new ProductService(_authService);
            var attributeService = new AttributeService(_authService);
            var productForm = new ProductForm(productService, attributeService)
            {
                MdiParent = this,
                Text = "Product Management",
                WindowState = FormWindowState.Maximized
            };

            productForm.Show();
            
            // Hide navigation panel when ProductForm is opened
            HideNavigationPanel();
        }

        private void OpenFinancialYearListForm()
        {
            // Check if FinancialYearListForm is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is FinancialYearListForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new financial year list form
            var financialYearService = new FinancialYearService(_authService);
            var financialYearListForm = new FinancialYearListForm(financialYearService, this)
            {
                MdiParent = this,
                Text = "Financial Year Management",
                WindowState = FormWindowState.Maximized
            };

            financialYearListForm.Show();
            
            // Hide navigation panel when FinancialYearListForm is opened
            HideNavigationPanel();
            
            // Add form closing event to ensure proper focus management
            financialYearListForm.FormClosed += (s, e) =>
            {
                // Ensure proper focus when form is closed
                this.BeginInvoke(new Action(() =>
                {
                    if (this.MdiChildren.Length == 0)
                    {
                        // Clear all highlights and show navigation panel with first button focused
                        ClearAllButtonHighlights();
                        ShowNavigationPanel();
                    }
                }));
            };
        }

        private void OpenLedgerListForm()
        {
            // Check if LedgerListForm is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is LedgerListForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new ledger list form
            var ledgerListForm = new LedgerListForm(_ledgerService)
            {
                MdiParent = this,
                Text = "Ledger Management",
                WindowState = FormWindowState.Maximized
            };

            ledgerListForm.Show();
            
            // Hide navigation panel when LedgerListForm is opened
            HideNavigationPanel();
            
            // Add form closing event to ensure proper focus management
            ledgerListForm.FormClosed += (s, e) =>
            {
                // Ensure proper focus when form is closed
                this.BeginInvoke(new Action(() =>
                {
                    if (this.MdiChildren.Length == 0)
                    {
                        // Clear all highlights and show navigation panel with first button focused
                        ClearAllButtonHighlights();
                        ShowNavigationPanel();
                    }
                }));
            };
        }

        private void OpenTaxListForm()
        {
            // Check if TaxListForm is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is TaxListForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new tax list form
            var taxListForm = new TaxListForm(_taxService)
            {
                MdiParent = this,
                Text = "Tax Management",
                WindowState = FormWindowState.Maximized
            };

            taxListForm.Show();
            
            // Hide navigation panel when TaxListForm is opened
            HideNavigationPanel();
            
            // Add form closing event to ensure proper focus management
            taxListForm.FormClosed += (s, e) =>
            {
                // Ensure proper focus when form is closed
                this.BeginInvoke(new Action(() =>
                {
                    if (this.MdiChildren.Length == 0)
                    {
                        // Clear all highlights and show navigation panel with first button focused
                        ClearAllButtonHighlights();
                        ShowNavigationPanel();
                    }
                }));
            };
        }

        private void OpenTransactionListForm()
        {
            // Check if TransactionListForm is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is TransactionListForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new transaction list form
            var transactionListForm = new TransactionListForm(_transactionService, _localStorageService)
            {
                MdiParent = this,
                Text = "Transaction Management",
                WindowState = FormWindowState.Maximized
            };

            transactionListForm.Show();
            
            // Hide navigation panel when TransactionListForm is opened
            HideNavigationPanel();
            
            // Add form closing event to ensure proper focus management
            transactionListForm.FormClosed += (s, e) =>
            {
                // Ensure proper focus when form is closed
                this.BeginInvoke(new Action(() =>
                {
                    if (this.MdiChildren.Length == 0)
                    {
                        // Clear all highlights and show navigation panel with first button focused
                        ClearAllButtonHighlights();
                        ShowNavigationPanel();
                    }
                }));
            };
        }

        private async void OpenCompanySelectForm()
        {
            // Check if CompanySelectForm is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is CompanySelectForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new company select form as MDI child
            var companySelectForm = new CompanySelectForm(_companyService, _localStorageService)
            {
                MdiParent = this,
                Text = "Select Company",
                WindowState = FormWindowState.Maximized
            };

            companySelectForm.Show();
            
            // Monitor the form closing to handle company selection
            companySelectForm.FormClosed += async (s, e) =>
            {
                // Refresh the title to show selected company
                UpdateTitleWithSelectedCompany();

                // Check if a company was actually selected (not just cancelled)
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany != null)
                {
                    // Clear the previous financial year when company changes
                    await _localStorageService.ClearSelectedFinancialYearAsync();
                    
                    // Show a brief message about the company change
                    MessageBox.Show($"Company changed to: {selectedCompany.DisplayName}. Financial year will be updated automatically.",
                        "Company Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
        }

        private async void UpdateTitleWithSelectedCompany()
        {
            try
            {
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                
                // Ensure UI updates happen on the UI thread
                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (selectedCompany != null)
                        {
                            this.Text = $"Main Application - {selectedCompany.DisplayName} - Alt+F=File Menu, Alt+S=Select Company, Alt+C=Manage Companies, Alt+Y=Refresh FY";
                            
                            // Update company display label if it exists
                            if (lblSelectedCompany != null)
                            {
                                lblSelectedCompany.Text = $"Selected Company: {selectedCompany.DisplayName}";
                                lblSelectedCompany.ForeColor = Color.DarkGreen;
                                lblSelectedCompany.BackColor = Color.FromArgb(240, 255, 240);
                                lblSelectedCompany.Visible = true;
                            }
                        }
                        else
                        {
                            this.Text = "Main Application - Alt+F=File Menu, Alt+D=Dashboard, Alt+S=Select Company, Alt+C=Manage Companies, Alt+F4=Exit, F1=Help";
                            
                            // Update company display label if it exists
                            if (lblSelectedCompany != null)
                            {
                                lblSelectedCompany.Text = "No Company Selected";
                                lblSelectedCompany.ForeColor = Color.Orange;
                                lblSelectedCompany.BackColor = Color.FromArgb(255, 255, 240);
                                lblSelectedCompany.Visible = true;
                            }

                            // Clear financial year display
                            if (lblActiveFinancialYear != null)
                            {
                                lblActiveFinancialYear.Text = "No Financial Year Selected";
                                lblActiveFinancialYear.ForeColor = Color.Gray;
                                lblActiveFinancialYear.BackColor = Color.FromArgb(248, 248, 248);
                                lblActiveFinancialYear.Visible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating UI in UpdateTitleWithSelectedCompany: {ex.Message}");
                        // If there's an error, just use the default title
                        this.Text = "Main Application - Alt+F=File Menu, Alt+D=Dashboard, Alt+S=Select Company, Alt+C=Manage Companies, Alt+F4=Exit, F1=Help";
                        
                        // Update company display label if it exists
                        if (lblSelectedCompany != null)
                        {
                            lblSelectedCompany.Text = "Error loading company";
                            lblSelectedCompany.ForeColor = Color.Red;
                            lblSelectedCompany.BackColor = Color.FromArgb(255, 240, 240);
                            lblSelectedCompany.Visible = true;
                        }

                        // Clear financial year display
                        if (lblActiveFinancialYear != null)
                        {
                            lblActiveFinancialYear.Text = "Error loading financial year";
                            lblActiveFinancialYear.ForeColor = Color.Red;
                            lblActiveFinancialYear.BackColor = Color.FromArgb(255, 240, 240);
                            lblActiveFinancialYear.Visible = true;
                        }
                    }
                }));

                // Fetch and display active financial year (if company exists)
                if (selectedCompany != null)
                {
                    await UpdateActiveFinancialYearDisplay(selectedCompany.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTitleWithSelectedCompany: {ex.Message}");
                // Ensure UI updates happen on the UI thread even for errors
                this.BeginInvoke(new Action(() =>
                {
                    this.Text = "Main Application - Alt+F=File Menu, Alt+D=Dashboard, Alt+S=Select Company, Alt+C=Manage Companies, Alt+F4=Exit, F1=Help";
                    
                    if (lblSelectedCompany != null)
                    {
                        lblSelectedCompany.Text = "Error loading company";
                        lblSelectedCompany.ForeColor = Color.Red;
                        lblSelectedCompany.BackColor = Color.FromArgb(255, 240, 240);
                        lblSelectedCompany.Visible = true;
                    }

                    if (lblActiveFinancialYear != null)
                    {
                        lblActiveFinancialYear.Text = "Error loading financial year";
                        lblActiveFinancialYear.ForeColor = Color.Red;
                        lblActiveFinancialYear.BackColor = Color.FromArgb(255, 240, 240);
                        lblActiveFinancialYear.Visible = true;
                    }
                }));
            }
        }

        private void ShowKeyboardHelp()
        {
            var helpMessage = @"Keyboard Navigation Help:

MAIN NAVIGATION:
• Esc - Open File Menu
• ↑↓ - Navigate through all buttons (across all sections)
• Alt+M - Jump to Masters Section (Products)
• Alt+T - Jump to Transactions Section (Sale)  
• Alt+R - Jump to Reports Section (Stock Report)
• Alt+F - Open File Menu
• Alt+F4 - Exit Application
• F1 - Show this help

COMPANY & FINANCIAL YEAR:
• Alt+S - Select Company
• Alt+C - Manage Companies
• Alt+Y - Refresh Financial Year Display

MASTERS SECTION:
• F2 - Products List
• F3 - Company List
• F4 - Select Company
• F5 - Financial Years
• F6 - Accounts

TRANSACTIONS SECTION:
• F7 - Sale
• F8 - Purchase
• F9 - Receipt
• F10 - Payment
• F11 - Journal

REPORTS SECTION:
• Ctrl+F1 - Stock Report
• Ctrl+F2 - Tax Report
• Ctrl+F3 - Sales Report
• Ctrl+F4 - Purchase Report
• Ctrl+F5 - Profit & Loss

FORM MANAGEMENT:
• Ctrl+Tab - Cycle through open forms
• Ctrl+W - Close active form
• Esc - Close active form (opens File menu)

SYSTEM FUNCTIONS:
• Alt+D - Dashboard

NAVIGATION TIP:
All buttons are now in one group for easy navigation. Use ↑↓ arrows to move through all buttons seamlessly, or use Alt+M/T/R to jump to specific sections.";

            MessageBox.Show(helpMessage, "Keyboard Navigation Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CycleChildForms()
        {
            if (this.MdiChildren.Length > 1)
            {
                var currentForm = this.ActiveMdiChild;
                var forms = this.MdiChildren.ToList();
                var currentIndex = forms.IndexOf(currentForm);
                var nextIndex = (currentIndex + 1) % forms.Count;
                forms[nextIndex].BringToFront();
                forms[nextIndex].Activate();
            }
        }


        public LocalStorageService GetLocalStorageService()
        {
            return _localStorageService;
        }

        public async Task RefreshCompanyDisplay()
        {
            await Task.Run(async () => 
            {
                await Task.Delay(100); // Small delay to ensure UI is ready
                this.BeginInvoke(new Action(() => UpdateTitleWithSelectedCompany()));
            });
        }

        public async Task RefreshFinancialYearDisplay()
        {
            try
            {
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany != null)
                {
                    await UpdateActiveFinancialYearDisplay(selectedCompany.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing financial year display: {ex.Message}");
            }
        }

        public async Task SetActiveFinancialYearAsync(FinancialYearModel financialYear)
        {
            try
            {
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany == null)
                {
                    MessageBox.Show("No company selected. Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Set as active in the API
                var success = await _financialYearService.SetActiveFinancialYearAsync(
                    Guid.Parse(selectedCompany.Id), 
                    financialYear.Id);

                if (success)
                {
                    // Save to local storage
                    await _localStorageService.SaveSelectedFinancialYearAsync(financialYear);
                    
                    // Update the display
                    UpdateFinancialYearLabel(financialYear);
                    
                    // Show success message
                    MessageBox.Show($"Financial Year '{financialYear.YearLabel}' has been set as active.", 
                        "Financial Year Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to set financial year as active. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting active financial year: {ex.Message}");
                MessageBox.Show($"Error setting active financial year: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateActiveFinancialYearDisplay(string companyId)
        {
            try
            {
                if (Guid.TryParse(companyId, out Guid companyGuid))
                {
                    // First try to get from local storage
                    var storedFinancialYear = await _localStorageService.GetSelectedFinancialYearAsync();
                    
                    // If we have a stored financial year, use it
                    if (storedFinancialYear != null)
                    {
                        this.BeginInvoke(new Action(() => UpdateFinancialYearLabel(storedFinancialYear)));
                        return;
                    }
                    
                    // Otherwise, fetch from API
                    var activeFinancialYear = await _financialYearService.GetActiveFinancialYearAsync(companyGuid);
                    
                    if (activeFinancialYear != null)
                    {
                        // Save to local storage
                        await _localStorageService.SaveSelectedFinancialYearAsync(activeFinancialYear);
                        this.BeginInvoke(new Action(() => UpdateFinancialYearLabel(activeFinancialYear)));
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() => UpdateFinancialYearLabel(null)));
                    }
                }
                else
                {
                    this.BeginInvoke(new Action(() => UpdateFinancialYearLabel(null, "Invalid Company ID")));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating financial year display: {ex.Message}");
                this.BeginInvoke(new Action(() => UpdateFinancialYearLabel(null, "Error loading financial year")));
            }
        }

        private void UpdateFinancialYearLabel(FinancialYearModel? financialYear, string? errorMessage = null)
        {
            if (lblActiveFinancialYear != null)
            {
                if (financialYear != null)
                {
                    lblActiveFinancialYear.Text = $"Active Financial Year: {financialYear.YearLabel}";
                    lblActiveFinancialYear.ForeColor = Color.DarkBlue;
                    lblActiveFinancialYear.BackColor = Color.FromArgb(240, 248, 255);
                    lblActiveFinancialYear.Visible = true;
                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    lblActiveFinancialYear.Text = errorMessage;
                    lblActiveFinancialYear.ForeColor = Color.Red;
                    lblActiveFinancialYear.BackColor = Color.FromArgb(255, 240, 240);
                    lblActiveFinancialYear.Visible = true;
                }
                else
                {
                    lblActiveFinancialYear.Text = "No Active Financial Year";
                    lblActiveFinancialYear.ForeColor = Color.Orange;
                    lblActiveFinancialYear.BackColor = Color.FromArgb(255, 255, 240);
                    lblActiveFinancialYear.Visible = true;
                }
            }
        }



        private async void MainMDIForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // If user is closing the main form, clear all data and logout
            try
            {
                await _localStorageService.ClearAllDataAsync();
                _authService.Logout();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during form closing cleanup: {ex.Message}");
            }
            finally
            {
                _companyService?.Dispose();
                _financialYearService?.Dispose();
            }
        }

        // Navigation Helper Methods
        public void ShowNavigationPanel()
        {
            navigationPanel.Visible = true;
            isNavigationVisible = true;

            // Apply visual styling to make navigation prominent
            SetupNavigationStyling();

            // Always clear all highlights first to ensure clean state
            ClearAllButtonHighlights();

            // Focus the first button in Masters section and highlight it
            if (productsListButton.Visible)
            {
                HighlightButton(productsListButton);
                productsListButton.Focus();
                
                // Ensure the button is visible in the viewport
                productsListButton.BringToFront();
            }
        }

        public void HideNavigationPanel()
        {
            navigationPanel.Visible = false;
            isNavigationVisible = false;
        }

        private void ShowNavigationAndFocusSection(string sectionName)
        {
            ShowNavigationPanel();

            // Clear previous highlights and focus the first button in the specified section
            ClearAllButtonHighlights();

            if (sectionName == "masters" && productsListButton.Visible)
            {
                HighlightButton(productsListButton);
                productsListButton.Focus();
            }
            else if (sectionName == "transactions" && saleButton.Visible)
            {
                HighlightButton(saleButton);
                saleButton.Focus();
            }
            else if (sectionName == "reports" && stockReportButton.Visible)
            {
                HighlightButton(stockReportButton);
                stockReportButton.Focus();
            }
        }

        public void SetFocusToNavigation()
        {
            // Clear all highlights first to ensure clean state
            ClearAllButtonHighlights();
            ShowNavigationPanel();
        }

        private void CheckAndShowNavigationIfNoChildForms()
        {
            if (this.MdiChildren.Length == 0)
            {
                this.BeginInvoke(new Action(() =>
                {
                    fileMenu.ShowDropDown();
                    Console.WriteLine("No child forms open, File menu opened automatically");
                }));
            }
            else
            {
                // Check if the active child form is a company/product/ledger/tax-related form
                if (this.ActiveMdiChild is CompanyForm || 
                    this.ActiveMdiChild is CompanySelectForm || 
                    this.ActiveMdiChild is CompanyEditForm ||
                    this.ActiveMdiChild is ProductForm ||
                    this.ActiveMdiChild is ProductEditForm ||
                    this.ActiveMdiChild is LedgerListForm ||
                    this.ActiveMdiChild is LedgerEditForm ||
                    this.ActiveMdiChild is TaxListForm ||
                    this.ActiveMdiChild is TaxEditForm)
                {
                    // Hide navigation panel when company/product/ledger-related forms are active
                    HideNavigationPanel();
                }
                else
                {
                    // Show navigation panel for other forms
                    ShowNavigationPanel();
                }
            }
        }

        private void HandleChildFormActivation()
        {
            if (this.ActiveMdiChild is CompanyForm || 
                this.ActiveMdiChild is CompanySelectForm || 
                this.ActiveMdiChild is CompanyEditForm ||
                this.ActiveMdiChild is ProductForm ||
                this.ActiveMdiChild is ProductEditForm ||
                this.ActiveMdiChild is FinancialYearListForm ||
                this.ActiveMdiChild is FinancialYearEditForm ||
                this.ActiveMdiChild is FinancialYearSelectForm ||
                this.ActiveMdiChild is LedgerListForm ||
                this.ActiveMdiChild is LedgerEditForm ||
                this.ActiveMdiChild is TaxListForm ||
                this.ActiveMdiChild is TaxEditForm)
            {
                // Hide navigation panel when company/product/ledger-related forms are active
                HideNavigationPanel();
                
                // Ensure forms stay maximized when they're the active form
                if (this.ActiveMdiChild is CompanyForm)
                {
                    this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
                    
                    // Force the CompanyForm to maintain its maximized state
                    this.BeginInvoke(new Action(() =>
                    {
                        if (this.ActiveMdiChild is CompanyForm companyForm)
                        {
                            companyForm.WindowState = FormWindowState.Maximized;
                        }
                    }));
                }
                else if (this.ActiveMdiChild is ProductForm)
                {
                    this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
                    
                    // Force the ProductForm to maintain its maximized state
                    this.BeginInvoke(new Action(() =>
                    {
                        if (this.ActiveMdiChild is ProductForm productForm)
                        {
                            productForm.WindowState = FormWindowState.Maximized;
                        }
                    }));
                }
                else if (this.ActiveMdiChild is FinancialYearListForm)
                {
                    this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
                    
                    // Force the FinancialYearListForm to maintain its maximized state
                    this.BeginInvoke(new Action(() =>
                    {
                        if (this.ActiveMdiChild is FinancialYearListForm financialYearListForm)
                        {
                            financialYearListForm.WindowState = FormWindowState.Maximized;
                        }
                    }));
                }
            }
            else if (this.ActiveMdiChild != null)
            {
                // Show navigation panel for other forms
                ShowNavigationPanel();
            }
            else
            {
                // No child forms, show navigation panel
                ShowNavigationPanel();
            }
        }

        // Arrow Key Navigation Methods (across all buttons)
        private bool NavigateToPreviousButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            // Create a combined array of all navigation buttons
            var allButtons = new[]
            {
                // Masters buttons
                productsListButton, companyListButton, selectCompanyButton, financialYearListButton, accountsButton, taxButton,
                // Transactions buttons
                saleButton, purchaseButton, receiptButton, paymentButton, journalButton,
                // Reports buttons
                stockReportButton, taxReportButton, salesReportButton, purchaseReportButton, profitLossButton
            };

            var currentIndex = Array.IndexOf(allButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to previous button in the entire list, wrap around to last if at first
            var previousIndex = currentIndex == 0 ? allButtons.Length - 1 : currentIndex - 1;
            var previousButton = allButtons[previousIndex];

            HighlightButton(previousButton);
            previousButton.Focus();

            return true;
        }

        private bool NavigateToNextButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            // Create a combined array of all navigation buttons
            var allButtons = new[]
            {
                // Masters buttons
                productsListButton, companyListButton, selectCompanyButton, financialYearListButton, accountsButton, taxButton,
                // Transactions buttons
                saleButton, purchaseButton, receiptButton, paymentButton, journalButton,
                // Reports buttons
                stockReportButton, taxReportButton, salesReportButton, purchaseReportButton, profitLossButton
            };

            var currentIndex = Array.IndexOf(allButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to next button in the entire list, wrap around to first if at last
            var nextIndex = currentIndex == allButtons.Length - 1 ? 0 : currentIndex + 1;
            var nextButton = allButtons[nextIndex];

            HighlightButton(nextButton);
            nextButton.Focus();

            return true;
        }

        // Visual Styling Methods
        private void SetupNavigationStyling()
        {
            // Enhanced navigation panel styling
            navigationPanel.BackColor = Color.FromArgb(240, 240, 240);
            navigationPanel.BorderStyle = BorderStyle.FixedSingle;

            // Style main group box
            mainNavigationGroupBox.BackColor = Color.White;
            mainNavigationGroupBox.ForeColor = Color.FromArgb(0, 102, 204);
            mainNavigationGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Style all buttons with default appearance
            SetupButtonStyling();
        }

        private void SetupButtonStyling()
        {
            var allButtons = new[]
            {
                // Masters buttons
                productsListButton, companyListButton, selectCompanyButton, financialYearListButton, accountsButton, taxButton,
                // Transactions buttons
                saleButton, purchaseButton, receiptButton, paymentButton, journalButton,
                // Reports buttons
                stockReportButton, taxReportButton, salesReportButton, purchaseReportButton, profitLossButton
            };

            foreach (Button btn in allButtons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                btn.Cursor = Cursors.Hand;

                // Add hover effects
                btn.MouseEnter += Button_MouseEnter;
                btn.MouseLeave += Button_MouseLeave;
                btn.GotFocus += Button_GotFocus;
                btn.Click += Button_Click; // Add click event for highlighting
            }
        }

        private void HighlightButton(Button button)
        {
            // Clear previous highlights first
            ClearAllButtonHighlights();

            // Highlight the selected button with more prominent styling
            button.BackColor = Color.FromArgb(0, 102, 204); // Brighter blue
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 70, 140); // Darker border
            button.FlatAppearance.BorderSize = 2; // Thicker border
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            
            // Add a subtle glow effect
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 102, 204);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 84, 168);
        }

        private void ClearAllButtonHighlights()
        {
            var allButtons = new[]
            {
                // Masters buttons
                productsListButton, companyListButton, selectCompanyButton, financialYearListButton, accountsButton, taxButton,
                // Transactions buttons
                saleButton, purchaseButton, receiptButton, paymentButton, journalButton,
                // Reports buttons
                stockReportButton, taxReportButton, salesReportButton, purchaseReportButton, profitLossButton
            };

            foreach (Button btn in allButtons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.BorderSize = 1; // Reset border size
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            }
        }

        // Button Event Handlers for Visual Effects
        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BackColor != Color.FromArgb(0, 120, 215))
            {
                btn.BackColor = Color.FromArgb(230, 245, 255);
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            }
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BackColor != Color.FromArgb(0, 120, 215))
            {
                btn.BackColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void Button_GotFocus(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                HighlightButton(btn);
            }
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                HighlightButton(btn);
                btn.Focus(); // Ensure focus stays on the clicked button
            }
        }

        // Masters Section Button Handlers
        private void productsListButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenProductForm();
        }

        private void companyListButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenCompanyFormMaximized();
        }

        private void selectCompanyButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenCompanySelectForm();
        }

        private void financialYearListButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenFinancialYearListForm();
        }

        private void accountsButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenLedgerListForm();
        }

        private void taxButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTaxListForm();
        }

        // Transactions Section Button Handlers
        private void saleButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTransactionListForm();
        }

        private void purchaseButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTransactionListForm();
        }

        private void receiptButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTransactionListForm();
        }

        private void paymentButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTransactionListForm();
        }

        private void journalButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenTransactionListForm();
        }

        // Reports Section Button Handlers
        private void stockReportButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Stock Report feature will be implemented here.", "Stock Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void taxReportButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Tax Report feature will be implemented here.", "Tax Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void salesReportButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Sales Report feature will be implemented here.", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void purchaseReportButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Purchase Report feature will be implemented here.", "Purchase Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void profitLossButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Profit & Loss Report feature will be implemented here.", "Profit & Loss", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mainNavigationGroupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
