using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Tax
{
    public partial class TaxListForm : Form
    {
        private readonly TaxService _taxService;
        private readonly LocalStorageService _localStorageService;
        private List<TaxModel> _taxes = new List<TaxModel>();
        private TaxModel? _selectedTax;

        // Form controls
        private DataGridView dgvTaxes = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;

        // Pagination fields (for future use)
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalCount = 0;
        private int _totalPages = 0;

        public TaxListForm(TaxService taxService)
        {
            _taxService = taxService;
            _localStorageService = new LocalStorageService();
            InitializeComponent();
            LoadTaxes();
        }

        private void InitializeComponent()
        {
            dgvTaxes = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnView = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 650);
            Name = "TaxListForm";
            Text = "Tax Management";
            StartPosition = FormStartPosition.CenterParent;

            // DataGridView
            dgvTaxes.Location = new Point(20, 20);
            dgvTaxes.Size = new Size(1160, 500);
            dgvTaxes.AllowUserToAddRows = false;
            dgvTaxes.ReadOnly = true;
            dgvTaxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaxes.MultiSelect = false;
            dgvTaxes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaxes.RowHeadersVisible = false;
            dgvTaxes.SelectionChanged += dgvTaxes_SelectionChanged;
            dgvTaxes.DoubleClick += dgvTaxes_DoubleClick;
            dgvTaxes.KeyDown += dgvTaxes_KeyDown;

            // Add columns
            dgvTaxes.Columns.Add("Name", "Name");
            dgvTaxes.Columns.Add("Category", "Category");
            dgvTaxes.Columns.Add("DefaultRate", "Default Rate (%)");
            dgvTaxes.Columns.Add("HSNCode", "HSN Code");
            dgvTaxes.Columns.Add("IsActive", "Active");
            dgvTaxes.Columns.Add("IsComposite", "Composite");

            Controls.Add(dgvTaxes);

            // Buttons
            btnNew.Location = new Point(20, 540);
            btnNew.Size = new Size(100, 35);
            btnNew.Text = "New Tax";
            btnNew.Click += btnNew_Click;
            Controls.Add(btnNew);

            btnEdit.Location = new Point(130, 540);
            btnEdit.Size = new Size(100, 35);
            btnEdit.Text = "Edit";
            btnEdit.Enabled = false;
            btnEdit.Click += btnEdit_Click;
            Controls.Add(btnEdit);

            btnView.Location = new Point(240, 540);
            btnView.Size = new Size(100, 35);
            btnView.Text = "View";
            btnView.Enabled = false;
            btnView.Click += btnView_Click;
            Controls.Add(btnView);

            btnDelete.Location = new Point(350, 540);
            btnDelete.Size = new Size(100, 35);
            btnDelete.Text = "Delete";
            btnDelete.Enabled = false;
            btnDelete.Click += btnDelete_Click;
            Controls.Add(btnDelete);

            btnRefresh.Location = new Point(460, 540);
            btnRefresh.Size = new Size(100, 35);
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += btnRefresh_Click;
            Controls.Add(btnRefresh);

            // Status label
            lblStatus.Location = new Point(20, 590);
            lblStatus.Size = new Size(1160, 25);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            Controls.Add(lblStatus);

            ResumeLayout(false);
        }

        private async void LoadTaxes()
        {
            try
            {
                lblStatus.Text = "Loading taxes...";
                lblStatus.ForeColor = Color.Blue;

                var company = await _localStorageService.GetSelectedCompanyAsync();
                if (company != null)
                {
                    _taxes = await _taxService.GetAllTaxesAsync(Guid.Parse(company.Id));
                    RefreshTaxList();
                    lblStatus.Text = $"Loaded {_taxes.Count} taxes";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "No company selected";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading taxes: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading taxes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshTaxList()
        {
            dgvTaxes.Rows.Clear();
            foreach (var tax in _taxes)
            {
                dgvTaxes.Rows.Add(
                    tax.Name,
                    tax.Category.ToString(),
                    $"{tax.DefaultRate:F2}%",
                    tax.HSNCode ?? "",
                    tax.IsActive ? "Yes" : "No",
                    tax.IsComposite ? "Yes" : "No"
                );
            }
        }

        private void dgvTaxes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvTaxes.CurrentRow != null && dgvTaxes.CurrentRow.Index >= 0 && dgvTaxes.CurrentRow.Index < _taxes.Count)
            {
                _selectedTax = _taxes[dgvTaxes.CurrentRow.Index];
                btnEdit.Enabled = true;
                btnView.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                _selectedTax = null;
                btnEdit.Enabled = false;
                btnView.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void dgvTaxes_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedTax != null)
            {
                OpenTaxEditForm(_selectedTax, false);
            }
        }

        private void dgvTaxes_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _selectedTax != null)
            {
                OpenTaxEditForm(_selectedTax, false);
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            OpenTaxEditForm(null, false);
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedTax == null) return;

            try
            {
                lblStatus.Text = "Loading tax details...";
                lblStatus.ForeColor = Color.Blue;

                // Fetch the latest tax data
                var updatedTax = await _taxService.GetTaxByIdAsync(_selectedTax.Id);
                if (updatedTax != null)
                {
                    OpenTaxEditForm(updatedTax, false);
                }
                else
                {
                    lblStatus.Text = "Failed to load tax details";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading tax details: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading tax details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedTax == null) return;

            try
            {
                lblStatus.Text = "Loading tax details...";
                lblStatus.ForeColor = Color.Blue;

                // Fetch the latest tax data
                var updatedTax = await _taxService.GetTaxByIdAsync(_selectedTax.Id);
                if (updatedTax != null)
                {
                    OpenTaxEditForm(updatedTax, true);
                }
                else
                {
                    lblStatus.Text = "Failed to load tax details";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading tax details: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading tax details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedTax == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete the tax '{_selectedTax.Name}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    lblStatus.Text = "Deleting tax...";
                    lblStatus.ForeColor = Color.Blue;

                    bool success = await _taxService.DeleteTaxAsync(_selectedTax.Id);
                    if (success)
                    {
                        lblStatus.Text = "Tax deleted successfully";
                        lblStatus.ForeColor = Color.Green;
                        LoadTaxes(); // Refresh the list
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete tax";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show("Failed to delete tax", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error deleting tax: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Error deleting tax: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadTaxes();
        }

        private void OpenTaxEditForm(TaxModel? tax, bool readOnly)
        {
            var taxEditForm = new TaxEditForm(_taxService, tax);
            
            if (readOnly)
            {
                taxEditForm.SetReadOnlyMode();
            }

            // Set as MDI child
            taxEditForm.MdiParent = this.MdiParent;
            taxEditForm.WindowState = FormWindowState.Maximized;
            
            // Hide navigation panel when form opens
            if (this.MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }

            // Show navigation panel when form closes
            taxEditForm.FormClosed += (s, e) =>
            {
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.ShowNavigationPanel();
                }
                LoadTaxes(); // Refresh the list when form closes
            };

            taxEditForm.Show();
        }
    }
}

