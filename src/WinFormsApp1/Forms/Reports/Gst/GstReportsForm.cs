using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class GstReportsForm : Form
    {
        private GroupBox gstReportsGroupBox = null!;
        private Label gstReportsLabel = null!;
        private Button gstr1Button = null!;
        private Button gstr2Button = null!;
        private Button gstr3bButton = null!;
        private Button gstSummaryButton = null!;
        private Button closeButton = null!;

        public GstReportsForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            gstReportsGroupBox = new GroupBox();
            gstReportsLabel = new Label();
            gstr1Button = new Button();
            gstr2Button = new Button();
            gstr3bButton = new Button();
            gstSummaryButton = new Button();
            closeButton = new Button();
            gstReportsGroupBox.SuspendLayout();
            SuspendLayout();
            
            // 
            // gstReportsGroupBox
            // 
            gstReportsGroupBox.Controls.Add(gstReportsLabel);
            gstReportsGroupBox.Controls.Add(gstr1Button);
            gstReportsGroupBox.Controls.Add(gstr2Button);
            gstReportsGroupBox.Controls.Add(gstr3bButton);
            gstReportsGroupBox.Controls.Add(gstSummaryButton);
            gstReportsGroupBox.Controls.Add(closeButton);
            gstReportsGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gstReportsGroupBox.Location = new Point(50, 50);
            gstReportsGroupBox.Margin = new Padding(3, 4, 3, 4);
            gstReportsGroupBox.Name = "gstReportsGroupBox";
            gstReportsGroupBox.Padding = new Padding(3, 4, 3, 4);
            gstReportsGroupBox.Size = new Size(500, 400);
            gstReportsGroupBox.TabIndex = 0;
            gstReportsGroupBox.TabStop = false;
            gstReportsGroupBox.Text = "GST REPORTS";
            
            // 
            // gstReportsLabel
            // 
            gstReportsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            gstReportsLabel.ForeColor = Color.FromArgb(0, 102, 204);
            gstReportsLabel.Location = new Point(20, 30);
            gstReportsLabel.Name = "gstReportsLabel";
            gstReportsLabel.Size = new Size(460, 30);
            gstReportsLabel.TabIndex = 0;
            gstReportsLabel.Text = "Select GST Report Type";
            gstReportsLabel.TextAlign = ContentAlignment.MiddleCenter;
            
            // 
            // gstr1Button
            // 
            gstr1Button.Font = new Font("Segoe UI", 10F);
            gstr1Button.Location = new Point(50, 80);
            gstr1Button.Margin = new Padding(3, 4, 3, 4);
            gstr1Button.Name = "gstr1Button";
            gstr1Button.Size = new Size(400, 50);
            gstr1Button.TabIndex = 1;
            gstr1Button.Text = "GSTR-1 (Outward Supplies)";
            gstr1Button.UseVisualStyleBackColor = true;
            gstr1Button.Click += Gstr1Button_Click;
            
            // 
            // gstr2Button
            // 
            gstr2Button.Font = new Font("Segoe UI", 10F);
            gstr2Button.Location = new Point(50, 140);
            gstr2Button.Margin = new Padding(3, 4, 3, 4);
            gstr2Button.Name = "gstr2Button";
            gstr2Button.Size = new Size(400, 50);
            gstr2Button.TabIndex = 2;
            gstr2Button.Text = "GSTR-2 (Inward Supplies)";
            gstr2Button.UseVisualStyleBackColor = true;
            gstr2Button.Click += Gstr2Button_Click;
            
            // 
            // gstr3bButton
            // 
            gstr3bButton.Font = new Font("Segoe UI", 10F);
            gstr3bButton.Location = new Point(50, 200);
            gstr3bButton.Margin = new Padding(3, 4, 3, 4);
            gstr3bButton.Name = "gstr3bButton";
            gstr3bButton.Size = new Size(400, 50);
            gstr3bButton.TabIndex = 3;
            gstr3bButton.Text = "GSTR-3B (Monthly Return)";
            gstr3bButton.UseVisualStyleBackColor = true;
            gstr3bButton.Click += Gstr3bButton_Click;
            
            // 
            // gstSummaryButton
            // 
            gstSummaryButton.Font = new Font("Segoe UI", 10F);
            gstSummaryButton.Location = new Point(50, 260);
            gstSummaryButton.Margin = new Padding(3, 4, 3, 4);
            gstSummaryButton.Name = "gstSummaryButton";
            gstSummaryButton.Size = new Size(400, 50);
            gstSummaryButton.TabIndex = 4;
            gstSummaryButton.Text = "GST Summary Report";
            gstSummaryButton.UseVisualStyleBackColor = true;
            gstSummaryButton.Click += GstSummaryButton_Click;
            
            // 
            // closeButton
            // 
            closeButton.Font = new Font("Segoe UI", 10F);
            closeButton.Location = new Point(200, 330);
            closeButton.Margin = new Padding(3, 4, 3, 4);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(100, 40);
            closeButton.TabIndex = 5;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            
            // 
            // GstReportsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 500);
            Controls.Add(gstReportsGroupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GstReportsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "GST Reports";
            KeyDown += GstReportsForm_KeyDown;
            gstReportsGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Setup button styling
            SetupButtonStyling();
        }

        private void SetupButtonStyling()
        {
            var buttons = new[] { gstr1Button, gstr2Button, gstr3bButton, gstSummaryButton, closeButton };

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
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private void ClearAllButtonHighlights()
        {
            var buttons = new[] { gstr1Button, gstr2Button, gstr3bButton, gstSummaryButton, closeButton };

            foreach (Button btn in buttons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.BorderSize = 1;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }
        }

        private void GstReportsForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    if (gstr1Button.Focused)
                        Gstr1Button_Click(null, EventArgs.Empty);
                    else if (gstr2Button.Focused)
                        Gstr2Button_Click(null, EventArgs.Empty);
                    else if (gstr3bButton.Focused)
                        Gstr3bButton_Click(null, EventArgs.Empty);
                    else if (gstSummaryButton.Focused)
                        GstSummaryButton_Click(null, EventArgs.Empty);
                    else if (closeButton.Focused)
                        CloseButton_Click(null, EventArgs.Empty);
                    e.Handled = true;
                    break;
                case Keys.Tab:
                    // Allow default tab navigation
                    break;
            }
        }

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

        private void GstSummaryButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("GST Summary Report will be implemented here.\n\nThis report provides a comprehensive summary of all GST transactions for the selected period.", 
                "GST Summary Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

