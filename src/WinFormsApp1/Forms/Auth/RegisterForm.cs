using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Auth
{
    public partial class RegisterForm : Form
    {
        private readonly AuthService _authService;

        // Form controls
        private TextBox txtFirstName = null!;
        private TextBox txtLastName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private Button btnRegister = null!;
        private Button btnCancel = null!;
        private Button btnResendVerification = null!;
        private Label lblStatus = null!;
        private CheckBox chkShowPassword = null!;
        private CheckBox chkShowConfirmPassword = null!;

        public RegisterForm()
        {
            _authService = new AuthService();
            InitializeComponent();
            SetupForm();
            
            // Add keyboard event handler for ESC key
            this.KeyPreview = true;
            this.KeyDown += RegisterForm_KeyDown;
        }

        private void InitializeComponent()
        {
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnRegister = new Button();
            btnCancel = new Button();
            btnResendVerification = new Button();
            lblStatus = new Label();
            chkShowPassword = new CheckBox();
            chkShowConfirmPassword = new CheckBox();
            
            SuspendLayout();

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 500);
            Name = "RegisterForm";
            Text = "Register New User";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Labels and controls layout
            var y = 30;
            var labelX = 30;
            var controlX = 150;
            var controlWidth = 250;
            var labelWidth = 100;
            var controlHeight = 25;
            var spacing = 35;

            // Title
            var lblTitle = new Label 
            { 
                Text = "Create New Account", 
                Location = new Point(labelX, y), 
                Size = new Size(350, 30),
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            Controls.Add(lblTitle);
            y += 40;

            // First Name
            var lblFirstName = new Label { Text = "First Name:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtFirstName.Location = new Point(controlX, y);
            txtFirstName.Size = new Size(controlWidth, controlHeight);
            txtFirstName.MaxLength = 100;
            Controls.Add(lblFirstName);
            Controls.Add(txtFirstName);
            y += spacing;

            // Last Name
            var lblLastName = new Label { Text = "Last Name:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtLastName.Location = new Point(controlX, y);
            txtLastName.Size = new Size(controlWidth, controlHeight);
            txtLastName.MaxLength = 100;
            Controls.Add(lblLastName);
            Controls.Add(txtLastName);
            y += spacing;

            // Email
            var lblEmail = new Label { Text = "Email:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtEmail.Location = new Point(controlX, y);
            txtEmail.Size = new Size(controlWidth, controlHeight);
            txtEmail.MaxLength = 255;
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            y += spacing;

            // Password
            var lblPassword = new Label { Text = "Password:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtPassword.Location = new Point(controlX, y);
            txtPassword.Size = new Size(controlWidth, controlHeight);
            txtPassword.MaxLength = 100;
            txtPassword.UseSystemPasswordChar = true;
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            y += 25;

            // Show Password checkbox
            chkShowPassword.Location = new Point(controlX, y);
            chkShowPassword.Size = new Size(150, 20);
            chkShowPassword.Text = "Show Password";
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
            Controls.Add(chkShowPassword);
            y += spacing;

            // Confirm Password
            var lblConfirmPassword = new Label { Text = "Confirm Password:", Location = new Point(labelX, y), Size = new Size(labelWidth, controlHeight) };
            txtConfirmPassword.Location = new Point(controlX, y);
            txtConfirmPassword.Size = new Size(controlWidth, controlHeight);
            txtConfirmPassword.MaxLength = 100;
            txtConfirmPassword.UseSystemPasswordChar = true;
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtConfirmPassword);
            y += 25;

            // Show Confirm Password checkbox
            chkShowConfirmPassword.Location = new Point(controlX, y);
            chkShowConfirmPassword.Size = new Size(150, 20);
            chkShowConfirmPassword.Text = "Show Confirm Password";
            chkShowConfirmPassword.CheckedChanged += chkShowConfirmPassword_CheckedChanged;
            Controls.Add(chkShowConfirmPassword);
            y += spacing + 20;

            // Buttons
            btnRegister.Location = new Point(controlX, y);
            btnRegister.Size = new Size(100, 35);
            btnRegister.Text = "Register";
            btnRegister.Click += btnRegister_Click;
            Controls.Add(btnRegister);

            btnCancel.Location = new Point(controlX + 120, y);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            Controls.Add(btnCancel);

            y += 50;

            // Resend Verification button
            btnResendVerification.Location = new Point(labelX, y);
            btnResendVerification.Size = new Size(150, 30);
            btnResendVerification.Text = "Resend Verification";
            btnResendVerification.Click += btnResendVerification_Click;
            btnResendVerification.Enabled = false;
            Controls.Add(btnResendVerification);

            // Status label
            lblStatus.Location = new Point(labelX, y + 40);
            lblStatus.Size = new Size(400, 50);
            lblStatus.Text = "Please fill in all required fields to register.";
            lblStatus.ForeColor = Color.Black;
            Controls.Add(lblStatus);

            ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Set form properties
            ShowInTaskbar = false;
        }

        private void chkShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void chkShowConfirmPassword_CheckedChanged(object? sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !chkShowConfirmPassword.Checked;
        }

        private async void btnRegister_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                btnRegister.Enabled = false;
                lblStatus.Text = "Registering...";
                lblStatus.ForeColor = Color.Blue;

                var registerModel = new RegisterModel
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text,
                    ConfirmPassword = txtConfirmPassword.Text
                };

                var response = await _authService.RegisterAsync(registerModel);

                if (response.Success || response.IsRegistrationSuccessful())
                {
                    lblStatus.Text = "Registration successful! Please check your email for verification.";
                    lblStatus.ForeColor = Color.Green;
                    btnResendVerification.Enabled = true;
                    
                    // Clear sensitive fields
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    
                    MessageBox.Show("Registration successful! Please check your email for verification instructions.", 
                        "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = $"Registration failed: {response.Message ?? response.Error}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Registration failed: {response.Message ?? response.Error}", 
                        "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error during registration: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error during registration: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }

        private async void btnResendVerification_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Please enter your email address first.", 
                        "Email Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnResendVerification.Enabled = false;
                lblStatus.Text = "Sending verification email...";
                lblStatus.ForeColor = Color.Blue;

                var response = await _authService.ResendVerificationAsync(txtEmail.Text.Trim());

                if (response.Success || response.IsRegistrationSuccessful())
                {
                    lblStatus.Text = "Verification email sent successfully! Please check your email.";
                    lblStatus.ForeColor = Color.Green;
                    MessageBox.Show("Verification email sent successfully! Please check your email.", 
                        "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = $"Failed to send verification email: {response.Message ?? response.Error}";
                    lblStatus.ForeColor = Color.Red;
                    MessageBox.Show($"Failed to send verification email: {response.Message ?? response.Error}", 
                        "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error sending verification email: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error sending verification email: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnResendVerification.Enabled = true;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Basic email validation
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please confirm your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void RegisterForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
