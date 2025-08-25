using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanyForm : Form
    {
        private readonly CompanyService _companyService;
        private readonly LocalStorageService _localStorageService;
        private ListBox lstCompanies = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private List<WinFormsApp1.Models.Company> _companies = new List<WinFormsApp1.Models.Company>();
        private WinFormsApp1.Models.Company? _selectedCompany;
        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalCount = 0;
        private int _totalPages = 0;

        public CompanyForm(CompanyService companyService)
        {
            _companyService = companyService;
            _localStorageService = new LocalStorageService();
            InitializeComponent();
            SetupForm();
            LoadCompanies();
        }

        private void InitializeComponent()
        {
            lstCompanies = new ListBox();
            btnNew = new Button();
            btnEdit = new Button();
            btnView = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            SuspendLayout();
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(600, 40);
            lblInstructions.Text = "Keyboard Navigation: ↑↓ to navigate, Enter to edit, V to view details, Insert for new, Delete to remove, F5 to refresh, Esc to close";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // lstCompanies
            // 
            lstCompanies.Location = new Point(12, 52);
            lstCompanies.Name = "lstCompanies";
            lstCompanies.Size = new Size(500, 350);
            lstCompanies.TabIndex = 0;
            lstCompanies.DisplayMember = "DisplayName";
            lstCompanies.SelectedIndexChanged += new EventHandler(lstCompanies_SelectedIndexChanged);
            lstCompanies.DoubleClick += new EventHandler(lstCompanies_DoubleClick);
            lstCompanies.KeyDown += new KeyEventHandler(lstCompanies_KeyDown);
            
            // 
            // btnNew
            // 
            btnNew.Location = new Point(530, 52);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(100, 30);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (Insert)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);
            
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(530, 92);
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
            btnView.Location = new Point(530, 132);
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
            btnDelete.Location = new Point(530, 172);
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
            btnRefresh.Location = new Point(530, 212);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "&Refresh (F5)";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 415);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(618, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // CompanyForm
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
            Controls.Add(lstCompanies);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "CompanyForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Company Management";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(CompanyForm_KeyDown);
            Load += new EventHandler(CompanyForm_Load);
            Resize += new EventHandler(CompanyForm_Resize);
            Activated += new EventHandler(CompanyForm_Activated);
            FormClosing += new FormClosingEventHandler(CompanyForm_FormClosing);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set focus to the list box for immediate keyboard navigation
            ActiveControl = lstCompanies;
            
            // Style the buttons for better visibility
            StyleButtons();
        }
        
        private void StyleButtons()
        {
            var buttons = new[] { btnNew, btnEdit, btnView, btnDelete, btnRefresh };
            
            foreach (var button in buttons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.FromArgb(240, 240, 240);
                button.ForeColor = Color.Black;
                button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                button.Cursor = Cursors.Hand;
                
                // Add hover effects
                button.MouseEnter += (s, e) => 
                {
                    if (s is Button btn)
                    {
                        btn.BackColor = Color.FromArgb(230, 245, 255);
                        btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
                    }
                };
                
                button.MouseLeave += (s, e) => 
                {
                    if (s is Button btn)
                    {
                        btn.BackColor = Color.FromArgb(240, 240, 240);
                        btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                    }
                };
            }
        }

        private void CompanyForm_Load(object? sender, EventArgs e)
        {
            // Open as maximized child form within MDI parent
            WindowState = FormWindowState.Maximized;
            
            // Resize controls to fit the maximized form
            ResizeControls();
            
            // Focus on the list box
            lstCompanies.Focus();
            
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
            
            // Reserve space for buttons on the right side (120px for buttons + 30px margin)
            int buttonAreaWidth = 150;
            int availableWidth = clientWidth - buttonAreaWidth - 50; // 50px for left margin
            int availableHeight = clientHeight - 100;
            
            // Ensure minimum width for the list
            if (availableWidth < 300)
            {
                availableWidth = 300;
            }
            
            // Resize the list box to use the available space
            lstCompanies.Size = new Size(availableWidth, availableHeight);
            
            // Position buttons on the right side, ensuring they're always visible
            int buttonX = availableWidth + 30; // 30px margin from list
            int buttonWidth = 120; // Slightly wider buttons for better visibility
            int buttonHeight = 35; // Slightly taller buttons
            
            btnNew.Location = new Point(buttonX, 52);
            btnNew.Size = new Size(buttonWidth, buttonHeight);
            
            btnEdit.Location = new Point(buttonX, 92);
            btnEdit.Size = new Size(buttonWidth, buttonHeight);
            
            btnView.Location = new Point(buttonX, 132);
            btnView.Size = new Size(buttonWidth, buttonHeight);
            
            btnDelete.Location = new Point(buttonX, 172);
            btnDelete.Size = new Size(buttonWidth, buttonHeight);
            
            btnRefresh.Location = new Point(buttonX, 212);
            btnRefresh.Size = new Size(buttonWidth, buttonHeight);
            
            // Reposition status label at the bottom
            lblStatus.Location = new Point(12, clientHeight - 35);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            // Resize instructions label
            lblInstructions.Size = new Size(clientWidth - 24, 40);
        }

        private void CompanyForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void CompanyForm_Activated(object? sender, EventArgs e)
        {
            // When CompanyForm is activated, ensure it's maximized and navigation is hidden
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
            
            // Force the form to stay maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void CompanyForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When CompanyForm is closing, ensure navigation panel is shown again
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
        }

        private async void LoadCompanies()
        {
            try
            {
                lblStatus.Text = "Loading companies...";
                lblStatus.ForeColor = Color.Blue;
                
                var paginatedResponse = await _companyService.GetCompanyListAsync(_currentPage, _pageSize);
                
                if (paginatedResponse != null)
                {
                    _companies = paginatedResponse.Data;
                    _totalCount = paginatedResponse.TotalCount;
                    _totalPages = paginatedResponse.TotalPages;
                    
                    Console.WriteLine($"Loaded {_companies.Count} companies (Page {_currentPage}/{_totalPages}, Total: {_totalCount}):");
                    foreach (var company in _companies.Take(5)) // Show first 5 for debugging
                    {
                        Console.WriteLine($"  - ID: '{company.Id}', Name: '{company.Name}', Code: '{company.Code}'");
                    }
                    
                    lstCompanies.DataSource = null;
                    lstCompanies.DataSource = _companies;
                    lstCompanies.DisplayMember = "DisplayName";
                    
                    if (_companies.Any())
                    {
                        lblStatus.Text = $"Page {_currentPage} of {_totalPages} - Showing {_companies.Count} of {_totalCount} companies";
                        lblStatus.ForeColor = Color.Green;
                        lstCompanies.SelectedIndex = 0;
                    }
                    else
                    {
                        lblStatus.Text = "No companies found";
                        lblStatus.ForeColor = Color.Orange;
                    }
                }
                else
                {
                    lblStatus.Text = "Failed to load companies";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading companies: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private async void lstCompanies_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedCompany = lstCompanies.SelectedItem as WinFormsApp1.Models.Company;
            btnEdit.Enabled = _selectedCompany != null;
            btnView.Enabled = _selectedCompany != null;
            btnDelete.Enabled = _selectedCompany != null;
            
            // Save selected company to local storage
            if (_selectedCompany != null)
            {
                try
                {
                    await _localStorageService.SaveSelectedCompanyAsync(_selectedCompany);
                    Console.WriteLine($"Selected company saved to local storage: {_selectedCompany.DisplayName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving selected company: {ex.Message}");
                }
            }
        }

        private void lstCompanies_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedCompany != null)
            {
                EditCompany();
            }
        }

        private void lstCompanies_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedCompany != null)
                    {
                        _ = Task.Run(async () => await EditCompany()); // Fire and forget for keyboard events
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.Insert:
                    NewCompany();
                    e.Handled = true;
                    break;
                    
                case Keys.V:
                    if (_selectedCompany != null)
                    {
                        _ = Task.Run(async () => await ViewCompany()); // Fire and forget for keyboard events
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.Delete:
                    if (_selectedCompany != null)
                    {
                        DeleteCompany();
                        e.Handled = true;
                    }
                    break;
                    

                    
                case Keys.F5:
                    LoadCompanies();
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
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

        private void CompanyForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
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
                    LoadCompanies();
                    e.Handled = true;
                    break;
                    
                case Keys.Insert:
                    NewCompany();
                    e.Handled = true;
                    break;
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            NewCompany();
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedCompany != null)
            {
                try
                {
                    await EditCompany();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing company: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedCompany != null)
            {
                try
                {
                    await ViewCompany();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error viewing company: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedCompany != null)
            {
                DeleteCompany();
            }
        }



        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadCompanies();
        }

        private void NewCompany()
        {
            // Check if CompanyEditForm is already open
            foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
            {
                if (childForm is CompanyEditForm editForm && !editForm.IsEditMode)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Create new company edit form as MDI child
            var companyEditForm = new CompanyEditForm(_companyService, new CountryService(), null)
            {
                MdiParent = this.MdiParent,
                Text = "New Company",
                WindowState = FormWindowState.Maximized
            };

            companyEditForm.Show();
            
            // Refresh the company list when the edit form is closed
            companyEditForm.FormClosed += (s, e) => LoadCompanies();
        }

        private async Task EditCompany()
        {
            if (_selectedCompany != null)
            {
                Console.WriteLine($"EditCompany called for: ID='{_selectedCompany.Id}', Name='{_selectedCompany.Name}'");
                try
                {
                    // Check if CompanyEditForm is already open for this company
                    foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
                    {
                        if (childForm is CompanyEditForm editForm && editForm.IsEditMode && editForm.CompanyId == _selectedCompany.Id)
                        {
                            childForm.BringToFront();
                            childForm.Activate();
                            return;
                        }
                    }

                    // Pass the selected company to the edit form
                    // The edit form will now fetch fresh data using GetCompanyByIdAsync
                    var companyEditForm = new CompanyEditForm(_companyService, new CountryService(), _selectedCompany)
                    {
                        MdiParent = this.MdiParent,
                        Text = $"Edit Company - {_selectedCompany.DisplayName}",
                        WindowState = FormWindowState.Maximized
                    };

                    companyEditForm.Show();
                    
                    // Refresh the company list when the edit form is closed
                    companyEditForm.FormClosed += (s, e) => LoadCompanies();
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error opening edit form: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Error loading company data: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task ViewCompany()
        {
            if (_selectedCompany != null)
            {
                Console.WriteLine($"ViewCompany called for: ID='{_selectedCompany.Id}', Name='{_selectedCompany.Name}'");
                try
                {
                    lblStatus.Text = "Loading company data...";
                    lblStatus.ForeColor = Color.Blue;

                    // Get the full company data from API using GetEditCompanyByIdAsync
                    Console.WriteLine($"Attempting to parse company ID: '{_selectedCompany.Id}'");
                    
                    var editCompanyModel = await _companyService.GetEditCompanyByIdAsync(Guid.Parse(_selectedCompany.Id));
                    
                    if (editCompanyModel != null)
                    {
                        lblStatus.Text = "Company data loaded successfully";
                        lblStatus.ForeColor = Color.Green;
                        
                        // Show company details form
                        var companyDetailsForm = new CompanyDetailsForm(editCompanyModel, _companyService);
                        if (companyDetailsForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadCompanies(); // Refresh if user edited the company from details form
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Failed to load company data from server";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show("Could not load company data from the server. Please try again.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error loading company data: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Error loading company data: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void DeleteCompany()
        {
            if (_selectedCompany == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete company '{_selectedCompany.DisplayName}'?",
                "Delete Company",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    lblStatus.Text = "Deleting company...";
                    lblStatus.ForeColor = Color.Blue;

                    var success = await _companyService.DeleteCompanyAsync(Guid.Parse(_selectedCompany.Id));
                    
                    if (success)
                    {
                        lblStatus.Text = "Company deleted successfully";
                        lblStatus.ForeColor = Color.Green;
                        LoadCompanies();
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete company";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error deleting company: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }
    }
}
