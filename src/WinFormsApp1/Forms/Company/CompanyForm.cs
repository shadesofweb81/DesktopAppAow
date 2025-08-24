using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanyForm : Form
    {
        private readonly CompanyService _companyService;
        private ListBox lstCompanies = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private List<SelectCompanyModel> _companies = new List<SelectCompanyModel>();
        private SelectCompanyModel? _selectedCompany;

        public CompanyForm(CompanyService companyService)
        {
            _companyService = companyService;
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
            ClientSize = new Size(650, 450);
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
            MinimumSize = new Size(666, 489);
            Name = "CompanyForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Company Management";
            KeyDown += new KeyEventHandler(CompanyForm_KeyDown);
            Load += new EventHandler(CompanyForm_Load);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set focus to the list box for immediate keyboard navigation
            ActiveControl = lstCompanies;
        }

        private void CompanyForm_Load(object? sender, EventArgs e)
        {
            lstCompanies.Focus();
        }

        private async void LoadCompanies()
        {
            try
            {
                lblStatus.Text = "Loading companies...";
                lblStatus.ForeColor = Color.Blue;
                
                _companies = await _companyService.GetAllCompaniesAsync();
                
                Console.WriteLine($"Loaded {_companies.Count} companies:");
                foreach (var company in _companies.Take(5)) // Show first 5 for debugging
                {
                    Console.WriteLine($"  - ID: '{company.Id}', CompanyId: '{company.CompanyId}', Name: '{company.CompanyName}'");
                }
                
                lstCompanies.DataSource = null;
                lstCompanies.DataSource = _companies;
                lstCompanies.DisplayMember = "DisplayName";
                
                if (_companies.Any())
                {
                    lblStatus.Text = $"Loaded {_companies.Count} companies";
                    lblStatus.ForeColor = Color.Green;
                    lstCompanies.SelectedIndex = 0;
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

        private void lstCompanies_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedCompany = lstCompanies.SelectedItem as SelectCompanyModel;
            btnEdit.Enabled = _selectedCompany != null;
            btnView.Enabled = _selectedCompany != null;
            btnDelete.Enabled = _selectedCompany != null;
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
            var companyEditForm = new CompanyEditForm(_companyService, null);
            if (companyEditForm.ShowDialog() == DialogResult.OK)
            {
                LoadCompanies();
            }
        }

        private async Task EditCompany()
        {
            if (_selectedCompany != null)
            {
                Console.WriteLine($"EditCompany called for: ID='{_selectedCompany.Id}', CompanyId='{_selectedCompany.CompanyId}', Name='{_selectedCompany.CompanyName}'");
                try
                {
                    lblStatus.Text = "Loading company data...";
                    lblStatus.ForeColor = Color.Blue;

                    // Get the full company data from API using GetCompanyByIdAsync
                    Console.WriteLine($"Attempting to parse company ID: '{_selectedCompany.Id}'");
                    Console.WriteLine($"Attempting to parse company CompanyId: '{_selectedCompany.CompanyId}'");
                    
                    Guid companyId = Guid.Empty;
                    bool idParsed = false;
                    
                    // Try to parse the Id field first
                    if (Guid.TryParse(_selectedCompany.Id, out companyId))
                    {
                        Console.WriteLine($"Successfully parsed company ID: {companyId}");
                        idParsed = true;
                    }
                    // If Id field didn't work, try CompanyId field
                    else if (Guid.TryParse(_selectedCompany.CompanyId, out companyId))
                    {
                        Console.WriteLine($"Successfully parsed company CompanyId: {companyId}");
                        idParsed = true;
                    }
                    
                    if (idParsed)
                    {
                        var editCompanyModel = await _companyService.GetEditCompanyByIdAsync(companyId);
                        
                        if (editCompanyModel != null)
                        {
                            lblStatus.Text = "Company data loaded successfully";
                            lblStatus.ForeColor = Color.Green;
                            
                            // Convert EditCompanyModel to Company for the edit form
                            var companyForEdit = editCompanyModel.ToCompany();
                            
                            var companyEditForm = new CompanyEditForm(_companyService, companyForEdit);
                            if (companyEditForm.ShowDialog() == DialogResult.OK)
                            {
                                LoadCompanies();
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
                    else
                    {
                        Console.WriteLine($"Failed to parse company ID: '{_selectedCompany.Id}' and CompanyId: '{_selectedCompany.CompanyId}' as GUID");
                        lblStatus.Text = "Invalid company ID format";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"The selected company has invalid ID formats: ID='{_selectedCompany.Id}', CompanyId='{_selectedCompany.CompanyId}'. Please try selecting a different company.", 
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

        private async Task ViewCompany()
        {
            if (_selectedCompany != null)
            {
                Console.WriteLine($"ViewCompany called for: ID='{_selectedCompany.Id}', CompanyId='{_selectedCompany.CompanyId}', Name='{_selectedCompany.CompanyName}'");
                try
                {
                    lblStatus.Text = "Loading company data...";
                    lblStatus.ForeColor = Color.Blue;

                    // Get the full company data from API using GetEditCompanyByIdAsync
                    Console.WriteLine($"Attempting to parse company ID: '{_selectedCompany.Id}'");
                    Console.WriteLine($"Attempting to parse company CompanyId: '{_selectedCompany.CompanyId}'");
                    
                    Guid companyId = Guid.Empty;
                    bool idParsed = false;
                    
                    // Try to parse the Id field first
                    if (Guid.TryParse(_selectedCompany.Id, out companyId))
                    {
                        Console.WriteLine($"Successfully parsed company ID: {companyId}");
                        idParsed = true;
                    }
                    // If Id field didn't work, try CompanyId field
                    else if (Guid.TryParse(_selectedCompany.CompanyId, out companyId))
                    {
                        Console.WriteLine($"Successfully parsed company CompanyId: {companyId}");
                        idParsed = true;
                    }
                    
                    if (idParsed)
                    {
                        var editCompanyModel = await _companyService.GetEditCompanyByIdAsync(companyId);
                        
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
                    else
                    {
                        Console.WriteLine($"Failed to parse company ID: '{_selectedCompany.Id}' and CompanyId: '{_selectedCompany.CompanyId}' as GUID");
                        lblStatus.Text = "Invalid company ID format";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"The selected company has invalid ID formats: ID='{_selectedCompany.Id}', CompanyId='{_selectedCompany.CompanyId}'. Please try selecting a different company.", 
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

                    if (Guid.TryParse(_selectedCompany.Id, out var companyId))
                    {
                        var success = await _companyService.DeleteCompanyAsync(companyId);
                        
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
                    else
                    {
                        lblStatus.Text = "Invalid company ID format";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show("The selected company has an invalid ID format. Cannot delete.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
