using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Auth
{
    public partial class ApiDebugForm : Form
    {
        private readonly CompanyService _companyService;
        private TextBox txtApiResponse = null!;
        private Button btnTestApi = null!;
        private Button btnClose = null!;
        private Label lblStatus = null!;

        public ApiDebugForm(CompanyService companyService)
        {
            _companyService = companyService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtApiResponse = new TextBox();
            btnTestApi = new Button();
            btnClose = new Button();
            lblStatus = new Label();
            SuspendLayout();

            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 9);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Click 'Test API' to see the raw API response";
            lblStatus.ForeColor = Color.Blue;

            // 
            // btnTestApi
            // 
            btnTestApi.Location = new Point(12, 35);
            btnTestApi.Name = "btnTestApi";
            btnTestApi.Size = new Size(100, 30);
            btnTestApi.TabIndex = 0;
            btnTestApi.Text = "&Test API";
            btnTestApi.UseVisualStyleBackColor = true;
            btnTestApi.Click += new EventHandler(btnTestApi_Click);

            // 
            // btnClose
            // 
            btnClose.Location = new Point(120, 35);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 1;
            btnClose.Text = "&Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += new EventHandler(btnClose_Click);

            // 
            // txtApiResponse
            // 
            txtApiResponse.Location = new Point(12, 75);
            txtApiResponse.Name = "txtApiResponse";
            txtApiResponse.Size = new Size(800, 500);
            txtApiResponse.TabIndex = 2;
            txtApiResponse.Multiline = true;
            txtApiResponse.ScrollBars = ScrollBars.Both;
            txtApiResponse.Font = new Font("Consolas", 9);
            txtApiResponse.Text = "API response will appear here...";

            // 
            // ApiDebugForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(824, 587);
            Controls.Add(txtApiResponse);
            Controls.Add(btnClose);
            Controls.Add(btnTestApi);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MinimumSize = new Size(840, 626);
            Name = "ApiDebugForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "API Debug - Company Service";
            KeyDown += new KeyEventHandler(ApiDebugForm_KeyDown);
            ResumeLayout(false);
        }

        private void ApiDebugForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.Handled = true;
            }
        }

        private async void btnTestApi_Click(object? sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Testing API...";
                lblStatus.ForeColor = Color.Blue;
                btnTestApi.Enabled = false;

                var response = await _companyService.GetRawApiResponseAsync();
                txtApiResponse.Text = response;

                lblStatus.Text = "API test completed. Check the response above.";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                txtApiResponse.Text = $"Error: {ex.Message}";
                lblStatus.Text = "API test failed";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnTestApi.Enabled = true;
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
