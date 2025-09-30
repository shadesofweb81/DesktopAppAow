using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class AccountBooksForm : BaseForm
    {
        // Main group box
        private GroupBox accountBooksGroupBox = null!;
        private Label accountBooksLabel = null!;
        
        // All buttons in single group
        private Button salesButton = null!;
        private Button purchaseReportsButton = null!;
        private Button ledgerReportButton = null!;
        private Button receiptsAndPaymentsButton = null!;
        private Button journalRegisterButton = null!;
        private Button dayBookButton = null!;
        private Button listOfAccountsButton = null!;
        private Button outstandingsButton = null!;
        private Button closeButton = null!;

        public AccountBooksForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            accountBooksGroupBox = new GroupBox();
            accountBooksLabel = new Label();
            salesButton = new Button();
            purchaseReportsButton = new Button();
            ledgerReportButton = new Button();
            receiptsAndPaymentsButton = new Button();
            journalRegisterButton = new Button();
            dayBookButton = new Button();
            listOfAccountsButton = new Button();
            outstandingsButton = new Button();
            closeButton = new Button();
            accountBooksGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // accountBooksGroupBox
            // 
            accountBooksGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            accountBooksGroupBox.Controls.Add(accountBooksLabel);
            accountBooksGroupBox.Controls.Add(ledgerReportButton);
            accountBooksGroupBox.Controls.Add(salesButton);
            accountBooksGroupBox.Controls.Add(purchaseReportsButton);
            accountBooksGroupBox.Controls.Add(receiptsAndPaymentsButton);
            accountBooksGroupBox.Controls.Add(journalRegisterButton);
            accountBooksGroupBox.Controls.Add(dayBookButton);
            accountBooksGroupBox.Controls.Add(listOfAccountsButton);
            accountBooksGroupBox.Controls.Add(outstandingsButton);
            accountBooksGroupBox.Controls.Add(closeButton);
            accountBooksGroupBox.FlatStyle = FlatStyle.Flat;
            accountBooksGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            accountBooksGroupBox.Location = new Point(0, 0);
            accountBooksGroupBox.Name = "accountBooksGroupBox";
            accountBooksGroupBox.Size = new Size(350, 400);
            accountBooksGroupBox.TabIndex = 0;
            accountBooksGroupBox.TabStop = false;
            accountBooksGroupBox.Text = "ACCOUNT BOOKS";
            // 
            // accountBooksLabel
            // 
            accountBooksLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            accountBooksLabel.ForeColor = Color.FromArgb(0, 102, 204);
            accountBooksLabel.Location = new Point(5, 5);
            accountBooksLabel.Name = "accountBooksLabel";
            accountBooksLabel.Size = new Size(340, 20);
            accountBooksLabel.TabIndex = 0;
            accountBooksLabel.Text = "Select Account Book Type";
            accountBooksLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ledgerReportButton
            // 
            ledgerReportButton.FlatStyle = FlatStyle.Flat;
            ledgerReportButton.Font = new Font("Segoe UI", 9F);
            ledgerReportButton.Location = new Point(5, 30);
            ledgerReportButton.Name = "ledgerReportButton";
            ledgerReportButton.Size = new Size(340, 35);
            ledgerReportButton.TabIndex = 1;
            ledgerReportButton.Text = "Ledger Report";
            ledgerReportButton.TextAlign = ContentAlignment.MiddleLeft;
            ledgerReportButton.UseVisualStyleBackColor = true;
            ledgerReportButton.Click += LedgerReportButton_Click;
            // 
            // salesButton
            // 
            salesButton.FlatStyle = FlatStyle.Flat;
            salesButton.Font = new Font("Segoe UI", 9F);
            salesButton.Location = new Point(5, 70);
            salesButton.Name = "salesButton";
            salesButton.Size = new Size(340, 35);
            salesButton.TabIndex = 2;
            salesButton.Text = "Sales";
            salesButton.TextAlign = ContentAlignment.MiddleLeft;
            salesButton.UseVisualStyleBackColor = true;
            salesButton.Click += SalesButton_Click;
            // 
            // purchaseReportsButton
            // 
            purchaseReportsButton.FlatStyle = FlatStyle.Flat;
            purchaseReportsButton.Font = new Font("Segoe UI", 9F);
            purchaseReportsButton.Location = new Point(5, 110);
            purchaseReportsButton.Name = "purchaseReportsButton";
            purchaseReportsButton.Size = new Size(340, 35);
            purchaseReportsButton.TabIndex = 3;
            purchaseReportsButton.Text = "Purchase Reports";
            purchaseReportsButton.TextAlign = ContentAlignment.MiddleLeft;
            purchaseReportsButton.UseVisualStyleBackColor = true;
            purchaseReportsButton.Click += PurchaseReportsButton_Click;
            // 
            // receiptsAndPaymentsButton
            // 
            receiptsAndPaymentsButton.FlatStyle = FlatStyle.Flat;
            receiptsAndPaymentsButton.Font = new Font("Segoe UI", 9F);
            receiptsAndPaymentsButton.Location = new Point(5, 150);
            receiptsAndPaymentsButton.Name = "receiptsAndPaymentsButton";
            receiptsAndPaymentsButton.Size = new Size(340, 35);
            receiptsAndPaymentsButton.TabIndex = 4;
            receiptsAndPaymentsButton.Text = "Receipts and Payments";
            receiptsAndPaymentsButton.TextAlign = ContentAlignment.MiddleLeft;
            receiptsAndPaymentsButton.UseVisualStyleBackColor = true;
            receiptsAndPaymentsButton.Click += ReceiptsAndPaymentsButton_Click;
            // 
            // journalRegisterButton
            // 
            journalRegisterButton.FlatStyle = FlatStyle.Flat;
            journalRegisterButton.Font = new Font("Segoe UI", 9F);
            journalRegisterButton.Location = new Point(5, 190);
            journalRegisterButton.Name = "journalRegisterButton";
            journalRegisterButton.Size = new Size(340, 35);
            journalRegisterButton.TabIndex = 5;
            journalRegisterButton.Text = "Journal Register";
            journalRegisterButton.TextAlign = ContentAlignment.MiddleLeft;
            journalRegisterButton.UseVisualStyleBackColor = true;
            journalRegisterButton.Click += JournalRegisterButton_Click;
            // 
            // dayBookButton
            // 
            dayBookButton.FlatStyle = FlatStyle.Flat;
            dayBookButton.Font = new Font("Segoe UI", 9F);
            dayBookButton.Location = new Point(5, 230);
            dayBookButton.Name = "dayBookButton";
            dayBookButton.Size = new Size(340, 35);
            dayBookButton.TabIndex = 6;
            dayBookButton.Text = "Day Book";
            dayBookButton.TextAlign = ContentAlignment.MiddleLeft;
            dayBookButton.UseVisualStyleBackColor = true;
            dayBookButton.Click += DayBookButton_Click;
            // 
            // listOfAccountsButton
            // 
            listOfAccountsButton.FlatStyle = FlatStyle.Flat;
            listOfAccountsButton.Font = new Font("Segoe UI", 9F);
            listOfAccountsButton.Location = new Point(5, 270);
            listOfAccountsButton.Name = "listOfAccountsButton";
            listOfAccountsButton.Size = new Size(340, 35);
            listOfAccountsButton.TabIndex = 7;
            listOfAccountsButton.Text = "List of Accounts";
            listOfAccountsButton.TextAlign = ContentAlignment.MiddleLeft;
            listOfAccountsButton.UseVisualStyleBackColor = true;
            listOfAccountsButton.Click += ListOfAccountsButton_Click;
            // 
            // outstandingsButton
            // 
            outstandingsButton.FlatStyle = FlatStyle.Flat;
            outstandingsButton.Font = new Font("Segoe UI", 9F);
            outstandingsButton.Location = new Point(5, 310);
            outstandingsButton.Name = "outstandingsButton";
            outstandingsButton.Size = new Size(340, 35);
            outstandingsButton.TabIndex = 8;
            outstandingsButton.Text = "Outstandings";
            outstandingsButton.TextAlign = ContentAlignment.MiddleLeft;
            outstandingsButton.UseVisualStyleBackColor = true;
            outstandingsButton.Click += OutstandingsButton_Click;
            // 
            // closeButton
            // 
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 10F);
            closeButton.Location = new Point(5, 350);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(340, 35);
            closeButton.TabIndex = 9;
            closeButton.Text = "Close";
            closeButton.TextAlign = ContentAlignment.MiddleLeft;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // AccountBooksForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(350, 400);
            Controls.Add(accountBooksGroupBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 30);
            Margin = new Padding(3, 2, 3, 2);
            Name = "AccountBooksForm";
            ShowInTaskbar = false;
            Text = "Account Books";
            accountBooksGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Setup button styling
            SetupButtonStyling();
            
            // Initialize navigation order
            InitializeNavigationOrder();
            
            // Enable key preview for form-level key handling
            this.KeyPreview = true;
            
            // Add form-level key handling
            this.KeyDown += AccountBooksForm_KeyDown;
            
            // Set focus to first button when form loads
            this.Load += (s, e) =>
            {
                if (_allButtons.Length > 0)
                {
                    _lastFocusedButton = _allButtons[0];
                    HighlightButton(_lastFocusedButton);
                    _lastFocusedButton.Focus();
                }
            };
        }

        private void SetupButtonStyling()
        {
            var buttons = new[] { 
                ledgerReportButton, salesButton, purchaseReportsButton, receiptsAndPaymentsButton,
                journalRegisterButton, dayBookButton, listOfAccountsButton, outstandingsButton, closeButton 
            };

            foreach (Button btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 245, 255);
                btn.Cursor = Cursors.Hand;

                // Add hover effects
                btn.MouseEnter += Button_MouseEnter;
                btn.MouseLeave += Button_MouseLeave;
                btn.GotFocus += Button_GotFocus;
                btn.Click += Button_Click; // Add click event for highlighting
            }
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.FromArgb(230, 245, 255);
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            }
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void Button_GotFocus(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                HighlightButton(btn);
                _lastFocusedButton = btn; // Update the last focused button
            }
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                HighlightButton(btn);
                btn.Focus(); // Ensure focus stays on the clicked button
                _lastFocusedButton = btn; // Update the last focused button
            }
        }

        private void HighlightButton(Button button)
        {
            // Clear previous highlights
            ClearAllButtonHighlights();

            // Highlight the selected button
            button.BackColor = Color.FromArgb(0, 102, 204);
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 70, 140);
            button.FlatAppearance.BorderSize = 2;
            button.Font = new Font(button.Font.FontFamily, button.Font.Size, FontStyle.Bold);
        }

        private void ClearAllButtonHighlights()
        {
            var buttons = new[] { 
                ledgerReportButton, salesButton, purchaseReportsButton, receiptsAndPaymentsButton,
                journalRegisterButton, dayBookButton, listOfAccountsButton, outstandingsButton, closeButton 
            };

            foreach (Button btn in buttons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.BorderSize = 1;
                btn.Font = new Font(btn.Font.FontFamily, btn.Font.Size, FontStyle.Regular);
            }
        }

        // Navigation state
        private Button? _lastFocusedButton = null;

        // Button navigation order for arrow key navigation
        private Button[] _allButtons = null!;

        private void InitializeNavigationOrder()
        {
            // Define navigation order for all buttons (top to bottom)
            _allButtons = new Button[]
            {
                ledgerReportButton,
                salesButton,
                purchaseReportsButton,
                receiptsAndPaymentsButton,
                journalRegisterButton,
                dayBookButton,
                listOfAccountsButton,
                outstandingsButton,
                closeButton
            };
        }

        private bool NavigateToPreviousButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            var currentIndex = Array.IndexOf(_allButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to previous button in the list, wrap around to last if at first
            var previousIndex = currentIndex == 0 ? _allButtons.Length - 1 : currentIndex - 1;
            var previousButton = _allButtons[previousIndex];

            HighlightButton(previousButton);
            previousButton.Focus();
            _lastFocusedButton = previousButton;

            return true;
        }

        private bool NavigateToNextButton()
        {
            var currentFocused = this.ActiveControl as Button;
            if (currentFocused == null) return false;

            var currentIndex = Array.IndexOf(_allButtons, currentFocused);
            if (currentIndex == -1) return false;

            // Move to next button in the list, wrap around to first if at last
            var nextIndex = currentIndex == _allButtons.Length - 1 ? 0 : currentIndex + 1;
            var nextButton = _allButtons[nextIndex];

            HighlightButton(nextButton);
            nextButton.Focus();
            _lastFocusedButton = nextButton;

            return true;
        }

        private void AccountBooksForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up: // Up arrow to navigate to previous button
                    if (NavigateToPreviousButton())
                    {
                        e.Handled = true;
                    }
                    break;

                case Keys.Down: // Down arrow to navigate to next button
                    if (NavigateToNextButton())
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        // Account Books Event Handlers
        private void SalesButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Sales Account Book report will be implemented here.\n\nThis report shows all sales transactions and related account entries.", 
                "Sales Account Book", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PurchaseReportsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Purchase Reports will be implemented here.\n\nThis section contains various purchase-related account book reports.", 
                "Purchase Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LedgerReportButton_Click(object? sender, EventArgs e)
        {
            try
            {
                // Get the main MDI form and call its method to open LedgerReportForm
                if (this.MdiParent is MainMDIForm mainMdiForm)
                {
                    mainMdiForm.OpenLedgerReportFormFromChild();
                }
                else
                {
                    MessageBox.Show("Unable to access main form. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Ledger Report: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiptsAndPaymentsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Receipts and Payments Account Book will be implemented here.\n\nThis report shows all cash receipts and payments transactions.", 
                "Receipts and Payments", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void JournalRegisterButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Journal Register will be implemented here.\n\nThis report shows all journal entries in chronological order.", 
                "Journal Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DayBookButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Day Book will be implemented here.\n\nThis report shows all transactions for a specific day or date range.", 
                "Day Book", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ListOfAccountsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("List of Accounts will be implemented here.\n\nThis report provides a comprehensive list of all accounts with their balances.", 
                "List of Accounts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OutstandingsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Outstandings Report will be implemented here.\n\nThis report shows outstanding receivables and payables.", 
                "Outstandings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void HandleEscapeKey()
        {
            this.Close();
        }

        protected override bool HandleEnterKey()
        {
            // Handle Enter key for button activation
            var focusedControl = this.ActiveControl as Button;
            if (focusedControl != null)
            {
                focusedControl.PerformClick();
                return true; // Indicate that Enter key was handled
            }
            return false; // Let BaseForm handle default Enter behavior
        }
    }
}
