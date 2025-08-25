using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Company
{
    public partial class CompanyDetailsForm : Form
    {
        private readonly EditCompanyModel _editCompanyModel;
        private readonly CompanyService _companyService;

        // Form controls
        private Label lblId = null!;
        private Label lblName = null!;
        private Label lblAddress = null!;
        private Label lblCity = null!;
        private Label lblState = null!;
        private Label lblZipCode = null!;
        private Label lblCountry = null!;
        private Label lblPhone = null!;
        private Label lblEmail = null!;
        private Label lblWebsite = null!;
        private Label lblTaxId = null!;
        private Label lblLogoUrl = null!;
        private Label lblCurrency = null!;
        private Label lblUserRole = null!;
        private Label lblStartingFinancialYearDate = null!;
        private Button btnEdit = null!;
        private Button btnClose = null!;

        public CompanyDetailsForm(EditCompanyModel editCompanyModel, CompanyService companyService)
        {
            _editCompanyModel = editCompanyModel;
            _companyService = companyService;
            InitializeComponent();
            SetupForm();
            LoadCompanyDetails();
        }

        private void InitializeComponent()
        {
            // Initialize controls
            lblId = new Label();
            lblName = new Label();
            lblAddress = new Label();
            lblCity = new Label();
            lblState = new Label();
            lblZipCode = new Label();
            lblCountry = new Label();
            lblPhone = new Label();
            lblEmail = new Label();
            lblWebsite = new Label();
            lblTaxId = new Label();
            lblLogoUrl = new Label();
            lblCurrency = new Label();
            lblUserRole = new Label();
            lblStartingFinancialYearDate = new Label();
            btnEdit = new Button();
            btnClose = new Button();

            SuspendLayout();

            // Form
            this.Text = "Company Details";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.KeyPreview = true;
            this.KeyDown += CompanyDetailsForm_KeyDown;

            // Create main panel with scrolling
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // Title
            var titleLabel = new Label
            {
                Text = "Company Details",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            // Create a table layout for the fields
            var tablePanel = new TableLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(740, 450),
                ColumnCount = 2,
                RowCount = 15,
                AutoSize = true
            };

            // Set column styles
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Add rows for each field
            AddDetailRow(tablePanel, 0, "ID:", lblId);
            AddDetailRow(tablePanel, 1, "Name:", lblName);
            AddDetailRow(tablePanel, 2, "Address:", lblAddress);
            AddDetailRow(tablePanel, 3, "City:", lblCity);
            AddDetailRow(tablePanel, 4, "State:", lblState);
            AddDetailRow(tablePanel, 5, "Zip Code:", lblZipCode);
            AddDetailRow(tablePanel, 6, "Country:", lblCountry);
            AddDetailRow(tablePanel, 7, "Phone:", lblPhone);
            AddDetailRow(tablePanel, 8, "Email:", lblEmail);
            AddDetailRow(tablePanel, 9, "Website:", lblWebsite);
            AddDetailRow(tablePanel, 10, "Tax ID:", lblTaxId);
            AddDetailRow(tablePanel, 11, "Logo URL:", lblLogoUrl);
            AddDetailRow(tablePanel, 12, "Currency:", lblCurrency);
            AddDetailRow(tablePanel, 13, "User Role:", lblUserRole);
            AddDetailRow(tablePanel, 14, "Financial Year Start:", lblStartingFinancialYearDate);

            // Buttons panel
            var buttonPanel = new Panel
            {
                Location = new Point(20, 520),
                Size = new Size(740, 40),
                Dock = DockStyle.Bottom
            };

            btnEdit.Text = "&Edit Company";
            btnEdit.Size = new Size(120, 30);
            btnEdit.Location = new Point(500, 5);
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;

            btnClose.Text = "&Close";
            btnClose.Size = new Size(100, 30);
            btnClose.Location = new Point(630, 5);
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnClose);

            // Add controls to main panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(tablePanel);
            
            // Add panels to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);

            ResumeLayout(false);
        }

        private void AddDetailRow(TableLayoutPanel table, int row, string labelText, Label valueLabel)
        {
            // Create label for field name
            var fieldLabel = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Right,
                Margin = new Padding(5)
            };

            // Configure value label
            valueLabel.Font = new Font("Segoe UI", 10);
            valueLabel.TextAlign = ContentAlignment.MiddleLeft;
            valueLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            valueLabel.Margin = new Padding(5);
            valueLabel.AutoSize = false;
            valueLabel.Size = new Size(500, 25);

            // Add to table
            table.Controls.Add(fieldLabel, 0, row);
            table.Controls.Add(valueLabel, 1, row);

            // Set row style
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        }

        private void SetupForm()
        {
            // Set Accept and Cancel buttons
            this.AcceptButton = btnEdit;
            this.CancelButton = btnClose;
        }

        private void LoadCompanyDetails()
        {
            lblId.Text = _editCompanyModel.Id;
            lblName.Text = _editCompanyModel.Name;
            lblAddress.Text = string.IsNullOrEmpty(_editCompanyModel.Address) ? "(Not specified)" : _editCompanyModel.Address;
            lblCity.Text = string.IsNullOrEmpty(_editCompanyModel.City) ? "(Not specified)" : _editCompanyModel.City;
            lblState.Text = string.IsNullOrEmpty(_editCompanyModel.State) ? "(Not specified)" : _editCompanyModel.State;
            lblZipCode.Text = string.IsNullOrEmpty(_editCompanyModel.ZipCode) ? "(Not specified)" : _editCompanyModel.ZipCode;
            lblCountry.Text = string.IsNullOrEmpty(_editCompanyModel.Country) ? "(Not specified)" : _editCompanyModel.Country;
            lblPhone.Text = string.IsNullOrEmpty(_editCompanyModel.Phone) ? "(Not specified)" : _editCompanyModel.Phone;
            lblEmail.Text = string.IsNullOrEmpty(_editCompanyModel.Email) ? "(Not specified)" : _editCompanyModel.Email;
            lblWebsite.Text = string.IsNullOrEmpty(_editCompanyModel.Website) ? "(Not specified)" : _editCompanyModel.Website;
            lblTaxId.Text = string.IsNullOrEmpty(_editCompanyModel.TaxId) ? "(Not specified)" : _editCompanyModel.TaxId;
            lblLogoUrl.Text = string.IsNullOrEmpty(_editCompanyModel.LogoUrl) ? "(Not specified)" : _editCompanyModel.LogoUrl;
            lblCurrency.Text = string.IsNullOrEmpty(_editCompanyModel.Currency) ? "(Not specified)" : _editCompanyModel.Currency;
            lblUserRole.Text = string.IsNullOrEmpty(_editCompanyModel.UserRole) ? "(Not specified)" : _editCompanyModel.UserRole;
            lblStartingFinancialYearDate.Text = _editCompanyModel.StartingFinancialYearDate?.ToString("yyyy-MM-dd") ?? "(Not specified)";
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            // Convert to Company model and open edit form
            var company = _editCompanyModel.ToCompany();
            var editForm = new CompanyEditForm(_companyService, new CountryService(), company);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                // Close this form and let the parent refresh
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void CompanyDetailsForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    e.Handled = true;
                    break;

                case Keys.F1:
                    ShowHelp();
                    e.Handled = true;
                    break;
            }
        }

        private void ShowHelp()
        {
            var helpMessage = @"Company Details Help:

Keyboard Navigation:
• Esc - Close this window
• Enter - Edit company
• F1 - Show this help

Company Information:
This form displays all available details for the selected company from the server.

Fields Displayed:
• Basic Information: ID, Name
• Address Details: Address, City, State, Zip Code, Country  
• Contact Information: Phone, Email, Website
• Business Details: Tax ID, Logo URL, Currency
• System Information: User Role, Financial Year Start Date

Actions:
• Edit Company - Opens the company edit form
• Close - Closes this details window

Note: Empty fields are shown as '(Not specified)'";

            MessageBox.Show(helpMessage, "Company Details Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

