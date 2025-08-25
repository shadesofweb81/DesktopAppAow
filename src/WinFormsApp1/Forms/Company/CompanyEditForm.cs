using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanyEditForm : Form
    {
        private readonly CompanyService _companyService;
        private readonly CountryService _countryService;
        private readonly WinFormsApp1.Models.Company? _company;
        private readonly bool _isEditMode;

        // Public properties for MDI child form management
        public bool IsEditMode => _isEditMode;
        public string CompanyId => _company?.Id ?? string.Empty;

        private TextBox txtName = null!;
        private TextBox txtCode = null!;
        private TextBox txtAddress = null!;
        private TextBox txtCity = null!;
        private TextBox txtState = null!;
        private TextBox txtZipCode = null!;
        private ComboBox cboCountry = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtWebsite = null!;
        private TextBox txtTaxId = null!;
        private TextBox txtLogoUrl = null!;
        private TextBox txtCurrency = null!;
        private TextBox txtUserRole = null!;
        private DateTimePicker dtpStartingFinancialYear = null!;
        private CheckBox chkIsActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;

        public CompanyEditForm(CompanyService companyService, CountryService countryService, WinFormsApp1.Models.Company? company)
        {
            _companyService = companyService;
            _countryService = countryService;
            _company = company;
            _isEditMode = company != null;
            
            InitializeComponent();
            SetupForm();
            LoadCompanyDataAsync();
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtCode = new TextBox();
            txtAddress = new TextBox();
            txtCity = new TextBox();
            txtState = new TextBox();
            txtZipCode = new TextBox();
            cboCountry = new ComboBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtWebsite = new TextBox();
            txtTaxId = new TextBox();
            txtLogoUrl = new TextBox();
            txtCurrency = new TextBox();
            txtUserRole = new TextBox();
            dtpStartingFinancialYear = new DateTimePicker();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            SuspendLayout();

            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(500, 25);
            lblInstructions.Text = "Keyboard Navigation: Tab/Shift+Tab to navigate, Enter to save, Esc to cancel | Country dropdown auto-updates currency";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);

            // Company Name
            var lblName = new Label();
            lblName.Location = new Point(12, 45);
            lblName.Size = new Size(80, 20);
            lblName.Text = "&Name:";
            lblName.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblName);

            txtName.Location = new Point(100, 42);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 23);
            txtName.TabIndex = 0;

            // Company Code
            var lblCode = new Label();
            lblCode.Location = new Point(12, 75);
            lblCode.Size = new Size(80, 20);
            lblCode.Text = "&Code:";
            lblCode.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblCode);

            txtCode.Location = new Point(100, 72);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(150, 23);
            txtCode.TabIndex = 1;

            // Address
            var lblAddress = new Label();
            lblAddress.Location = new Point(12, 105);
            lblAddress.Size = new Size(80, 20);
            lblAddress.Text = "&Address:";
            lblAddress.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblAddress);

            txtAddress.Location = new Point(100, 102);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(350, 23);
            txtAddress.TabIndex = 2;

            // City
            var lblCity = new Label();
            lblCity.Location = new Point(12, 135);
            lblCity.Size = new Size(80, 20);
            lblCity.Text = "C&ity:";
            lblCity.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblCity);

            txtCity.Location = new Point(100, 132);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(150, 23);
            txtCity.TabIndex = 3;

            // State
            var lblState = new Label();
            lblState.Location = new Point(260, 135);
            lblState.Size = new Size(40, 20);
            lblState.Text = "&State:";
            lblState.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblState);

            txtState.Location = new Point(308, 132);
            txtState.Name = "txtState";
            txtState.Size = new Size(80, 23);
            txtState.TabIndex = 4;

            // Zip Code
            var lblZipCode = new Label();
            lblZipCode.Location = new Point(12, 165);
            lblZipCode.Size = new Size(80, 20);
            lblZipCode.Text = "&Zip Code:";
            lblZipCode.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblZipCode);

            txtZipCode.Location = new Point(100, 162);
            txtZipCode.Name = "txtZipCode";
            txtZipCode.Size = new Size(100, 23);
            txtZipCode.TabIndex = 5;

            // Country
            var lblCountry = new Label();
            lblCountry.Location = new Point(210, 165);
            lblCountry.Size = new Size(50, 20);
            lblCountry.Text = "Co&untry:";
            lblCountry.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblCountry);

            cboCountry.Location = new Point(268, 162);
            cboCountry.Name = "cboCountry";
            cboCountry.Size = new Size(120, 23);
            cboCountry.TabIndex = 6;
            cboCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCountry.SelectedIndexChanged += new EventHandler(cboCountry_SelectedIndexChanged);

            // Phone
            var lblPhone = new Label();
            lblPhone.Location = new Point(12, 195);
            lblPhone.Size = new Size(80, 20);
            lblPhone.Text = "&Phone:";
            lblPhone.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblPhone);

            txtPhone.Location = new Point(100, 192);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(150, 23);
            txtPhone.TabIndex = 7;

            // Email
            var lblEmail = new Label();
            lblEmail.Location = new Point(12, 225);
            lblEmail.Size = new Size(80, 20);
            lblEmail.Text = "&Email:";
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblEmail);

            txtEmail.Location = new Point(100, 222);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 8;

            // Website
            var lblWebsite = new Label();
            lblWebsite.Location = new Point(12, 255);
            lblWebsite.Size = new Size(80, 20);
            lblWebsite.Text = "&Website:";
            lblWebsite.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblWebsite);

            txtWebsite.Location = new Point(100, 252);
            txtWebsite.Name = "txtWebsite";
            txtWebsite.Size = new Size(250, 23);
            txtWebsite.TabIndex = 9;

            // Tax ID
            var lblTaxId = new Label();
            lblTaxId.Location = new Point(12, 285);
            lblTaxId.Size = new Size(80, 20);
            lblTaxId.Text = "&Tax ID:";
            lblTaxId.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblTaxId);

            txtTaxId.Location = new Point(100, 282);
            txtTaxId.Name = "txtTaxId";
            txtTaxId.Size = new Size(150, 23);
            txtTaxId.TabIndex = 10;

            // Logo URL
            var lblLogoUrl = new Label();
            lblLogoUrl.Location = new Point(12, 315);
            lblLogoUrl.Size = new Size(80, 20);
            lblLogoUrl.Text = "&Logo URL:";
            lblLogoUrl.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblLogoUrl);

            txtLogoUrl.Location = new Point(100, 312);
            txtLogoUrl.Name = "txtLogoUrl";
            txtLogoUrl.Size = new Size(250, 23);
            txtLogoUrl.TabIndex = 11;

            // Currency
            var lblCurrency = new Label();
            lblCurrency.Location = new Point(12, 345);
            lblCurrency.Size = new Size(80, 20);
            lblCurrency.Text = "&Currency:";
            lblCurrency.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblCurrency);

            txtCurrency.Location = new Point(100, 342);
            txtCurrency.Name = "txtCurrency";
            txtCurrency.Size = new Size(100, 23);
            txtCurrency.TabIndex = 12;
            txtCurrency.ReadOnly = true;
            txtCurrency.BackColor = Color.LightGray;

            // User Role
            var lblUserRole = new Label();
            lblUserRole.Location = new Point(210, 345);
            lblUserRole.Size = new Size(60, 20);
            lblUserRole.Text = "&Role:";
            lblUserRole.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblUserRole);

            txtUserRole.Location = new Point(278, 342);
            txtUserRole.Name = "txtUserRole";
            txtUserRole.Size = new Size(120, 23);
            txtUserRole.TabIndex = 13;

            // Starting Financial Year Date
            var lblStartingFinancialYear = new Label();
            lblStartingFinancialYear.Location = new Point(12, 375);
            lblStartingFinancialYear.Size = new Size(80, 20);
            lblStartingFinancialYear.Text = "&Financial Year:";
            lblStartingFinancialYear.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblStartingFinancialYear);

            dtpStartingFinancialYear.Location = new Point(100, 372);
            dtpStartingFinancialYear.Name = "dtpStartingFinancialYear";
            dtpStartingFinancialYear.Size = new Size(150, 23);
            dtpStartingFinancialYear.TabIndex = 14;
            dtpStartingFinancialYear.Format = DateTimePickerFormat.Short;

            // Is Active
            chkIsActive.Location = new Point(100, 405);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(120, 24);
            chkIsActive.TabIndex = 15;
            chkIsActive.Text = "Is &Active";
            chkIsActive.UseVisualStyleBackColor = true;
            chkIsActive.Checked = true;

            // Save Button
            btnSave.Location = new Point(200, 440);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 16;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            // Cancel Button
            btnCancel.Location = new Point(310, 440);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // Status Label
            lblStatus.Location = new Point(12, 480);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(450, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // CompanyEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 510);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(dtpStartingFinancialYear);
            Controls.Add(txtUserRole);
            Controls.Add(txtCurrency);
            Controls.Add(txtLogoUrl);
            Controls.Add(txtTaxId);
            Controls.Add(txtWebsite);
            Controls.Add(txtEmail);
            Controls.Add(txtPhone);
            Controls.Add(cboCountry);
            Controls.Add(txtZipCode);
            Controls.Add(txtState);
            Controls.Add(txtCity);
            Controls.Add(txtAddress);
            Controls.Add(txtCode);
            Controls.Add(txtName);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "CompanyEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _isEditMode ? "Edit Company" : "New Company";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(CompanyEditForm_KeyDown);
            Load += new EventHandler(CompanyEditForm_Load);
            Resize += new EventHandler(CompanyEditForm_Resize);
            Activated += new EventHandler(CompanyEditForm_Activated);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            
            // Load country dropdown
            LoadCountryDropdown();
            
            // Focus on name field
            txtName.Focus();
        }

        private void CompanyEditForm_Load(object? sender, EventArgs e)
        {
            // Check if this is an MDI child form or a dialog
            if (MdiParent != null)
            {
                // This is an MDI child form - maximize it
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                // This is a dialog - center it and make it a reasonable size
                WindowState = FormWindowState.Normal;
                StartPosition = FormStartPosition.CenterParent;
                Size = new Size(800, 600);
            }
            
            // Resize controls to fit the form
            ResizeControls();
            
            // Focus on name field
            txtName.Focus();
        }

        private void ResizeControls()
        {
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            // Account for MDI navigation panel width (approximately 343px)
            int availableWidth = clientWidth - 150;
            int availableHeight = clientHeight - 150;
            
            // Reposition and resize controls for better layout
            // Instructions label
            lblInstructions.Size = new Size(availableWidth - 24, 25);
            
            // Status label at the bottom
            lblStatus.Location = new Point(12, clientHeight - 35);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            // Buttons at the bottom
            btnSave.Location = new Point(availableWidth - 200, clientHeight - 80);
            btnCancel.Location = new Point(availableWidth - 100, clientHeight - 80);
        }

        private void CompanyEditForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void CompanyEditForm_Activated(object? sender, EventArgs e)
        {
            // Ensure MDI navigation panel is visible when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                // The MDI form's MdiChildActivate event will handle showing the navigation panel
            }
        }

        private void LoadCountryDropdown()
        {
            try
            {
                var countries = _countryService.GetCountries();
                
                // Create a new list with default option
                var countryList = new List<CountryOption>
                {
                    new CountryOption { Code = "", Name = "-- Select Country --" }
                };
                countryList.AddRange(countries);
                
                cboCountry.DataSource = countryList;
                cboCountry.DisplayMember = "Name";
                cboCountry.ValueMember = "Code";
                cboCountry.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading countries: {ex.Message}");
            }
        }

        private void cboCountry_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboCountry.SelectedItem is CountryOption selectedCountry && !string.IsNullOrEmpty(selectedCountry.Code))
            {
                // Update currency based on selected country
                var currency = _countryService.GetCurrencyByCountry(selectedCountry.Code);
                txtCurrency.Text = currency;
                
                Console.WriteLine($"Country changed to: {selectedCountry.Name}, Currency set to: {currency}");
            }
        }

        private void SetCountrySelection(string countryName)
        {
            try
            {
                if (string.IsNullOrEmpty(countryName))
                {
                    cboCountry.SelectedIndex = 0; // Select default option
                    return;
                }

                // Find the country by name
                for (int i = 0; i < cboCountry.Items.Count; i++)
                {
                    if (cboCountry.Items[i] is CountryOption country && 
                        country.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase))
                    {
                        cboCountry.SelectedIndex = i;
                        return;
                    }
                }

                // If not found, select default
                cboCountry.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting country selection: {ex.Message}");
                cboCountry.SelectedIndex = 0;
            }
        }

        private string GetSelectedCountryName()
        {
            if (cboCountry.SelectedItem is CountryOption selectedCountry)
            {
                return selectedCountry.Name;
            }
            return string.Empty;
        }

        private async void LoadCompanyDataAsync()
        {
            if (_isEditMode && _company != null)
            {
                try
                {
                    lblStatus.Text = "Loading company data...";
                    lblStatus.ForeColor = Color.Blue;
                    
                    // Get fresh company data from API using GetCompanyByIdAsync
                    var companyId = Guid.Parse(_company.Id);
                    var freshCompanyData = await _companyService.GetCompanyByIdAsync(companyId);
                    
                    if (freshCompanyData != null)
                    {
                        // Update the form with fresh data from API
                        txtName.Text = freshCompanyData.Name;
                        txtCode.Text = freshCompanyData.Code;
                        txtAddress.Text = freshCompanyData.Address;
                        txtCity.Text = freshCompanyData.City;
                        txtState.Text = freshCompanyData.State;
                        txtZipCode.Text = freshCompanyData.ZipCode;
                        SetCountrySelection(freshCompanyData.Country);
                        txtPhone.Text = freshCompanyData.Phone;
                        txtEmail.Text = freshCompanyData.Email;
                        txtWebsite.Text = freshCompanyData.Website;
                        txtTaxId.Text = freshCompanyData.TaxId;
                        txtLogoUrl.Text = freshCompanyData.LogoUrl;
                        txtCurrency.Text = freshCompanyData.Currency;
                        txtUserRole.Text = freshCompanyData.UserRole;
                        dtpStartingFinancialYear.Value = freshCompanyData.StartingFinancialYearDate ?? DateTime.Now;
                        chkIsActive.Checked = freshCompanyData.IsActive;
                        
                        lblStatus.Text = "Company data loaded successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        // Fallback to the passed company data if API call fails
                        LoadCompanyDataFromPassedData();
                        lblStatus.Text = "Warning: Using cached data (API call failed)";
                        lblStatus.ForeColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    // Fallback to the passed company data if there's an error
                    LoadCompanyDataFromPassedData();
                    lblStatus.Text = $"Error loading data: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                // For new company, just clear the form
                ClearForm();
                lblStatus.Text = "Ready to create new company";
                lblStatus.ForeColor = Color.Green;
            }
        }

        private void LoadCompanyDataFromPassedData()
        {
            if (_company != null)
            {
                txtName.Text = _company.Name;
                txtCode.Text = _company.Code;
                txtAddress.Text = _company.Address;
                txtCity.Text = _company.City;
                txtState.Text = _company.State;
                txtZipCode.Text = _company.ZipCode;
                SetCountrySelection(_company.Country);
                txtPhone.Text = _company.Phone;
                txtEmail.Text = _company.Email;
                txtWebsite.Text = _company.Website;
                txtTaxId.Text = _company.TaxId;
                txtLogoUrl.Text = _company.LogoUrl;
                txtCurrency.Text = _company.Currency;
                txtUserRole.Text = _company.UserRole;
                dtpStartingFinancialYear.Value = _company.StartingFinancialYearDate ?? DateTime.Now;
                chkIsActive.Checked = _company.IsActive;
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            cboCountry.SelectedIndex = 0; // Select default "Select Country" option
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtTaxId.Text = string.Empty;
            txtLogoUrl.Text = string.Empty;
            txtCurrency.Text = string.Empty;
            txtUserRole.Text = string.Empty;
            dtpStartingFinancialYear.Value = DateTime.Now;
            chkIsActive.Checked = true;
        }

        private void CompanyEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    // Check if this is an MDI child form
                    if (MdiParent != null)
                    {
                        // This is an MDI child form - just close it
                        Close();
                    }
                    else
                    {
                        // This is a dialog - set result and close
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                    e.Handled = true;
                    break;
                    
                case Keys.Enter:
                    if (e.Control) // Ctrl+Enter to save
                    {
                        SaveCompany();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            await SaveCompany();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            // Check if this is an MDI child form
            if (MdiParent != null)
            {
                // This is an MDI child form - just close it
                Close();
            }
            else
            {
                // This is a dialog - set result and close
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private async Task SaveCompany()
        {
            if (!ValidateForm())
                return;

            try
            {
                lblStatus.Text = _isEditMode ? "Updating company..." : "Creating company...";
                lblStatus.ForeColor = Color.Blue;
                
                var request = new CompanyCreateRequest
                {
                    Name = txtName.Text.Trim(),
                    Code = txtCode.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    State = txtState.Text.Trim(),
                    ZipCode = txtZipCode.Text.Trim(),
                    Country = GetSelectedCountryName(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Website = txtWebsite.Text.Trim(),
                    TaxId = txtTaxId.Text.Trim(),
                    LogoUrl = txtLogoUrl.Text.Trim(),
                    Currency = txtCurrency.Text.Trim(),
                    UserRole = txtUserRole.Text.Trim(),
                    StartingFinancialYearDate = dtpStartingFinancialYear.Value,
                    IsActive = chkIsActive.Checked
                };

                bool success = false;
                
                if (_isEditMode && _company != null)
                {
                    success = await _companyService.UpdateCompanyAsync(Guid.Parse(_company.Id), request);
                }
                else
                {
                    var createdCompany = await _companyService.CreateCompanyAsync(request);
                    success = createdCompany != null;
                }

                if (success)
                {
                    lblStatus.Text = _isEditMode ? "Company updated successfully!" : "Company created successfully!";
                    lblStatus.ForeColor = Color.Green;
                    
                    // Check if this is an MDI child form
                    if (MdiParent != null)
                    {
                        // This is an MDI child form - stay open and show success message
                        // The parent form will handle refreshing the list
                        await Task.Delay(2000); // Show success message for 2 seconds
                        lblStatus.Text = "Ready";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        // This is a dialog - close after a short delay
                        await Task.Delay(1000);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    lblStatus.Text = _isEditMode ? "Failed to update company" : "Failed to create company";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Company name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtState.Text))
            {
                MessageBox.Show("Company code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            // Validate email format if provided
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    var email = new System.Net.Mail.MailAddress(txtEmail.Text);
                }
                catch
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}
