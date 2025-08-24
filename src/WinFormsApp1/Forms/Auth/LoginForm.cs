using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnTestConnection = null!;
        private Label lblUsername = null!;
        private Label lblPassword = null!;
        private Label lblStatus = null!;

        public LoginForm(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnTestConnection = new Button();
            lblUsername = new Label();
            lblPassword = new Label();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(50, 50);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 47);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(200, 23);
            txtUsername.TabIndex = 0;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(50, 90);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(60, 15);
            lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 87);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(200, 23);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(120, 130);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(100, 30);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += new EventHandler(btnLogin_Click);
            // 
            // btnTestConnection
            // 
            btnTestConnection.Location = new Point(230, 130);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(100, 30);
            btnTestConnection.TabIndex = 3;
            btnTestConnection.Text = "Test API";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += new EventHandler(btnTestConnection_Click);
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(120, 170);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.ForeColor = Color.Red;
            lblStatus.MaximumSize = new Size(250, 0);
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 250);
            Controls.Add(lblStatus);
            Controls.Add(btnTestConnection);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login - Press Tab to navigate, Enter to login, Esc to cancel";
            KeyDown += new KeyEventHandler(LoginForm_KeyDown);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Allow Enter key to trigger login
            AcceptButton = btnLogin;
            CancelButton = null; // We'll handle Esc manually
            
            // Set focus to username field
            txtUsername.Focus();
            
            // Enable keyboard navigation for password field
            txtPassword.KeyDown += txtPassword_KeyDown;
        }

        private void LoginForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    e.Handled = true;
                    break;
                    
                case Keys.F1:
                    ShowLoginHelp();
                    e.Handled = true;
                    break;
            }
        }

        private void txtPassword_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void ShowLoginHelp()
        {
            var helpMessage = @"Login Help:

Keyboard Navigation:
• Tab - Move to next field
• Shift+Tab - Move to previous field
• Enter - Login (from any field)
• Esc - Cancel and close

Fields:
• Username: Enter your username or email
• Password: Enter your password

Buttons:
• Login - Authenticate with the server
• Test API - Check server connection

Press F1 anytime for this help.";

            MessageBox.Show(helpMessage, "Login Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "Please enter both username and password.";
                return;
            }

            btnLogin.Enabled = false;
            lblStatus.Text = "Logging in...";
            lblStatus.ForeColor = Color.Blue;

            try
            {
                var result = await _authService.LoginAsync(txtUsername.Text, txtPassword.Text);
                
                if (result.Success)
                {
                    lblStatus.Text = "Login successful!";
                    lblStatus.ForeColor = Color.Green;
                    
                    // Close the login form after a short delay
                    await Task.Delay(1000);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblStatus.Text = result.Message;
                    lblStatus.ForeColor = Color.Red;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private async void btnTestConnection_Click(object? sender, EventArgs e)
        {
            btnTestConnection.Enabled = false;
            lblStatus.Text = "Testing API connection...";
            lblStatus.ForeColor = Color.Blue;

            try
            {
                var isConnected = await _authService.TestApiConnection();
                if (isConnected)
                {
                    lblStatus.Text = "API connection successful!";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "API connection failed. Check console for details.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Connection test failed: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnTestConnection.Enabled = true;
            }
        }
    }
}
