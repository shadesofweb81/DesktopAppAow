using WinFormsApp1.Models;
using WinFormsApp1.Services;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using System.ComponentModel;

namespace WinFormsApp1.Forms.Transaction
{
    // Custom class for grid editing with settable debit/credit amounts
    public class EditableJournalEntryDisplay : JournalEntryDisplay
    {
        public new decimal DebitAmount
        {
            get => EntryType == JournalEntryLedgerType.Debit ? Amount : 0;
            set
            {
                if (value > 0)
                {
                    Amount = value;
                    EntryType = JournalEntryLedgerType.Debit;
                }
                else
                {
                    Amount = 0;
                }
            }
        }

        public new decimal CreditAmount
        {
            get => EntryType == JournalEntryLedgerType.Credit ? Amount : 0;
            set
            {
                if (value > 0)
                {
                    Amount = value;
                    EntryType = JournalEntryLedgerType.Credit;
                }
                else
                {
                    Amount = 0;
                }
            }
        }
    }

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
        private SfDataGrid sfDataGrid = null!;
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
        private ContextMenuStrip contextMenuStrip = null!;

        // Data
        private List<EditableJournalEntryDisplay> _ledgerEntries = new List<EditableJournalEntryDisplay>();
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
            sfDataGrid = new SfDataGrid();
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
            contextMenuStrip = new ContextMenuStrip();

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

            // SfDataGrid for ledger entries
            sfDataGrid.Location = new Point(10, 25);
            sfDataGrid.Size = new Size(1120, 300);
            sfDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            entriesGroup.Controls.Add(sfDataGrid);

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
            SetupSfDataGrid();
            SetupContextMenu();
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

        private void SetupContextMenu()
        {
            try
            {
                // Create context menu items
                var editMenuItem = new ToolStripMenuItem("&Edit Entry", null, ContextMenu_Edit_Click)
                {
                    ShortcutKeys = Keys.F3,
                    Tag = "Edit"
                };

                var deleteMenuItem = new ToolStripMenuItem("&Delete Entry", null, ContextMenu_Delete_Click)
                {
                    ShortcutKeys = Keys.Delete,
                    Tag = "Delete"
                };

                // Add separator and items to context menu
                contextMenuStrip.Items.Add(editMenuItem);
                contextMenuStrip.Items.Add(new ToolStripSeparator());
                contextMenuStrip.Items.Add(deleteMenuItem);

                // Assign context menu to the grid
                sfDataGrid.ContextMenuStrip = contextMenuStrip;

                // Enable/disable menu items based on selection
                contextMenuStrip.Opening += ContextMenuStrip_Opening;
                
                // Set context menu properties
                contextMenuStrip.ShowCheckMargin = false;
                contextMenuStrip.ShowImageMargin = false;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error setting up context menu: {ex.Message}");
                Console.WriteLine($"Error setting up context menu: {ex}");
            }
        }

