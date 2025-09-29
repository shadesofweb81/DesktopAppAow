using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1.Forms.Reports
{
    public partial class TaxReportForm : BaseForm
    {
        private GroupBox taxReportGroupBox = null!;
        private Button gstButton = null!;
        private Button incomeTaxButton = null!;
        private Button othersTaxesButton = null!;
        private Button closeButton = null!;

        public TaxReportForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            taxReportGroupBox = new GroupBox();
            gstButton = new Button();
            incomeTaxButton = new Button();
            othersTaxesButton = new Button();
            closeButton = new Button();
            taxReportGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // taxReportGroupBox
            // 
            taxReportGroupBox.BackColor = Color.FromArgb(240, 240, 240);
            taxReportGroupBox.Controls.Add(gstButton);
            taxReportGroupBox.Controls.Add(incomeTaxButton);
            taxReportGroupBox.Controls.Add(othersTaxesButton);
            taxReportGroupBox.Controls.Add(closeButton);
            taxReportGroupBox.FlatStyle = FlatStyle.Flat;
            taxReportGroupBox.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            taxReportGroupBox.Location = new Point(12, 12);
            taxReportGroupBox.Name = "taxReportGroupBox";
            taxReportGroupBox.Size = new Size(200, 230);
            taxReportGroupBox.TabIndex = 0;
            taxReportGroupBox.TabStop = false;
            taxReportGroupBox.Text = "TAX REPORTS";
            // 
            // gstButton
            // 
            gstButton.FlatStyle = FlatStyle.Flat;
            gstButton.Font = new Font("Segoe UI", 8F);
            gstButton.Location = new Point(5, 25);
            gstButton.Name = "gstButton";
            gstButton.Size = new Size(190, 25);
            gstButton.TabIndex = 1;
            gstButton.Text = "GST Reports";
            gstButton.TextAlign = ContentAlignment.MiddleLeft;
            gstButton.UseVisualStyleBackColor = true;
            gstButton.Click += GstButton_Click;
            // 
            // incomeTaxButton
            // 
            incomeTaxButton.FlatStyle = FlatStyle.Flat;
            incomeTaxButton.Font = new Font("Segoe UI", 8F);
            incomeTaxButton.Location = new Point(5, 55);
            incomeTaxButton.Name = "incomeTaxButton";
            incomeTaxButton.Size = new Size(190, 25);
            incomeTaxButton.TabIndex = 2;
            incomeTaxButton.Text = "Income Tax Reports";
            incomeTaxButton.TextAlign = ContentAlignment.MiddleLeft;
            incomeTaxButton.UseVisualStyleBackColor = true;
            incomeTaxButton.Click += IncomeTaxButton_Click;
            // 
            // othersTaxesButton
            // 
            othersTaxesButton.FlatStyle = FlatStyle.Flat;
            othersTaxesButton.Font = new Font("Segoe UI", 8F);
            othersTaxesButton.Location = new Point(5, 85);
            othersTaxesButton.Name = "othersTaxesButton";
            othersTaxesButton.Size = new Size(190, 25);
            othersTaxesButton.TabIndex = 3;
            othersTaxesButton.Text = "Other Taxes Reports";
            othersTaxesButton.TextAlign = ContentAlignment.MiddleLeft;
            othersTaxesButton.UseVisualStyleBackColor = true;
            othersTaxesButton.Click += OthersTaxesButton_Click;
            // 
            // closeButton
            // 
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Segoe UI", 8F);
            closeButton.Location = new Point(5, 115);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(190, 25);
            closeButton.TabIndex = 4;
            closeButton.Text = "Close";
            closeButton.TextAlign = ContentAlignment.MiddleLeft;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += CloseButton_Click;
            // 
            // TaxReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(265, 391);
            Controls.Add(taxReportGroupBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(0, 30);
            Margin = new Padding(3, 2, 3, 2);
            Name = "TaxReportForm";
            ShowInTaskbar = false;
            Text = "Tax Reports";
            taxReportGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Setup button styling
            SetupButtonStyling();
        }

        private void SetupButtonStyling()
        {
            var buttons = new[] { gstButton, incomeTaxButton, othersTaxesButton, closeButton };

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
            var buttons = new[] { gstButton, incomeTaxButton, othersTaxesButton, closeButton };

            foreach (Button btn in buttons)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                btn.FlatAppearance.BorderSize = 1;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }
        }


        private void GstButton_Click(object? sender, EventArgs e)
        {
            // Create GST Report Form as MDI child
            var gstReportForm = new GstReportForm()
            {
                MdiParent = this.MdiParent,
                Text = "GST Reports",
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false
            };

            // Position to overlap the MDI form's navigation menu area
            gstReportForm.Location = new Point(0, 30); // Same as TaxReportForm position
            
            // Hide TaxReportForm first
            this.Hide();
            
            // Show GST Report Form
            gstReportForm.Show();

            // Handle when GstReportForm closes
            gstReportForm.FormClosed += (s, args) =>
            {
                // Show TaxReportForm menu again
                this.Show();
                this.Activate();
            };
        }

        private void IncomeTaxButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Income Tax Reports feature will be implemented here.\n\nThis will include various income tax related reports and forms.", 
                "Income Tax Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OthersTaxesButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Other Taxes Reports feature will be implemented here.\n\nThis will include other tax types like TDS, TCS, etc.", 
                "Other Taxes Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CloseButton_Click(object? sender, EventArgs e)
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
