using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanySelectForm : Form
    {
        private readonly CompanyService _companyService;
        private readonly LocalStorageService _localStorageService;
        private ComboBox cmbCompanies = null!;
        private Button btnSelect = null!;
        private Button btnCancel = null!;
        private Button btnRefresh = null!;
        private Label lblInstructions = null!;
        private Label lblCurrentSelection = null!;
        private Label lblStatus = null!;
        private GroupBox grpCompanyDetails = null!;
        private Label lblCompanyName = null!;
        private Label lblCompanyCode = null!;
        private Label lblCompanyAddress = null!;
        private Label lblCompanyPhone = null!;
        private Label lblCompanyEmail = null!;
        private List<SelectCompanyModel> _companies = new List<SelectCompanyModel>();
        private SelectCompanyModel? _selectedCompany;
        private WinFormsApp1.Models.Company? _currentStoredCompany;

        public CompanySelectForm(CompanyService companyService, LocalStorageService localStorageService)
        {
            _companyService = companyService;
            _localStorageService = localStorageService;
            InitializeComponent();
            SetupForm();
            LoadCurrentSelection();
            LoadCompanies();
        }

        private void InitializeComponent()
        {
            lblInstructions = new Label();
            lblCurrentSelection = new Label();
            cmbCompanies = new ComboBox();
            btnSelect = new Button();
            btnCancel = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            grpCompanyDetails = new GroupBox();
            lblCompanyName = new Label();
            lblCompanyCode = new Label();
            lblCompanyAddress = new Label();
            lblCompanyPhone = new Label();
            lblCompanyEmail = new Label();
            SuspendLayout();

            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(550, 40);
            lblInstructions.Text = "Select Company: ↑↓ or type to select, Enter to confirm, F5 to refresh, Esc to cancel";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);

            // 
            // lblCurrentSelection
            // 
            lblCurrentSelection.Location = new Point(12, 55);
            lblCurrentSelection.Name = "lblCurrentSelection";
            lblCurrentSelection.Size = new Size(550, 20);
            lblCurrentSelection.Text = "Current Selection: None";
            lblCurrentSelection.ForeColor = Color.DarkGreen;
            lblCurrentSelection.Font = new Font("Arial", 9, FontStyle.Bold);

            // 
            // cmbCompanies
            // 
            cmbCompanies.Location = new Point(12, 85);
            cmbCompanies.Name = "cmbCompanies";
            cmbCompanies.Size = new Size(400, 23);
            cmbCompanies.TabIndex = 0;
            cmbCompanies.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompanies.DisplayMember = "DisplayName";
            cmbCompanies.SelectedIndexChanged += new EventHandler(cmbCompanies_SelectedIndexChanged);
            cmbCompanies.KeyDown += new KeyEventHandler(cmbCompanies_KeyDown);

            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(430, 84);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(80, 25);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "&Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnSelect.Enabled = false;

            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(520, 84);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(60, 25);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "&Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);

            // 
            // grpCompanyDetails
            // 
            grpCompanyDetails.Location = new Point(12, 125);
            grpCompanyDetails.Name = "grpCompanyDetails";
            grpCompanyDetails.Size = new Size(568, 200);
            grpCompanyDetails.TabIndex = 3;
            grpCompanyDetails.TabStop = false;
            grpCompanyDetails.Text = "Company Details";

            // 
            // lblCompanyName
            // 
            lblCompanyName.Location = new Point(15, 25);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new Size(540, 25);
            lblCompanyName.Text = "Name: ";
            lblCompanyName.Font = new Font("Arial", 10, FontStyle.Bold);

            // 
            // lblCompanyCode
            // 
            lblCompanyCode.Location = new Point(15, 50);
            lblCompanyCode.Name = "lblCompanyCode";
            lblCompanyCode.Size = new Size(540, 20);
            lblCompanyCode.Text = "Code: ";

            // 
            // lblCompanyAddress
            // 
            lblCompanyAddress.Location = new Point(15, 75);
            lblCompanyAddress.Name = "lblCompanyAddress";
            lblCompanyAddress.Size = new Size(540, 60);
            lblCompanyAddress.Text = "Address: ";

            // 
            // lblCompanyPhone
            // 
            lblCompanyPhone.Location = new Point(15, 140);
            lblCompanyPhone.Name = "lblCompanyPhone";
            lblCompanyPhone.Size = new Size(260, 20);
            lblCompanyPhone.Text = "Phone: ";

            // 
            // lblCompanyEmail
            // 
            lblCompanyEmail.Location = new Point(15, 165);
            lblCompanyEmail.Name = "lblCompanyEmail";
            lblCompanyEmail.Size = new Size(540, 20);
            lblCompanyEmail.Text = "Email: ";

            // Add labels to group box
            grpCompanyDetails.Controls.Add(lblCompanyName);
            grpCompanyDetails.Controls.Add(lblCompanyCode);
            grpCompanyDetails.Controls.Add(lblCompanyAddress);
            grpCompanyDetails.Controls.Add(lblCompanyPhone);
            grpCompanyDetails.Controls.Add(lblCompanyEmail);

            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(430, 340);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 345);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(400, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // CompanySelectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(grpCompanyDetails);
            Controls.Add(btnRefresh);
            Controls.Add(btnSelect);
            Controls.Add(cmbCompanies);
            Controls.Add(lblCurrentSelection);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "CompanySelectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Company";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(CompanySelectForm_KeyDown);
            Load += new EventHandler(CompanySelectForm_Load);
            Resize += new EventHandler(CompanySelectForm_Resize);
            Activated += new EventHandler(CompanySelectForm_Activated);
            FormClosing += new FormClosingEventHandler(CompanySelectForm_FormClosing);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set default and cancel buttons
            AcceptButton = btnSelect;
            CancelButton = btnCancel;
            
            // Clear company details initially
            ClearCompanyDetails();
        }

        private void CompanySelectForm_Load(object? sender, EventArgs e)
        {
            // Ensure the form opens maximized
            WindowState = FormWindowState.Maximized;
            
            // Resize controls to fit the maximized form
            ResizeControls();
            
            cmbCompanies.Focus();
            
            // Hide MDI navigation panel when this form is maximized
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }
        
        private void CompanySelectForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }
        
        private void CompanySelectForm_Activated(object? sender, EventArgs e)
        {
            // When CompanySelectForm is activated, ensure it's maximized and navigation is hidden
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            // Hide navigation panel when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }
        
        private void CompanySelectForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When CompanySelectForm is closing, ensure navigation panel is shown again
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
        }
        
        private void ResizeControls()
        {
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            // Resize and reposition controls for maximized form
            lblInstructions.Size = new Size(clientWidth - 24, 40);
            
            lblCurrentSelection.Location = new Point(12, 55);
            lblCurrentSelection.Size = new Size(clientWidth - 24, 25);
            
            cmbCompanies.Location = new Point(12, 85);
            cmbCompanies.Size = new Size(clientWidth - 200, 30);
            
            btnSelect.Location = new Point(clientWidth - 180, 84);
            btnSelect.Size = new Size(80, 32);
            
            btnRefresh.Location = new Point(clientWidth - 90, 84);
            btnRefresh.Size = new Size(80, 32);
            
            grpCompanyDetails.Location = new Point(12, 125);
            grpCompanyDetails.Size = new Size(clientWidth - 24, 200);
            
            btnCancel.Location = new Point(clientWidth - 100, clientHeight - 50);
            btnCancel.Size = new Size(80, 35);
            
            lblStatus.Location = new Point(12, clientHeight - 45);
            lblStatus.Size = new Size(clientWidth - 120, 25);
        }

        private async void LoadCurrentSelection()
        {
            try
            {
                _currentStoredCompany = await _localStorageService.GetSelectedCompanyAsync();
                
                if (_currentStoredCompany != null)
                {
                    lblCurrentSelection.Text = $"Current Selection: {_currentStoredCompany.DisplayName}";
                    lblCurrentSelection.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lblCurrentSelection.Text = "Current Selection: None";
                    lblCurrentSelection.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading current selection: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private async void LoadCompanies()
        {
            try
            {
                lblStatus.Text = "Loading companies...";
                lblStatus.ForeColor = Color.Blue;
                
                //// Try to load from cache first
                //var cachedCompanies = await _localStorageService.GetCompanyCacheAsync();
                //if (cachedCompanies != null && cachedCompanies.Any())
                //{
                //    // Convert cached Company objects to SelectCompanyModel
                //    _companies = cachedCompanies.Select(c => new SelectCompanyModel
                //    {
                //        Id = c.Id.ToString(),
                //        CompanyId = c.Id.ToString(),
                //        CompanyName = c.Name,
                //        CompanyCode = c.Code,
                //        IsActive = c.IsActive
                //    }).ToList();
                //}
                //else
                //{
                //    _companies = new List<SelectCompanyModel>();
                //}
                
                // If cache is empty or expired, load from API
                if (!_companies.Any())
                {
                    _companies = await _companyService.GetAllCompaniesAsync();
                    
                    // Save to cache for future use (convert back to Company objects for cache)
                    if (_companies.Any())
                    {
                        var companiesForCache = _companies.Select(sc => new WinFormsApp1.Models.Company
                        {
                            Id = sc.CompanyId,
                            Name = sc.CompanyName,
                            Code = sc.CompanyCode,
                            IsActive = sc.IsActive
                        }).ToList();
                        await _localStorageService.SaveCompanyCacheAsync(companiesForCache);
                    }
                }
                
                // Populate dropdown
                cmbCompanies.DataSource = null;
                cmbCompanies.DataSource = _companies;
                cmbCompanies.DisplayMember = "DisplayName";
                
                if (_companies.Any())
                {
                    lblStatus.Text = $"Loaded {_companies.Count} companies";
                    lblStatus.ForeColor = Color.Green;
                    
                    // Try to select the currently stored company
                    if (_currentStoredCompany != null)
                    {
                        var matchingCompany = _companies.FirstOrDefault(c => 
                            c.Id == _currentStoredCompany.Id.ToString() || 
                            c.CompanyId == _currentStoredCompany.Id.ToString());
                        if (matchingCompany != null)
                        {
                            cmbCompanies.SelectedItem = matchingCompany;
                        }
                    }
                    
                    // If nothing selected, select first item
                    if (cmbCompanies.SelectedIndex == -1 && _companies.Any())
                    {
                        cmbCompanies.SelectedIndex = 0;
                    }
                }
                else
                {
                    lblStatus.Text = "No companies found";
                    lblStatus.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading companies: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void cmbCompanies_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedCompany = cmbCompanies.SelectedItem as SelectCompanyModel;
            btnSelect.Enabled = _selectedCompany != null;
            
            if (_selectedCompany != null)
            {
                DisplayCompanyDetails(_selectedCompany);
            }
            else
            {
                ClearCompanyDetails();
            }
        }

        private void DisplayCompanyDetails(SelectCompanyModel company)
        {
            lblCompanyName.Text = $"Name: {company.CompanyName}";
            lblCompanyCode.Text = $"Code: {company.CompanyCode}";
            
            // For SelectCompanyModel, we only have basic info
            lblCompanyAddress.Text = "Address: Not available in selection view";
            lblCompanyPhone.Text = "Phone: Not available in selection view";
            lblCompanyEmail.Text = "Email: Not available in selection view";
        }

        private void ClearCompanyDetails()
        {
            lblCompanyName.Text = "Name: ";
            lblCompanyCode.Text = "Code: ";
            lblCompanyAddress.Text = "Address: ";
            lblCompanyPhone.Text = "Phone: ";
            lblCompanyEmail.Text = "Email: ";
        }

        private void cmbCompanies_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedCompany != null)
                    {
                        SelectCompany();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.F5:
                    _ = RefreshCompanies(); // Fire and forget
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
                    DialogResult = DialogResult.Cancel;
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private void CompanySelectForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    _ = RefreshCompanies(); // Fire and forget
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
                    DialogResult = DialogResult.Cancel;
                    Close();
                    e.Handled = true;
                    break;
                    
                case Keys.F1:
                    ShowHelp();
                    e.Handled = true;
                    break;
            }
        }

        private async void btnSelect_Click(object? sender, EventArgs e)
        {
            await SelectCompany();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            // When closing with Cancel button, ensure navigation panel is shown
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await RefreshCompanies();
        }

        private async Task SelectCompany()
        {
            if (_selectedCompany == null)
            {
                MessageBox.Show("Please select a company first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "Fetching company details...";
                lblStatus.ForeColor = Color.Blue;
                
                // Parse the company ID to GUID - use CompanyId as it contains the actual data
                string companyIdString = !string.IsNullOrEmpty(_selectedCompany.Id) ? _selectedCompany.Id : _selectedCompany.CompanyId;
                
                Console.WriteLine($"Selected Company - Id: '{_selectedCompany.Id}', CompanyId: '{_selectedCompany.CompanyId}', Using: '{companyIdString}'");
                
                if (!Guid.TryParse(companyIdString, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                // Fetch complete company details from API
                var fullCompanyDetails = await _companyService.GetCompanyByIdAsync(companyId);
                
                if (fullCompanyDetails == null)
                {
                    lblStatus.Text = "Failed to fetch company details";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                lblStatus.Text = "Saving selected company...";
                lblStatus.ForeColor = Color.Blue;
                
                // Save the complete company details to local storage
                await _localStorageService.SaveSelectedCompanyAsync(fullCompanyDetails);
                
                lblStatus.Text = $"Company '{fullCompanyDetails.DisplayName}' selected successfully!";
                lblStatus.ForeColor = Color.Green;
                
                // Close after a short delay
                await Task.Delay(1000);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving selection: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"SelectCompany exception: {ex.Message}");
            }
        }

        private async Task RefreshCompanies()
        {
            try
            {
                // Clear cache and reload from API
                await _localStorageService.ClearCompanyCacheAsync();
                LoadCompanies(); // This is now async void, so we don't await it
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error refreshing companies: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void ShowHelp()
        {
            var helpMessage = @"Company Selection Help:

Keyboard Navigation:
• ↑↓ Arrow keys - Navigate dropdown
• Enter - Select highlighted company
• F5 - Refresh company list
• Esc - Cancel and close
• F1 - Show this help

Mouse Operation:
• Click dropdown to expand
• Click company to select
• Click Select button to confirm

Features:
• Company details shown for selected item
• Current selection displayed at top
• Data cached for faster loading
• All selections saved to local storage

The selected company will be used as the context for all business operations in the application.";

            MessageBox.Show(helpMessage, "Company Selection Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