        private void SetupSfDataGrid()
        {
            try
            {
                // Configure SfDataGrid properties
                sfDataGrid.AutoGenerateColumns = false;
                sfDataGrid.AllowEditing = true;
                sfDataGrid.AllowDeleting = true;
                sfDataGrid.AllowGrouping = false;
                sfDataGrid.AllowSorting = false;
                sfDataGrid.AllowFiltering = false;
                sfDataGrid.ShowRowHeader = false;
                
                // Disable adding new rows directly in the grid
                sfDataGrid.AddNewRowPosition = RowPosition.None;
                
                // Configure editing behavior
                sfDataGrid.EditMode = EditMode.SingleClick;

                // Set data source
                if (_ledgerEntries == null)
                {
                    _ledgerEntries = new List<EditableJournalEntryDisplay>();
                }
                sfDataGrid.DataSource = _ledgerEntries;

                // Add columns
                sfDataGrid.Columns.Add(new GridTextColumn()
                {
                    MappingName = "SerialNumber",
                    HeaderText = "S.No",
                    Width = 50,
                    AllowEditing = false
                });

                sfDataGrid.Columns.Add(new GridTextColumn()
                {
                    MappingName = "LedgerName",
                    HeaderText = "Ledger Name",
                    Width = 300,
                    AllowEditing = false
                });

                sfDataGrid.Columns.Add(new GridTextColumn()
                {
                    MappingName = "LedgerCode",
                    HeaderText = "Code",
                    Width = 100,
                    AllowEditing = false
                });

                // Configure Debit Amount column for inline editing
                var debitColumn = new GridNumericColumn()
                {
                    MappingName = "DebitAmount",
                    HeaderText = "Debit Amount",
                    Width = 120,
                    AllowEditing = true
                };
                sfDataGrid.Columns.Add(debitColumn);

                // Configure Credit Amount column for inline editing
                var creditColumn = new GridNumericColumn()
                {
                    MappingName = "CreditAmount",
                    HeaderText = "Credit Amount",
                    Width = 120,
                    AllowEditing = true
                };
                sfDataGrid.Columns.Add(creditColumn);

                // Configure Description column for inline editing
                var descriptionColumn = new GridTextColumn()
                {
                    MappingName = "Description",
                    HeaderText = "Description",
                    Width = 200,
                    AllowEditing = true
                };
                sfDataGrid.Columns.Add(descriptionColumn);

                // Configure selection
                sfDataGrid.SelectionMode = GridSelectionMode.Single;
                sfDataGrid.SelectionUnit = SelectionUnit.Cell;

                // Configure appearance
                sfDataGrid.Style.BorderColor = Color.LightGray;

                // Add event handlers for inline editing
                sfDataGrid.CurrentCellBeginEdit += SfDataGrid_CurrentCellBeginEdit;
                sfDataGrid.CurrentCellEndEdit += SfDataGrid_CurrentCellEndEdit;
                sfDataGrid.RecordDeleted += SfDataGrid_RecordDeleted;
                sfDataGrid.CellClick += SfDataGrid_CellClick;
                sfDataGrid.MouseClick += SfDataGrid_MouseClick;
                sfDataGrid.SelectionChanged += SfDataGrid_SelectionChanged;
                sfDataGrid.KeyDown += SfDataGrid_KeyDown;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error setting up SfDataGrid: {ex.Message}");
                Console.WriteLine($"Error setting up SfDataGrid: {ex}");
            }
        }

        private void SetupEventHandlers()
        {
            // Form events
            KeyDown += JournalEntryForm_KeyDown;
            Load += JournalEntryForm_Load;

            // Control events
            txtTransactionNumber.TextChanged += (s, e) => UpdateStatus();
            dtpTransactionDate.ValueChanged += (s, e) => UpdateStatus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Intercept Enter key at the form level to prevent tab behavior
            if (keyData == Keys.Enter && sfDataGrid.Focused)
            {
                // Let the grid handle the Enter key
                return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
            try
            {
                // Ensure SfDataGrid is properly initialized
                if (sfDataGrid != null && _ledgerEntries != null)
                {
                    RefreshGrid();
                }

                cmbJournalType.Focus();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading form: {ex.Message}");
                Console.WriteLine($"Error in Form_Load: {ex}");
            }
        }

        private void JournalEntryForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.Handled = true;
                    break;                  
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
                    if (sfDataGrid.Focused)
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



        private void SfDataGrid_RecordDeleted(object? sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletedEventArgs e)
        {
            try
            {
                // Update totals when record is deleted
                UpdateTotals();
                UpdateStatus("Entry deleted successfully");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error deleting record: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_RecordDeleted: {ex}");
            }
        }

        private void BtnAddEntry_Click(object? sender, EventArgs e)
        {
            // Add a new entry directly to the grid for inline editing
            var newEntry = CreateNewEntry();
            _ledgerEntries.Add(newEntry);
            RefreshGrid();
            
            // Select the new row and focus on the first editable cell
            sfDataGrid.SelectedIndex = _ledgerEntries.Count - 1;
            UpdateStatus("New entry added - double-click on Ledger Name to select a ledger, then enter amounts");
        }

        private void BtnEditEntry_Click(object? sender, EventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                var selectedEntry = sfDataGrid.SelectedItem as EditableJournalEntryDisplay;
                if (selectedEntry != null)
                {
                    ShowLedgerEntryDialog(selectedEntry);
                }
            }
        }

