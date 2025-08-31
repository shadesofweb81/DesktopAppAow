using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Tax
{
    // Helper class for displaying enum values in ComboBox
    public class TaxCategoryDisplayItem
    {
        public TaxCategory Value { get; set; }
        public string DisplayName { get; set; }

        public TaxCategoryDisplayItem(TaxCategory value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public partial class TaxEditForm : BaseForm
    {
        private readonly TaxService _taxService;
        private readonly LocalStorageService _localStorageService;
        private TaxModel? _tax;
        private TaxByIdDto? _taxDto;
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
        private DataGridView dgvComponents = null!;
        private Button btnAddComponent = null!;
        private Button btnEditComponent = null!;
        private Button btnDeleteComponent = null!;
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
            dgvComponents = new DataGridView();
            btnAddComponent = new Button();
            btnEditComponent = new Button();
            btnDeleteComponent = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            
            // Create main panel for scrolling
            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.AutoScroll = true;
            mainPanel.Padding = new Padding(10);
            
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 700); // Reduced height for smaller screens
            Name = "TaxEditForm";
            Text = _isNewTax ? "New Tax" : "Edit Tax";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 600); // Set minimum size

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
            mainPanel.Controls.Add(lblName);
            mainPanel.Controls.Add(txtName);
            y += spacing;

            // Description
            var lblDescription = new Label { Text = "Description:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtDescription.Location = new Point(controlX, y);
            txtDescription.Size = new Size(controlWidth, controlHeight);
            txtDescription.MaxLength = 500;
            mainPanel.Controls.Add(lblDescription);
            mainPanel.Controls.Add(txtDescription);
            y += spacing;

            // Category
            var lblCategory = new Label { Text = "Category:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            cmbCategory.Location = new Point(controlX, y);
            cmbCategory.Size = new Size(controlWidth, controlHeight);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            mainPanel.Controls.Add(lblCategory);
            mainPanel.Controls.Add(cmbCategory);
            y += spacing;

            // Default Rate
            var lblDefaultRate = new Label { Text = "Default Rate (%):", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            nudDefaultRate.Location = new Point(controlX, y);
            nudDefaultRate.Size = new Size(controlWidth, controlHeight);
            nudDefaultRate.DecimalPlaces = 2;
            nudDefaultRate.Minimum = 0;
            nudDefaultRate.Maximum = 100;
            nudDefaultRate.Increment = 0.01m;
            mainPanel.Controls.Add(lblDefaultRate);
            mainPanel.Controls.Add(nudDefaultRate);
            y += spacing;

            // HSN Code
            var lblHSNCode = new Label { Text = "HSN Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtHSNCode.Location = new Point(controlX, y);
            txtHSNCode.Size = new Size(controlWidth, controlHeight);
            txtHSNCode.MaxLength = 50;
            mainPanel.Controls.Add(lblHSNCode);
            mainPanel.Controls.Add(txtHSNCode);
            y += spacing;

            // Section Code
            var lblSectionCode = new Label { Text = "Section Code:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtSectionCode.Location = new Point(controlX, y);
            txtSectionCode.Size = new Size(controlWidth, controlHeight);
            txtSectionCode.MaxLength = 50;
            mainPanel.Controls.Add(lblSectionCode);
            mainPanel.Controls.Add(txtSectionCode);
            y += spacing;

            // Return Form Number
            var lblReturnFormNumber = new Label { Text = "Return Form Number:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtReturnFormNumber.Location = new Point(controlX, y);
            txtReturnFormNumber.Size = new Size(controlWidth, controlHeight);
            txtReturnFormNumber.MaxLength = 50;
            mainPanel.Controls.Add(lblReturnFormNumber);
            mainPanel.Controls.Add(txtReturnFormNumber);
            y += spacing;

            // Is Active
            chkIsActive.Location = new Point(controlX, y);
            chkIsActive.Size = new Size(200, controlHeight);
            chkIsActive.Text = "Is Active";
            chkIsActive.Checked = true;
            mainPanel.Controls.Add(chkIsActive);
            y += spacing;

            // Is Composite
            chkIsComposite.Location = new Point(controlX, y);
            chkIsComposite.Size = new Size(200, controlHeight);
            chkIsComposite.Text = "Is Composite";
            mainPanel.Controls.Add(chkIsComposite);
            y += spacing;

            // Is Reverse Charge Applicable
            chkIsReverseChargeApplicable.Location = new Point(controlX, y);
            chkIsReverseChargeApplicable.Size = new Size(250, controlHeight);
            chkIsReverseChargeApplicable.Text = "Is Reverse Charge Applicable";
            mainPanel.Controls.Add(chkIsReverseChargeApplicable);
            y += spacing;

            // Is Deductible At Source
            chkIsDeductibleAtSource.Location = new Point(controlX, y);
            chkIsDeductibleAtSource.Size = new Size(200, controlHeight);
            chkIsDeductibleAtSource.Text = "Is Deductible At Source";
            mainPanel.Controls.Add(chkIsDeductibleAtSource);
            y += spacing;

            // Is Collected At Source
            chkIsCollectedAtSource.Location = new Point(controlX, y);
            chkIsCollectedAtSource.Size = new Size(200, controlHeight);
            chkIsCollectedAtSource.Text = "Is Collected At Source";
            mainPanel.Controls.Add(chkIsCollectedAtSource);
            y += spacing + 20;

            // Components Section
            var lblComponents = new Label { Text = "Tax Components:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight), Font = new Font(Font, FontStyle.Bold) };
            mainPanel.Controls.Add(lblComponents);
            y += spacing;

            // Components DataGridView
            dgvComponents.Location = new Point(labelX, y);
            dgvComponents.Size = new Size(900, 200);
            dgvComponents.AllowUserToAddRows = false;
            dgvComponents.AllowUserToDeleteRows = false;
            dgvComponents.ReadOnly = true;
            dgvComponents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvComponents.MultiSelect = false;
            dgvComponents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvComponents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            // Add columns
            dgvComponents.Columns.Add("Name", "Name");
            dgvComponents.Columns.Add("Description", "Description");
            dgvComponents.Columns.Add("Type", "Type");
            dgvComponents.Columns.Add("Rate", "Rate (%)");
            dgvComponents.Columns.Add("LedgerCode", "Ledger Code");
            dgvComponents.Columns.Add("IsActive", "Active");
            
            mainPanel.Controls.Add(dgvComponents);
            y += 220;

            // Component buttons
            btnAddComponent.Location = new Point(labelX, y);
            btnAddComponent.Size = new Size(100, 30);
            btnAddComponent.Text = "Add";
            btnAddComponent.Click += btnAddComponent_Click;
            mainPanel.Controls.Add(btnAddComponent);

            btnEditComponent.Location = new Point(labelX + 110, y);
            btnEditComponent.Size = new Size(100, 30);
            btnEditComponent.Text = "Edit";
            btnEditComponent.Click += btnEditComponent_Click;
            mainPanel.Controls.Add(btnEditComponent);

            btnDeleteComponent.Location = new Point(labelX + 220, y);
            btnDeleteComponent.Size = new Size(100, 30);
            btnDeleteComponent.Text = "Delete";
            btnDeleteComponent.Click += btnDeleteComponent_Click;
            mainPanel.Controls.Add(btnDeleteComponent);

            y += 40;

            // Add some spacing before the main action buttons
            y += 20;

            // Main action buttons (Save/Cancel) - positioned on the right side
            var buttonRightX = 650; // Position buttons on the right side
            btnSave.Location = new Point(buttonRightX, y);
            btnSave.Size = new Size(100, 35);
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            mainPanel.Controls.Add(btnSave);

            btnCancel.Location = new Point(buttonRightX + 120, y);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            mainPanel.Controls.Add(btnCancel);

            // Status label
            lblStatus.Location = new Point(labelX, y + 50);
            lblStatus.Size = new Size(700, 25);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            mainPanel.Controls.Add(lblStatus);

            // Set panel size to accommodate all controls
            mainPanel.Size = new Size(980, y + 100);
            
            // Add panel to form
            Controls.Add(mainPanel);

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
                // Load categories with display names
                cmbCategory.Items.Clear();
                var categories = Enum.GetValues<TaxCategory>();
                var displayItems = new List<TaxCategoryDisplayItem>();
                
                foreach (var category in categories)
                {
                    string displayName = GetCategoryDisplayName(category);
                    displayItems.Add(new TaxCategoryDisplayItem(category, displayName));
                }
                
                cmbCategory.Items.AddRange(displayItems.ToArray());

                if (_isNewTax)
                {
                    _tax = new TaxModel();
                    var company = await _localStorageService.GetSelectedCompanyAsync();
                    if (company != null)
                    {
                        _tax.CompanyId = Guid.Parse(company.Id);
                    }
                    
                    // Set default category for new tax
                    if (cmbCategory.Items.Count > 0)
                    {
                        cmbCategory.SelectedIndex = 0; // Select first category as default
                    }
                }
                else
                {
                    // Load existing tax data using DTO for complete details
                    await LoadExistingTaxData();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExistingTaxData()
        {
            if (_tax == null) return;

            try
            {
                // Load full tax details using the new DTO
                _taxDto = await _taxService.GetTaxByIdDtoAsync(_tax.Id);
                
                if (_taxDto != null)
                {
                    // Load form data from DTO
                    txtName.Text = _taxDto.Name;
                    txtDescription.Text = _taxDto.Description;
                    
                    // Set category - find the matching display item
                    if (Enum.TryParse<TaxCategory>(_taxDto.Category, out var category))
                    {
                        if (cmbCategory.Items.Count > 0)
                        {
                            for (int i = 0; i < cmbCategory.Items.Count; i++)
                            {
                                if (cmbCategory.Items[i] is TaxCategoryDisplayItem item && item.Value == category)
                                {
                                    cmbCategory.SelectedIndex = i;
                                    break;
                                }
                            }
                            
                            // If not found, set to first item as fallback
                            if (cmbCategory.SelectedIndex == -1 && cmbCategory.Items.Count > 0)
                            {
                                cmbCategory.SelectedIndex = 0;
                            }
                        }
                    }
                    
                    nudDefaultRate.Value = _taxDto.DefaultRate;
                    txtHSNCode.Text = _taxDto.HSNCode ?? "";
                    txtSectionCode.Text = _taxDto.SectionCode ?? "";
                    txtReturnFormNumber.Text = _taxDto.ReturnFormNumber ?? "";
                    chkIsActive.Checked = _taxDto.IsActive;
                    chkIsComposite.Checked = _taxDto.IsComposite;
                    chkIsReverseChargeApplicable.Checked = _taxDto.IsReverseChargeApplicable;
                    chkIsDeductibleAtSource.Checked = _taxDto.IsDeductibleAtSource;
                    chkIsCollectedAtSource.Checked = _taxDto.IsCollectedAtSource;

                    // Load components from DTO
                    LoadComponentsFromDto();
                    
                    lblStatus.Text = $"Loaded tax: {_taxDto.Name} with {_taxDto.Components.Count} components";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    // Fallback to original tax data if DTO load fails
                    LoadFallbackTaxData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tax details: {ex.Message}");
                LoadFallbackTaxData();
            }
        }

        private void LoadFallbackTaxData()
        {
            if (_tax == null) return;

            txtName.Text = _tax.Name;
            txtDescription.Text = _tax.Description;
            
            // Set category - find the matching display item
            if (cmbCategory.Items.Count > 0)
            {
                for (int i = 0; i < cmbCategory.Items.Count; i++)
                {
                    if (cmbCategory.Items[i] is TaxCategoryDisplayItem item && item.Value == _tax.Category)
                    {
                        cmbCategory.SelectedIndex = i;
                        break;
                    }
                }
                
                // If not found, set to first item as fallback
                if (cmbCategory.SelectedIndex == -1 && cmbCategory.Items.Count > 0)
                {
                    cmbCategory.SelectedIndex = 0;
                }
            }
            
            nudDefaultRate.Value = _tax.DefaultRate;
            txtHSNCode.Text = _tax.HSNCode ?? "";
            txtSectionCode.Text = _tax.SectionCode ?? "";
            txtReturnFormNumber.Text = _tax.ReturnFormNumber ?? "";
            chkIsActive.Checked = _tax.IsActive;
            chkIsComposite.Checked = _tax.IsComposite;
            chkIsReverseChargeApplicable.Checked = _tax.IsReverseChargeApplicable;
            chkIsDeductibleAtSource.Checked = _tax.IsDeductibleAtSource;
            chkIsCollectedAtSource.Checked = _tax.IsCollectedAtSource;

            // Load components
            LoadComponents();
            
            lblStatus.Text = "Loaded tax data (fallback mode)";
            lblStatus.ForeColor = Color.Orange;
        }

        private string GetCategoryDisplayName(TaxCategory category)
        {
            return category switch
            {
                TaxCategory.GST => "GST - Goods and Services Tax",
                TaxCategory.IncomeTax => "Income Tax - TDS, TCS",
                TaxCategory.CustomsDuty => "Customs Duty - Import/Export",
                TaxCategory.Cess => "Cess - Various Types",
                TaxCategory.ServiceTax => "Service Tax",
                TaxCategory.VAT => "VAT - Value Added Tax",
                TaxCategory.Other => "Other - Miscellaneous Taxes",
                _ => category.ToString()
            };
        }

        private void LoadComponents()
        {
            dgvComponents.Rows.Clear();
            
            if (_tax?.Components != null)
            {
                foreach (var component in _tax.Components)
                {
                    dgvComponents.Rows.Add(
                        component.Name,
                        component.Description,
                        component.Type.ToString(),
                        component.Rate.ToString("F2"),
                        component.LedgerCode,
                        component.IsActive ? "Yes" : "No"
                    );
                }
            }
        }

        private void LoadComponentsFromDto()
        {
            dgvComponents.Rows.Clear();
            
            if (_taxDto?.Components != null)
            {
                Console.WriteLine($"Loading {_taxDto.Components.Count} components from DTO");
                
                foreach (var comp in _taxDto.Components)
                {
                    Console.WriteLine($"Adding component: {comp.Name}, Type: {comp.Type}, Rate: {comp.Rate}");
                    
                    dgvComponents.Rows.Add(
                        comp.Name,
                        comp.Description,
                        comp.Type,
                        comp.Rate.ToString("F2"),
                        comp.LedgerCode,
                        comp.IsActive ? "Yes" : "No"
                    );
                }
                
                Console.WriteLine($"Added {dgvComponents.Rows.Count} rows to components grid");
            }
            else
            {
                Console.WriteLine("No components found in DTO");
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
            
            // Disable component controls
            dgvComponents.Enabled = false;
            btnAddComponent.Enabled = false;
            btnEditComponent.Enabled = false;
            btnDeleteComponent.Enabled = false;
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
                _tax.Category = cmbCategory.SelectedItem is TaxCategoryDisplayItem item ? item.Value : TaxCategory.Other;
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

            if (cmbCategory.SelectedItem == null || cmbCategory.SelectedItem is not TaxCategoryDisplayItem)
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

        private void btnAddComponent_Click(object? sender, EventArgs e)
        {
            try
            {
                var componentForm = new TaxComponentEditForm();
                if (componentForm.ShowDialog() == DialogResult.OK)
                {
                    var newComponent = componentForm.GetComponent();
                    if (newComponent != null)
                    {
                        // Set the tax ID for the new component
                        newComponent.TaxId = _tax!.Id;
                        
                        // Add to the tax's components list
                        if (_tax.Components == null)
                            _tax.Components = new List<TaxComponent>();
                        
                        _tax.Components.Add(newComponent);
                        
                        // Refresh the components grid
                        LoadComponents();
                        
                        lblStatus.Text = "Component added successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error adding component: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error adding component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditComponent_Click(object? sender, EventArgs e)
        {
            if (dgvComponents.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedIndex = dgvComponents.SelectedRows[0].Index;
                    if (_tax?.Components != null)
                    {
                        var componentToEdit = _tax.Components.ElementAt(selectedIndex);
                        var componentForm = new TaxComponentEditForm(componentToEdit);
                        
                        if (componentForm.ShowDialog() == DialogResult.OK)
                        {
                            var updatedComponent = componentForm.GetComponent();
                            if (updatedComponent != null)
                            {
                                // Remove the old component and add the updated one
                                _tax.Components.Remove(componentToEdit);
                                _tax.Components.Add(updatedComponent);
                                
                                // Refresh the components grid
                                LoadComponents();
                                
                                lblStatus.Text = "Component updated successfully";
                                lblStatus.ForeColor = Color.Green;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error editing component: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Error editing component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a component to edit", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteComponent_Click(object? sender, EventArgs e)
        {
            if (dgvComponents.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this component?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                                            var selectedIndex = dgvComponents.SelectedRows[0].Index;
                    if (_tax?.Components != null)
                    {
                        var componentToDelete = _tax.Components.ElementAt(selectedIndex);
                        // Remove the component from the list
                        _tax.Components.Remove(componentToDelete);
                        
                        // Refresh the components grid
                        LoadComponents();
                        
                        lblStatus.Text = "Component deleted successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = $"Error deleting component: {ex.Message}";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"Error deleting component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a component to delete", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
