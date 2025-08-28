using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Tax
{
    public partial class TaxEditForm : Form
    {
        private readonly TaxService _taxService;
        private readonly LocalStorageService _localStorageService;
        private TaxModel? _tax;
        private bool _isNewTax;
        private bool _isReadOnly = false;

        // Form controls
        private TextBox txtName = null!;
        private TextBox txtDescription = null!;
        private ComboBox cmbCategory = null!;
        private CheckBox chkIsActive = null!;
        private CheckBox chkIsComposite = null!;
        private NumericUpDown nudDefaultRate = null!;
        private TextBox txtHSNCode = null!;
        private TextBox txtSectionCode = null!;
        private CheckBox chkIsReverseChargeApplicable = null!;
        private CheckBox chkIsDeductibleAtSource = null!;
        private CheckBox chkIsCollectedAtSource = null!;
        private TextBox txtReturnFormNumber = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;

        public TaxEditForm(TaxService taxService, TaxModel? tax)
        {
            _taxService = taxService;
            _localStorageService = new LocalStorageService();
            _tax = tax;
            _isNewTax = tax == null;
            InitializeComponent();
            SetupForm();
            LoadData();
            
            // Add keyboard event handler for ESC key
            this.KeyPreview = true;
            this.KeyDown += TaxEditForm_KeyDown;
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtDescription = new TextBox();
            cmbCategory = new ComboBox();
            chkIsActive = new CheckBox();
            chkIsComposite = new CheckBox();
            nudDefaultRate = new NumericUpDown();
            txtHSNCode = new TextBox();
            txtSectionCode = new TextBox();
            chkIsReverseChargeApplicable = new CheckBox();
            chkIsDeductibleAtSource = new CheckBox();
            chkIsCollectedAtSource = new CheckBox();
            txtReturnFormNumber = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 700);
            Name = "TaxEditForm";
            Text = _isNewTax ? "New Tax" : "Edit Tax";
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

            // Description
            var lblDescription = new Label { Text = "Description:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtDescription.Location = new Point(controlX, y);
            txtDescription.Size = new Size(controlWidth, controlHeight);
            txtDescription.MaxLength = 500;
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            y += spacing;

            // Category
            var lblCategory = new Label { Text = "Category:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbCategory.Location = new Point(controlX, y);
            cmbCategory.Size = new Size(controlWidth, controlHeight);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            y += spacing;

            // Default Rate
            var lblDefaultRate = new Label { Text = "Default Rate (%):", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            nudDefaultRate.Location = new Point(controlX, y);
            nudDefaultRate.Size = new Size(controlWidth, controlHeight);
            nudDefaultRate.DecimalPlaces = 2;
            nudDefaultRate.Minimum = 0;
            nudDefaultRate.Maximum = 100;
            nudDefaultRate.Increment = 0.01m;
            Controls.Add(lblDefaultRate);
            Controls.Add(nudDefaultRate);
            y += spacing;

            // HSN Code
            var lblHSNCode = new Label { Text = "HSN Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtHSNCode.Location = new Point(controlX, y);
            txtHSNCode.Size = new Size(controlWidth, controlHeight);
            txtHSNCode.MaxLength = 50;
            Controls.Add(lblHSNCode);
            Controls.Add(txtHSNCode);
            y += spacing;

            // Section Code
            var lblSectionCode = new Label { Text = "Section Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtSectionCode.Location = new Point(controlX, y);
            txtSectionCode.Size = new Size(controlWidth, controlHeight);
            txtSectionCode.MaxLength = 50;
            Controls.Add(lblSectionCode);
            Controls.Add(txtSectionCode);
            y += spacing;

            // Return Form Number
            var lblReturnFormNumber = new Label { Text = "Return Form Number:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtReturnFormNumber.Location = new Point(controlX, y);
            txtReturnFormNumber.Size = new Size(controlWidth, controlHeight);
            txtReturnFormNumber.MaxLength = 50;
            Controls.Add(lblReturnFormNumber);
            Controls.Add(txtReturnFormNumber);
            y += spacing;

            // Is Active
            chkIsActive.Location = new Point(controlX, y);
            chkIsActive.Size = new Size(200, controlHeight);
            chkIsActive.Text = "Is Active";
            chkIsActive.Checked = true;
            Controls.Add(chkIsActive);
            y += spacing;

            // Is Composite
            chkIsComposite.Location = new Point(controlX, y);
            chkIsComposite.Size = new Size(200, controlHeight);
            chkIsComposite.Text = "Is Composite";
            Controls.Add(chkIsComposite);
            y += spacing;

            // Is Reverse Charge Applicable
            chkIsReverseChargeApplicable.Location = new Point(controlX, y);
            chkIsReverseChargeApplicable.Size = new Size(250, controlHeight);
            chkIsReverseChargeApplicable.Text = "Is Reverse Charge Applicable";
            Controls.Add(chkIsReverseChargeApplicable);
            y += spacing;

            // Is Deductible At Source
            chkIsDeductibleAtSource.Location = new Point(controlX, y);
            chkIsDeductibleAtSource.Size = new Size(200, controlHeight);
            chkIsDeductibleAtSource.Text = "Is Deductible At Source";
            Controls.Add(chkIsDeductibleAtSource);
            y += spacing;

            // Is Collected At Source
            chkIsCollectedAtSource.Location = new Point(controlX, y);
            chkIsCollectedAtSource.Size = new Size(200, controlHeight);
            chkIsCollectedAtSource.Text = "Is Collected At Source";
            Controls.Add(chkIsCollectedAtSource);
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
                cmbCategory.Items.AddRange(Enum.GetValues<TaxCategory>());

                if (_isNewTax)
                {
                    _tax = new TaxModel();
                    var company = await _localStorageService.GetSelectedCompanyAsync();
                    if (company != null)
                    {
                        _tax.CompanyId = Guid.Parse(company.Id);
                    }
                }
                else
                {
                    // Load existing tax data
                    txtName.Text = _tax!.Name;
                    txtDescription.Text = _tax.Description;
                    cmbCategory.SelectedItem = _tax.Category;
                    nudDefaultRate.Value = _tax.DefaultRate;
                    txtHSNCode.Text = _tax.HSNCode ?? "";
                    txtSectionCode.Text = _tax.SectionCode ?? "";
                    txtReturnFormNumber.Text = _tax.ReturnFormNumber ?? "";
                    chkIsActive.Checked = _tax.IsActive;
                    chkIsComposite.Checked = _tax.IsComposite;
                    chkIsReverseChargeApplicable.Checked = _tax.IsReverseChargeApplicable;
                    chkIsDeductibleAtSource.Checked = _tax.IsDeductibleAtSource;
                    chkIsCollectedAtSource.Checked = _tax.IsCollectedAtSource;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetReadOnlyMode()
        {
            _isReadOnly = true;
            Text = "View Tax";
            btnSave.Visible = false;
            btnCancel.Text = "Close";
            
            // Disable all controls
            txtName.ReadOnly = true;
            txtDescription.ReadOnly = true;
            cmbCategory.Enabled = false;
            nudDefaultRate.Enabled = false;
            txtHSNCode.ReadOnly = true;
            txtSectionCode.ReadOnly = true;
            txtReturnFormNumber.ReadOnly = true;
            chkIsActive.Enabled = false;
            chkIsComposite.Enabled = false;
            chkIsReverseChargeApplicable.Enabled = false;
            chkIsDeductibleAtSource.Enabled = false;
            chkIsCollectedAtSource.Enabled = false;
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

                // Update tax model
                _tax!.Name = txtName.Text.Trim();
                _tax.Description = txtDescription.Text.Trim();
                _tax.Category = (TaxCategory)cmbCategory.SelectedItem;
                _tax.DefaultRate = nudDefaultRate.Value;
                _tax.HSNCode = txtHSNCode.Text.Trim();
                _tax.SectionCode = txtSectionCode.Text.Trim();
                _tax.ReturnFormNumber = txtReturnFormNumber.Text.Trim();
                _tax.IsActive = chkIsActive.Checked;
                _tax.IsComposite = chkIsComposite.Checked;
                _tax.IsReverseChargeApplicable = chkIsReverseChargeApplicable.Checked;
                _tax.IsDeductibleAtSource = chkIsDeductibleAtSource.Checked;
                _tax.IsCollectedAtSource = chkIsCollectedAtSource.Checked;

                bool success;
                if (_isNewTax)
                {
                    success = await _taxService.CreateTaxAsync(_tax);
                }
                else
                {
                    success = await _taxService.UpdateTaxAsync(_tax.Id, _tax);
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
                    MessageBox.Show("Failed to save tax", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error saving tax: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (nudDefaultRate.Value < 0 || nudDefaultRate.Value > 100)
            {
                MessageBox.Show("Default rate must be between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudDefaultRate.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void TaxEditForm_KeyDown(object? sender, KeyEventArgs e)
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
