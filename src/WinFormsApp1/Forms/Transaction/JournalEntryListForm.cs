using WinFormsApp1.Models;
using WinFormsApp1.Services;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class JournalEntryListForm : BaseForm
    {
        private readonly JournalEntryService _journalEntryService;
        private readonly LocalStorageService _localStorageService;
        private readonly LedgerService _ledgerService;
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
        private Label lblCompanyInfo = null!;
        private Label lblInstructions = null!;
        private Label lblFilter = null!;

        // Data
        private List<JournalEntryListDto> _journalEntries = new List<JournalEntryListDto>();
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalPages = 1;
        private bool _isInitializing = true;
        private bool _isLoading = false;
        private System.Windows.Forms.Timer? _searchTimer;

        public JournalEntryListForm(JournalEntryService journalEntryService, LocalStorageService localStorageService,
            LedgerService ledgerService, Models.Company selectedCompany, FinancialYearModel selectedFinancialYear)
        {
            _journalEntryService = journalEntryService;
            _localStorageService = localStorageService;
            _ledgerService = ledgerService;
            _selectedCompany = selectedCompany;
            _selectedFinancialYear = selectedFinancialYear;

            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
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
            lblCompanyInfo = new Label();
            lblInstructions = new Label();
            lblFilter = new Label();

            SuspendLayout();

            // 
            // lblCompanyInfo
            // 
            lblCompanyInfo.Location = new Point(12, 9);
            lblCompanyInfo.Name = "lblCompanyInfo";
            lblCompanyInfo.Size = new Size(700, 25);
            lblCompanyInfo.Text = _selectedCompany != null ? $"Journal Entries for: {_selectedCompany.DisplayName}" : "No company selected";
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            lblCompanyInfo.Font = new Font("Arial", 10, FontStyle.Bold);

            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 40);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(700, 40);
            lblInstructions.Text = GetInstructionsText();
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);

            // 
            // lblFilter
            // 
            lblFilter.Location = new Point(12, 85);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(100, 20);
            lblFilter.Text = "Filter by:";
            lblFilter.ForeColor = Color.DarkBlue;
            lblFilter.Font = new Font("Arial", 9, FontStyle.Bold);

            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(120, 82);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 25);
            txtSearch.PlaceholderText = "Search by entry number, reference...";
            txtSearch.Font = new Font("Arial", 9, FontStyle.Regular);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

            // 
            // cmbTypeFilter
            // 
            cmbTypeFilter.Location = new Point(330, 82);
            cmbTypeFilter.Name = "cmbTypeFilter";
            cmbTypeFilter.Size = new Size(150, 25);
            cmbTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTypeFilter.Font = new Font("Arial", 9, FontStyle.Regular);
            cmbTypeFilter.SelectedIndexChanged += new EventHandler(cmbTypeFilter_SelectedIndexChanged);

            // 
            // cmbStatusFilter
            // 
            cmbStatusFilter.Location = new Point(490, 82);
            cmbStatusFilter.Name = "cmbStatusFilter";
            cmbStatusFilter.Size = new Size(100, 25);
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Font = new Font("Arial", 9, FontStyle.Regular);
            cmbStatusFilter.SelectedIndexChanged += new EventHandler(cmbStatusFilter_SelectedIndexChanged);

            // 
            // dgvJournalEntries
            // 
            dgvJournalEntries.Location = new Point(12, 115);
            dgvJournalEntries.Name = "dgvJournalEntries";
            dgvJournalEntries.Size = new Size(900, 350);
            dgvJournalEntries.TabIndex = 0;
            dgvJournalEntries.AllowUserToAddRows = false;
            dgvJournalEntries.AllowUserToDeleteRows = false;
            dgvJournalEntries.ReadOnly = true;
            dgvJournalEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournalEntries.MultiSelect = false;
            dgvJournalEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJournalEntries.RowHeadersVisible = false;
            dgvJournalEntries.AutoGenerateColumns = false;
            dgvJournalEntries.SelectionChanged += new EventHandler(dgvJournalEntries_SelectionChanged);
            dgvJournalEntries.DoubleClick += new EventHandler(dgvJournalEntries_DoubleClick);
            dgvJournalEntries.KeyDown += new KeyEventHandler(dgvJournalEntries_KeyDown);

            // 
            // btnNew
            // 
            btnNew.Location = new Point(530, 115);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(100, 30);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (F2)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);

            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(530, 155);
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
            btnView.Location = new Point(530, 195);
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
            btnDelete.Location = new Point(530, 235);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "&Delete (Del)";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDelete.Enabled = false;

            // 
            // btnPost
            // 
            btnPost.Location = new Point(530, 275);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(100, 30);
            btnPost.TabIndex = 5;
            btnPost.Text = "&Post (F5)";
            btnPost.UseVisualStyleBackColor = true;
            btnPost.Click += new EventHandler(btnPost_Click);
            btnPost.Enabled = false;

            // 
            // btnUnpost
            // 
            btnUnpost.Location = new Point(530, 315);
            btnUnpost.Name = "btnUnpost";
            btnUnpost.Size = new Size(100, 30);
            btnUnpost.TabIndex = 6;
            btnUnpost.Text = "&Unpost (F6)";
            btnUnpost.UseVisualStyleBackColor = true;
            btnUnpost.Click += new EventHandler(btnUnpost_Click);
            btnUnpost.Enabled = false;

            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(530, 355);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "&Refresh (F7)";
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
            // lblTotalEntries
            // 
            lblTotalEntries.Location = new Point(12, 475);
            lblTotalEntries.Name = "lblTotalEntries";
            lblTotalEntries.Size = new Size(600, 20);
            lblTotalEntries.Text = "Total Entries: 0";
            lblTotalEntries.ForeColor = Color.DarkGreen;

            // 
            // JournalEntryListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(lblTotalEntries);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnUnpost);
            Controls.Add(btnPost);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(dgvJournalEntries);
            Controls.Add(cmbStatusFilter);
            Controls.Add(cmbTypeFilter);
            Controls.Add(txtSearch);
            Controls.Add(lblFilter);
            Controls.Add(lblInstructions);
            Controls.Add(lblCompanyInfo);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            CancelButton = null;
            Name = "JournalEntryListForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Journal Entries";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(JournalEntryListForm_KeyDown);
            Load += new EventHandler(JournalEntryListForm_Load);
            Resize += new EventHandler(JournalEntryListForm_Resize);
            Activated += new EventHandler(JournalEntryListForm_Activated);
            FormClosing += new FormClosingEventHandler(JournalEntryListForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnEdit;
            CancelButton = null; // Remove default cancel button to prevent conflicts

            // Setup DataGridView columns
            SetupDataGridViewColumns();

            // Setup combo boxes
            SetupComboBoxes();

            // Setup event handlers
            SetupEventHandlers();

            // Load data
            LoadData();

            // Mark initialization as complete
            _isInitializing = false;

            // Focus on journal entry grid
            dgvJournalEntries.Focus();
        }

        private void SetupDataGridViewColumns()
        {
            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionNumber",
                HeaderText = "Entry Number",
                DataPropertyName = "TransactionNumber",
                Width = 150
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "JournalEntryType",
                HeaderText = "Type",
                DataPropertyName = "JournalEntryType",
                Width = 120
            });

            dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TransactionDate",
                HeaderText = "Entry Date",
                DataPropertyName = "TransactionDate",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
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
            // Type filter - using the actual values from the API response
            cmbTypeFilter.Items.Add("All Types");
            cmbTypeFilter.Items.AddRange(new string[] { "Journal", "OpeningBalance" });
            cmbTypeFilter.SelectedIndex = 0;

            // Status filter - using the actual values from the API response
            cmbStatusFilter.Items.AddRange(new string[] { "All Status", "Draft", "Completed", "Posted", "Cancelled" });
            cmbStatusFilter.SelectedIndex = 0;
        }

        private void SetupEventHandlers()
        {
            // Form events
            KeyDown += JournalEntryListForm_KeyDown;
            Load += JournalEntryListForm_Load;

            // Grid events
            dgvJournalEntries.KeyDown += dgvJournalEntries_KeyDown;
            dgvJournalEntries.DoubleClick += dgvJournalEntries_DoubleClick;
            dgvJournalEntries.SelectionChanged += dgvJournalEntries_SelectionChanged;

            // Button events
            btnNew.Click += btnNew_Click;
            btnEdit.Click += btnEdit_Click;
            btnView.Click += btnView_Click;
            btnDelete.Click += btnDelete_Click;
            btnPost.Click += btnPost_Click;
            btnUnpost.Click += btnUnpost_Click;
            btnRefresh.Click += btnRefresh_Click;

            // Filter events
            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbTypeFilter.SelectedIndexChanged += cmbTypeFilter_SelectedIndexChanged;
            cmbStatusFilter.SelectedIndexChanged += cmbStatusFilter_SelectedIndexChanged;
        }

        private async void LoadData()
        {
            // Prevent multiple concurrent API calls
            if (_isLoading)
            {
                return;
            }

            try
            {
                _isLoading = true;
                UpdateStatus("Loading journal entries...");
                var companyId = Guid.Parse(_selectedCompany.Id);
                
                // Convert filter selections to match API expectations
                JournalEntryType? typeFilter = null;
                if (cmbTypeFilter.SelectedItem?.ToString() != "All Types")
                {
                    if (Enum.TryParse<JournalEntryType>(cmbTypeFilter.SelectedItem?.ToString(), true, out var type))
                    {
                        typeFilter = type;
                    }
                }
                
                var statusFilter = cmbStatusFilter.SelectedItem?.ToString() == "All Status" ? null : cmbStatusFilter.SelectedItem?.ToString();
                var searchTerm = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text;

                var response = await _journalEntryService.GetJournalEntriesAsync(companyId, _currentPage, _pageSize, searchTerm, typeFilter, statusFilter);

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
            finally
            {
                _isLoading = false;
            }
        }

        private void UpdateStatus(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblStatus.Text = message;
            }
        }


        private void JournalEntryListForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                    if (selectedEntry != null)
                    {
                        OpenJournalEntryEditForm(selectedEntry);
                    }
                    break;
                case Keys.Escape:
                    // When closing with Escape, ensure navigation panel is shown
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

                case Keys.F2:
                    BtnNew_Click(null, EventArgs.Empty);
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

                case Keys.V:
                    if (dgvJournalEntries.SelectedRows.Count > 0)
                    {
                        BtnView_Click(null, EventArgs.Empty);
                        e.Handled = true;
                    }
                    break;
            }
        }


        private void BtnNew_Click(object? sender, EventArgs e)
        {
            try
            {
                // Check if a new JournalEntryForm is already open
                if (this.MdiParent != null)
                {
                    foreach (Form childForm in this.MdiParent.MdiChildren)
                    {
                        if (childForm is JournalEntryForm existingForm && 
                            existingForm.Text.Contains("New Journal Entry"))
                        {
                            existingForm.BringToFront();
                            existingForm.Activate();
                            return;
                        }
                    }
                }

                // Create a new JournalEntryForm for creating a new entry
                var journalEntryForm = new JournalEntryForm(_journalEntryService, _localStorageService,
                    _ledgerService, _selectedCompany, _selectedFinancialYear)
                {
                    Text = "New Journal Entry",
                    MdiParent = this.MdiParent,
                    WindowState = FormWindowState.Maximized
                };

                // Show the form as an MDI child
                journalEntryForm.Show();
                
                // Hide the navigation panel when the form is maximized
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }

                // Handle form closing to refresh the list
                journalEntryForm.FormClosed += (s, e) =>
                {
                    // Refresh the list when the form is closed
                    LoadData();
                    UpdateStatus("Journal entry form closed - list refreshed");
                    
                    // Show navigation panel when no child forms are active
                    if (this.MdiParent is MainMDIForm parentMdiForm)
                    {
                        parentMdiForm.ShowNavigationPanel();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening new journal entry form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvJournalEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvJournalEntries.SelectedRows[0].DataBoundItem as JournalEntryListDto;
                if (selectedEntry != null)
                {
                    OpenJournalEntryEditForm(selectedEntry);
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
                    MessageBox.Show($"View Journal Entry: {selectedEntry.TransactionNumber}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var result = MessageBox.Show($"Are you sure you want to delete journal entry '{selectedEntry.TransactionNumber}'?",
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
                    if (selectedEntry.Status == "Posted" || selectedEntry.Status == "Completed")
                    {
                        MessageBox.Show("This journal entry is already posted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var result = MessageBox.Show($"Are you sure you want to post journal entry '{selectedEntry.TransactionNumber}'?",
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

                    var result = MessageBox.Show($"Are you sure you want to unpost journal entry '{selectedEntry.TransactionNumber}'?",
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

        // New event handlers for the updated form
        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            if (!_isInitializing)
            {
                // Debounce search to prevent multiple API calls while typing
                _searchTimer?.Stop();
                _searchTimer = new System.Windows.Forms.Timer();
                _searchTimer.Interval = 500; // 500ms delay
                _searchTimer.Tick += (s, args) =>
                {
                    _searchTimer?.Stop();
                    LoadData();
                };
                _searchTimer.Start();
            }
        }

        private void cmbTypeFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!_isInitializing)
            {
                LoadData();
            }
        }

        private void cmbStatusFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!_isInitializing)
            {
                LoadData();
            }
        }

        private void dgvJournalEntries_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnEdit_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                BtnDelete_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void dgvJournalEntries_DoubleClick(object? sender, EventArgs e)
        {
            BtnView_Click(null, EventArgs.Empty);
        }

        private void dgvJournalEntries_SelectionChanged(object? sender, EventArgs e)
        {
            var hasSelection = dgvJournalEntries.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnView.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnPost.Enabled = hasSelection;
            btnUnpost.Enabled = hasSelection;
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            BtnNew_Click(sender, e);
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        private void btnView_Click(object? sender, EventArgs e)
        {
            BtnView_Click(sender, e);
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            BtnDelete_Click(sender, e);
        }

        private void btnPost_Click(object? sender, EventArgs e)
        {
            BtnPost_Click(sender, e);
        }

        private void btnUnpost_Click(object? sender, EventArgs e)
        {
            BtnUnpost_Click(sender, e);
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            BtnRefresh_Click(sender, e);
        }

        // Form event handlers
        private void JournalEntryListForm_Load(object? sender, EventArgs e)
        {
            // Open as maximized child form within MDI parent
            WindowState = FormWindowState.Maximized;

            // Resize controls to fit the maximized form
            ResizeControls();

            // Focus on the data grid
            dgvJournalEntries.Focus();

            // Hide MDI navigation panel when this form is maximized
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }

            // Ensure the form stays maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void JournalEntryListForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void JournalEntryListForm_Activated(object? sender, EventArgs e)
        {
            // When JournalEntryListForm is activated, ensure it's maximized and navigation is hidden
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }

            // Hide navigation panel when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }

            // Ensure the form takes focus and maintains its state
            this.BringToFront();
            this.Activate();

            // Force the form to stay maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void JournalEntryListForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Clean up timer
            _searchTimer?.Stop();
            _searchTimer?.Dispose();
            
            // When JournalEntryListForm is closing, ensure navigation panel is shown again
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
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;

            // Reserve space for buttons on the right side
            int buttonAreaWidth = 150;
            int availableWidth = clientWidth - buttonAreaWidth - 30; // 30px margin
            int availableHeight = clientHeight - 200; // Increased to accommodate filter controls

            // Ensure minimum grid width
            if (availableWidth < 600)
            {
                availableWidth = 600;
            }

            // Resize the data grid to use most of the available space
            dgvJournalEntries.Size = new Size(availableWidth, availableHeight);

            // Reposition buttons on the right side with 30px margin
            int buttonX = availableWidth + 30;
            btnNew.Location = new Point(buttonX, 115);
            btnEdit.Location = new Point(buttonX, 155);
            btnView.Location = new Point(buttonX, 195);
            btnDelete.Location = new Point(buttonX, 235);
            btnPost.Location = new Point(buttonX, 275);
            btnUnpost.Location = new Point(buttonX, 315);
            btnRefresh.Location = new Point(buttonX, 355);

            // Reposition status labels at the bottom
            lblStatus.Location = new Point(12, clientHeight - 50);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            lblTotalEntries.Location = new Point(12, clientHeight - 25);
            lblTotalEntries.Size = new Size(clientWidth - 24, 20);

            // Resize company info and instructions labels
            lblCompanyInfo.Size = new Size(clientWidth - 24, 25);
            lblInstructions.Size = new Size(clientWidth - 24, 40);

            // Reposition filter controls
            lblFilter.Location = new Point(12, 85);
            txtSearch.Location = new Point(120, 82);
            cmbTypeFilter.Location = new Point(330, 82);
            cmbStatusFilter.Location = new Point(490, 82);
        }

        private string GetInstructionsText()
        {
            return "Keyboard Navigation: ↑↓ to navigate rows, Enter to edit, V to view details, F2 for new, Delete to remove, F7 to refresh, Esc to close | Journal Entries | Use filters to refine results | Uses selected company from local storage";
        }

        private void OpenJournalEntryEditForm(JournalEntryListDto selectedEntry)
        {
            try
            {
                // Check if a JournalEntryForm is already open for this entry
                if (this.MdiParent != null)
                {
                    foreach (Form childForm in this.MdiParent.MdiChildren)
                    {
                        if (childForm is JournalEntryForm existingForm && 
                            existingForm.Text.Contains(selectedEntry.TransactionNumber))
                        {
                            existingForm.BringToFront();
                            existingForm.Activate();
                            return;
                        }
                    }
                }

                // Create a new JournalEntryForm for editing
                var journalEntryForm = new JournalEntryForm(_journalEntryService, _localStorageService,
                    _ledgerService, _selectedCompany, _selectedFinancialYear)
                {
                    Text = $"Edit Journal Entry - {selectedEntry.TransactionNumber}",
                    MdiParent = this.MdiParent,
                    WindowState = FormWindowState.Maximized
                };

                // Show the form as an MDI child
                journalEntryForm.Show();
                
                // Hide the navigation panel when the form is maximized
                if (this.MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }

                // Handle form closing to refresh the list
                journalEntryForm.FormClosed += (s, e) =>
                {
                    // Refresh the list when the form is closed
                    LoadData();
                    UpdateStatus("Journal entry form closed - list refreshed");
                    
                    // Show navigation panel when no child forms are active
                    if (this.MdiParent is MainMDIForm parentMdiForm)
                    {
                        parentMdiForm.ShowNavigationPanel();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening journal entry edit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }
    }
}

