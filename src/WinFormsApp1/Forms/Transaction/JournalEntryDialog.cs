using WinFormsApp1.Models;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class JournalEntryDialog : Form
    {
        private readonly List<LedgerModel> _availableLedgers;
        private readonly JournalEntryDisplay? _existingEntry;

        // Form controls
        private ComboBox cmbLedger = null!;
        private ComboBox cmbEntryType = null!;
        private TextBox txtAmount = null!;
        private TextBox txtDescription = null!;
        private Label lblLedgerInfo = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        public JournalEntryDisplay? JournalEntry { get; private set; }

        public JournalEntryDialog(List<LedgerModel> availableLedgers, JournalEntryDisplay? existingEntry = null)
        {
            _availableLedgers = availableLedgers ?? new List<LedgerModel>();
            _existingEntry = existingEntry;
            
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Text = _existingEntry == null ? "Add Journal Entry" : "Edit Journal Entry";
            Size = new Size(500, 350);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            // Initialize controls
            cmbLedger = new ComboBox();
            cmbEntryType = new ComboBox();
            txtAmount = new TextBox();
            txtDescription = new TextBox();
            lblLedgerInfo = new Label();
            btnOK = new Button();
            btnCancel = new Button();

            SuspendLayout();
            SetupLayout();
            ResumeLayout(false);
            PerformLayout();

            // Event handlers
            KeyDown += JournalEntryDialog_KeyDown;
            cmbLedger.SelectedIndexChanged += CmbLedger_SelectedIndexChanged;
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            // Load data
            LoadLedgers();
            LoadEntryTypes();
            LoadExistingData();
        }

        private void SetupLayout()
        {
            int yPos = 20;
            int labelWidth = 100;
            int controlWidth = 300;
            int spacing = 35;

            // Ledger Selection
            AddLabelAndControl("Ledger:", cmbLedger, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Entry Type
            AddLabelAndControl("Entry Type:", cmbEntryType, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Amount
            AddLabelAndControl("Amount:", txtAmount, 20, yPos, labelWidth, controlWidth);
            yPos += spacing;

            // Description
            AddLabelAndControl("Description:", txtDescription, 20, yPos, labelWidth, controlWidth);
            yPos += spacing + 10;

            // Ledger Info
            lblLedgerInfo.Location = new Point(20, yPos);
            lblLedgerInfo.Size = new Size(450, 40);
            lblLedgerInfo.ForeColor = Color.DarkBlue;
            lblLedgerInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblLedgerInfo.Text = "Select a ledger to view additional information";
            Controls.Add(lblLedgerInfo);

            yPos += 50;

            // Buttons
            btnOK.Location = new Point(320, yPos);
            btnOK.Size = new Size(75, 30);
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            Controls.Add(btnOK);

            btnCancel.Location = new Point(405, yPos);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
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

        private void LoadLedgers()
        {
            cmbLedger.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLedger.DisplayMember = "Name";
            cmbLedger.ValueMember = "Id";
            cmbLedger.DataSource = _availableLedgers.OrderBy(l => l.Name).ToList();
        }

        private void LoadEntryTypes()
        {
            cmbEntryType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEntryType.DataSource = Enum.GetValues<JournalEntryLedgerType>();
            
            // Ensure the ComboBox is properly initialized
            if (cmbEntryType.Items.Count > 0)
            {
                cmbEntryType.SelectedIndex = 0; // Set default selection
            }
        }

        private void LoadExistingData()
        {
            try
            {
                if (_existingEntry != null)
                {
                    // Select the ledger
                    var ledger = _availableLedgers.FirstOrDefault(l => l.Id == _existingEntry.LedgerId);
                    if (ledger != null)
                    {
                        cmbLedger.SelectedItem = ledger;
                    }

                    // Set entry type
                    cmbEntryType.SelectedItem = _existingEntry.EntryType;

                    // Set amount and description
                    txtAmount.Text = _existingEntry.Amount.ToString("N2");
                    txtDescription.Text = _existingEntry.Description;
                }
                else
                {
                    // Set defaults - only if ComboBox has items
                    if (cmbEntryType.Items.Count > 0)
                    {
                        cmbEntryType.SelectedIndex = 0; // Default to Debit
                    }
                    txtAmount.Text = "0.00";
                }

                // Focus on amount field
                txtAmount.Focus();
                txtAmount.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading existing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbLedger_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbLedger.SelectedItem is LedgerModel selectedLedger)
            {
                var info = $"Code: {selectedLedger.Code} | Category: {selectedLedger.Category}";
                if (selectedLedger.Parent != null)
                {
                    info += $" | Parent: {selectedLedger.Parent.Name}";
                }
                lblLedgerInfo.Text = info;
            }
            else
            {
                lblLedgerInfo.Text = "Select a ledger to view additional information";
            }
        }

        private void JournalEntryDialog_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnCancel_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var selectedLedger = cmbLedger.SelectedItem as LedgerModel;
                var selectedEntryType = (JournalEntryLedgerType)cmbEntryType.SelectedItem;
                var amount = decimal.Parse(txtAmount.Text);

                JournalEntry = new JournalEntryDisplay
                {
                    Id = _existingEntry?.Id ?? Guid.NewGuid(),
                    LedgerId = selectedLedger!.Id,
                    LedgerName = selectedLedger.Name,
                    LedgerCode = selectedLedger.Code,
                    EntryType = selectedEntryType,
                    Amount = amount,
                    Description = txtDescription.Text.Trim(),
                    SerialNumber = _existingEntry?.SerialNumber ?? 0
                };

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating journal entry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateForm()
        {
            if (cmbLedger.SelectedItem == null)
            {
                MessageBox.Show("Please select a ledger.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLedger.Focus();
                return false;
            }

            if (cmbEntryType.SelectedItem == null)
            {
                MessageBox.Show("Please select an entry type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEntryType.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                txtAmount.SelectAll();
                return false;
            }

            return true;
        }
    }
}