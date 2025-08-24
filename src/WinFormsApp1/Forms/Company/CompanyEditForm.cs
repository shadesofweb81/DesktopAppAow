using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanyEditForm : Form
    {
        private readonly CompanyService _companyService;
        private readonly WinFormsApp1.Models.Company? _company;
        private readonly bool _isEditMode;

        private TextBox txtName = null!;
        private TextBox txtCode = null!;
        private TextBox txtAddress = null!;
        private TextBox txtCity = null!;
        private TextBox txtState = null!;
        private TextBox txtZipCode = null!;
        private TextBox txtCountry = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtWebsite = null!;
        private TextBox txtTaxId = null!;
        private CheckBox chkIsActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;

        public CompanyEditForm(CompanyService companyService, WinFormsApp1.Models.Company? company)
        {
            _companyService = companyService;
            _company = company;
            _isEditMode = company != null;
            
            InitializeComponent();
            SetupForm();
            LoadCompanyData();
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtCode = new TextBox();
            txtAddress = new TextBox();
            txtCity = new TextBox();
            txtState = new TextBox();
            txtZipCode = new TextBox();
            txtCountry = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtWebsite = new TextBox();
            txtTaxId = new TextBox();
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
            lblInstructions.Text = "Keyboard Navigation: Tab/Shift+Tab to navigate, Enter to save, Esc to cancel";
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

            txtCountry.Location = new Point(268, 162);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(120, 23);
            txtCountry.TabIndex = 6;

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

            // Is Active
            chkIsActive.Location = new Point(100, 315);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(120, 24);
            chkIsActive.TabIndex = 11;
            chkIsActive.Text = "Is &Active";
            chkIsActive.UseVisualStyleBackColor = true;
            chkIsActive.Checked = true;

            // Save Button
            btnSave.Location = new Point(200, 350);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 12;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            // Cancel Button
            btnCancel.Location = new Point(310, 350);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // Status Label
            lblStatus.Location = new Point(12, 390);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(450, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // CompanyEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 420);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(txtTaxId);
            Controls.Add(txtWebsite);
            Controls.Add(txtEmail);
            Controls.Add(txtPhone);
            Controls.Add(txtCountry);
            Controls.Add(txtZipCode);
            Controls.Add(txtState);
            Controls.Add(txtCity);
            Controls.Add(txtAddress);
            Controls.Add(txtCode);
            Controls.Add(txtName);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CompanyEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _isEditMode ? "Edit Company" : "New Company";
            KeyDown += new KeyEventHandler(CompanyEditForm_KeyDown);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            
            // Focus on name field
            txtName.Focus();
        }

        private void LoadCompanyData()
        {
            if (_company != null)
            {
                txtName.Text = _company.Name;
                txtCode.Text = _company.Code;
                txtAddress.Text = _company.Address;
                txtCity.Text = _company.City;
                txtState.Text = _company.State;
                txtZipCode.Text = _company.ZipCode;
                txtCountry.Text = _company.Country;
                txtPhone.Text = _company.Phone;
                txtEmail.Text = _company.Email;
                txtWebsite.Text = _company.Website;
                txtTaxId.Text = _company.TaxId;
                chkIsActive.Checked = _company.IsActive;
            }
        }

        private void CompanyEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    Close();
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
            DialogResult = DialogResult.Cancel;
            Close();
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
                    Country = txtCountry.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Website = txtWebsite.Text.Trim(),
                    TaxId = txtTaxId.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                bool success = false;
                
                if (_isEditMode && _company != null)
                {
                    success = await _companyService.UpdateCompanyAsync(_company.Id, request);
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
                    
                    // Close after a short delay
                    await Task.Delay(1000);
                    DialogResult = DialogResult.OK;
                    Close();
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

            if (string.IsNullOrWhiteSpace(txtCode.Text))
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
