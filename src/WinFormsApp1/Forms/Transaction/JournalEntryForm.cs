using WinFormsApp1.Models;
using WinFormsApp1.Services;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class JournalEntryForm : BaseForm
    {
        private readonly JournalEntryService _journalEntryService;
        private readonly LocalStorageService _localStorageService;
        private readonly LedgerService _ledgerService;
        private Models.Company _selectedCompany;
        private FinancialYearModel _selectedFinancialYear;

        // Form controls
        private ComboBox cmbJournalType = null!;
        private TextBox txtTransactionNumber = null!;
        private DateTimePicker dtpTransactionDate = null!;
        private TextBox txtReferenceNumber = null!;
        private TextBox txtNotes = null!;
        private DataGridView dgvLedgerEntries = null!;
        private Button btnAddEntry = null!;
        private Button btnEditEntry = null!;
        private Button btnDeleteEntry = null!;
        private Button btnSelectLedger = null!;
        private TextBox txtTotalDebit = null!;
        private TextBox txtTotalCredit = null!;
        private TextBox txtDifference = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;

        // Data
        private List<JournalEntryDisplay> _ledgerEntries = new List<JournalEntryDisplay>();
        private List<LedgerModel> _availableLedgers = new List<LedgerModel>();
        private int _nextSerialNumber = 1;

        public JournalEntryForm(JournalEntryService journalEntryService, LocalStorageService localStorageService, 
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
            // Form properties
            Text = "Journal Entry";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            // Initialize controls
            cmbJournalType = new ComboBox();
            txtTransactionNumber = new TextBox();
            dtpTransactionDate = new DateTimePicker();
            txtReferenceNumber = new TextBox();
            txtNotes = new TextBox();
            dgvLedgerEntries = new DataGridView();
            btnAddEntry = new Button();
            btnEditEntry = new Button();
            btnDeleteEntry = new Button();
            btnSelectLedger = new Button();
            txtTotalDebit = new TextBox();
            txtTotalCredit = new TextBox();
            txtDifference = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();

            SuspendLayout();
            SetupLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupLayout()
        {
            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 200;
            int spacing = 30;

            // Journal Type
            AddLabelAndControl("Journal Type:", cmbJournalType, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Transaction Number
            AddLabelAndControl("Entry Number:", txtTransactionNumber, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Transaction Date
            AddLabelAndControl("Entry Date:", dtpTransactionDate, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Reference Number
            AddLabelAndControl("Reference:", txtReferenceNumber, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Notes
            AddLabelAndControl("Notes:", txtNotes, 20, yPos, labelWidth, controlWidth + 100);
            yPos += spacing + 20;

            // Ledger Entries Group
            var entriesGroup = new GroupBox
            {
                Text = "Journal Entries (Debit and Credit columns - must be balanced)",
                Location = new Point(20, yPos),
                Size = new Size(1140, 400),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            Controls.Add(entriesGroup);

            // Ledger Entries Grid
            dgvLedgerEntries.Location = new Point(10, 25);
            dgvLedgerEntries.Size = new Size(1120, 300);
            dgvLedgerEntries.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            entriesGroup.Controls.Add(dgvLedgerEntries);

            // Buttons for ledger entries
            btnAddEntry.Location = new Point(10, 335);
            btnAddEntry.Size = new Size(100, 30);
            btnAddEntry.Text = "&Add Entry (F2)";
            btnAddEntry.UseVisualStyleBackColor = true;
            btnAddEntry.Click += BtnAddEntry_Click;
            entriesGroup.Controls.Add(btnAddEntry);

            btnEditEntry.Location = new Point(120, 335);
            btnEditEntry.Size = new Size(100, 30);
            btnEditEntry.Text = "&Edit Entry (F3)";
            btnEditEntry.UseVisualStyleBackColor = true;
            btnEditEntry.Click += BtnEditEntry_Click;
            entriesGroup.Controls.Add(btnEditEntry);

            btnDeleteEntry.Location = new Point(230, 335);
            btnDeleteEntry.Size = new Size(100, 30);
            btnDeleteEntry.Text = "&Delete Entry (Del)";
            btnDeleteEntry.UseVisualStyleBackColor = true;
            btnDeleteEntry.Click += BtnDeleteEntry_Click;
            entriesGroup.Controls.Add(btnDeleteEntry);

            btnSelectLedger.Location = new Point(340, 335);
            btnSelectLedger.Size = new Size(120, 30);
            btnSelectLedger.Text = "&Select Ledger (F4)";
            btnSelectLedger.UseVisualStyleBackColor = true;
            btnSelectLedger.Click += BtnSelectLedger_Click;
            entriesGroup.Controls.Add(btnSelectLedger);

            yPos += 420;

            // Totals Group
            var totalsGroup = new GroupBox
            {
                Text = "Entry Totals",
                Location = new Point(20, yPos),
                Size = new Size(400, 100),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            Controls.Add(totalsGroup);

            // Total Debit
            AddLabelAndControlToParent("Total Debit:", txtTotalDebit, totalsGroup, 10, 25, 80, 100, true);
            
            // Total Credit
            AddLabelAndControlToParent("Total Credit:", txtTotalCredit, totalsGroup, 10, 55, 80, 100, true);

            // Difference
            AddLabelAndControlToParent("Difference:", txtDifference, totalsGroup, 200, 25, 80, 100, true);

            // Status Label
            lblStatus.Location = new Point(20, yPos + 110);
            lblStatus.Size = new Size(400, 20);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Controls.Add(lblStatus);

            // Action Buttons
            btnSave.Location = new Point(1000, yPos + 50);
            btnSave.Size = new Size(75, 30);
            btnSave.Text = "&Save (Ctrl+S)";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            btnCancel.Location = new Point(1085, yPos + 50);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "&Cancel (Esc)";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            Controls.Add(btnCancel);
        }

        private void AddLabelAndControl(string labelText, Control control, int x, int y, int labelWidth, int controlWidth)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 3),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(label);

            control.Location = new Point(x + labelWidth + 5, y);
            control.Size = new Size(controlWidth, 25);
            Controls.Add(control);
        }

        private void AddLabelAndControlToParent(string labelText, Control control, Control parent, int x, int y, int labelWidth, int controlWidth, bool readOnly = false)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 3),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(label);

            control.Location = new Point(x + labelWidth + 5, y);
            control.Size = new Size(controlWidth, 25);
            if (readOnly)
            {
                if (control is TextBox textBox)
                {
                    textBox.ReadOnly = true;
                    textBox.BackColor = Color.LightGray;
                }
            }
            parent.Controls.Add(control);
        }

        private void SetupForm()
        {
            SetupComboBoxes();
            SetupDataGridView();
            SetupEventHandlers();
            LoadData();
            UpdateTotals();
        }

        private void SetupComboBoxes()
        {
            // Journal Type ComboBox
            cmbJournalType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJournalType.DataSource = Enum.GetValues<JournalEntryType>();
            cmbJournalType.SelectedIndex = 0;

            // Set default values
            txtTransactionNumber.Text = GenerateTransactionNumber();
            dtpTransactionDate.Value = DateTime.Today;
        }

        private void SetupDataGridView()
        {
            dgvLedgerEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLedgerEntries.MultiSelect = false;
            dgvLedgerEntries.AllowUserToAddRows = false;
            dgvLedgerEntries.AllowUserToDeleteRows = false;
            dgvLedgerEntries.ReadOnly = true;
            dgvLedgerEntries.RowHeadersVisible = false;
            dgvLedgerEntries.AutoGenerateColumns = false;
            dgvLedgerEntries.DataSource = _ledgerEntries;

            // Add columns
            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "S.No",
                DataPropertyName = "SerialNumber",
                Width = 50
            });

            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LedgerName",
                HeaderText = "Ledger Name",
                DataPropertyName = "LedgerName",
                Width = 300
            });

            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LedgerCode",
                HeaderText = "Code",
                DataPropertyName = "LedgerCode",
                Width = 100
            });

            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DebitAmount",
                HeaderText = "Debit Amount",
                DataPropertyName = "DebitAmount",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreditAmount",
                HeaderText = "Credit Amount",
                DataPropertyName = "CreditAmount",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvLedgerEntries.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            });

            // Set up visual styling
            dgvLedgerEntries.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvLedgerEntries.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLedgerEntries.GridColor = Color.LightGray;
            dgvLedgerEntries.BorderStyle = BorderStyle.Fixed3D;
        }

        private void SetupEventHandlers()
        {
            // Form events
            KeyDown += JournalEntryForm_KeyDown;
            Load += JournalEntryForm_Load;

            // Grid events
            dgvLedgerEntries.KeyDown += DgvLedgerEntries_KeyDown;
            dgvLedgerEntries.DoubleClick += DgvLedgerEntries_DoubleClick;
            dgvLedgerEntries.SelectionChanged += DgvLedgerEntries_SelectionChanged;

            // Control events
            txtTransactionNumber.TextChanged += (s, e) => UpdateStatus();
            dtpTransactionDate.ValueChanged += (s, e) => UpdateStatus();
        }

        private async void LoadData()
        {
            try
            {
                // Load available ledgers
                _availableLedgers = await _ledgerService.GetAllLedgersAsync(Guid.Parse(_selectedCompany.Id));
                UpdateStatus("Ledgers loaded successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private string GenerateTransactionNumber()
        {
            var prefix = "JE";
            var date = DateTime.Today.ToString("yyyyMMdd");
            var random = new Random().Next(1000, 9999);
            return $"{prefix}-{date}-{random}";
        }

        private void UpdateTotals()
        {
            var totalDebit = _ledgerEntries.Where(e => e.EntryType == JournalEntryLedgerType.Debit).Sum(e => e.Amount);
            var totalCredit = _ledgerEntries.Where(e => e.EntryType == JournalEntryLedgerType.Credit).Sum(e => e.Amount);
            var difference = totalDebit - totalCredit;

            txtTotalDebit.Text = totalDebit.ToString("N2");
            txtTotalCredit.Text = totalCredit.ToString("N2");
            txtDifference.Text = difference.ToString("N2");

            // Update status based on balance
            if (Math.Abs(difference) < 0.01m) // Consider balanced if difference is less than 1 cent
            {
                lblStatus.Text = "✓ Entries are balanced";
                lblStatus.ForeColor = Color.Green;
                btnSave.Enabled = true;
            }
            else
            {
                lblStatus.Text = $"⚠ Entries are not balanced (Difference: {difference:N2})";
                lblStatus.ForeColor = Color.Red;
                btnSave.Enabled = false;
            }
        }

        private void UpdateStatus(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                lblStatus.Text = message;
                lblStatus.ForeColor = Color.Blue;
            }
        }

        // Event Handlers
        private void JournalEntryForm_Load(object? sender, EventArgs e)
        {
            cmbJournalType.Focus();
        }

        private void JournalEntryForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    BtnAddEntry_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F3:
                    BtnEditEntry_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.F4:
                    BtnSelectLedger_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    if (dgvLedgerEntries.Focused)
                    {
                        BtnDeleteEntry_Click(null, EventArgs.Empty);
                        e.Handled = true;
                    }
                    break;
                case Keys.S when e.Control:
                    BtnSave_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    BtnCancel_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
            }
        }

        private void DgvLedgerEntries_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnEditEntry_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                BtnDeleteEntry_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvLedgerEntries_DoubleClick(object? sender, EventArgs e)
        {
            BtnEditEntry_Click(null, EventArgs.Empty);
        }

        private void DgvLedgerEntries_SelectionChanged(object? sender, EventArgs e)
        {
            btnEditEntry.Enabled = dgvLedgerEntries.SelectedRows.Count > 0;
            btnDeleteEntry.Enabled = dgvLedgerEntries.SelectedRows.Count > 0;
        }

        private void BtnAddEntry_Click(object? sender, EventArgs e)
        {
            ShowLedgerEntryDialog();
        }

        private void BtnEditEntry_Click(object? sender, EventArgs e)
        {
            if (dgvLedgerEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvLedgerEntries.SelectedRows[0].DataBoundItem as JournalEntryDisplay;
                if (selectedEntry != null)
                {
                    ShowLedgerEntryDialog(selectedEntry);
                }
            }
        }

        private void BtnDeleteEntry_Click(object? sender, EventArgs e)
        {
            if (dgvLedgerEntries.SelectedRows.Count > 0)
            {
                var selectedEntry = dgvLedgerEntries.SelectedRows[0].DataBoundItem as JournalEntryDisplay;
                if (selectedEntry != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete this entry?\n\nLedger: {selectedEntry.LedgerName}\nType: {selectedEntry.EntryType}\nAmount: {selectedEntry.Amount:N2}", 
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        _ledgerEntries.Remove(selectedEntry);
                        RefreshGrid();
                        UpdateTotals();
                        UpdateStatus("Entry deleted successfully");
                    }
                }
            }
        }

        private void BtnSelectLedger_Click(object? sender, EventArgs e)
        {
            ShowLedgerSelectionDialog();
        }

        private void ShowLedgerEntryDialog(JournalEntryDisplay? existingEntry = null)
        {
            try
            {
                var dialog = new JournalEntryDialog(_availableLedgers, existingEntry);
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.JournalEntry != null)
                {
                    if (existingEntry != null)
                    {
                        // Update existing entry
                        var index = _ledgerEntries.IndexOf(existingEntry);
                        if (index >= 0 && index < _ledgerEntries.Count)
                        {
                            _ledgerEntries[index] = dialog.JournalEntry;
                            UpdateStatus("Entry updated successfully");
                        }
                        else
                        {
                            // If entry not found, add as new entry
                            dialog.JournalEntry.SerialNumber = _nextSerialNumber++;
                            _ledgerEntries.Add(dialog.JournalEntry);
                            UpdateStatus("Entry added successfully");
                        }
                    }
                    else
                    {
                        // Add new entry
                        dialog.JournalEntry.SerialNumber = _nextSerialNumber++;
                        _ledgerEntries.Add(dialog.JournalEntry);
                        UpdateStatus("Entry added successfully");
                    }
                    
                    RefreshGrid();
                    UpdateTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void ShowLedgerSelectionDialog()
        {
            var dialog = new LedgerSelectionDialog(_availableLedgers, "Select Ledger for Journal Entry");
            dialog.StartPosition = FormStartPosition.CenterParent;
            
            if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedLedger != null)
            {
                // Create a new entry with the selected ledger
                var newEntry = new JournalEntryDisplay
                {
                    LedgerId = dialog.SelectedLedger.Id,
                    LedgerName = dialog.SelectedLedger.Name,
                    LedgerCode = dialog.SelectedLedger.Code,
                    EntryType = JournalEntryLedgerType.Debit, // Default to Debit
                    Amount = 0,
                    Description = ""
                };
                
                ShowLedgerEntryDialog(newEntry);
            }
        }

        private void RefreshGrid()
        {
            dgvLedgerEntries.DataSource = null;
            dgvLedgerEntries.DataSource = _ledgerEntries;
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                btnSave.Enabled = false;
                UpdateStatus("Saving journal entry...");

                var request = CreateJournalEntryRequestFromForm();
                var result = await _journalEntryService.CreateJournalEntryAsync(request);

                if (result != null)
                {
                    MessageBox.Show("Journal entry saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to save journal entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Failed to save journal entry");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"Error: {ex.Message}");
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (_ledgerEntries.Any())
            {
                var result = MessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.", 
                    "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }
            
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTransactionNumber.Text))
            {
                MessageBox.Show("Please enter a transaction number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTransactionNumber.Focus();
                return false;
            }

            if (!_ledgerEntries.Any())
            {
                MessageBox.Show("Please add at least one ledger entry.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnAddEntry.Focus();
                return false;
            }

            var totalDebit = _ledgerEntries.Where(e => e.EntryType == JournalEntryLedgerType.Debit).Sum(e => e.Amount);
            var totalCredit = _ledgerEntries.Where(e => e.EntryType == JournalEntryLedgerType.Credit).Sum(e => e.Amount);
            var difference = Math.Abs(totalDebit - totalCredit);

            if (difference >= 0.01m)
            {
                MessageBox.Show($"Journal entries are not balanced. Difference: {difference:N2}\n\nTotal Debit: {totalDebit:N2}\nTotal Credit: {totalCredit:N2}", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private CreateJournalEntryRequest CreateJournalEntryRequestFromForm()
        {
            var request = new CreateJournalEntryRequest
            {
                EntryNumber = txtTransactionNumber.Text,
                Type = (JournalEntryType)cmbJournalType.SelectedItem,
                EntryDate = dtpTransactionDate.Value,
                ReferenceNumber = txtReferenceNumber.Text,
                Notes = txtNotes.Text,
                Status = "Draft",
                CompanyId = _selectedCompany.Id,
                FinancialYearId = _selectedFinancialYear.Id.ToString(),
                LedgerEntries = new List<CreateJournalEntryLedgerRequest>()
            };

            // Add ledger entries
            foreach (var entry in _ledgerEntries)
            {
                request.LedgerEntries.Add(new CreateJournalEntryLedgerRequest
                {
                    LedgerId = entry.LedgerId.ToString(),
                    Type = entry.EntryType,
                    Amount = entry.Amount,
                    Description = entry.Description,
                    SerialNumber = entry.SerialNumber
                });
            }

            return request;
        }
    }

}
