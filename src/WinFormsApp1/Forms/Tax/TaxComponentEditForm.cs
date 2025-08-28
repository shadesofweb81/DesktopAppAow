using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Tax
{
    public partial class TaxComponentEditForm : Form
    {
        private TaxComponent? _component;
        private bool _isNewComponent;
        private bool _isReadOnly = false;
        private readonly LedgerService _ledgerService;

        // Form controls
        private TextBox txtName = null!;
        private TextBox txtDescription = null!;
        private ComboBox cmbType = null!;
        private NumericUpDown nudRate = null!;
        private TextBox txtLedgerCode = null!;
        private CheckBox chkIsCreditAllowed = null!;
        private TextBox txtReturnFormNumber = null!;
        private ComboBox cmbLedger = null!;
        private ComboBox cmbReceivableLedger = null!;
        private ComboBox cmbPayableLedger = null!;
        private CheckBox chkIsActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;

        public TaxComponentEditForm(TaxComponent? component = null)
        {
            _component = component;
            _isNewComponent = component == null;
            _ledgerService = new LedgerService(new AuthService());
            InitializeComponent();
            SetupForm();
            LoadData();
            
            // Add keyboard event handler for ESC key
            this.KeyPreview = true;
            this.KeyDown += TaxComponentEditForm_KeyDown;
        }

        private void InitializeComponent()
        {
            txtName = new TextBox();
            txtDescription = new TextBox();
            cmbType = new ComboBox();
            nudRate = new NumericUpDown();
            txtLedgerCode = new TextBox();
            chkIsCreditAllowed = new CheckBox();
            txtReturnFormNumber = new TextBox();
            cmbLedger = new ComboBox();
            cmbReceivableLedger = new ComboBox();
            cmbPayableLedger = new ComboBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 500);
            Name = "TaxComponentEditForm";
            Text = _isNewComponent ? "New Tax Component" : "Edit Tax Component";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Labels and controls layout
            var y = 30;
            var labelX = 30;
            var controlX = 200;
            var controlWidth = 350;
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

            // Type
            var lblType = new Label { Text = "Type:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbType.Location = new Point(controlX, y);
            cmbType.Size = new Size(controlWidth, controlHeight);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.DisplayMember = "DisplayName";
            cmbType.ValueMember = "Value";
            Controls.Add(lblType);
            Controls.Add(cmbType);
            y += spacing;

            // Rate
            var lblRate = new Label { Text = "Rate (%):", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            nudRate.Location = new Point(controlX, y);
            nudRate.Size = new Size(controlWidth, controlHeight);
            nudRate.DecimalPlaces = 2;
            nudRate.Minimum = 0;
            nudRate.Maximum = 100;
            nudRate.Increment = 0.01m;
            Controls.Add(lblRate);
            Controls.Add(nudRate);
            y += spacing;

            // Ledger Code
            var lblLedgerCode = new Label { Text = "Ledger Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtLedgerCode.Location = new Point(controlX, y);
            txtLedgerCode.Size = new Size(controlWidth, controlHeight);
            txtLedgerCode.MaxLength = 50;
            Controls.Add(lblLedgerCode);
            Controls.Add(txtLedgerCode);
            y += spacing;

            // Primary Ledger
            var lblLedger = new Label { Text = "Primary Ledger:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbLedger.Location = new Point(controlX, y);
            cmbLedger.Size = new Size(controlWidth, controlHeight);
            cmbLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblLedger);
            Controls.Add(cmbLedger);
            y += spacing;

            // Receivable Ledger
            var lblReceivableLedger = new Label { Text = "Receivable Ledger:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbReceivableLedger.Location = new Point(controlX, y);
            cmbReceivableLedger.Size = new Size(controlWidth, controlHeight);
            cmbReceivableLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblReceivableLedger);
            Controls.Add(cmbReceivableLedger);
            y += spacing;

            // Payable Ledger
            var lblPayableLedger = new Label { Text = "Payable Ledger:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbPayableLedger.Location = new Point(controlX, y);
            cmbPayableLedger.Size = new Size(controlWidth, controlHeight);
            cmbPayableLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(lblPayableLedger);
            Controls.Add(cmbPayableLedger);
            y += spacing;

            // Return Form Number
            var lblReturnFormNumber = new Label { Text = "Return Form Number:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtReturnFormNumber.Location = new Point(controlX, y);
            txtReturnFormNumber.Size = new Size(controlWidth, controlHeight);
            txtReturnFormNumber.MaxLength = 50;
            Controls.Add(lblReturnFormNumber);
            Controls.Add(txtReturnFormNumber);
            y += spacing;

            // Is Credit Allowed
            chkIsCreditAllowed.Location = new Point(controlX, y);
            chkIsCreditAllowed.Size = new Size(200, controlHeight);
            chkIsCreditAllowed.Text = "Is Credit Allowed";
            Controls.Add(chkIsCreditAllowed);
            y += spacing;

            // Is Active
            chkIsActive.Location = new Point(controlX, y);
            chkIsActive.Size = new Size(200, controlHeight);
            chkIsActive.Text = "Is Active";
            chkIsActive.Checked = true;
            Controls.Add(chkIsActive);
            y += spacing + 20;

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
            lblStatus.Size = new Size(500, 25);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            Controls.Add(lblStatus);

            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set form properties
            ShowInTaskbar = false;
        }

        private async void LoadData()
        {
            try
            {
                // Load tax types with custom display names
                cmbType.Items.Clear();
                var taxTypes = new[]
                {
                    new { Value = TaxType.CGST, DisplayName = "CGST" },
                    new { Value = TaxType.SGST, DisplayName = "SGST" },
                    new { Value = TaxType.IGST, DisplayName = "IGST" },
                    new { Value = TaxType.UTGST, DisplayName = "UTGST" },
                    new { Value = TaxType.CompensationCess, DisplayName = "Compensation Cess" },
                    new { Value = TaxType.TDS, DisplayName = "TDS" },
                    new { Value = TaxType.TCS, DisplayName = "TCS" },
                    new { Value = TaxType.EducationCess, DisplayName = "Education Cess" },
                    new { Value = TaxType.HigherEducationCess, DisplayName = "Higher Education Cess" },
                    new { Value = TaxType.Other, DisplayName = "Other" }
                };
                
                cmbType.Items.AddRange(taxTypes);

                // Load ledgers
                await LoadLedgers();

                if (_isNewComponent)
                {
                    _component = new TaxComponent();
                }
                else
                {
                    // Load existing component data
                    txtName.Text = _component!.Name;
                    txtDescription.Text = _component.Description;
                    SetSelectedTaxType(_component.Type);
                    nudRate.Value = _component.Rate;
                    txtLedgerCode.Text = _component.LedgerCode;
                    chkIsCreditAllowed.Checked = _component.IsCreditAllowed;
                    txtReturnFormNumber.Text = _component.ReturnFormNumber ?? "";
                    chkIsActive.Checked = _component.IsActive;

                    // Set selected ledgers
                    SetSelectedLedger(cmbLedger, _component.LedgerId);
                    SetSelectedLedger(cmbReceivableLedger, _component.ReceivableLedgerId);
                    SetSelectedLedger(cmbPayableLedger, _component.PayableLedgerId);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadLedgers()
        {
            try
            {
                // Get company ID from localStorage or use a default
                var localStorageService = new LocalStorageService();
                var company = await localStorageService.GetSelectedCompanyAsync();
                var companyId = company != null ? Guid.Parse(company.Id) : Guid.Empty;
                
                var ledgers = await _ledgerService.GetAllLedgersAsync(companyId);
                
                cmbLedger.Items.Clear();
                cmbReceivableLedger.Items.Clear();
                cmbPayableLedger.Items.Clear();

                // Add empty option for optional ledgers
                cmbReceivableLedger.Items.Add(new { Id = (Guid?)null, Name = "-- Select Receivable Ledger --" });
                cmbPayableLedger.Items.Add(new { Id = (Guid?)null, Name = "-- Select Payable Ledger --" });

                foreach (var ledger in ledgers)
                {
                    var item = new { Id = ledger.Id, Name = $"{ledger.Code} - {ledger.Name}" };
                    cmbLedger.Items.Add(item);
                    cmbReceivableLedger.Items.Add(item);
                    cmbPayableLedger.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading ledgers: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void SetSelectedLedger(ComboBox comboBox, Guid? ledgerId)
        {
            if (ledgerId.HasValue)
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    var item = comboBox.Items[i];
                    var idProperty = item.GetType().GetProperty("Id");
                    if (idProperty != null && idProperty.GetValue(item)?.Equals(ledgerId.Value) == true)
                    {
                        comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private Guid? GetSelectedLedgerId(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                var idProperty = comboBox.SelectedItem.GetType().GetProperty("Id");
                if (idProperty != null)
                {
                    return (Guid?)idProperty.GetValue(comboBox.SelectedItem);
                }
            }
            return null;
        }

        private void SetSelectedTaxType(TaxType taxType)
        {
            for (int i = 0; i < cmbType.Items.Count; i++)
            {
                var item = cmbType.Items[i];
                var valueProperty = item.GetType().GetProperty("Value");
                if (valueProperty != null && valueProperty.GetValue(item)?.Equals(taxType) == true)
                {
                    cmbType.SelectedIndex = i;
                    break;
                }
            }
        }

        private TaxType? GetSelectedTaxType()
        {
            if (cmbType.SelectedItem != null)
            {
                var valueProperty = cmbType.SelectedItem.GetType().GetProperty("Value");
                if (valueProperty != null)
                {
                    return (TaxType?)valueProperty.GetValue(cmbType.SelectedItem);
                }
            }
            return null;
        }

        public void SetReadOnlyMode()
        {
            _isReadOnly = true;
            Text = "View Tax Component";
            btnSave.Visible = false;
            btnCancel.Text = "Close";
            
            // Disable all controls
            txtName.ReadOnly = true;
            txtDescription.ReadOnly = true;
            cmbType.Enabled = false;
            nudRate.Enabled = false;
            txtLedgerCode.ReadOnly = true;
            chkIsCreditAllowed.Enabled = false;
            txtReturnFormNumber.ReadOnly = true;
            cmbLedger.Enabled = false;
            cmbReceivableLedger.Enabled = false;
            cmbPayableLedger.Enabled = false;
            chkIsActive.Enabled = false;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (_isReadOnly) return;

            try
            {
                if (!ValidateForm())
                    return;

                lblStatus.Text = "Saving...";
                lblStatus.ForeColor = Color.Blue;

                // Update component model
                _component!.Name = txtName.Text.Trim();
                _component.Description = txtDescription.Text.Trim();
                _component.Type = GetSelectedTaxType() ?? TaxType.Other;
                _component.Rate = nudRate.Value;
                _component.LedgerCode = txtLedgerCode.Text.Trim();
                _component.IsCreditAllowed = chkIsCreditAllowed.Checked;
                _component.ReturnFormNumber = txtReturnFormNumber.Text.Trim();
                _component.IsActive = chkIsActive.Checked;
                _component.LedgerId = GetSelectedLedgerId(cmbLedger) ?? Guid.Empty;
                _component.ReceivableLedgerId = GetSelectedLedgerId(cmbReceivableLedger);
                _component.PayableLedgerId = GetSelectedLedgerId(cmbPayableLedger);

                // Set dialog result to OK to indicate successful save
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error saving component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (GetSelectedTaxType() == null)
            {
                MessageBox.Show("Type is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLedgerCode.Text))
            {
                MessageBox.Show("Ledger Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLedgerCode.Focus();
                return false;
            }

            if (nudRate.Value < 0 || nudRate.Value > 100)
            {
                MessageBox.Show("Rate must be between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudRate.Focus();
                return false;
            }

            if (cmbLedger.SelectedItem == null)
            {
                MessageBox.Show("Primary Ledger is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLedger.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TaxComponentEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        public TaxComponent? GetComponent()
        {
            return _component;
        }
    }
}
