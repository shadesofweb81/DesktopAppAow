using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class TrialBalanceNavForm : BaseForm
    {
        // Main group box
        private GroupBox trialBalanceGroupBox = null!;
        private Label trialBalanceLabel = null!;
        
        // All buttons in single group
        private Button trialBalanceAllAccountsButton = null!;
        private Button trialBalanceGroupedButton = null!;
        private Button trialBalanceHierarchicalButton = null!;
        private Button closeButton = null!;

        public TrialBalanceNavForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            trialBalanceGroupBox = new GroupBox();
            trialBalanceLabel = new Label();
            trialBalanceAllAccountsButton = new Button();
            trialBalanceGroupedButton = new Button();
            trialBalanceHierarchicalButton = new Button();
            closeButton = new Button();
            trialBalanceGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // trialBalanceGroupBox
            // 
            trialBalanceGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            trialBalanceGroupBox.Controls.Add(trialBalanceLabel);
            trialBalanceGroupBox.Controls.Add(trialBalanceAllAccountsButton);
            trialBalanceGroupBox.Controls.Add(trialBalanceGroupedButton);
            trialBalanceGroupBox.Controls.Add(trialBalanceHierarchicalButton);
            trialBalanceGroupBox.Controls.Add(closeButton);
            trialBalanceGroupBox.FlatStyle = FlatStyle.Flat;
            trialBalanceGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            trialBalanceGroupBox.Location = new Point(0, 0);
            trialBalanceGroupBox.Name = "trialBalanceGroupBox";
            trialBalanceGroupBox.Size = new Size(280, 200);
            trialBalanceGroupBox.TabIndex = 0;
            trialBalanceGroupBox.TabStop = false;
            trialBalanceGroupBox.Text = "TRIAL BALANCE REPORTS";
            // 
            // trialBalanceLabel
            // 
            trialBalanceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            trialBalanceLabel.ForeColor = Color.FromArgb(0, 102, 204);
            trialBalanceLabel.Location = new Point(5, 25);
            trialBalanceLabel.Name = "trialBalanceLabel";
            trialBalanceLabel.Size = new Size(270, 20);
            trialBalanceLabel.TabIndex = 0;
            trialBalanceLabel.Text = "Select Trial Balance Report Type";
            trialBalanceLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // trialBalanceAllAccountsButton
            // 
            trialBalanceAllAccountsButton.FlatStyle = FlatStyle.Flat;
            trialBalanceAllAccountsButton.Font = new Font("Segoe UI", 9F);
            trialBalanceAllAccountsButton.Location = new Point(5, 60);
            trialBalanceAllAccountsButton.Name = "trialBalanceAllAccountsButton";
            trialBalanceAllAccountsButton.Size = new Size(270, 25);
            trialBalanceAllAccountsButton.TabIndex = 1;
            trialBalanceAllAccountsButton.Text = "Trial Balance - All Accounts";
            trialBalanceAllAccountsButton.TextAlign = ContentAlignment.MiddleLeft;
            trialBalanceAllAccountsButton.UseVisualStyleBackColor = true;
            trialBalanceAllAccountsButton.Click += TrialBalanceAllAccountsButton_Click;
            // 
            // trialBalanceGroupedButton
            // 
            trialBalanceGroupedButton.FlatStyle = FlatStyle.Flat;
            trialBalanceGroupedButton.Font = new Font("Segoe UI", 9F);
            trialBalanceGroupedButton.Location = new Point(5, 90);
            trialBalanceGroupedButton.Name = "trialBalanceGroupedButton";
            trialBalanceGroupedButton.Size = new Size(270, 25);
            trialBalanceGroupedButton.TabIndex = 2;
            trialBalanceGroupedButton.Text = "Trial Balance - Grouped";
            trialBalanceGroupedButton.TextAlign = ContentAlignment.MiddleLeft;
            trialBalanceGroupedButton.UseVisualStyleBackColor = true;
            trialBalanceGroupedButton.Click += TrialBalanceGroupedButton_Click;
            // 
            // trialBalanceHierarchicalButton
            // 
            trialBalanceHierarchicalButton.FlatStyle = FlatStyle.Flat;
            trialBalanceHierarchicalButton.Font = new Font("Segoe UI", 9F);
            trialBalanceHierarchicalButton.Location = new Point(5, 120);
            trialBalanceHierarchicalButton.Name = "trialBalanceHierarchicalButton";
            trialBalanceHierarchicalButton.Size = new Size(270, 25);
            trialBalanceHierarchicalButton.TabIndex = 3;
            trialBalanceHierarchicalButton.Text = "Trial Balance - Hierarchical";
            trialBalanceHierarchicalButton.TextAlign = ContentAlignment.MiddleLeft;
            trialBalanceHierarchicalButton.UseVisualStyleBackColor = true;
            trialBalanceHierarchicalButton.Click += TrialBalanceHierarchicalButton_Click;
            // 
            // closeButton
            // 
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 10F);
            closeButton.Location = new Point(5, 160);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(270, 25);
            closeButton.TabIndex = 4;
            closeButton.Text = "Close";
            closeButton.TextAlign = ContentAlignment.MiddleLeft;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // TrialBalanceNavForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(280, 200);
            Controls.Add(trialBalanceGroupBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 30);
            Margin = new Padding(3, 2, 3, 2);
            Name = "TrialBalanceNavForm";
            ShowInTaskbar = false;
            Text = "Trial Balance Reports";
            trialBalanceGroupBox.ResumeLayout(false);
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
            this.KeyDown += TrialBalanceNavForm_KeyDown;
            
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
                trialBalanceAllAccountsButton, trialBalanceGroupedButton,
                trialBalanceHierarchicalButton, closeButton 
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
                trialBalanceAllAccountsButton, trialBalanceGroupedButton,
                trialBalanceHierarchicalButton, closeButton 
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
                trialBalanceAllAccountsButton,
                trialBalanceGroupedButton,
                trialBalanceHierarchicalButton,
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

        private void TrialBalanceNavForm_KeyDown(object? sender, KeyEventArgs e)
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

        // Trial Balance Event Handlers
        private void TrialBalanceAllAccountsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Trial Balance - All Accounts report will be implemented here.\n\nThis report shows all accounts with their debit and credit balances in a flat list format.", 
                "Trial Balance - All Accounts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TrialBalanceGroupedButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Trial Balance - Grouped report will be implemented here.\n\nThis report shows accounts grouped by their account types (Assets, Liabilities, Equity, Income, Expenses).", 
                "Trial Balance - Grouped", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TrialBalanceHierarchicalButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Trial Balance - Hierarchical report will be implemented here.\n\nThis report shows accounts in a hierarchical tree structure with parent-child relationships.", 
                "Trial Balance - Hierarchical", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