        private void BtnDeleteEntry_Click(object? sender, EventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                var selectedEntry = sfDataGrid.SelectedItem as EditableJournalEntryDisplay;
                if (selectedEntry != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete this entry?\n\nLedger: {selectedEntry.LedgerName}\nDebit: {selectedEntry.DebitAmount:N2}\nCredit: {selectedEntry.CreditAmount:N2}", 
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

        private void ShowLedgerEntryDialog(EditableJournalEntryDisplay? existingEntry = null)
        {
            try
            {
                var dialog = new JournalEntryDialog(_availableLedgers, existingEntry);
                dialog.StartPosition = FormStartPosition.CenterParent;
                
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.JournalEntry != null)
                {
                    // Convert JournalEntryDisplay to EditableJournalEntryDisplay
                    var editableEntry = new EditableJournalEntryDisplay
                    {
                        Id = dialog.JournalEntry.Id,
                        LedgerId = dialog.JournalEntry.LedgerId,
                        LedgerName = dialog.JournalEntry.LedgerName,
                        LedgerCode = dialog.JournalEntry.LedgerCode,
                        EntryType = dialog.JournalEntry.EntryType,
                        Amount = dialog.JournalEntry.Amount,
                        Description = dialog.JournalEntry.Description,
                        SerialNumber = dialog.JournalEntry.SerialNumber
                    };

                    if (existingEntry != null)
                    {
                        // Update existing entry
                        var index = _ledgerEntries.IndexOf(existingEntry);
                        if (index >= 0 && index < _ledgerEntries.Count)
                        {
                            _ledgerEntries[index] = editableEntry;
                            UpdateStatus("Entry updated successfully");
                        }
                        else
                        {
                            // If entry not found, add as new entry
                            editableEntry.SerialNumber = _nextSerialNumber++;
                            _ledgerEntries.Add(editableEntry);
                            UpdateStatus("Entry added successfully");
                        }
                    }
                    else
                    {
                        // Add new entry
                        editableEntry.SerialNumber = _nextSerialNumber++;
                        _ledgerEntries.Add(editableEntry);
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
                var newEntry = new EditableJournalEntryDisplay
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
            try
            {
                if (sfDataGrid == null || _ledgerEntries == null)
                    return;

                // Refresh the data source
                sfDataGrid.DataSource = null;
                sfDataGrid.DataSource = _ledgerEntries;

                // Ensure the grid reflects the current data
                if (_ledgerEntries.Count > 0)
                {
                    sfDataGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error refreshing grid: {ex.Message}");
                Console.WriteLine($"Error refreshing grid: {ex}");
            }
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
                Type = cmbJournalType.SelectedItem is JournalEntryType type ? type : JournalEntryType.Journal,
                TransactionDate = dtpTransactionDate.Value,
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
                    EntryType = entry.EntryType,
                    Amount = entry.Amount,
                    Description = entry.Description,
                    SerialNumber = entry.SerialNumber
                });
            }

            return request;
        }

        private void SfDataGrid_CellClick(object? sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                // Handle double-click on Ledger Name column to open ledger selection
                if (e.MouseEventArgs?.Clicks == 2)
                {
                    // Check if we're clicking on the Ledger Name column
                    var currentCell = sfDataGrid.CurrentCell;
                    if (currentCell?.Column != null)
                    {
                        // Try to get the column name to check if it's LedgerName
                        var columnName = currentCell.Column.GetType().GetProperty("MappingName")?.GetValue(currentCell.Column)?.ToString();
                        if (columnName == "LedgerName")
                        {
                            if (sfDataGrid.SelectedItem is EditableJournalEntryDisplay entry)
                            {
                                // Open ledger selection dialog
                                ShowLedgerSelectionDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error handling cell click: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_CellClick: {ex}");
            }
        }


        private void SfDataGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            try
            {
                // Handle right-click to show context menu
                if (e.Button == MouseButtons.Right)
                {
                    // Check if we have any entries and a selected item
                    if (_ledgerEntries.Count > 0 && sfDataGrid.SelectedItem != null)
                    {
                        // Show context menu at mouse position
                        contextMenuStrip.Show(sfDataGrid, e.Location);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error handling mouse click: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_MouseClick: {ex}");
            }
        }

        private void SfDataGrid_SelectionChanged(object? sender, Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs e)
        {
            try
            {
                // Update button states based on current cell selection
                bool hasSelection = sfDataGrid.SelectedItem != null;
                btnEditEntry.Enabled = hasSelection;
                btnDeleteEntry.Enabled = hasSelection;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error handling selection change: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_SelectionChanged: {ex}");
            }
        }

        private void SfDataGrid_KeyDown(object? sender, KeyEventArgs e)
        {
            try
            {
                // Handle Enter key to start editing the current cell
                if (e.KeyCode == Keys.Enter)
                {
                    // Always prevent default Enter behavior when grid has focus
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    
                    // Check if we have a selected item and the current column is editable
                    if (sfDataGrid.SelectedItem != null)
                    {
                        var currentCell = sfDataGrid.CurrentCell;
                        if (currentCell != null && currentCell.Column.AllowEditing)
                        {
                            // Start editing by simulating a mouse double-click on the current cell
                            StartCellEditingDirect();
                            
                            UpdateStatus("Cell ready for editing - start typing to edit");
                            return;
                        }
                    }
                    
                    // If we reach here, just prevent the default behavior without starting edit
                    UpdateStatus("Enter key pressed - focus stays in grid");
                    return;
                }
                // Handle F2 key to start editing (standard Windows behavior)
                else if (e.KeyCode == Keys.F2)
                {
                    if (sfDataGrid.SelectedItem != null)
                    {
                        var currentCell = sfDataGrid.CurrentCell;
                        if (currentCell != null && currentCell.Column.AllowEditing)
                        {
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            
                            StartCellEditingDirect();
                            
                            UpdateStatus("Cell ready for editing - start typing to edit");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error handling key down: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_KeyDown: {ex}");
            }
        }

        private void StartCellEditingDirect()
        {
            try
            {
                var currentCell = sfDataGrid.CurrentCell;
                if (currentCell != null)
                {
                    // Get the column mapping name to identify the column type
                    var columnName = currentCell.Column.GetType().GetProperty("MappingName")?.GetValue(currentCell.Column)?.ToString();
                    
                    // Check if the column allows editing
                    if (!currentCell.Column.AllowEditing)
                    {
                        UpdateStatus($"Column '{columnName}' does not allow editing");
                        return;
                    }
                    
                    // Ensure the grid has focus
                    sfDataGrid.Focus();
                    
                    // Use a timer to delay the SendKeys to ensure focus is properly set
                    var timer = new System.Windows.Forms.Timer();
                    timer.Interval = 50; // 50ms delay
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        timer.Dispose();
                        
                        try
                        {
                            // Check if cell is now in editing mode
                            if (sfDataGrid.CurrentCell != null && sfDataGrid.CurrentCell.IsEditing)
                            {
                                UpdateStatus($"Cell is now in editing mode - {columnName} (AllowEditing: {currentCell.Column.AllowEditing})");
                                return;
                            }
                            
                            // For numeric columns (DebitAmount, CreditAmount), trigger edit mode differently
                            if (columnName == "DebitAmount" || columnName == "CreditAmount")
                            {
                                // For numeric columns, simulate typing a number to trigger edit mode
                                SendKeys.SendWait("0");
                                SendKeys.SendWait("{BACKSPACE}");
                                UpdateStatus($"Editing {columnName} - enter numeric value (AllowEditing: {currentCell.Column.AllowEditing}, IsEditing: {sfDataGrid.CurrentCell?.IsEditing})");
                            }
                            else if (columnName == "Description")
                            {
                                // For text columns, simulate typing a character
                                SendKeys.SendWait("a");
                                SendKeys.SendWait("{BACKSPACE}");
                                UpdateStatus($"Editing {columnName} - enter text value (AllowEditing: {currentCell.Column.AllowEditing}, IsEditing: {sfDataGrid.CurrentCell?.IsEditing})");
                            }
                            else
                            {
                                UpdateStatus($"Column '{columnName}' - AllowEditing: {currentCell.Column.AllowEditing}");
                            }
                        }
                        catch (Exception ex)
                        {
                            UpdateStatus($"Error in timer callback: {ex.Message}");
                        }
                    };
                    timer.Start();
                }
                else
                {
                    UpdateStatus("No cell selected");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error starting cell editing: {ex.Message}");
                Console.WriteLine($"Error in StartCellEditingDirect: {ex}");
            }
        }

        private void SfDataGrid_CurrentCellBeginEdit(object? sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellBeginEditEventArgs e)
        {
            try
            {
                // Customize editing behavior when cell editing begins
                var currentCell = sfDataGrid.CurrentCell;
                if (currentCell != null)
                {
                    // Get the column mapping name to identify the column type
                    var columnName = currentCell.Column.GetType().GetProperty("MappingName")?.GetValue(currentCell.Column)?.ToString();
                    
                    // Check if the column allows editing
                    bool allowEditing = currentCell.Column.AllowEditing;
                    bool isEditing = currentCell.IsEditing;
                    
                    // Only proceed if editing is allowed
                    if (allowEditing)
                    {
                        // Provide specific feedback for different column types
                        switch (columnName)
                        {
                            case "DebitAmount":
                                UpdateStatus($"Editing Debit Amount - enter numeric value (AllowEditing: {allowEditing}, IsEditing: {isEditing})");
                                break;
                            case "CreditAmount":
                                UpdateStatus($"Editing Credit Amount - enter numeric value (AllowEditing: {allowEditing}, IsEditing: {isEditing})");
                                break;
                            case "Description":
                                UpdateStatus($"Editing Description - enter text value (AllowEditing: {allowEditing}, IsEditing: {isEditing})");
                                break;
                            default:
                                UpdateStatus($"Editing cell... (AllowEditing: {allowEditing}, IsEditing: {isEditing})");
                                break;
                        }
                        
                        // Ensure the cell is properly focused for editing
                        sfDataGrid.Focus();
                    }
                    else
                    {
                        UpdateStatus($"Column '{columnName}' - AllowEditing: {allowEditing} (editing not allowed)");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error beginning cell edit: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_CurrentCellBeginEdit: {ex}");
            }
        }

        private void SfDataGrid_CurrentCellEndEdit(object? sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellEndEditEventArgs e)
        {
            try
            {
                // Handle when cell editing ends
                var currentCell = sfDataGrid.CurrentCell;
                if (currentCell != null)
                {
                    // Get the column mapping name to identify the column type
                    var columnName = currentCell.Column.GetType().GetProperty("MappingName")?.GetValue(currentCell.Column)?.ToString();
                    
                    // Update totals when editing ends
                UpdateTotals();
                    
                    // Provide specific feedback for different column types
                    switch (columnName)
                    {
                        case "DebitAmount":
                        case "CreditAmount":
                            UpdateStatus("Amount updated - totals recalculated");
                            break;
                        case "Description":
                            UpdateStatus("Description updated");
                            break;
                        default:
                            UpdateStatus("Cell editing completed");
                            break;
                    }
                }
                else
                {
                    UpdateTotals();
                    UpdateStatus("Cell editing completed");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error updating cell value: {ex.Message}");
                Console.WriteLine($"Error in SfDataGrid_CurrentCellEndEdit: {ex}");
            }
        }

        private EditableJournalEntryDisplay CreateNewEntry()
        {
            // Create a new entry with default values
            var newEntry = new EditableJournalEntryDisplay
            {
                Id = Guid.NewGuid(),
                LedgerId = Guid.Empty,
                LedgerName = "",
                LedgerCode = "",
                EntryType = JournalEntryLedgerType.Debit,
                Amount = 0,
                Description = "",
                SerialNumber = _nextSerialNumber++
            };
            
            return newEntry;
        }

        // Context Menu Event Handlers
        private void ContextMenuStrip_Opening(object? sender, CancelEventArgs e)
        {
            try
            {
                // Enable/disable menu items based on whether a row is selected
                bool hasSelection = sfDataGrid.SelectedItem != null && _ledgerEntries.Count > 0;
                
                foreach (ToolStripItem item in contextMenuStrip.Items)
                {
                    if (item is ToolStripMenuItem menuItem && menuItem != null)
                    {
                        menuItem.Enabled = hasSelection;
                    }
                }
                
                // If no selection, cancel the menu opening
                if (!hasSelection)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error in context menu opening: {ex.Message}");
                Console.WriteLine($"Error in ContextMenuStrip_Opening: {ex}");
                e.Cancel = true;
            }
        }

        private void ContextMenu_Edit_Click(object? sender, EventArgs e)
        {
            try
            {
                // Get the selected item and edit it
                if (sfDataGrid.SelectedItem is EditableJournalEntryDisplay selectedEntry)
                {
                    ShowLedgerEntryDialog(selectedEntry);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error editing entry from context menu: {ex.Message}");
                Console.WriteLine($"Error in ContextMenu_Edit_Click: {ex}");
            }
        }

        private void ContextMenu_Delete_Click(object? sender, EventArgs e)
        {
            try
            {
                // Get the selected item and delete it
                if (sfDataGrid.SelectedItem is EditableJournalEntryDisplay selectedEntry)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete this entry?\n\nLedger: {selectedEntry.LedgerName}\nDebit: {selectedEntry.DebitAmount:N2}\nCredit: {selectedEntry.CreditAmount:N2}", 
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
            catch (Exception ex)
            {
                UpdateStatus($"Error deleting entry from context menu: {ex.Message}");
                Console.WriteLine($"Error in ContextMenu_Delete_Click: {ex}");
            }
        }
    }

}

