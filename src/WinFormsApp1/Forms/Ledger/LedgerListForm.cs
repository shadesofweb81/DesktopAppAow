using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Ledger
{
    public partial class LedgerListForm : Form
    {
        private readonly LedgerService _ledgerService;
        private readonly LocalStorageService _localStorageService;
        private DataGridView dgvLedgers = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private List<LedgerModel> _ledgers = new List<LedgerModel>();
        private LedgerModel? _selectedLedger;
        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalCount = 0;
        private int _totalPages = 0;

        public LedgerListForm(LedgerService ledgerService)
        {
            _ledgerService = ledgerService;
            _localStorageService = new LocalStorageService();
            InitializeComponent();
            SetupForm();
            LoadLedgers();
        }

        private void InitializeComponent()
        {
            dgvLedgers = new DataGridView();
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
            // dgvLedgers
            // 
            dgvLedgers.Location = new Point(12, 52);
            dgvLedgers.Name = "dgvLedgers";
            dgvLedgers.Size = new Size(1000, 500);
            dgvLedgers.TabIndex = 0;
            dgvLedgers.AllowUserToAddRows = false;
            dgvLedgers.AllowUserToDeleteRows = false;
            dgvLedgers.ReadOnly = true;
            dgvLedgers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLedgers.MultiSelect = false;
            dgvLedgers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLedgers.SelectionChanged += new EventHandler(dgvLedgers_SelectionChanged);
            dgvLedgers.DoubleClick += new EventHandler(dgvLedgers_DoubleClick);
            dgvLedgers.KeyDown += new KeyEventHandler(dgvLedgers_KeyDown);
            
            // 
            // btnNew
            // 
            btnNew.Location = new Point(1030, 52);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(120, 35);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (Insert)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);
            
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(1030, 97);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(120, 35);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "&Edit (Enter)";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Enabled = false;
            
            // 
            // btnView
            // 
            btnView.Location = new Point(1030, 142);
            btnView.Name = "btnView";
            btnView.Size = new Size(120, 35);
            btnView.TabIndex = 3;
            btnView.Text = "&View (V)";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += new EventHandler(btnView_Click);
            btnView.Enabled = false;
            
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1030, 187);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 35);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "&Delete (Del)";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDelete.Enabled = false;
            
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1030, 232);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "&Refresh (F5)";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 570);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1100, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // LedgerListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1170, 600);
            Controls.Add(lblInstructions);
            Controls.Add(dgvLedgers);
            Controls.Add(btnNew);
            Controls.Add(btnEdit);
            Controls.Add(btnView);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(lblStatus);
            Name = "LedgerListForm";
            Text = "Ledger List";
            KeyDown += new KeyEventHandler(LedgerListForm_KeyDown);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set form properties
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            ShowInTaskbar = true;
        }

        private async void LoadLedgers()
        {
            try
            {
                lblStatus.Text = "Loading ledgers...";
                lblStatus.ForeColor = Color.Blue;

                var company = await _localStorageService.GetSelectedCompanyAsync();
                if (company == null)
                {
                    MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }

                _ledgers = await _ledgerService.GetAllLedgersAsync(Guid.Parse(company.Id));
                RefreshLedgerList();
                
                lblStatus.Text = $"Loaded {_ledgers.Count} ledgers";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading ledgers: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error loading ledgers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshLedgerList()
        {
            dgvLedgers.Rows.Clear();
            
            // Add columns if they don't exist
            if (dgvLedgers.Columns.Count == 0)
            {
                dgvLedgers.Columns.Add("Name", "Name");
                dgvLedgers.Columns.Add("Code", "Code");
                dgvLedgers.Columns.Add("Category", "Category");
                dgvLedgers.Columns.Add("City", "City");
                dgvLedgers.Columns.Add("State", "State");
                dgvLedgers.Columns.Add("Country", "Country");
            }
            
            foreach (var ledger in _ledgers)
            {
                dgvLedgers.Rows.Add(
                    ledger.Name,
                    ledger.Code,
                    ledger.Category,
                    ledger.City ?? "",
                    ledger.State ?? "",
                    ledger.Country ?? ""
                );
            }
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            btnEdit.Enabled = _selectedLedger != null;
            btnView.Enabled = _selectedLedger != null;
            btnDelete.Enabled = _selectedLedger != null;
        }

        private void dgvLedgers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvLedgers.CurrentRow != null && dgvLedgers.CurrentRow.Index >= 0 && dgvLedgers.CurrentRow.Index < _ledgers.Count)
            {
                _selectedLedger = _ledgers[dgvLedgers.CurrentRow.Index];
            }
            else
            {
                _selectedLedger = null;
            }
            UpdateButtonStates();
        }

        private void dgvLedgers_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedLedger != null)
            {
                btnEdit_Click(sender, e);
            }
        }

        private void dgvLedgers_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedLedger != null)
                    {
                        btnEdit_Click(sender, e);
                    }
                    break;
                case Keys.Insert:
                    btnNew_Click(sender, e);
                    break;
                case Keys.Delete:
                    if (_selectedLedger != null)
                    {
                        btnDelete_Click(sender, e);
                    }
                    break;
                case Keys.F5:
                    btnRefresh_Click(sender, e);
                    break;
                case Keys.V:
                    if (_selectedLedger != null)
                    {
                        btnView_Click(sender, e);
                    }
                    break;
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            try
            {
                var editForm = new LedgerEditForm(_ledgerService, null);
                editForm.MdiParent = this.MdiParent;
                editForm.WindowState = FormWindowState.Maximized;
                editForm.Show();
                
                // Hide navigation panel when edit form is opened
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }
                
                // Refresh the list when the form is closed
                editForm.FormClosed += (s, args) => 
                {
                    LoadLedgers();
                    // Show navigation panel when edit form is closed
                    if (this.MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.ShowNavigationPanel();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating new ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedLedger == null) return;

            try
            {
                lblStatus.Text = "Loading ledger details...";
                lblStatus.ForeColor = Color.Blue;

                // Fetch the latest ledger data from the server
                var latestLedger = await _ledgerService.GetLedgerByIdAsync(_selectedLedger.Id);
                if (latestLedger == null)
                {
                    lblStatus.Text = "Failed to load ledger details";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show("Failed to load ledger details from server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var editForm = new LedgerEditForm(_ledgerService, latestLedger);
                editForm.MdiParent = this.MdiParent;
                editForm.WindowState = FormWindowState.Maximized;
                editForm.Show();
                
                // Hide navigation panel when edit form is opened
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }
                
                // Refresh the list when the form is closed
                editForm.FormClosed += (s, args) => 
                {
                    LoadLedgers();
                    // Show navigation panel when edit form is closed
                    if (this.MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.ShowNavigationPanel();
                    }
                };
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error editing ledger: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error editing ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedLedger == null) return;

            try
            {
                lblStatus.Text = "Loading ledger details...";
                lblStatus.ForeColor = Color.Blue;

                // Fetch the latest ledger data from the server
                var latestLedger = await _ledgerService.GetLedgerByIdAsync(_selectedLedger.Id);
                if (latestLedger == null)
                {
                    lblStatus.Text = "Failed to load ledger details";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show("Failed to load ledger details from server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var editForm = new LedgerEditForm(_ledgerService, latestLedger);
                editForm.SetReadOnlyMode();
                editForm.MdiParent = this.MdiParent;
                editForm.WindowState = FormWindowState.Maximized;
                editForm.Show();
                
                // Hide navigation panel when view form is opened
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }
                
                // Show navigation panel when view form is closed
                editForm.FormClosed += (s, args) => 
                {
                    if (this.MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.ShowNavigationPanel();
                    }
                };
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error viewing ledger: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error viewing ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedLedger == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete the ledger '{_selectedLedger.Name}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    lblStatus.Text = "Deleting ledger...";
                    lblStatus.ForeColor = Color.Blue;

                    var success = await _ledgerService.DeleteLedgerAsync(_selectedLedger.Id);
                    if (success)
                    {
                        LoadLedgers();
                        lblStatus.Text = "Ledger deleted successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to delete ledger";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show("Failed to delete ledger", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error deleting ledger: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Error deleting ledger: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadLedgers();
        }

        private void LedgerListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
