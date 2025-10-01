using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class GstReportForm : BaseForm
    {
        // Main group box
        private GroupBox gstReportGroupBox = null!;
        private Label gstReportLabel = null!;
        
        // All buttons in single group
        private Button gstr1Button = null!;
        private Button gstr2Button = null!;
        private Button gstr3bButton = null!;
        private Button gstByItemButton = null!;
        private Button gstByItemGroupButton = null!;
        private Button gstByPartyButton = null!;
        private Button gstByItemPartyButton = null!;
        private Button gstSummaryButton = null!;
        private Button closeButton = null!;

        public GstReportForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            gstReportGroupBox = new GroupBox();
            gstReportLabel = new Label();
            gstr1Button = new Button();
            gstr2Button = new Button();
            gstr3bButton = new Button();
            gstByItemButton = new Button();
            gstByItemGroupButton = new Button();
            gstByPartyButton = new Button();
            gstByItemPartyButton = new Button();
            gstSummaryButton = new Button();
            closeButton = new Button();
            gstReportGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // gstReportGroupBox
            // 
            gstReportGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            gstReportGroupBox.Controls.Add(gstReportLabel);
            gstReportGroupBox.Controls.Add(gstr1Button);
            gstReportGroupBox.Controls.Add(gstr2Button);
            gstReportGroupBox.Controls.Add(gstr3bButton);
            gstReportGroupBox.Controls.Add(gstByItemButton);
            gstReportGroupBox.Controls.Add(gstByItemGroupButton);
            gstReportGroupBox.Controls.Add(gstByPartyButton);
            gstReportGroupBox.Controls.Add(gstByItemPartyButton);
            gstReportGroupBox.Controls.Add(gstSummaryButton);
            gstReportGroupBox.Controls.Add(closeButton);
            gstReportGroupBox.FlatStyle = FlatStyle.Flat;
            gstReportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gstReportGroupBox.Location = new Point(0, 0);
            gstReportGroupBox.Name = "gstReportGroupBox";
            gstReportGroupBox.Size = new Size(200, 300);
            gstReportGroupBox.TabIndex = 0;
            gstReportGroupBox.TabStop = false;
            gstReportGroupBox.Text = "GST REPORTS";
            // 
            // gstReportLabel
            // 
            gstReportLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            gstReportLabel.ForeColor = Color.FromArgb(0, 102, 204);
            gstReportLabel.Location = new Point(5, 5);
            gstReportLabel.Name = "gstReportLabel";
            gstReportLabel.Size = new Size(190, 20);
            gstReportLabel.TabIndex = 0;
            gstReportLabel.Text = "Select GST Report Type";
            gstReportLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gstr1Button
            // 
            gstr1Button.FlatStyle = FlatStyle.Flat;
            gstr1Button.Font = new Font("Segoe UI", 9F);
            gstr1Button.Location = new Point(5, 30);
            gstr1Button.Name = "gstr1Button";
            gstr1Button.Size = new Size(190, 25);
            gstr1Button.TabIndex = 1;
            gstr1Button.Text = "GSTR-1";
            gstr1Button.TextAlign = ContentAlignment.MiddleLeft;
            gstr1Button.UseVisualStyleBackColor = true;
            gstr1Button.Click += Gstr1Button_Click;
            // 
            // gstr2Button
            // 
            gstr2Button.FlatStyle = FlatStyle.Flat;
            gstr2Button.Font = new Font("Segoe UI", 9F);
            gstr2Button.Location = new Point(5, 60);
            gstr2Button.Name = "gstr2Button";
            gstr2Button.Size = new Size(190, 25);
            gstr2Button.TabIndex = 2;
            gstr2Button.Text = "GSTR-2";
            gstr2Button.TextAlign = ContentAlignment.MiddleLeft;
            gstr2Button.UseVisualStyleBackColor = true;
            gstr2Button.Click += Gstr2Button_Click;
            // 
            // gstr3bButton
            // 
            gstr3bButton.FlatStyle = FlatStyle.Flat;
            gstr3bButton.Font = new Font("Segoe UI", 9F);
            gstr3bButton.Location = new Point(5, 90);
            gstr3bButton.Name = "gstr3bButton";
            gstr3bButton.Size = new Size(190, 25);
            gstr3bButton.TabIndex = 3;
            gstr3bButton.Text = "GSTR-3B";
            gstr3bButton.TextAlign = ContentAlignment.MiddleLeft;
            gstr3bButton.UseVisualStyleBackColor = true;
            gstr3bButton.Click += Gstr3bButton_Click;
            // 
            // gstByItemButton
            // 
            gstByItemButton.FlatStyle = FlatStyle.Flat;
            gstByItemButton.Font = new Font("Segoe UI", 9F);
            gstByItemButton.Location = new Point(5, 120);
            gstByItemButton.Name = "gstByItemButton";
            gstByItemButton.Size = new Size(190, 25);
            gstByItemButton.TabIndex = 4;
            gstByItemButton.Text = "GST by Item";
            gstByItemButton.TextAlign = ContentAlignment.MiddleLeft;
            gstByItemButton.UseVisualStyleBackColor = true;
            gstByItemButton.Click += GstByItemButton_Click;
            // 
            // gstByItemGroupButton
            // 
            gstByItemGroupButton.FlatStyle = FlatStyle.Flat;
            gstByItemGroupButton.Font = new Font("Segoe UI", 9F);
            gstByItemGroupButton.Location = new Point(5, 150);
            gstByItemGroupButton.Name = "gstByItemGroupButton";
            gstByItemGroupButton.Size = new Size(190, 25);
            gstByItemGroupButton.TabIndex = 5;
            gstByItemGroupButton.Text = "GST by Item Group";
            gstByItemGroupButton.TextAlign = ContentAlignment.MiddleLeft;
            gstByItemGroupButton.UseVisualStyleBackColor = true;
            gstByItemGroupButton.Click += GstByItemGroupButton_Click;
            // 
            // gstByPartyButton
            // 
            gstByPartyButton.FlatStyle = FlatStyle.Flat;
            gstByPartyButton.Font = new Font("Segoe UI", 9F);
            gstByPartyButton.Location = new Point(5, 180);
            gstByPartyButton.Name = "gstByPartyButton";
            gstByPartyButton.Size = new Size(190, 25);
            gstByPartyButton.TabIndex = 6;
            gstByPartyButton.Text = "GST by Party";
            gstByPartyButton.TextAlign = ContentAlignment.MiddleLeft;
            gstByPartyButton.UseVisualStyleBackColor = true;
            gstByPartyButton.Click += GstByPartyButton_Click;
            // 
            // gstByItemPartyButton
            // 
            gstByItemPartyButton.FlatStyle = FlatStyle.Flat;
            gstByItemPartyButton.Font = new Font("Segoe UI", 9F);
            gstByItemPartyButton.Location = new Point(5, 210);
            gstByItemPartyButton.Name = "gstByItemPartyButton";
            gstByItemPartyButton.Size = new Size(190, 25);
            gstByItemPartyButton.TabIndex = 7;
            gstByItemPartyButton.Text = "GST by Item Party";
            gstByItemPartyButton.TextAlign = ContentAlignment.MiddleLeft;
            gstByItemPartyButton.UseVisualStyleBackColor = true;
            gstByItemPartyButton.Click += GstByItemPartyButton_Click;
            // 
            // gstSummaryButton
            // 
            gstSummaryButton.FlatStyle = FlatStyle.Flat;
            gstSummaryButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gstSummaryButton.Location = new Point(5, 240);
            gstSummaryButton.Name = "gstSummaryButton";
            gstSummaryButton.Size = new Size(190, 25);
            gstSummaryButton.TabIndex = 8;
            gstSummaryButton.Text = "GST Summary";
            gstSummaryButton.TextAlign = ContentAlignment.MiddleLeft;
            gstSummaryButton.UseVisualStyleBackColor = true;
            gstSummaryButton.Click += GstSummaryButton_Click;
            // 
            // closeButton
            // 
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 10F);
            closeButton.Location = new Point(5, 270);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(190, 25);
            closeButton.TabIndex = 9;
            closeButton.Text = "Close";
            closeButton.TextAlign = ContentAlignment.MiddleLeft;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // GstReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(200, 300);
            Controls.Add(gstReportGroupBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 30);
            Margin = new Padding(3, 2, 3, 2);
            Name = "GstReportForm";
            ShowInTaskbar = false;
            Text = "GST Reports";
            gstReportGroupBox.ResumeLayout(false);
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
            this.KeyDown += GstReportForm_KeyDown;
            
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
                gstr1Button, gstr2Button, gstr3bButton,
                gstByItemButton, gstByItemGroupButton,
                gstByPartyButton, gstByItemPartyButton,
                gstSummaryButton, closeButton 
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
                gstr1Button, gstr2Button, gstr3bButton,
                gstByItemButton, gstByItemGroupButton,
                gstByPartyButton, gstByItemPartyButton,
                gstSummaryButton, closeButton 
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
                gstr1Button,
                gstr2Button,
                gstr3bButton,
                gstByItemButton,
                gstByItemGroupButton,
                gstByPartyButton,
                gstByItemPartyButton,
                gstSummaryButton,
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

        private void GstReportForm_KeyDown(object? sender, KeyEventArgs e)
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

        // GST Returns Event Handlers
        private void Gstr1Button_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GSTR-1 (Outward Supplies) report will be implemented here.\n\nThis report contains details of all outward supplies made during the tax period.", 
                "GSTR-1 Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Gstr2Button_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GSTR-2 (Inward Supplies) report will be implemented here.\n\nThis report contains details of all inward supplies received during the tax period.", 
                "GSTR-2 Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Gstr3bButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GSTR-3B (Monthly Return) report will be implemented here.\n\nThis is a monthly summary return containing details of outward supplies, inward supplies, and tax liability.", 
                "GSTR-3B Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // GST Item Wise Event Handlers
        private void GstByItemButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST by Item report will be implemented here.\n\nThis report shows GST details for individual items.", 
                "GST by Item Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GstByItemGroupButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST by Item Group report will be implemented here.\n\nThis report shows GST details grouped by item categories.", 
                "GST by Item Group Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // GST by Party Event Handlers
        private void GstByPartyButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST by Party report will be implemented here.\n\nThis report shows GST details for individual parties/customers.", 
                "GST by Party Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GstByItemPartyButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST by Item Party report will be implemented here.\n\nThis report shows GST details for items by party combination.", 
                "GST by Item Party Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // GST Summary Event Handler
        private void GstSummaryButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST Summary Report will be implemented here.\n\nThis report provides a comprehensive summary of all GST transactions for the selected period.", 
                "GST Summary Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
