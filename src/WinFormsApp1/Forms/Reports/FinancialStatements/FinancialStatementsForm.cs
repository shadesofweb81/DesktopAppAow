using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class FinancialStatementsForm : BaseForm
    {
        // Main group box
        private GroupBox financialStatementsGroupBox = null!;
        private Label financialStatementsLabel = null!;
        
        // All buttons in single group
        private Button balanceSheetButton = null!;
        private Button incomeStatementButton = null!;
        private Button cashFlowStatementButton = null!;
        private Button shareholdersEquityButton = null!;
        private Button closeButton = null!;

        public FinancialStatementsForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            financialStatementsGroupBox = new GroupBox();
            financialStatementsLabel = new Label();
            balanceSheetButton = new Button();
            incomeStatementButton = new Button();
            cashFlowStatementButton = new Button();
            shareholdersEquityButton = new Button();
            closeButton = new Button();
            financialStatementsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // financialStatementsGroupBox
            // 
            financialStatementsGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            financialStatementsGroupBox.Controls.Add(financialStatementsLabel);
            financialStatementsGroupBox.Controls.Add(balanceSheetButton);
            financialStatementsGroupBox.Controls.Add(incomeStatementButton);
            financialStatementsGroupBox.Controls.Add(cashFlowStatementButton);
            financialStatementsGroupBox.Controls.Add(shareholdersEquityButton);
            financialStatementsGroupBox.Controls.Add(closeButton);
            financialStatementsGroupBox.FlatStyle = FlatStyle.Flat;
            financialStatementsGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            financialStatementsGroupBox.Location = new Point(0, 0);
            financialStatementsGroupBox.Name = "financialStatementsGroupBox";
            financialStatementsGroupBox.Size = new Size(300, 250);
            financialStatementsGroupBox.TabIndex = 0;
            financialStatementsGroupBox.TabStop = false;
            financialStatementsGroupBox.Text = "FINANCIAL STATEMENTS";
            // 
            // financialStatementsLabel
            // 
            financialStatementsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            financialStatementsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            financialStatementsLabel.Location = new Point(5, 5);
            financialStatementsLabel.Name = "financialStatementsLabel";
            financialStatementsLabel.Size = new Size(290, 20);
            financialStatementsLabel.TabIndex = 0;
            financialStatementsLabel.Text = "Select Financial Statement Type";
            financialStatementsLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // balanceSheetButton
            // 
            balanceSheetButton.FlatStyle = FlatStyle.Flat;
            balanceSheetButton.Font = new Font("Segoe UI", 9F);
            balanceSheetButton.Location = new Point(5, 30);
            balanceSheetButton.Name = "balanceSheetButton";
            balanceSheetButton.Size = new Size(290, 35);
            balanceSheetButton.TabIndex = 1;
            balanceSheetButton.Text = "Balance Sheet";
            balanceSheetButton.TextAlign = ContentAlignment.MiddleLeft;
            balanceSheetButton.UseVisualStyleBackColor = true;
            balanceSheetButton.Click += BalanceSheetButton_Click;
            // 
            // incomeStatementButton
            // 
            incomeStatementButton.FlatStyle = FlatStyle.Flat;
            incomeStatementButton.Font = new Font("Segoe UI", 9F);
            incomeStatementButton.Location = new Point(5, 70);
            incomeStatementButton.Name = "incomeStatementButton";
            incomeStatementButton.Size = new Size(290, 35);
            incomeStatementButton.TabIndex = 2;
            incomeStatementButton.Text = "Income Statement (Profit & Loss)";
            incomeStatementButton.TextAlign = ContentAlignment.MiddleLeft;
            incomeStatementButton.UseVisualStyleBackColor = true;
            incomeStatementButton.Click += IncomeStatementButton_Click;
            // 
            // cashFlowStatementButton
            // 
            cashFlowStatementButton.FlatStyle = FlatStyle.Flat;
            cashFlowStatementButton.Font = new Font("Segoe UI", 9F);
            cashFlowStatementButton.Location = new Point(5, 110);
            cashFlowStatementButton.Name = "cashFlowStatementButton";
            cashFlowStatementButton.Size = new Size(290, 35);
            cashFlowStatementButton.TabIndex = 3;
            cashFlowStatementButton.Text = "Cash Flow Statement";
            cashFlowStatementButton.TextAlign = ContentAlignment.MiddleLeft;
            cashFlowStatementButton.UseVisualStyleBackColor = true;
            cashFlowStatementButton.Click += CashFlowStatementButton_Click;
            // 
            // shareholdersEquityButton
            // 
            shareholdersEquityButton.FlatStyle = FlatStyle.Flat;
            shareholdersEquityButton.Font = new Font("Segoe UI", 9F);
            shareholdersEquityButton.Location = new Point(5, 150);
            shareholdersEquityButton.Name = "shareholdersEquityButton";
            shareholdersEquityButton.Size = new Size(290, 35);
            shareholdersEquityButton.TabIndex = 4;
            shareholdersEquityButton.Text = "Statement of Shareholders' Equity";
            shareholdersEquityButton.TextAlign = ContentAlignment.MiddleLeft;
            shareholdersEquityButton.UseVisualStyleBackColor = true;
            shareholdersEquityButton.Click += ShareholdersEquityButton_Click;
            // 
            // closeButton
            // 
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 10F);
            closeButton.Location = new Point(5, 190);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(290, 35);
            closeButton.TabIndex = 5;
            closeButton.Text = "Close";
            closeButton.TextAlign = ContentAlignment.MiddleLeft;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // FinancialStatementsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(300, 250);
            Controls.Add(financialStatementsGroupBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 30);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FinancialStatementsForm";
            ShowInTaskbar = false;
            Text = "Financial Statements";
            financialStatementsGroupBox.ResumeLayout(false);
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
            this.KeyDown += FinancialStatementsForm_KeyDown;
            
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
                balanceSheetButton, incomeStatementButton, cashFlowStatementButton,
                shareholdersEquityButton, closeButton 
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
                balanceSheetButton, incomeStatementButton, cashFlowStatementButton,
                shareholdersEquityButton, closeButton 
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
                balanceSheetButton,
                incomeStatementButton,
                cashFlowStatementButton,
                shareholdersEquityButton,
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

        private void FinancialStatementsForm_KeyDown(object? sender, KeyEventArgs e)
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

        // Financial Statement Event Handlers
        private void BalanceSheetButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Balance Sheet report will be implemented here.\n\nThis statement shows the company's assets, liabilities, and shareholders' equity at a specific point in time.", 
                "Balance Sheet", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void IncomeStatementButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Income Statement (Profit & Loss) report will be implemented here.\n\nThis statement shows the company's revenues, expenses, and net income over a specific period.", 
                "Income Statement", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CashFlowStatementButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Cash Flow Statement report will be implemented here.\n\nThis statement shows the company's cash inflows and outflows from operating, investing, and financing activities.", 
                "Cash Flow Statement", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShareholdersEquityButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Statement of Shareholders' Equity report will be implemented here.\n\nThis statement shows changes in shareholders' equity over a specific period.", 
                "Statement of Shareholders' Equity", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

