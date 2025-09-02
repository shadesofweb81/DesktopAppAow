using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class LedgerSelectionDialog : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvLedgers = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;
        private ComboBox cmbFilterType = null!;
        private CheckBox chkShowGroups = null!;
        private List<LedgerModel> _allLedgers;
        private List<LedgerModel> _filteredLedgers;

        public LedgerModel? SelectedLedger { get; private set; }
        public string DialogTitle { get; set; } = "Select Ledger";
        public string FilterHint { get; set; } = "All Ledgers";
        public string DefaultFilterType { get; set; } = "All Ledgers";

        public LedgerSelectionDialog(List<LedgerModel> ledgers, string title = "Select Ledger", string filterHint = "All Ledgers", string defaultFilterType = "All Ledgers")
        {
            _allLedgers = ledgers ?? new List<LedgerModel>();
            _filteredLedgers = new List<LedgerModel>(_allLedgers);
            DialogTitle = title;
            FilterHint = filterHint;
            DefaultFilterType = defaultFilterType;
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Text = DialogTitle;
            Size = new Size(900, 650);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;
            
            txtSearch = new TextBox();
            cmbFilterType = new ComboBox();
            chkShowGroups = new CheckBox();
            dgvLedgers = new DataGridView();
            btnOK = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Filter type dropdown
            var lblFilter = new Label
            {
                Location = new Point(20, 25),
                Size = new Size(80, 20),
                Text = "Filter Type:",
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbFilterType.Location = new Point(105, 22);
            cmbFilterType.Size = new Size(150, 25);
            cmbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterType.Items.AddRange(new string[] 
            { 
                "All Ledgers", 
                "Party Ledgers", 
                "Account Ledgers", 
                "Assets", 
                "Liabilities", 
                "Income", 
                "Expenses",
                "Sundry Debtors",
                "Sundry Creditors",
                "Bank Accounts",
                "Cash Accounts"
            });
            // Set default filter type if specified
            var defaultIndex = cmbFilterType.Items.IndexOf(DefaultFilterType);
            cmbFilterType.SelectedIndex = defaultIndex >= 0 ? defaultIndex : 0;
            cmbFilterType.SelectedIndexChanged += CmbFilterType_SelectedIndexChanged;

            // Show groups checkbox
            chkShowGroups.Location = new Point(270, 22);
            chkShowGroups.Size = new Size(120, 25);
            chkShowGroups.Text = "Show Groups";
            chkShowGroups.CheckedChanged += ChkShowGroups_CheckedChanged;

            // Search textbox
            var lblSearch = new Label
            {
                Location = new Point(20, 60),
                Size = new Size(50, 20),
                Text = "Search:",
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtSearch.Location = new Point(75, 57);
            txtSearch.Size = new Size(790, 25);
            txtSearch.PlaceholderText = $"Type to search ledgers by name, code, or category... ({FilterHint})";
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Ledgers grid
            dgvLedgers.Location = new Point(20, 95);
            dgvLedgers.Size = new Size(845, 500);
            dgvLedgers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLedgers.MultiSelect = false;
            dgvLedgers.AllowUserToAddRows = false;
            dgvLedgers.AllowUserToDeleteRows = false;
            dgvLedgers.ReadOnly = true;
            dgvLedgers.RowHeadersVisible = false;
            dgvLedgers.AutoGenerateColumns = false;
            dgvLedgers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            
            SetupLedgerGrid();

            // Buttons
            btnOK.Location = new Point(710, 605);
            btnOK.Size = new Size(75, 30);
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += BtnOK_Click;
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            btnCancel.Location = new Point(790, 605);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Info label
            var lblInfo = new Label
            {
                Location = new Point(20, 610),
                Size = new Size(600, 20),
                Text = "Double-click or press Enter to select. Use filter options to narrow down results.",
                ForeColor = Color.DarkBlue,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };

            Controls.AddRange(new Control[] { 
                lblFilter, cmbFilterType, chkShowGroups, 
                lblSearch, txtSearch, 
                dgvLedgers, 
                btnOK, btnCancel, lblInfo 
            });

            LoadLedgers();
            txtSearch.Focus();

            ResumeLayout(false);
            PerformLayout();

            // Event handlers
            KeyDown += LedgerSelectionDialog_KeyDown;
            dgvLedgers.KeyDown += DgvLedgers_KeyDown;
            dgvLedgers.DoubleClick += DgvLedgers_DoubleClick;
        }

        private void SetupLedgerGrid()
        {
            dgvLedgers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Code",
                HeaderText = "Code",
                DataPropertyName = "Code",
                Width = 100
            });

            dgvLedgers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Ledger Name",
                DataPropertyName = "Name",
                Width = 250
            });

            dgvLedgers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });

            dgvLedgers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ParentName",
                HeaderText = "Parent Group",
                DataPropertyName = "Parent.Name",
                Width = 150
            });

            dgvLedgers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsGroup",
                HeaderText = "Group",
                DataPropertyName = "IsGroup",
                Width = 60
            });

            dgvLedgers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsRegistered",
                HeaderText = "Registered",
                DataPropertyName = "IsRegistered",
                Width = 80
            });
        }

        private void LoadLedgers()
        {
            // Apply filters
            var filtered = _allLedgers.AsEnumerable();

            // Filter by show groups checkbox
            if (!chkShowGroups.Checked)
            {
                filtered = filtered.Where(l => !l.IsGroup);
            }

            // Filter by type
            var filterType = cmbFilterType.SelectedItem?.ToString() ?? "All Ledgers";
            switch (filterType)
            {
                case "Party Ledgers":
                    filtered = filtered.Where(l => IsPartyLedger(l));
                    break;
                case "Account Ledgers":
                    filtered = filtered.Where(l => IsAccountLedger(l));
                    break;
                case "Assets":
                    filtered = filtered.Where(l => l.Category.Contains("Asset", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Liabilities":
                    filtered = filtered.Where(l => l.Category.Contains("Liability", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Category.Contains("Creditor", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Income":
                    filtered = filtered.Where(l => l.Category.Contains("Income", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Category.Contains("Sales", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Expenses":
                    filtered = filtered.Where(l => l.Category.Contains("Expense", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Category.Contains("Purchase", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Sundry Debtors":
                    filtered = filtered.Where(l => l.Category.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Parent?.Category.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase) == true);
                    break;
                case "Sundry Creditors":
                    filtered = filtered.Where(l => l.Category.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Parent?.Category.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase) == true);
                    break;
                case "Bank Accounts":
                    filtered = filtered.Where(l => l.Category.Contains("Bank", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Name.Contains("Bank", StringComparison.OrdinalIgnoreCase));
                    break;
                case "Cash Accounts":
                    filtered = filtered.Where(l => l.Category.Contains("Cash", StringComparison.OrdinalIgnoreCase) ||
                                                   l.Name.Contains("Cash", StringComparison.OrdinalIgnoreCase));
                    break;
            }

            // Apply search filter
            var searchText = txtSearch.Text?.ToLower() ?? "";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(l => 
                    l.Code.ToLower().Contains(searchText) ||
                    l.Name.ToLower().Contains(searchText) ||
                    l.Category.ToLower().Contains(searchText) ||
                    (l.Parent?.Name?.ToLower().Contains(searchText) == true)
                );
            }

            _filteredLedgers = filtered.OrderBy(l => l.IsGroup ? 0 : 1) // Groups first
                                      .ThenBy(l => l.Category)
                                      .ThenBy(l => l.Name)
                                      .ToList();
            
            dgvLedgers.DataSource = _filteredLedgers;

            // Update status
            var statusText = $"Showing {_filteredLedgers.Count} of {_allLedgers.Count} ledgers";
            if (!string.IsNullOrEmpty(searchText))
            {
                statusText += $" (filtered by: '{searchText}')";
            }
            Text = $"{DialogTitle} - {statusText}";
        }

        private bool IsPartyLedger(LedgerModel ledger)
        {
            // Party ledgers are typically customers (Sundry Debtors) and suppliers (Sundry Creditors)
            return ledger.Category.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Customer", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Supplier", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Parent?.Category.Contains("Sundry Debtor", StringComparison.OrdinalIgnoreCase) == true ||
                   ledger.Parent?.Category.Contains("Sundry Creditor", StringComparison.OrdinalIgnoreCase) == true;
        }

        private bool IsAccountLedger(LedgerModel ledger)
        {
            // Account ledgers are typically income, expense, asset, liability accounts
            return ledger.Category.Contains("Sales", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Purchase", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Income", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Expense", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Asset", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Category.Contains("Liability", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Name.Equals("Sales", StringComparison.OrdinalIgnoreCase) ||
                   ledger.Name.Equals("Purchases", StringComparison.OrdinalIgnoreCase);
        }

        private void CmbFilterType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadLedgers();
        }

        private void ChkShowGroups_CheckedChanged(object? sender, EventArgs e)
        {
            LoadLedgers();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadLedgers();
        }

        private void LedgerSelectionDialog_KeyDown(object? sender, KeyEventArgs e)
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
            else if (e.KeyCode == Keys.F1)
            {
                ShowHelp();
                e.Handled = true;
            }
        }

        private void DgvLedgers_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvLedgers_DoubleClick(object? sender, EventArgs e)
        {
            BtnOK_Click(null, EventArgs.Empty);
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (dgvLedgers.SelectedRows.Count > 0)
            {
                SelectedLedger = dgvLedgers.SelectedRows[0].DataBoundItem as LedgerModel;
                
                if (SelectedLedger != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid ledger selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a ledger.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ShowHelp()
        {
            var helpMessage = @"Ledger Selection Dialog - Help:

Navigation:
• Tab/Shift+Tab - Move between controls
• Arrow keys - Navigate grid rows
• Enter - Select highlighted ledger
• Escape - Cancel and close dialog
• F1 - Show this help

Search and Filter:
• Type in search box to filter by name, code, or category
• Use Filter Type dropdown to narrow results:
  - All Ledgers: Show all available ledgers
  - Party Ledgers: Customers and Suppliers (Sundry Debtors/Creditors)
  - Account Ledgers: Income, Expense, Asset, Liability accounts
  - Assets/Liabilities/Income/Expenses: Specific categories
  - Bank/Cash Accounts: Financial accounts
• Check 'Show Groups' to include group ledgers

Selection:
• Single-click to highlight a ledger
• Double-click or press Enter to select
• Selected ledger details are shown in the grid

Grid Columns:
• Code: Ledger code/reference
• Ledger Name: Full name of the ledger
• Category: Ledger category/type
• Parent Group: Parent ledger (if any)
• Type: Ledger type classification
• Group: Whether this is a group ledger
• Active: Whether the ledger is active
• Opening Balance: Starting balance amount

Tips:
• Use filter types for faster navigation
• Search supports partial matches
• Groups are shown first when included
• Only active ledgers are typically used for transactions";

            MessageBox.Show(helpMessage, "Ledger Selection Help", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
