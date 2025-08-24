using WinFormsApp1.Forms.Company;
using WinFormsApp1.Forms.Dashboard;
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
        private MenuStrip menuStrip = null!;
        private ToolStripMenuItem fileMenu = null!;
        private ToolStripMenuItem companyMenuItem = null!;
        private ToolStripMenuItem selectCompanyMenuItem = null!;
        private ToolStripMenuItem debugApiMenuItem = null!;
        private ToolStripMenuItem logoutMenuItem = null!;
        private ToolStripMenuItem exitMenuItem = null!;
        private ToolStripMenuItem windowMenu = null!;
        private ToolStripMenuItem cascadeMenuItem = null!;
        private ToolStripMenuItem tileHorizontalMenuItem = null!;
        private ToolStripMenuItem tileVerticalMenuItem = null!;
        private ToolStripMenuItem closeAllMenuItem = null!;

        // Navigation Panel Controls
        private Panel navigationPanel = null!;
        private GroupBox mainNavigationGroupBox = null!;
        private Label mastersLabel = null!;
        private Label transactionsLabel = null!;
        private Label reportsLabel = null!;
        
        // Masters Section Buttons
        private Button productsListButton = null!;
        private Button companyListButton = null!;
        private Button customersButton = null!;
        private Button suppliersButton = null!;
        private Button accountsButton = null!;
        
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
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.fileMenu = new ToolStripMenuItem();
            this.companyMenuItem = new ToolStripMenuItem();
            this.selectCompanyMenuItem = new ToolStripMenuItem();
            this.debugApiMenuItem = new ToolStripMenuItem();
            this.logoutMenuItem = new ToolStripMenuItem();
            this.exitMenuItem = new ToolStripMenuItem();
            this.windowMenu = new ToolStripMenuItem();
            this.cascadeMenuItem = new ToolStripMenuItem();
            this.tileHorizontalMenuItem = new ToolStripMenuItem();
            this.tileVerticalMenuItem = new ToolStripMenuItem();
            this.closeAllMenuItem = new ToolStripMenuItem();
            
            // Initialize navigation panel controls
            this.navigationPanel = new Panel();
            this.mainNavigationGroupBox = new GroupBox();
            this.mastersLabel = new Label();
            this.transactionsLabel = new Label();
            this.reportsLabel = new Label();
            
            // Initialize Masters section buttons
            this.productsListButton = new Button();
            this.companyListButton = new Button();
            this.customersButton = new Button();
            this.suppliersButton = new Button();
            this.accountsButton = new Button();
            
            // Initialize Transactions section buttons
            this.saleButton = new Button();
            this.purchaseButton = new Button();
            this.receiptButton = new Button();
            this.paymentButton = new Button();
            this.journalButton = new Button();
            
            // Initialize Reports section buttons
            this.stockReportButton = new Button();
            this.taxReportButton = new Button();
            this.salesReportButton = new Button();
            this.purchaseReportButton = new Button();
            this.profitLossButton = new Button();
            
            this.menuStrip.SuspendLayout();
            this.navigationPanel.SuspendLayout();
            this.mainNavigationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
            this.fileMenu,
            this.windowMenu});
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(800, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
            this.selectCompanyMenuItem,
            this.companyMenuItem,
            this.debugApiMenuItem,
            new ToolStripSeparator(),
            this.logoutMenuItem,
            this.exitMenuItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new Size(37, 20);
            this.fileMenu.Text = "File";
            
            // 
            // selectCompanyMenuItem
            // 
            this.selectCompanyMenuItem.Name = "selectCompanyMenuItem";
            this.selectCompanyMenuItem.Size = new Size(180, 22);
            this.selectCompanyMenuItem.Text = "&Select Company";
            this.selectCompanyMenuItem.Click += new EventHandler(this.selectCompanyMenuItem_Click);
            
            // 
            // companyMenuItem
            // 
            this.companyMenuItem.Name = "companyMenuItem";
            this.companyMenuItem.Size = new Size(180, 22);
            this.companyMenuItem.Text = "&Manage Companies";
            this.companyMenuItem.Click += new EventHandler(this.companyMenuItem_Click);
            
        
            // 
            // logoutMenuItem
            // 
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new Size(112, 22);
            this.logoutMenuItem.Text = "Logout";
            this.logoutMenuItem.Click += new EventHandler(this.logoutMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new Size(112, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new EventHandler(this.exitMenuItem_Click);
            // 
            // windowMenu
            // 
            this.windowMenu.DropDownItems.AddRange(new ToolStripItem[] {
            this.cascadeMenuItem,
            this.tileHorizontalMenuItem,
            this.tileVerticalMenuItem,
            this.closeAllMenuItem});
            this.windowMenu.Name = "windowMenu";
            this.windowMenu.Size = new Size(63, 20);
            this.windowMenu.Text = "Window";
            // 
            // cascadeMenuItem
            // 
            this.cascadeMenuItem.Name = "cascadeMenuItem";
            this.cascadeMenuItem.Size = new Size(151, 22);
            this.cascadeMenuItem.Text = "Cascade";
            this.cascadeMenuItem.Click += new EventHandler(this.cascadeMenuItem_Click);
            // 
            // tileHorizontalMenuItem
            // 
            this.tileHorizontalMenuItem.Name = "tileHorizontalMenuItem";
            this.tileHorizontalMenuItem.Size = new Size(151, 22);
            this.tileHorizontalMenuItem.Text = "Tile Horizontal";
            this.tileHorizontalMenuItem.Click += new EventHandler(this.tileHorizontalMenuItem_Click);
            // 
            // tileVerticalMenuItem
            // 
            this.tileVerticalMenuItem.Name = "tileVerticalMenuItem";
            this.tileVerticalMenuItem.Size = new Size(151, 22);
            this.tileVerticalMenuItem.Text = "Tile Vertical";
            this.tileVerticalMenuItem.Click += new EventHandler(this.tileVerticalMenuItem_Click);
            // 
            // closeAllMenuItem
            // 
            this.closeAllMenuItem.Name = "closeAllMenuItem";
            this.closeAllMenuItem.Size = new Size(151, 22);
            this.closeAllMenuItem.Text = "Close All";
            this.closeAllMenuItem.Click += new EventHandler(this.closeAllMenuItem_Click);
            
            // 
            // navigationPanel
            // 
            this.navigationPanel.BackColor = Color.FromArgb(245, 245, 245);
            this.navigationPanel.BorderStyle = BorderStyle.FixedSingle;
            this.navigationPanel.Controls.Add(this.mainNavigationGroupBox);
            this.navigationPanel.Dock = DockStyle.Left;
            this.navigationPanel.Location = new Point(0, 24);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.Size = new Size(300, 576);
            this.navigationPanel.TabIndex = 1;
            this.navigationPanel.Padding = new Padding(5);
            
            // 
            // mainNavigationGroupBox
            // 
            this.mainNavigationGroupBox.Controls.Add(this.mastersLabel);
            this.mainNavigationGroupBox.Controls.Add(this.productsListButton);
            this.mainNavigationGroupBox.Controls.Add(this.companyListButton);
            this.mainNavigationGroupBox.Controls.Add(this.customersButton);
            this.mainNavigationGroupBox.Controls.Add(this.suppliersButton);
            this.mainNavigationGroupBox.Controls.Add(this.accountsButton);
            this.mainNavigationGroupBox.Controls.Add(this.transactionsLabel);
            this.mainNavigationGroupBox.Controls.Add(this.saleButton);
            this.mainNavigationGroupBox.Controls.Add(this.purchaseButton);
            this.mainNavigationGroupBox.Controls.Add(this.receiptButton);
            this.mainNavigationGroupBox.Controls.Add(this.paymentButton);
            this.mainNavigationGroupBox.Controls.Add(this.journalButton);
            this.mainNavigationGroupBox.Controls.Add(this.reportsLabel);
            this.mainNavigationGroupBox.Controls.Add(this.stockReportButton);
            this.mainNavigationGroupBox.Controls.Add(this.taxReportButton);
            this.mainNavigationGroupBox.Controls.Add(this.salesReportButton);
            this.mainNavigationGroupBox.Controls.Add(this.purchaseReportButton);
            this.mainNavigationGroupBox.Controls.Add(this.profitLossButton);
            this.mainNavigationGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.mainNavigationGroupBox.Location = new Point(10, 10);
            this.mainNavigationGroupBox.Name = "mainNavigationGroupBox";
            this.mainNavigationGroupBox.Size = new Size(280, 625);
            this.mainNavigationGroupBox.TabIndex = 0;
            this.mainNavigationGroupBox.TabStop = false;
            this.mainNavigationGroupBox.Text = "APPLICATION NAVIGATION";
            
            // 
            // mastersLabel
            // 
            this.mastersLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.mastersLabel.ForeColor = Color.FromArgb(0, 102, 204);
            this.mastersLabel.Location = new Point(8, 25);
            this.mastersLabel.Name = "mastersLabel";
            this.mastersLabel.Size = new Size(150, 20);
            this.mastersLabel.TabIndex = 0;
            this.mastersLabel.Text = "MASTERS (Alt+M)";
            
            // 
            // productsListButton
            // 
            this.productsListButton.Font = new Font("Segoe UI", 9F);
            this.productsListButton.Location = new Point(8, 50);
            this.productsListButton.Name = "productsListButton";
            this.productsListButton.Size = new Size(264, 28);
            this.productsListButton.TabIndex = 1;
            this.productsListButton.Text = "&Products List (F2)";
            this.productsListButton.UseVisualStyleBackColor = true;
            this.productsListButton.Click += new EventHandler(this.productsListButton_Click);
            
            // 
            // companyListButton
            // 
            this.companyListButton.Font = new Font("Segoe UI", 9F);
            this.companyListButton.Location = new Point(8, 84);
            this.companyListButton.Name = "companyListButton";
            this.companyListButton.Size = new Size(264, 28);
            this.companyListButton.TabIndex = 1;
            this.companyListButton.Text = "&Company List (F3)";
            this.companyListButton.UseVisualStyleBackColor = true;
            this.companyListButton.Click += new EventHandler(this.companyListButton_Click);
            
            // 
            // customersButton
            // 
            this.customersButton.Font = new Font("Segoe UI", 9F);
            this.customersButton.Location = new Point(8, 118);
            this.customersButton.Name = "customersButton";
            this.customersButton.Size = new Size(264, 28);
            this.customersButton.TabIndex = 2;
            this.customersButton.Text = "C&ustomers (F4)";
            this.customersButton.UseVisualStyleBackColor = true;
            this.customersButton.Click += new EventHandler(this.customersButton_Click);
            
            // 
            // suppliersButton
            // 
            this.suppliersButton.Font = new Font("Segoe UI", 9F);
            this.suppliersButton.Location = new Point(8, 152);
            this.suppliersButton.Name = "suppliersButton";
            this.suppliersButton.Size = new Size(264, 28);
            this.suppliersButton.TabIndex = 3;
            this.suppliersButton.Text = "&Suppliers (F5)";
            this.suppliersButton.UseVisualStyleBackColor = true;
            this.suppliersButton.Click += new EventHandler(this.suppliersButton_Click);
            
            // 
            // accountsButton
            // 
            this.accountsButton.Font = new Font("Segoe UI", 9F);
            this.accountsButton.Location = new Point(8, 186);
            this.accountsButton.Name = "accountsButton";
            this.accountsButton.Size = new Size(264, 28);
            this.accountsButton.TabIndex = 4;
            this.accountsButton.Text = "&Accounts (F6)";
            this.accountsButton.UseVisualStyleBackColor = true;
            this.accountsButton.Click += new EventHandler(this.accountsButton_Click);
            
            // 
            // transactionsLabel
            // 
            this.transactionsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.transactionsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            this.transactionsLabel.Location = new Point(8, 225);
            this.transactionsLabel.Name = "transactionsLabel";
            this.transactionsLabel.Size = new Size(200, 20);
            this.transactionsLabel.TabIndex = 5;
            this.transactionsLabel.Text = "TRANSACTIONS (Alt+T)";
            
            // 
            // saleButton
            // 
            this.saleButton.Font = new Font("Segoe UI", 9F);
            this.saleButton.Location = new Point(8, 250);
            this.saleButton.Name = "saleButton";
            this.saleButton.Size = new Size(264, 28);
            this.saleButton.TabIndex = 6;
            this.saleButton.Text = "&Sale (F7)";
            this.saleButton.UseVisualStyleBackColor = true;
            this.saleButton.Click += new EventHandler(this.saleButton_Click);
            
            // 
            // purchaseButton
            // 
            this.purchaseButton.Font = new Font("Segoe UI", 9F);
            this.purchaseButton.Location = new Point(8, 284);
            this.purchaseButton.Name = "purchaseButton";
            this.purchaseButton.Size = new Size(264, 28);
            this.purchaseButton.TabIndex = 7;
            this.purchaseButton.Text = "&Purchase (F8)";
            this.purchaseButton.UseVisualStyleBackColor = true;
            this.purchaseButton.Click += new EventHandler(this.purchaseButton_Click);
            
            // 
            // receiptButton
            // 
            this.receiptButton.Font = new Font("Segoe UI", 9F);
            this.receiptButton.Location = new Point(8, 318);
            this.receiptButton.Name = "receiptButton";
            this.receiptButton.Size = new Size(264, 28);
            this.receiptButton.TabIndex = 8;
            this.receiptButton.Text = "&Receipt (F9)";
            this.receiptButton.UseVisualStyleBackColor = true;
            this.receiptButton.Click += new EventHandler(this.receiptButton_Click);
            
            // 
            // paymentButton
            // 
            this.paymentButton.Font = new Font("Segoe UI", 9F);
            this.paymentButton.Location = new Point(8, 352);
            this.paymentButton.Name = "paymentButton";
            this.paymentButton.Size = new Size(264, 28);
            this.paymentButton.TabIndex = 9;
            this.paymentButton.Text = "Pa&yment (F10)";
            this.paymentButton.UseVisualStyleBackColor = true;
            this.paymentButton.Click += new EventHandler(this.paymentButton_Click);
            
            // 
            // journalButton
            // 
            this.journalButton.Font = new Font("Segoe UI", 9F);
            this.journalButton.Location = new Point(8, 386);
            this.journalButton.Name = "journalButton";
            this.journalButton.Size = new Size(264, 28);
            this.journalButton.TabIndex = 10;
            this.journalButton.Text = "&Journal (F11)";
            this.journalButton.UseVisualStyleBackColor = true;
            this.journalButton.Click += new EventHandler(this.journalButton_Click);
            
            // 
            // reportsLabel
            // 
            this.reportsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.reportsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            this.reportsLabel.Location = new Point(8, 425);
            this.reportsLabel.Name = "reportsLabel";
            this.reportsLabel.Size = new Size(150, 20);
            this.reportsLabel.TabIndex = 11;
            this.reportsLabel.Text = "REPORTS (Alt+R)";
            
            // 
            // stockReportButton
            // 
            this.stockReportButton.Font = new Font("Segoe UI", 9F);
            this.stockReportButton.Location = new Point(8, 450);
            this.stockReportButton.Name = "stockReportButton";
            this.stockReportButton.Size = new Size(264, 28);
            this.stockReportButton.TabIndex = 12;
            this.stockReportButton.Text = "St&ock Report (Ctrl+F1)";
            this.stockReportButton.UseVisualStyleBackColor = true;
            this.stockReportButton.Click += new EventHandler(this.stockReportButton_Click);
            
            // 
            // taxReportButton
            // 
            this.taxReportButton.Font = new Font("Segoe UI", 9F);
            this.taxReportButton.Location = new Point(8, 484);
            this.taxReportButton.Name = "taxReportButton";
            this.taxReportButton.Size = new Size(264, 28);
            this.taxReportButton.TabIndex = 13;
            this.taxReportButton.Text = "Ta&x Report (Ctrl+F2)";
            this.taxReportButton.UseVisualStyleBackColor = true;
            this.taxReportButton.Click += new EventHandler(this.taxReportButton_Click);
            
            // 
            // salesReportButton
            // 
            this.salesReportButton.Font = new Font("Segoe UI", 9F);
            this.salesReportButton.Location = new Point(8, 518);
            this.salesReportButton.Name = "salesReportButton";
            this.salesReportButton.Size = new Size(264, 28);
            this.salesReportButton.TabIndex = 14;
            this.salesReportButton.Text = "Sales &Report (Ctrl+F3)";
            this.salesReportButton.UseVisualStyleBackColor = true;
            this.salesReportButton.Click += new EventHandler(this.salesReportButton_Click);
            
            // 
            // purchaseReportButton
            // 
            this.purchaseReportButton.Font = new Font("Segoe UI", 9F);
            this.purchaseReportButton.Location = new Point(8, 552);
            this.purchaseReportButton.Name = "purchaseReportButton";
            this.purchaseReportButton.Size = new Size(264, 28);
            this.purchaseReportButton.TabIndex = 15;
            this.purchaseReportButton.Text = "Purchase Re&port (Ctrl+F4)";
            this.purchaseReportButton.UseVisualStyleBackColor = true;
            this.purchaseReportButton.Click += new EventHandler(this.purchaseReportButton_Click);
            
            // 
            // profitLossButton
            // 
            this.profitLossButton.Font = new Font("Segoe UI", 9F);
            this.profitLossButton.Location = new Point(8, 586);
            this.profitLossButton.Name = "profitLossButton";
            this.profitLossButton.Size = new Size(264, 28);
            this.profitLossButton.TabIndex = 16;
            this.profitLossButton.Text = "Profit && &Loss (Ctrl+F5)";
            this.profitLossButton.UseVisualStyleBackColor = true;
            this.profitLossButton.Click += new EventHandler(this.profitLossButton_Click);
            
            // 
            // MainMDIForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this.navigationPanel);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.KeyPreview = true;
            this.Name = "MainMDIForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Main Application - Esc=File Menu | ↑↓=Navigate All | Alt+M/T/R=Jump Sections | F1=Help";
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += new FormClosingEventHandler(this.MainMDIForm_FormClosing);
            this.KeyDown += new KeyEventHandler(this.MainMDIForm_KeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.navigationPanel.ResumeLayout(false);
            this.mainNavigationGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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
            this.Load += (s, e) => SetFocusToNavigation();
            
            // Monitor child form closing events
            this.MdiChildActivate += (s, e) => CheckAndShowNavigationIfNoChildForms();
        }

        private void InitializeNavigationOrder()
        {
            // Define a single array for all navigation buttons for easy arrow key navigation
            mastersButtons = new Button[]
            {
                productsListButton,
                companyListButton,
                customersButton,
                suppliersButton,
                accountsButton,
                saleButton,
                purchaseButton,
                receiptButton,
                paymentButton,
                journalButton,
                stockReportButton,
                taxReportButton,
                salesReportButton,
                purchaseReportButton,
                profitLossButton
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
                    // Company is already selected, show navigation panel
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
            // Check if Dashboard form is already open
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm is DashboardForm)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new dashboard form
            var dashboardForm = new DashboardForm(_localStorageService, _companyService)
            {
                MdiParent = this,
                Text = "Company Dashboard"
            };
            
            dashboardForm.Show();
        }

        private void logoutMenuItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?\n\nThis will clear your saved login and require you to log in again next time.", "Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                _authService.Logout();
                this.Close();
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

     

        private void MainMDIForm_KeyDown(object? sender, KeyEventArgs e)
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
                    
                case Keys.S when e.Alt: // Alt+S for Select Company
                    OpenCompanySelectForm();
                    e.Handled = true;
                    break;
                    
                case Keys.D when e.Alt: // Alt+D for Dashboard
                    CreateDashboardForm();
                    e.Handled = true;
                    break;
                    
                case Keys.C when e.Alt: // Alt+C for Manage Companies
                    OpenCompanyForm();
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
                    customersButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F5 when !e.Control && !e.Alt:
                    suppliersButton_Click(null, EventArgs.Empty);
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
                        
                        // If this was the last child form, open file menu
                        if (this.MdiChildren.Length == 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                fileMenu.ShowDropDown();
                                Console.WriteLine("Last MDI form closed, File menu opened");
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
                        
                        // If this was the last child form, open file menu
                        if (this.MdiChildren.Length == 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                fileMenu.ShowDropDown();
                                Console.WriteLine("Last MDI form closed with Ctrl+W, File menu opened");
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
            OpenCompanyForm();
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
        }

        private void OpenCompanySelectForm()
        {
            var companySelectForm = new CompanySelectForm(_companyService, _localStorageService);
            if (companySelectForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the title to show selected company
                UpdateTitleWithSelectedCompany();
                
                // Show dashboard for the selected company
                CreateDashboardForm();
                
                // Show a brief message about the company change
                var selectedCompany = _localStorageService.GetSelectedCompanyAsync().Result;
                if (selectedCompany != null)
                {
                    MessageBox.Show($"Company changed to: {selectedCompany.DisplayName}\n\nDashboard opened for the selected company.", 
                        "Company Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void UpdateTitleWithSelectedCompany()
        {
            try
            {
                var selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                if (selectedCompany != null)
                {
                    this.Text = $"Main Application - {selectedCompany.DisplayName} - Alt+F=File Menu, Alt+S=Select Company, Alt+C=Manage Companies";
                }
                else
                {
                    this.Text = "Main Application - Alt+F=File Menu, Alt+D=Dashboard, Alt+S=Select Company, Alt+C=Manage Companies, Alt+F4=Exit, F1=Help";
                }
            }
            catch
            {
                // If there's an error, just use the default title
                this.Text = "Main Application - Alt+F=File Menu, Alt+D=Dashboard, Alt+S=Select Company, Alt+C=Manage Companies, Alt+F4=Exit, F1=Help";
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

MASTERS SECTION:
• F2 - Products List
• F3 - Company List
• F4 - Customers
• F5 - Suppliers
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
• Alt+S - Select Company
• Alt+C - Manage Companies
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



        private void MainMDIForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // If user is closing the main form, logout and exit
            _authService.Logout();
            _companyService?.Dispose();
        }

        // Navigation Helper Methods
        private void ShowNavigationPanel()
        {
            navigationPanel.Visible = true;
            isNavigationVisible = true;
            
            // Apply visual styling to make navigation prominent
            SetupNavigationStyling();
            
            // Focus the first button in Masters section and highlight it
            if (productsListButton.Visible)
            {
                HighlightButton(productsListButton);
                productsListButton.Focus();
            }
        }

        private void HideNavigationPanel()
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

        private void SetFocusToNavigation()
        {
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
        }

        // Arrow Key Navigation Methods (across all buttons)
        private bool NavigateToPreviousButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            var currentIndex = Array.IndexOf(mastersButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to previous button in the entire list, wrap around to last if at first
            var previousIndex = currentIndex == 0 ? mastersButtons.Length - 1 : currentIndex - 1;
            var previousButton = mastersButtons[previousIndex];
            
            HighlightButton(previousButton);
            previousButton.Focus();
            
            return true;
        }

        private bool NavigateToNextButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            var currentIndex = Array.IndexOf(mastersButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to next button in the entire list, wrap around to first if at last
            var nextIndex = currentIndex == mastersButtons.Length - 1 ? 0 : currentIndex + 1;
            var nextButton = mastersButtons[nextIndex];
            
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
                productsListButton, companyListButton, customersButton, suppliersButton, accountsButton,
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
            }
        }

        private void HighlightButton(Button button)
        {
            // Clear previous highlights first
            ClearAllButtonHighlights();
            
            // Highlight the selected button
            button.BackColor = Color.FromArgb(0, 120, 215);
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 84, 153);
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void ClearAllButtonHighlights()
        {
            var allButtons = new[]
            {
                // Masters buttons
                productsListButton, companyListButton, customersButton, suppliersButton, accountsButton,
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

        // Masters Section Button Handlers
        private void productsListButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Products List feature will be implemented here.", "Products List", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void companyListButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            OpenCompanyForm();
        }

        private void customersButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Customers feature will be implemented here.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void suppliersButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Suppliers feature will be implemented here.", "Suppliers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void accountsButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Accounts feature will be implemented here.", "Accounts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Transactions Section Button Handlers
        private void saleButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Sale transaction feature will be implemented here.", "Sale", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void purchaseButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Purchase transaction feature will be implemented here.", "Purchase", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void receiptButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Receipt transaction feature will be implemented here.", "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void paymentButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Payment transaction feature will be implemented here.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void journalButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn) HighlightButton(btn);
            MessageBox.Show("Journal entry feature will be implemented here.", "Journal", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
