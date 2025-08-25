using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.FinancialYear
{
    public partial class FinancialYearSelectForm : Form
    {
        private readonly FinancialYearService _financialYearService;
        private readonly LocalStorageService _localStorageService;
        private ComboBox cmbFinancialYears = null!;
        private Button btnSelect = null!;
        private Button btnCancel = null!;
        private Button btnRefresh = null!;
        private Label lblInstructions = null!;
        private Label lblCurrentSelection = null!;
        private Label lblStatus = null!;
        private GroupBox grpFinancialYearDetails = null!;
        private Label lblYearLabel = null!;
        private Label lblStartDate = null!;
        private Label lblEndDate = null!;
        private Label lblIsActive = null!;
        private List<FinancialYearModel> _financialYears = new List<FinancialYearModel>();
        private FinancialYearModel? _selectedFinancialYear;
        private FinancialYearModel? _currentStoredFinancialYear;
        private WinFormsApp1.Models.Company? _company;

        public FinancialYearSelectForm(FinancialYearService financialYearService, LocalStorageService localStorageService)
        {
            _financialYearService = financialYearService;
            _localStorageService = localStorageService;
            InitializeComponent();
            SetupForm();
            LoadCurrentSelection();
            LoadCompanyAndFinancialYears();
        }

        private void InitializeComponent()
        {
            lblInstructions = new Label();
            lblCurrentSelection = new Label();
            cmbFinancialYears = new ComboBox();
            btnSelect = new Button();
            btnCancel = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            grpFinancialYearDetails = new GroupBox();
            lblYearLabel = new Label();
            lblStartDate = new Label();
            lblEndDate = new Label();
            lblIsActive = new Label();
            SuspendLayout();

            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(550, 40);
            lblInstructions.Text = "Select Financial Year: ↑↓ or type to select, Enter to confirm, F5 to refresh, Esc to cancel";
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
            // cmbFinancialYears
            // 
            cmbFinancialYears.Location = new Point(12, 85);
            cmbFinancialYears.Name = "cmbFinancialYears";
            cmbFinancialYears.Size = new Size(400, 23);
            cmbFinancialYears.TabIndex = 0;
            cmbFinancialYears.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFinancialYears.DisplayMember = "YearLabel";
            cmbFinancialYears.SelectedIndexChanged += new EventHandler(cmbFinancialYears_SelectedIndexChanged);
            cmbFinancialYears.KeyDown += new KeyEventHandler(cmbFinancialYears_KeyDown);

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
            // grpFinancialYearDetails
            // 
            grpFinancialYearDetails.Location = new Point(12, 125);
            grpFinancialYearDetails.Name = "grpFinancialYearDetails";
            grpFinancialYearDetails.Size = new Size(568, 150);
            grpFinancialYearDetails.TabIndex = 3;
            grpFinancialYearDetails.TabStop = false;
            grpFinancialYearDetails.Text = "Financial Year Details";

            // 
            // lblYearLabel
            // 
            lblYearLabel.Location = new Point(15, 25);
            lblYearLabel.Name = "lblYearLabel";
            lblYearLabel.Size = new Size(540, 25);
            lblYearLabel.Text = "Year Label: ";
            lblYearLabel.Font = new Font("Arial", 10, FontStyle.Bold);

            // 
            // lblStartDate
            // 
            lblStartDate.Location = new Point(15, 50);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(540, 20);
            lblStartDate.Text = "Start Date: ";

            // 
            // lblEndDate
            // 
            lblEndDate.Location = new Point(15, 75);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(540, 20);
            lblEndDate.Text = "End Date: ";

            // 
            // lblIsActive
            // 
            lblIsActive.Location = new Point(15, 100);
            lblIsActive.Name = "lblIsActive";
            lblIsActive.Size = new Size(540, 20);
            lblIsActive.Text = "Is Active: ";

            // Add labels to group box
            grpFinancialYearDetails.Controls.Add(lblYearLabel);
            grpFinancialYearDetails.Controls.Add(lblStartDate);
            grpFinancialYearDetails.Controls.Add(lblEndDate);
            grpFinancialYearDetails.Controls.Add(lblIsActive);

            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(430, 290);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 295);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(400, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // FinancialYearSelectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(grpFinancialYearDetails);
            Controls.Add(btnRefresh);
            Controls.Add(btnSelect);
            Controls.Add(cmbFinancialYears);
            Controls.Add(lblCurrentSelection);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "FinancialYearSelectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Financial Year";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(FinancialYearSelectForm_KeyDown);
            Load += new EventHandler(FinancialYearSelectForm_Load);
            Resize += new EventHandler(FinancialYearSelectForm_Resize);
            Activated += new EventHandler(FinancialYearSelectForm_Activated);
            FormClosing += new FormClosingEventHandler(FinancialYearSelectForm_FormClosing);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            AcceptButton = btnSelect;
            CancelButton = btnCancel;
            ClearFinancialYearDetails();
        }

        private void FinancialYearSelectForm_Load(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            ResizeControls();
            cmbFinancialYears.Focus();
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }
        
        private void FinancialYearSelectForm_Resize(object? sender, EventArgs e)
        {
            ResizeControls();
        }
        
        private void FinancialYearSelectForm_Activated(object? sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }
        
        private void FinancialYearSelectForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
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
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            lblInstructions.Size = new Size(clientWidth - 24, 40);
            
            lblCurrentSelection.Location = new Point(12, 55);
            lblCurrentSelection.Size = new Size(clientWidth - 24, 25);
            
            cmbFinancialYears.Location = new Point(12, 85);
            cmbFinancialYears.Size = new Size(clientWidth - 200, 30);
            
            btnSelect.Location = new Point(clientWidth - 180, 84);
            btnSelect.Size = new Size(80, 32);
            
            btnRefresh.Location = new Point(clientWidth - 90, 84);
            btnRefresh.Size = new Size(80, 32);
            
            grpFinancialYearDetails.Location = new Point(12, 125);
            grpFinancialYearDetails.Size = new Size(clientWidth - 24, 150);
            
            btnCancel.Location = new Point(clientWidth - 100, clientHeight - 50);
            btnCancel.Size = new Size(80, 35);
            
            lblStatus.Location = new Point(12, clientHeight - 45);
            lblStatus.Size = new Size(clientWidth - 120, 25);
        }

        private async void LoadCurrentSelection()
        {
            try
            {
                _currentStoredFinancialYear = await _localStorageService.GetSelectedFinancialYearAsync();
                
                if (_currentStoredFinancialYear != null)
                {
                    lblCurrentSelection.Text = $"Current Selection: {_currentStoredFinancialYear.YearLabel}";
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

        private async void LoadCompanyAndFinancialYears()
        {
            try
            {
                _company = await _localStorageService.GetSelectedCompanyAsync();
                
                if (_company == null)
                {
                    lblStatus.Text = "No company selected. Please select a company first.";
                    lblStatus.ForeColor = Color.Orange;
                    lblCurrentSelection.Text = "No company selected";
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_company.Id))
                {
                    lblStatus.Text = "Invalid company data. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCurrentSelection.Text = "Invalid company data";
                    return;
                }
                
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCurrentSelection.Text = "Invalid company ID format";
                    return;
                }
                
                lblCurrentSelection.Text = $"Financial Years for: {_company.DisplayName}";
                await LoadFinancialYears();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading company: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load company exception: {ex.Message}");
            }
        }

        private async Task LoadFinancialYears()
        {
            try
            {
                if (_company == null)
                {
                    lblStatus.Text = "No company selected";
                    lblStatus.ForeColor = Color.Orange;
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_company.Id))
                {
                    lblStatus.Text = "Invalid company data";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                lblStatus.Text = "Loading financial years...";
                lblStatus.ForeColor = Color.Blue;
                
                var financialYears = await _financialYearService.GetFinancialYearsByCompanyAsync(companyId);
                
                _financialYears = financialYears;
                
                cmbFinancialYears.DataSource = null;
                cmbFinancialYears.DataSource = _financialYears;
                cmbFinancialYears.DisplayMember = "YearLabel";
                
                if (_financialYears.Any())
                {
                    lblStatus.Text = $"Loaded {_financialYears.Count} financial years";
                    lblStatus.ForeColor = Color.Green;
                    
                    if (_currentStoredFinancialYear != null)
                    {
                        var matchingFinancialYear = _financialYears.FirstOrDefault(fy => fy.Id == _currentStoredFinancialYear.Id);
                        if (matchingFinancialYear != null)
                        {
                            cmbFinancialYears.SelectedItem = matchingFinancialYear;
                        }
                    }
                    
                    if (cmbFinancialYears.SelectedIndex == -1 && _financialYears.Any())
                    {
                        cmbFinancialYears.SelectedIndex = 0;
                    }
                }
                else
                {
                    lblStatus.Text = "No financial years found";
                    lblStatus.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading financial years: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void cmbFinancialYears_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedFinancialYear = cmbFinancialYears.SelectedItem as FinancialYearModel;
            btnSelect.Enabled = _selectedFinancialYear != null;
            
            if (_selectedFinancialYear != null)
            {
                DisplayFinancialYearDetails(_selectedFinancialYear);
            }
            else
            {
                ClearFinancialYearDetails();
            }
        }

        private void DisplayFinancialYearDetails(FinancialYearModel financialYear)
        {
            lblYearLabel.Text = $"Year Label: {financialYear.YearLabel}";
            lblStartDate.Text = $"Start Date: {financialYear.StartDate.ToString("dd/MM/yyyy")}";
            lblEndDate.Text = $"End Date: {financialYear.EndDate.ToString("dd/MM/yyyy")}";
            lblIsActive.Text = $"Is Active: {(financialYear.IsActive ? "Yes" : "No")}";
        }

        private void ClearFinancialYearDetails()
        {
            lblYearLabel.Text = "Year Label: ";
            lblStartDate.Text = "Start Date: ";
            lblEndDate.Text = "End Date: ";
            lblIsActive.Text = "Is Active: ";
        }

        private void cmbFinancialYears_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedFinancialYear != null)
                    {
                        SelectFinancialYear();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.F5:
                    _ = RefreshFinancialYears();
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
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

        private void FinancialYearSelectForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    _ = RefreshFinancialYears();
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
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
            await SelectFinancialYear();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
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
            await RefreshFinancialYears();
        }

        private async Task SelectFinancialYear()
        {
            if (_selectedFinancialYear == null)
            {
                MessageBox.Show("Please select a financial year first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "Saving selected financial year...";
                lblStatus.ForeColor = Color.Blue;
                
                await _localStorageService.SaveSelectedFinancialYearAsync(_selectedFinancialYear);
                
                lblStatus.Text = $"Financial year '{_selectedFinancialYear.YearLabel}' selected successfully!";
                lblStatus.ForeColor = Color.Green;
                
                await Task.Delay(1000);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving selection: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"SelectFinancialYear exception: {ex.Message}");
            }
        }

        private async Task RefreshFinancialYears()
        {
            try
            {
                await LoadFinancialYears();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error refreshing financial years: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void ShowHelp()
        {
            var helpMessage = @"Financial Year Selection Help:

Keyboard Navigation:
• ↑↓ Arrow keys - Navigate dropdown
• Enter - Select highlighted financial year
• F5 - Refresh financial year list
• Esc - Cancel and close
• F1 - Show this help

Mouse Operation:
• Click dropdown to expand
• Click financial year to select
• Click Select button to confirm

Features:
• Financial year details shown for selected item
• Current selection displayed at top
• All selections saved to local storage
• Works with currently selected company

The selected financial year will be used as the context for all financial operations in the application.";

            MessageBox.Show(helpMessage, "Financial Year Selection Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
