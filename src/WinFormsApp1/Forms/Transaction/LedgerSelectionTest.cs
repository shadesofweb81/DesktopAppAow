using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms.Transaction
{
    /// <summary>
    /// Simple test form to demonstrate the LedgerSelectionDialog functionality
    /// </summary>
    public partial class LedgerSelectionTest : Form
    {
        private Button btnTestPartyLedger = null!;
        private Button btnTestAccountLedger = null!;
        private Label lblSelectedParty = null!;
        private Label lblSelectedAccount = null!;
        private TextBox txtResults = null!;
        private List<LedgerModel> _testLedgers;

        public LedgerSelectionTest()
        {
            _testLedgers = CreateTestLedgers();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Ledger Selection Dialog Test";
            Size = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;

            btnTestPartyLedger = new Button();
            btnTestAccountLedger = new Button();
            lblSelectedParty = new Label();
            lblSelectedAccount = new Label();
            txtResults = new TextBox();

            SuspendLayout();

            // Test Party Ledger Button
            btnTestPartyLedger.Location = new Point(20, 20);
            btnTestPartyLedger.Size = new Size(150, 30);
            btnTestPartyLedger.Text = "Select Party Ledger (F4)";
            btnTestPartyLedger.UseVisualStyleBackColor = true;
            btnTestPartyLedger.Click += BtnTestPartyLedger_Click;

            // Test Account Ledger Button
            btnTestAccountLedger.Location = new Point(180, 20);
            btnTestAccountLedger.Size = new Size(150, 30);
            btnTestAccountLedger.Text = "Select Account Ledger (F5)";
            btnTestAccountLedger.UseVisualStyleBackColor = true;
            btnTestAccountLedger.Click += BtnTestAccountLedger_Click;

            // Selected Party Label
            lblSelectedParty.Location = new Point(20, 60);
            lblSelectedParty.Size = new Size(550, 20);
            lblSelectedParty.Text = "Selected Party Ledger: None";
            lblSelectedParty.ForeColor = Color.DarkBlue;

            // Selected Account Label
            lblSelectedAccount.Location = new Point(20, 85);
            lblSelectedAccount.Size = new Size(550, 20);
            lblSelectedAccount.Text = "Selected Account Ledger: None";
            lblSelectedAccount.ForeColor = Color.DarkGreen;

            // Results TextBox
            txtResults.Location = new Point(20, 115);
            txtResults.Size = new Size(550, 220);
            txtResults.Multiline = true;
            txtResults.ScrollBars = ScrollBars.Vertical;
            txtResults.ReadOnly = true;
            txtResults.Text = GetTestInfo();

            Controls.AddRange(new Control[] { 
                btnTestPartyLedger, btnTestAccountLedger, 
                lblSelectedParty, lblSelectedAccount, txtResults 
            });

            ResumeLayout(false);
            PerformLayout();

            // Enable keyboard shortcuts
            KeyPreview = true;
            KeyDown += LedgerSelectionTest_KeyDown;
        }

        private void LedgerSelectionTest_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                BtnTestPartyLedger_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                BtnTestAccountLedger_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void BtnTestPartyLedger_Click(object? sender, EventArgs e)
        {
            try
            {
                var dialog = new LedgerSelectionDialog(
                    _testLedgers, 
                    "Select Party Ledger - Test", 
                    "Party Ledgers (Customers/Suppliers)"
                );
                
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    lblSelectedParty.Text = $"Selected Party Ledger: {dialog.SelectedLedger.DisplayName}";
                    AppendResult($"Party Ledger Selected: {dialog.SelectedLedger.DisplayName} (Category: {dialog.SelectedLedger.Category})");
                }
                else
                {
                    AppendResult("Party ledger selection cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendResult($"Error selecting party ledger: {ex.Message}");
            }
        }

        private void BtnTestAccountLedger_Click(object? sender, EventArgs e)
        {
            try
            {
                var dialog = new LedgerSelectionDialog(
                    _testLedgers, 
                    "Select Account Ledger - Test", 
                    "Account Ledgers (Income/Expense/Asset/Liability)"
                );
                
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedLedger != null)
                {
                    lblSelectedAccount.Text = $"Selected Account Ledger: {dialog.SelectedLedger.DisplayName}";
                    AppendResult($"Account Ledger Selected: {dialog.SelectedLedger.DisplayName} (Category: {dialog.SelectedLedger.Category})");
                }
                else
                {
                    AppendResult("Account ledger selection cancelled.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendResult($"Error selecting account ledger: {ex.Message}");
            }
        }

        private void AppendResult(string message)
        {
            txtResults.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\r\n");
            txtResults.ScrollToCaret();
        }

        private string GetTestInfo()
        {
            return $@"Ledger Selection Dialog Test
============================

This test form demonstrates the LedgerSelectionDialog functionality.

Available Features:
• F4 or Click - Select Party Ledger (Customers/Suppliers)
• F5 or Click - Select Account Ledger (Income/Expense/Assets/Liabilities)

Test Data:
• {_testLedgers.Count} test ledgers available
• Various categories: Customers, Suppliers, Sales, Purchases, Bank, Cash, etc.
• Filter and search functionality
• Keyboard navigation support

Instructions:
1. Click the buttons or use F4/F5 to open selection dialogs
2. Try the search and filter options in the dialog
3. Use keyboard navigation (arrows, enter, escape)
4. Selected ledgers will be displayed above

Test Results:
";
        }

        private List<LedgerModel> CreateTestLedgers()
        {
            var ledgers = new List<LedgerModel>();
            var companyId = Guid.NewGuid();

            // Create test party ledgers (customers and suppliers)
            ledgers.AddRange(new[]
            {
                new LedgerModel { Id = Guid.NewGuid(), Name = "ABC Corp", Category = "Sundry Debtor", Code = "CUST001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "XYZ Ltd", Category = "Sundry Debtor", Code = "CUST002", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Quick Mart", Category = "Sundry Debtor", Code = "CUST003", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Global Supplies", Category = "Sundry Creditor", Code = "SUPP001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Office Depot", Category = "Sundry Creditor", Code = "SUPP002", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Tech Solutions", Category = "Sundry Creditor", Code = "SUPP003", CompanyId = companyId, IsGroup = false }
            });

            // Create test account ledgers (income, expense, assets, liabilities)
            ledgers.AddRange(new[]
            {
                new LedgerModel { Id = Guid.NewGuid(), Name = "Sales", Category = "Income", Code = "INC001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Service Revenue", Category = "Income", Code = "INC002", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Purchases", Category = "Expense", Code = "EXP001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Office Supplies", Category = "Expense", Code = "EXP002", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Rent Expense", Category = "Expense", Code = "EXP003", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Bank Account - Main", Category = "Bank", Code = "BANK001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Cash in Hand", Category = "Cash", Code = "CASH001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Accounts Receivable", Category = "Asset", Code = "ASSET001", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Inventory", Category = "Asset", Code = "ASSET002", CompanyId = companyId, IsGroup = false },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Accounts Payable", Category = "Liability", Code = "LIAB001", CompanyId = companyId, IsGroup = false }
            });

            // Create some group ledgers
            ledgers.AddRange(new[]
            {
                new LedgerModel { Id = Guid.NewGuid(), Name = "Current Assets", Category = "Asset Group", Code = "GRP001", CompanyId = companyId, IsGroup = true },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Current Liabilities", Category = "Liability Group", Code = "GRP002", CompanyId = companyId, IsGroup = true },
                new LedgerModel { Id = Guid.NewGuid(), Name = "Operating Income", Category = "Income Group", Code = "GRP003", CompanyId = companyId, IsGroup = true }
            });

            return ledgers;
        }
    }
}
