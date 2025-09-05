using WinFormsApp1.Models;
using WinFormsApp1.Services;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class JournalEntryListForm : BaseForm
    {
        private readonly JournalEntryService _journalEntryService;
        private readonly LocalStorageService _localStorageService;
        private Models.Company _selectedCompany;
        private FinancialYearModel _selectedFinancialYear;

        // Form controls
        private DataGridView dgvJournalEntries = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnDelete = null!;
        private Button btnView = null!;
        private Button btnPost = null!;
        private Button btnUnpost = null!;
        private Button btnRefresh = null!;
        private TextBox txtSearch = null!;
        private ComboBox cmbTypeFilter = null!;
        private ComboBox cmbStatusFilter = null!;
        private Label lblStatus = null!;
        private Label lblTotalEntries = null!;

        // Data
        private List<JournalEntryListDto> _journalEntries = new List<JournalEntryListDto>();
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalPages = 1;

        public JournalEntryListForm(JournalEntryService journalEntryService, LocalStorageService localStorageService, 
            Models.Company selectedCompany, FinancialYearModel selectedFinancialYear)
        {
            _journalEntryService = journalEntryService;
            _localStorageService = localStorageService;
            _selectedCompany = selectedCompany;
            _selectedFinancialYear = selectedFinancialYear;
            
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            // Form properties
            Text = "Journal Entries";
            Size = new Size(1400, 800);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            // Initialize controls
            dgvJournalEntries = new DataGridView();
            btnNew = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnView = new Button();
            btnPost = new Button();
            btnUnpost = new Button();
            btnRefresh = new Button();
            txtSearch = new TextBox();
            cmbTypeFilter = new ComboBox();
            cmbStatusFilter = new ComboBox();
            lblStatus = new Label();
            lblTotalEntries = new Label();

            SuspendLayout();
            SetupLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            int yPos = 20;
            int buttonWidth = 100;
            int buttonHeight = 30;
            int spacing = 10;

            // Search and Filter Controls
            var searchGroup = new GroupBox
            {
                Text = "Search & Filter",
                Location = new Point(20, yPos),
                Size = new Size(1350, 80),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            Controls.Add(searchGroup);

            // Search textbox
            var lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(10, 25),
                Size = new Size(50, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            searchGroup.Controls.Add(lblSearch);

            txtSearch.Location = new Point(65, 22);
            txtSearch.Size = new Size(200, 25);
            txtSearch.PlaceholderText = "Search by entry number, reference...";
            searchGroup.Controls.Add(txtSearch);

            // Type filter
            var lblType = new Label
            {
                Text = "Type:",
                Location = new Point(280, 25),
                Size = new Size(40, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            searchGroup.Controls.Add(lblType);

            cmbTypeFilter.Location = new Point(325, 22);
            cmbTypeFilter.Size = new Size(150, 25);
            cmbTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            searchGroup.Controls.Add(cmbTypeFilter);

            // Status filter
            var lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(490, 25),
                Size = new Size(50, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            searchGroup.Controls.Add(lblStatus);

            cmbStatusFilter.Location = new Point(545, 22);
            cmbStatusFilter.Size = new Size(100, 25);
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            searchGroup.Controls.Add(cmbStatusFilter);

            // Refresh button
            btnRefresh.Location = new Point(660, 22);
            btnRefresh.Size = new Size(80, 25);
            btnRefresh.Text = "&Refresh (F7)";
            btnRefresh.UseVisualStyleBackColor = true;
            searchGroup.Controls.Add(btnRefresh);

            yPos += 100;

            // Action Buttons
            var actionGroup = new GroupBox
            {
                Text = "Actions",
                Location = new Point(20, yPos),
                Size = new Size(1350, 60),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            Controls.Add(actionGroup);

            btnNew.Location = new Point(10, 25);
            btnNew.Size = new Size(buttonWidth, buttonHeight);
            btnNew.Text = "&New (F2)";
            btnNew.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnNew);

            btnEdit.Location = new Point(120, 25);
            btnEdit.Size = new Size(buttonWidth, buttonHeight);
            btnEdit.Text = "&Edit (F3)";
            btnEdit.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnEdit);

            btnView.Location = new Point(230, 25);
            btnView.Size = new Size(buttonWidth, buttonHeight);
            btnView.Text = "&View (F4)";
            btnView.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnView);

            btnDelete.Location = new Point(340, 25);
            btnDelete.Size = new Size(buttonWidth, buttonHeight);
            btnDelete.Text = "&Delete (Del)";
            btnDelete.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnDelete);

            btnPost.Location = new Point(450, 25);
            btnPost.Size = new Size(buttonWidth, buttonHeight);
            btnPost.Text = "&Post (F5)";
            btnPost.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnPost);

            btnUnpost.Location = new Point(560, 25);
            btnUnpost.Size = new Size(buttonWidth, buttonHeight);
            btnUnpost.Text = "&Unpost (F6)";
            btnUnpost.UseVisualStyleBackColor = true;
            actionGroup.Controls.Add(btnUnpost);

            yPos += 80;

            // Data Grid
            dgvJournalEntries.Location = new Point(20, yPos);
            dgvJournalEntries.Size = new Size(1350, 500);
            dgvJournalEntries.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvJournalEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournalEntries.MultiSelect = false;
            dgvJournalEntries.AllowUserToAddRows = false;
            dgvJournalEntries.AllowUserToDeleteRows = false;
            dgvJournalEntries.ReadOnly = true;
            dgvJournalEntries.RowHeadersVisible = false;
            dgvJournalEntries.AutoGenerateColumns = false;
            Controls.Add(dgvJournalEntries);

            // Status labels
            lblStatus.Location = new Point(20, yPos + 520);
            lblStatus.Size = new Size(600, 20);
            lblStatus.ForeColor = Color.Blue;
            Controls.Add(lblStatus);

            lblTotalEntries.Location = new Point(20, yPos + 545);
            lblTotalEntries.Size = new Size(600, 20);
            lblTotalEntries.ForeColor = Color.DarkGreen;
            Controls.Add(lblTotalEntries);
        }

        private void SetupForm()
        {
            SetupDataGridView();
            SetupComboBoxes();
            SetupEventHandlers();
            LoadData();
        }

        private void SetupDataGridView()
        {
            // Add columns
            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EntryNumber",
                HeaderText = "Entry Number",
                DataPropertyName = "EntryNumber",
                Width = 150
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Type",
                HeaderText = "Type",
                DataPropertyName = "Type",
                Width = 120
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EntryDate",
                HeaderText = "Entry Date",
                DataPropertyName = "EntryDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReferenceNumber",
                HeaderText = "Reference",
                DataPropertyName = "ReferenceNumber",
                Width = 120
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 80
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalDebit",
                HeaderText = "Total Debit",
                DataPropertyName = "TotalDebit",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalCredit",
                HeaderText = "Total Credit",
                DataPropertyName = "TotalCredit",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Difference",
                HeaderText = "Difference",
                DataPropertyName = "Difference",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvJournalEntries.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsBalanced",
                HeaderText = "Balanced",
                DataPropertyName = "IsBalanced",
                Width = 80
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                HeaderText = "Notes",
                DataPropertyName = "Notes",
                Width = 200
            });

            // Set up visual styling
            dgvJournalEntries.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvJournalEntries.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvJournalEntries.GridColor = Color.LightGray;
            dgvJournalEntries.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetupComboBoxes()
        {
            // Type filter
            cmbTypeFilter.Items.Add("All Types");
            cmbTypeFilter.Items.AddRange(Enum.GetValues<JournalEntryType>().Cast<object>().ToArray());
            cmbTypeFilter.SelectedIndex = 0;

            // Status filter
            cmbStatusFilter.Items.AddRange(new string[] { "All Status", "Draft", "Posted", "Cancelled" });
            cmbStatusFilter.SelectedIndex = 0;
        }

        private void SetupEventHandlers()
        {
            // Form events
            KeyDown += JournalEntryListForm_KeyDown;
            Load += JournalEntryListForm_Load;

            // Grid events
            dgvJournalEntries.KeyDown += DgvJournalEntries_KeyDown;
            dgvJournalEntries.DoubleClick += DgvJournalEntries_DoubleClick;
            dgvJournalEntries.SelectionChanged += DgvJournalEntries_SelectionChanged;

            // Button events
            btnNew.Click += BtnNew_Click;
            btnEdit.Click += BtnEdit_Click;
            btnView.Click += BtnView_Click;
            btnDelete.Click += BtnDelete_Click;
            btnPost.Click += BtnPost_Click;
            btnUnpost.Click += BtnUnpost_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Filter events
            txtSearch.TextChanged += (s, e) => LoadData();
            cmbTypeFilter.SelectedIndexChanged += (s, e) => LoadData();
            cmbStatusFilter.SelectedIndexChanged += (s, e) => LoadData();
        }

        private async void LoadData()
        {
            try
            {
                UpdateStatus("Loading journal entries...");

                var typeFilter = cmbTypeFilter.SelectedItem is JournalEntryType type ? type : (JournalEntryType?)null;
                var statusFilter = cmbStatusFilter.SelectedItem?.ToString() == "All Status" ? null : cmbStatusFilter.SelectedItem?.ToString();
                var searchTerm = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text;

                var response = await _journalEntryService.GetJournalEntriesAsync(_currentPage, _pageSize, searchTerm, typeFilter, statusFilter);

                if (response != null)
                {
                    _journalEntries = response.Items;
                    _totalPages = response.TotalPages;
                    
                    dgvJournalEntries.DataSource = _journalEntries;
                    
                    UpdateStatus($"Loaded {_journalEntries.Count} of {response.TotalItems} journal entries");
                    lblTotalEntries.Text = $"Total Entries: {response.TotalItems} | Page {_currentPage} of {_totalPages}";
                }
                else
                {
                    UpdateStatus("Failed to load journal entries");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading journal entries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void UpdateStatus(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblStatus.Text = message;
            }
        }

        // Event Handlers
        private void JournalEntryListForm_Load(object? sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void JournalEntryListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    BtnNew_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F3:
                    BtnEdit_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F4:
                    BtnView_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F5:
                    BtnPost_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F6:
                    BtnUnpost_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F7:
                    BtnRefresh_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    if (dgvJournalEntries.Focused)
                    {
                        BtnDelete_Click(null, EventArgs.Empty);
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void DgvJournalEntries_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnView_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                BtnDelete_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvJournalEntries_DoubleClick(object? sender, EventArgs e)
        {
            BtnView_Click(null, EventArgs.Empty);
        }

        private void DgvJournalEntries_SelectionChanged(object? sender, EventArgs e)
        {
            var hasSelection = dgvJournalEntries.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnView.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnPost.Enabled = hasSelection;
            btnUnpost.Enabled = hasSelection;
        }

        private void BtnNew_Click(object? sender, EventArgs e)
        {
            // This would open the JournalEntryForm for creating a new entry
            // For now, just show a message
            MessageBox.Show("New Journal Entry functionality will be implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    MessageBox.Show($"Edit Journal Entry: {selectedEntry.EntryNumber}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnView_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    MessageBox.Show($"View Journal Entry: {selectedEntry.EntryNumber}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete journal entry '{selectedEntry.EntryNumber}'?", 
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            var success = await _journalEntryService.DeleteJournalEntryAsync(Guid.Parse(selectedEntry.Id));
                            if (success)
                            {
                                MessageBox.Show("Journal entry deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete journal entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private async void BtnPost_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    if (selectedEntry.Status == "Posted")
                    {
                        MessageBox.Show("This journal entry is already posted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var result = MessageBox.Show($"Are you sure you want to post journal entry '{selectedEntry.EntryNumber}'?", 
                        "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            var success = await _journalEntryService.PostJournalEntryAsync(Guid.Parse(selectedEntry.Id));
                            if (success)
                            {
                                MessageBox.Show("Journal entry posted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to post journal entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error posting journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private async void BtnUnpost_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    if (selectedEntry.Status == "Draft")
                    {
                        MessageBox.Show("This journal entry is already in draft status.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var result = MessageBox.Show($"Are you sure you want to unpost journal entry '{selectedEntry.EntryNumber}'?", 
                        "Confirm Unpost", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            var success = await _journalEntryService.UnpostJournalEntryAsync(Guid.Parse(selectedEntry.Id));
                            if (success)
                            {
                                MessageBox.Show("Journal entry unposted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to unpost journal entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error unposting journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }
    }
}
