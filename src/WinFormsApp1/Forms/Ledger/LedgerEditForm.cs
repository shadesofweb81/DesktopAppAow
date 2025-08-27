using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Ledger
{
    public partial class LedgerEditForm : Form
    {
        private readonly LedgerService _ledgerService;
        private readonly LocalStorageService _localStorageService;
        private LedgerModel? _ledger;
        private bool _isNewLedger;
        private bool _isReadOnly = false;

        // Form controls
        private TextBox txtName = null!;
        private ComboBox cmbCategory = null!;
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
        private CheckBox chkIsGroup = null!;
        private CheckBox chkIsRegistered = null!;
        private ComboBox cmbParent = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;

        public LedgerEditForm(LedgerService ledgerService, LedgerModel? ledger)
        {
            _ledgerService = ledgerService;
            _localStorageService = new LocalStorageService();
            _ledger = ledger;
            _isNewLedger = ledger == null;
            InitializeComponent();
            SetupForm();
            LoadData();
            
            // Add keyboard event handler for ESC key
            this.KeyPreview = true;
            this.KeyDown += LedgerEditForm_KeyDown;
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            cmbCategory = new ComboBox();
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
            chkIsGroup = new CheckBox();
            chkIsRegistered = new CheckBox();
            cmbParent = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 700);
            Name = "LedgerEditForm";
            Text = _isNewLedger ? "New Ledger" : "Edit Ledger";
            StartPosition = FormStartPosition.CenterParent;

            // Labels and controls layout
            var y = 30;
            var labelX = 30;
            var controlX = 200;
            var controlWidth = 400;
            var labelWidth = 150;
            var controlHeight = 25;
            var spacing = 35;

            // Name
            var lblName = new Label { Text = "Name:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtName.Location = new Point(controlX, y);
            txtName.Size = new Size(controlWidth, controlHeight);
            txtName.MaxLength = 100;
            Controls.Add(lblName);
            Controls.Add(txtName);
            y += spacing;

            // Category
            var lblCategory = new Label { Text = "Category:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbCategory.Location = new Point(controlX, y);
            cmbCategory.Size = new Size(controlWidth, controlHeight);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            y += spacing;

            // Code
            var lblCode = new Label { Text = "Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtCode.Location = new Point(controlX, y);
            txtCode.Size = new Size(controlWidth, controlHeight);
            txtCode.MaxLength = 50;
            Controls.Add(lblCode);
            Controls.Add(txtCode);
            y += spacing;

            // Address
            var lblAddress = new Label { Text = "Address:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtAddress.Location = new Point(controlX, y);
            txtAddress.Size = new Size(controlWidth, controlHeight);
            txtAddress.MaxLength = 200;
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            y += spacing;

            // City
            var lblCity = new Label { Text = "City:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtCity.Location = new Point(controlX, y);
            txtCity.Size = new Size(controlWidth, controlHeight);
            txtCity.MaxLength = 100;
            Controls.Add(lblCity);
            Controls.Add(txtCity);
            y += spacing;

            // State
            var lblState = new Label { Text = "State:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtState.Location = new Point(controlX, y);
            txtState.Size = new Size(controlWidth, controlHeight);
            txtState.MaxLength = 50;
            Controls.Add(lblState);
            Controls.Add(txtState);
            y += spacing;

            // Zip Code
            var lblZipCode = new Label { Text = "Zip Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtZipCode.Location = new Point(controlX, y);
            txtZipCode.Size = new Size(controlWidth, controlHeight);
            txtZipCode.MaxLength = 20;
            Controls.Add(lblZipCode);
            Controls.Add(txtZipCode);
            y += spacing;

            // Country
            var lblCountry = new Label { Text = "Country:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtCountry.Location = new Point(controlX, y);
            txtCountry.Size = new Size(controlWidth, controlHeight);
            txtCountry.MaxLength = 100;
            Controls.Add(lblCountry);
            Controls.Add(txtCountry);
            y += spacing;

            // Phone
            var lblPhone = new Label { Text = "Phone:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtPhone.Location = new Point(controlX, y);
            txtPhone.Size = new Size(controlWidth, controlHeight);
            txtPhone.MaxLength = 30;
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            y += spacing;

            // Email
            var lblEmail = new Label { Text = "Email:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtEmail.Location = new Point(controlX, y);
            txtEmail.Size = new Size(controlWidth, controlHeight);
            txtEmail.MaxLength = 100;
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            y += spacing;

            // Website
            var lblWebsite = new Label { Text = "Website:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtWebsite.Location = new Point(controlX, y);
            txtWebsite.Size = new Size(controlWidth, controlHeight);
            txtWebsite.MaxLength = 100;
            Controls.Add(lblWebsite);
            Controls.Add(txtWebsite);
            y += spacing;

            // Tax ID
            var lblTaxId = new Label { Text = "Tax ID:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtTaxId.Location = new Point(controlX, y);
            txtTaxId.Size = new Size(controlWidth, controlHeight);
            txtTaxId.MaxLength = 50;
            Controls.Add(lblTaxId);
            Controls.Add(txtTaxId);
            y += spacing;

            // Is Group
            chkIsGroup.Location = new Point(controlX, y);
            chkIsGroup.Size = new Size(200, controlHeight);
            chkIsGroup.Text = "Is Group";
            Controls.Add(chkIsGroup);
            y += spacing;

            // Is Registered
            chkIsRegistered.Location = new Point(controlX, y);
            chkIsRegistered.Size = new Size(200, controlHeight);
            chkIsRegistered.Text = "Is Registered";
            Controls.Add(chkIsRegistered);
            y += spacing;

            // Parent
            var lblParent = new Label { Text = "Parent:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbParent.Location = new Point(controlX, y);
            cmbParent.Size = new Size(controlWidth, controlHeight);
            cmbParent.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblParent);
            Controls.Add(cmbParent);
            y += spacing + 10;

            // Buttons
            btnSave.Location = new Point(controlX, y);
            btnSave.Size = new Size(100, 35);
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            Controls.Add(btnSave);

            btnCancel.Location = new Point(controlX + 120, y);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            Controls.Add(btnCancel);

            // Status label
            lblStatus.Location = new Point(labelX, y + 50);
            lblStatus.Size = new Size(700, 25);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            Controls.Add(lblStatus);

            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set form properties
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            ShowInTaskbar = true;
        }

        private async void LoadData()
        {
            try
            {
                // Load categories
                cmbCategory.Items.Clear();
                cmbCategory.Items.AddRange(new string[] { "Customer", "Supplier", "Bank", "Cash", "Asset", "Liability", "Income", "Expense", "Equity" });

                // Load parent ledgers first
                await LoadParentLedgers();

                if (_isNewLedger)
                {
                    _ledger = new LedgerModel();
                    var company = await _localStorageService.GetSelectedCompanyAsync();
                    if (company != null)
                    {
                        _ledger.CompanyId = Guid.Parse(company.Id);
                    }
                }
                else
                {
                    // Load existing ledger data
                    txtName.Text = _ledger!.Name;
                    cmbCategory.Text = _ledger.Category;
                    txtCode.Text = _ledger.Code;
                    txtAddress.Text = _ledger.Address ?? "";
                    txtCity.Text = _ledger.City ?? "";
                    txtState.Text = _ledger.State ?? "";
                    txtZipCode.Text = _ledger.ZipCode ?? "";
                    txtCountry.Text = _ledger.Country ?? "";
                    txtPhone.Text = _ledger.Phone ?? "";
                    txtEmail.Text = _ledger.Email ?? "";
                    txtWebsite.Text = _ledger.Website ?? "";
                    txtTaxId.Text = _ledger.TaxId ?? "";
                    chkIsGroup.Checked = _ledger.IsGroup;
                    chkIsRegistered.Checked = _ledger.IsRegistered ?? false;

                    // Set parent if exists
                    if (_ledger.ParentId.HasValue)
                    {
                        // Find the parent ledger in the combo box
                        for (int i = 0; i < cmbParent.Items.Count; i++)
                        {
                            if (cmbParent.Items[i] is LedgerModel parentLedger && parentLedger.Id == _ledger.ParentId.Value)
                            {
                                cmbParent.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadParentLedgers()
        {
            try
            {
                var company = await _localStorageService.GetSelectedCompanyAsync();
                if (company != null)
                {
                    var ledgers = await _ledgerService.GetAllLedgersAsync(Guid.Parse(company.Id));
                    cmbParent.Items.Clear();
                    cmbParent.Items.Add(""); // Empty option for no parent
                    
                    foreach (var ledger in ledgers.Where(l => l.IsGroup))
                    {
                        cmbParent.Items.Add(ledger);
                    }
                    
                    // Set display member for the combo box
                    cmbParent.DisplayMember = "DisplayName";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading parent ledgers: {ex.Message}");
            }
        }

        public void SetReadOnlyMode()
        {
            _isReadOnly = true;
            Text = "View Ledger";
            btnSave.Visible = false;
            btnCancel.Text = "Close";
            
            // Disable all controls
            txtName.ReadOnly = true;
            cmbCategory.Enabled = false;
            txtCode.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtState.ReadOnly = true;
            txtZipCode.ReadOnly = true;
            txtCountry.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtWebsite.ReadOnly = true;
            txtTaxId.ReadOnly = true;
            chkIsGroup.Enabled = false;
            chkIsRegistered.Enabled = false;
            cmbParent.Enabled = false;
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isReadOnly) return;

            try
            {
                if (!ValidateForm())
                    return;

                lblStatus.Text = "Saving...";
                lblStatus.ForeColor = Color.Blue;

                // Update ledger model
                _ledger!.Name = txtName.Text.Trim();
                _ledger.Category = cmbCategory.Text;
                _ledger.Code = txtCode.Text.Trim();
                _ledger.Address = txtAddress.Text.Trim();
                _ledger.City = txtCity.Text.Trim();
                _ledger.State = txtState.Text.Trim();
                _ledger.ZipCode = txtZipCode.Text.Trim();
                _ledger.Country = txtCountry.Text.Trim();
                _ledger.Phone = txtPhone.Text.Trim();
                _ledger.Email = txtEmail.Text.Trim();
                
                // Format website URL to ensure it has a protocol
                string websiteUrl = txtWebsite.Text.Trim();
                if (!string.IsNullOrWhiteSpace(websiteUrl))
                {
                    if (!websiteUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
                        !websiteUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && 
                        !websiteUrl.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                    {
                        websiteUrl = "https://" + websiteUrl;
                    }
                }
                _ledger.Website = websiteUrl;
                
                _ledger.TaxId = txtTaxId.Text.Trim();
                _ledger.IsGroup = chkIsGroup.Checked;
                _ledger.IsRegistered = chkIsRegistered.Checked;

                // Set parent ID
                if (cmbParent.SelectedItem is LedgerModel parentLedger)
                {
                    _ledger.ParentId = parentLedger.Id;
                }
                else
                {
                    _ledger.ParentId = null;
                }

                bool success;
                if (_isNewLedger)
                {
                    success = await _ledgerService.CreateLedgerAsync(_ledger);
                }
                else
                {
                    // Create UpdateLedgerModel for update operation
                    var updateModel = new UpdateLedgerModel
                    {
                        Name = _ledger.Name,
                        Category = _ledger.Category,
                        Code = _ledger.Code,
                        Address = _ledger.Address,
                        City = _ledger.City,
                        State = _ledger.State,
                        ZipCode = _ledger.ZipCode,
                        Country = _ledger.Country,
                        Phone = _ledger.Phone,
                        Email = _ledger.Email,
                        Website = _ledger.Website,
                        TaxId = _ledger.TaxId,
                        IsGroup = _ledger.IsGroup,
                        IsRegistered = _ledger.IsRegistered,
                        ParentId = _ledger.ParentId,
                        CompanyId = _ledger.CompanyId
                    };
                    
                    success = await _ledgerService.UpdateLedgerAsync(_ledger.Id, updateModel);
                }

                if (success)
                {
                    lblStatus.Text = "Saved successfully";
                    lblStatus.ForeColor = Color.Green;
                    Close();
                }
                else
                {
                    lblStatus.Text = "Failed to save";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show("Failed to save ledger", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error saving ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    var email = new System.Net.Mail.MailAddress(txtEmail.Text);
                }
                catch
                {
                    MessageBox.Show("Please enter a valid email address (e.g., user@example.com).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtWebsite.Text))
            {
                // Ensure the URL has a protocol
                string websiteUrl = txtWebsite.Text.Trim();
                if (!websiteUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
                    !websiteUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && 
                    !websiteUrl.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
                {
                    websiteUrl = "https://" + websiteUrl;
                }

                if (!Uri.TryCreate(websiteUrl, UriKind.Absolute, out var uri) || 
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeFtp))
                {
                    MessageBox.Show("Please enter a valid website URL with protocol (e.g., https://www.example.com).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWebsite.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void LedgerEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            
            // Show navigation panel when this form is closed
            if (this.MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.ShowNavigationPanel();
            }
        }
    }
}
