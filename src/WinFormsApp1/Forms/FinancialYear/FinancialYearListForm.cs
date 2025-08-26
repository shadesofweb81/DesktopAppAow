using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.FinancialYear
{
    public partial class FinancialYearListForm : Form
    {
        private readonly FinancialYearService _financialYearService;
        private readonly LocalStorageService _localStorageService;
        private WinFormsApp1.Models.Company? _company;
        private DataGridView dgvFinancialYears = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnSetActive = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private Label lblCompanyInfo = null!;
        private List<FinancialYearModel> _financialYears = new List<FinancialYearModel>();
        private FinancialYearModel? _selectedFinancialYear;

        public FinancialYearListForm(FinancialYearService financialYearService)
        {
            _financialYearService = financialYearService;
            _localStorageService = new LocalStorageService();
            InitializeComponent();
            SetupForm();
            LoadCompanyAndFinancialYears();
        }

        private void InitializeComponent()
        {
            dgvFinancialYears = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnSetActive = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            lblCompanyInfo = new Label();
            SuspendLayout();
            
            // 
            // lblCompanyInfo
            // 
            lblCompanyInfo.Location = new Point(12, 9);
            lblCompanyInfo.Name = "lblCompanyInfo";
            lblCompanyInfo.Size = new Size(600, 25);
            lblCompanyInfo.Text = _company != null ? $"Financial Years for: {_company.DisplayName}" : "No company selected";
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            lblCompanyInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 40);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(600, 50);
            lblInstructions.Text = "Keyboard Navigation: ↑↓ to navigate rows, Enter to edit, Insert for new, Delete to remove, F5 to refresh, Esc to close | Click 'Activate' button in Action column to set financial year as active | Uses selected company from local storage";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // dgvFinancialYears
            // 
            dgvFinancialYears.Location = new Point(12, 85);
            dgvFinancialYears.Name = "dgvFinancialYears";
            dgvFinancialYears.Size = new Size(800, 350);
            dgvFinancialYears.TabIndex = 0;
            dgvFinancialYears.AllowUserToAddRows = false;
            dgvFinancialYears.AllowUserToDeleteRows = false;
            dgvFinancialYears.ReadOnly = true;
            dgvFinancialYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFinancialYears.MultiSelect = false;
            dgvFinancialYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFinancialYears.RowHeadersVisible = false;
            dgvFinancialYears.SelectionChanged += new EventHandler(dgvFinancialYears_SelectionChanged);
            dgvFinancialYears.DoubleClick += new EventHandler(dgvFinancialYears_DoubleClick);
            dgvFinancialYears.KeyDown += new KeyEventHandler(dgvFinancialYears_KeyDown);
            dgvFinancialYears.CellClick += new DataGridViewCellEventHandler(dgvFinancialYears_CellClick);
            
            // 
            // btnNew
            // 
            btnNew.Location = new Point(530, 85);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(100, 30);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (Insert)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);
            
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(530, 125);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "&Edit (Enter)";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Enabled = false;
            
            // 
            // btnSetActive
            // 
            btnSetActive.Location = new Point(530, 165);
            btnSetActive.Name = "btnSetActive";
            btnSetActive.Size = new Size(100, 30);
            btnSetActive.TabIndex = 3;
            btnSetActive.Text = "Set &Active";
            btnSetActive.UseVisualStyleBackColor = true;
            btnSetActive.Click += new EventHandler(btnSetActive_Click);
            btnSetActive.Enabled = false;
            
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(530, 205);
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
            btnRefresh.Location = new Point(530, 245);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "&Refresh (F5)";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 450);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // FinancialYearListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnSetActive);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(dgvFinancialYears);
            Controls.Add(lblInstructions);
            Controls.Add(lblCompanyInfo);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            CancelButton = null;
            Name = "FinancialYearListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _company != null ? $"Financial Years - {_company.DisplayName}" : "Financial Years - No Company Selected";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(FinancialYearListForm_KeyDown);
            Load += new EventHandler(FinancialYearListForm_Load);
            Resize += new EventHandler(FinancialYearListForm_Resize);
            Activated += new EventHandler(FinancialYearListForm_Activated);
            FormClosing += new FormClosingEventHandler(FinancialYearListForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            AcceptButton = btnEdit;
            CancelButton = null;
            dgvFinancialYears.Focus();
        }

        private void FinancialYearListForm_Load(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            ResizeControls();
            dgvFinancialYears.Focus();
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
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
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            int buttonAreaWidth = 150;
            int availableWidth = clientWidth - buttonAreaWidth - 30;
            int availableHeight = clientHeight - 160;
            
            if (availableWidth < 600)
            {
                availableWidth = 600;
            }
            
            dgvFinancialYears.Size = new Size(availableWidth, availableHeight);
            
            int buttonX = availableWidth + 30;
            btnNew.Location = new Point(buttonX, 85);
            btnEdit.Location = new Point(buttonX, 125);
            btnSetActive.Location = new Point(buttonX, 165);
            btnDelete.Location = new Point(buttonX, 205);
            btnRefresh.Location = new Point(buttonX, 245);
            
            lblStatus.Location = new Point(12, clientHeight - 30);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            lblCompanyInfo.Size = new Size(clientWidth - 24, 25);
            lblInstructions.Size = new Size(clientWidth - 24, 50);
        }

        private void FinancialYearListForm_Resize(object? sender, EventArgs e)
        {
            ResizeControls();
        }

        private void FinancialYearListForm_Activated(object? sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
            this.BringToFront();
            this.Activate();
            
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void FinancialYearListForm_FormClosing(object? sender, FormClosingEventArgs e)
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

        private async void LoadCompanyAndFinancialYears()
        {
            try
            {
                _company = await _localStorageService.GetSelectedCompanyAsync();
                
                if (_company == null)
                {
                    lblStatus.Text = "No company selected. Please select a company first.";
                    lblStatus.ForeColor = Color.Orange;
                    lblCompanyInfo.Text = "No company selected";
                    Text = "Financial Years - No Company Selected";
                    UpdateButtonStates();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_company.Id))
                {
                    lblStatus.Text = "Invalid company data. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company data";
                    Text = "Financial Years - Invalid Company Data";
                    UpdateButtonStates();
                    return;
                }
                
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company ID format";
                    Text = "Financial Years - Invalid Company ID";
                    UpdateButtonStates();
                    return;
                }
                
                lblCompanyInfo.Text = $"Financial Years for: {_company.DisplayName}";
                Text = $"Financial Years - {_company.DisplayName}";
                
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
                
                dgvFinancialYears.Columns.Clear();
                dgvFinancialYears.Columns.Add("YearLabel", "Year Label");
                dgvFinancialYears.Columns.Add("StartDate", "Start Date");
                dgvFinancialYears.Columns.Add("EndDate", "End Date");
                dgvFinancialYears.Columns.Add("IsActive", "Active");
                
                // Add button column for activation
                var activateButtonColumn = new DataGridViewButtonColumn();
                activateButtonColumn.Name = "ActivateButton";
                activateButtonColumn.HeaderText = "Action";
                activateButtonColumn.Text = "Activate";
                activateButtonColumn.UseColumnTextForButtonValue = true;
                dgvFinancialYears.Columns.Add(activateButtonColumn);
                
                if (dgvFinancialYears.Columns["YearLabel"] != null) dgvFinancialYears.Columns["YearLabel"].Width = 120;
                if (dgvFinancialYears.Columns["StartDate"] != null) dgvFinancialYears.Columns["StartDate"].Width = 100;
                if (dgvFinancialYears.Columns["EndDate"] != null) dgvFinancialYears.Columns["EndDate"].Width = 100;
                if (dgvFinancialYears.Columns["IsActive"] != null) dgvFinancialYears.Columns["IsActive"].Width = 60;
                if (dgvFinancialYears.Columns["ActivateButton"] != null) dgvFinancialYears.Columns["ActivateButton"].Width = 80;
                
                foreach (var financialYear in _financialYears)
                {
                    int rowIndex = dgvFinancialYears.Rows.Add(
                        financialYear.YearLabel ?? "",
                        financialYear.StartDate.ToString("dd/MM/yyyy"),
                        financialYear.EndDate.ToString("dd/MM/yyyy"),
                        financialYear.IsActive ? "Yes" : "No",
                        financialYear.IsActive ? "" : "Activate"
                    );
                    
                    // Store the financial year ID in the row tag for easy access
                    dgvFinancialYears.Rows[rowIndex].Tag = financialYear;
                }
                
                lblStatus.Text = $"Loaded {_financialYears.Count} financial years";
                lblStatus.ForeColor = Color.Green;
                
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading financial years: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load financial years exception: {ex.Message}");
            }
        }

        private void UpdateButtonStates()
        {
            bool hasCompany = _company != null;
            bool hasSelection = _selectedFinancialYear != null;
            bool isSelectedActive = _selectedFinancialYear?.IsActive ?? false;
            
            btnNew.Enabled = hasCompany;
            btnEdit.Enabled = hasCompany && hasSelection;
            btnSetActive.Enabled = hasCompany && hasSelection && !isSelectedActive;
            btnDelete.Enabled = hasCompany && hasSelection;
            btnRefresh.Enabled = hasCompany;
        }

        private void dgvFinancialYears_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvFinancialYears.CurrentRow != null && dgvFinancialYears.CurrentRow.Index >= 0 && dgvFinancialYears.CurrentRow.Index < _financialYears.Count)
            {
                _selectedFinancialYear = _financialYears[dgvFinancialYears.CurrentRow.Index];
            }
            else
            {
                _selectedFinancialYear = null;
            }
            UpdateButtonStates();
        }

        private void dgvFinancialYears_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedFinancialYear != null)
            {
                EditFinancialYear();
            }
        }

        private void dgvFinancialYears_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedFinancialYear != null)
                    {
                        EditFinancialYear();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.Insert:
                    NewFinancialYear();
                    e.Handled = true;
                    break;
                    
                case Keys.Delete:
                    if (_selectedFinancialYear != null)
                    {
                        DeleteFinancialYear();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadFinancialYears());
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
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private async void dgvFinancialYears_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex == dgvFinancialYears.Columns["ActivateButton"].Index)
            {
                var row = dgvFinancialYears.Rows[e.RowIndex];
                if (row.Tag is FinancialYearModel financialYear && !financialYear.IsActive)
                {
                    try
                    {
                        await SetActiveFinancialYear(financialYear);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error activating financial year: {ex.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FinancialYearListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
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
                    _ = Task.Run(async () => await LoadFinancialYears());
                    e.Handled = true;
                    break;
                    
                case Keys.Insert:
                    NewFinancialYear();
                    e.Handled = true;
                    break;
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            NewFinancialYear();
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedFinancialYear != null)
            {
                try
                {
                    await EditFinancialYear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing financial year: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnSetActive_Click(object? sender, EventArgs e)
        {
            if (_selectedFinancialYear != null)
            {
                try
                {
                    await SetActiveFinancialYear(_selectedFinancialYear);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting active financial year: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedFinancialYear != null)
            {
                DeleteFinancialYear();
            }
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadFinancialYears();
        }

        private void NewFinancialYear()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
            {
                if (childForm is FinancialYearEditForm editForm && !editForm.IsEditMode)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            if (!Guid.TryParse(_company.Id, out Guid companyId))
            {
                MessageBox.Show("Invalid company ID format. Please select a company again.", "Invalid Company", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var financialYearEditForm = new FinancialYearEditForm(_financialYearService, null, companyId)
            {
                MdiParent = this.MdiParent,
                Text = "New Financial Year",
                WindowState = FormWindowState.Maximized
            };

            financialYearEditForm.Show();
            financialYearEditForm.FormClosed += (s, e) => LoadFinancialYears();
        }

        private async Task EditFinancialYear()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_selectedFinancialYear != null)
            {
                foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
                {
                    if (childForm is FinancialYearEditForm editForm && editForm.IsEditMode && editForm.FinancialYearId == _selectedFinancialYear.Id)
                    {
                        childForm.BringToFront();
                        childForm.Activate();
                        return;
                    }
                }

                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    MessageBox.Show("Invalid company ID format. Please select a company again.", "Invalid Company", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var financialYearEditForm = new FinancialYearEditForm(_financialYearService, _selectedFinancialYear, companyId)
                {
                    MdiParent = this.MdiParent,
                    Text = $"Edit Financial Year - {_selectedFinancialYear.YearLabel}",
                    WindowState = FormWindowState.Maximized
                };

                financialYearEditForm.Show();
                financialYearEditForm.FormClosed += (s, e) => LoadFinancialYears();
            }
        }

        private async Task SetActiveFinancialYear(FinancialYearModel? financialYear = null)
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var targetFinancialYear = financialYear ?? _selectedFinancialYear;
            
            if (targetFinancialYear != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to set '{targetFinancialYear.YearLabel}' as the active financial year for company '{_company.DisplayName}'?",
                    "Confirm Set Active",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        lblStatus.Text = "Setting active financial year...";
                        lblStatus.ForeColor = Color.Blue;
                        
                        if (!Guid.TryParse(_company.Id, out Guid companyId))
                        {
                            lblStatus.Text = "Invalid company ID format";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        
                        var success = await _financialYearService.SetActiveFinancialYearAsync(companyId, targetFinancialYear.Id);
                        
                        if (success)
                        {
                            lblStatus.Text = "Active financial year set successfully";
                            lblStatus.ForeColor = Color.Green;
                            await LoadFinancialYears();
                        }
                        else
                        {
                            lblStatus.Text = "Failed to set active financial year";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = $"Error setting active financial year: {ex.Message}";
                        lblStatus.ForeColor = Color.Red;
                        Console.WriteLine($"Set active financial year exception: {ex.Message}");
                    }
                }
            }
        }

        private async void DeleteFinancialYear()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_selectedFinancialYear != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the financial year '{_selectedFinancialYear.YearLabel}' from company '{_company.DisplayName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        lblStatus.Text = "Deleting financial year...";
                        lblStatus.ForeColor = Color.Blue;
                        
                        var success = await _financialYearService.DeleteFinancialYearAsync(_selectedFinancialYear.Id);
                        
                        if (success)
                        {
                            lblStatus.Text = "Financial year deleted successfully";
                            lblStatus.ForeColor = Color.Green;
                            await LoadFinancialYears();
                        }
                        else
                        {
                            lblStatus.Text = "Failed to delete financial year";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = $"Error deleting financial year: {ex.Message}";
                        lblStatus.ForeColor = Color.Red;
                        Console.WriteLine($"Delete financial year exception: {ex.Message}");
                    }
                }
            }
        }
    }
}


