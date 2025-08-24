using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Dashboard
{
    public partial class DashboardForm : Form
    {
        private readonly LocalStorageService _localStorageService;
        private readonly CompanyService _companyService;
        private WinFormsApp1.Models.Company? _selectedCompany;

        public DashboardForm(LocalStorageService localStorageService, CompanyService companyService)
        {
            _localStorageService = localStorageService;
            _companyService = companyService;
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // DashboardForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 800);
            this.Name = "DashboardForm";
            this.Text = "Company Dashboard";
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.DashboardForm_KeyDown);
            
            // Create main panel
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // Create header panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            // Company name label
            var companyNameLabel = new Label
            {
                Name = "lblCompanyName",
                Text = "Loading company...",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Welcome message
            var welcomeLabel = new Label
            {
                Name = "lblWelcome",
                Text = "Welcome to your company dashboard",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            headerPanel.Controls.Add(companyNameLabel);
            headerPanel.Controls.Add(welcomeLabel);

            // Create content panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Create dashboard grid
            var dashboardGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                ColumnStyles = 
                {
                    new ColumnStyle(SizeType.Percent, 50F),
                    new ColumnStyle(SizeType.Percent, 50F)
                },
                RowStyles = 
                {
                    new RowStyle(SizeType.Percent, 50F),
                    new RowStyle(SizeType.Percent, 50F)
                }
            };

            // Quick Actions Card
            var quickActionsCard = CreateDashboardCard("Quick Actions", "Common tasks and shortcuts");
            var quickActionsList = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };
            quickActionsList.Items.AddRange(new object[]
            {
                "• Alt+S - Select Company",
                "• Alt+C - Manage Companies", 
                "• F1 - Show Help",
                "• Esc - Close this form",
                "• Alt+F - Open File Menu"
            });
            quickActionsCard.Controls.Add(quickActionsList);

            // Company Info Card
            var companyInfoCard = CreateDashboardCard("Company Information", "Current company details");
            var companyInfoLabel = new Label
            {
                Name = "lblCompanyInfo",
                Text = "Loading company information...",
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(10)
            };
            companyInfoCard.Controls.Add(companyInfoLabel);

            // System Status Card
            var systemStatusCard = CreateDashboardCard("System Status", "Application and connection status");
            var systemStatusLabel = new Label
            {
                Name = "lblSystemStatus",
                Text = "Checking system status...",
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(10)
            };
            systemStatusCard.Controls.Add(systemStatusLabel);

            // Recent Activity Card
            var recentActivityCard = CreateDashboardCard("Recent Activity", "Latest actions and updates");
            var recentActivityList = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };
            recentActivityList.Items.AddRange(new object[]
            {
                "• Company selected successfully",
                "• Dashboard loaded",
                "• Ready for operations"
            });
            recentActivityCard.Controls.Add(recentActivityList);

            // Add cards to grid
            dashboardGrid.Controls.Add(quickActionsCard, 0, 0);
            dashboardGrid.Controls.Add(companyInfoCard, 1, 0);
            dashboardGrid.Controls.Add(systemStatusCard, 0, 1);
            dashboardGrid.Controls.Add(recentActivityCard, 1, 1);

            contentPanel.Controls.Add(dashboardGrid);

            // Add panels to form
            this.Controls.Add(contentPanel);
            this.Controls.Add(headerPanel);

            this.ResumeLayout(false);
        }

        private Panel CreateDashboardCard(string title, string subtitle)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            var subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 40)
            };

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Top = 70,
                Padding = new Padding(10)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(subtitleLabel);
            card.Controls.Add(contentPanel);

            return card;
        }

        private void SetupForm()
        {
            LoadCompanyInformation();
            UpdateSystemStatus();
        }

        private async void LoadCompanyInformation()
        {
            try
            {
                _selectedCompany = await _localStorageService.GetSelectedCompanyAsync();
                
                if (_selectedCompany != null)
                {
                    // Update company name in header
                    var companyNameLabel = this.Controls.Find("lblCompanyName", true).FirstOrDefault() as Label;
                    if (companyNameLabel != null)
                    {
                        companyNameLabel.Text = _selectedCompany.DisplayName;
                    }

                    // Update company info
                    var companyInfoLabel = this.Controls.Find("lblCompanyInfo", true).FirstOrDefault() as Label;
                    if (companyInfoLabel != null)
                    {
                        companyInfoLabel.Text = $"Company Name: {_selectedCompany.Name}\n" +
                                               $"Company Code: {_selectedCompany.Code}\n" +
                                               $"Status: {((_selectedCompany.IsActive) ? "Active" : "Inactive")}\n" +
                                               $"ID: {_selectedCompany.Id}\n\n" +
                                               $"This is your main dashboard for managing {_selectedCompany.Name}.";
                    }
                }
                else
                {
                    var companyNameLabel = this.Controls.Find("lblCompanyName", true).FirstOrDefault() as Label;
                    if (companyNameLabel != null)
                    {
                        companyNameLabel.Text = "No Company Selected";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading company information: {ex.Message}");
            }
        }

        private async void UpdateSystemStatus()
        {
            try
            {
                var systemStatusLabel = this.Controls.Find("lblSystemStatus", true).FirstOrDefault() as Label;
                if (systemStatusLabel != null)
                {
                    systemStatusLabel.Text = "Checking API connection...";
                    
                    // Test API connection
                    var companies = await _companyService.GetAllCompaniesAsync();
                    
                    systemStatusLabel.Text = $"API Status: Connected ✓\n" +
                                           $"Companies Available: {companies?.Count ?? 0}\n" +
                                           $"Last Check: {DateTime.Now:HH:mm:ss}\n" +
                                           $"System: Ready";
                }
            }
            catch (Exception ex)
            {
                var systemStatusLabel = this.Controls.Find("lblSystemStatus", true).FirstOrDefault() as Label;
                if (systemStatusLabel != null)
                {
                    systemStatusLabel.Text = $"API Status: Error ✗\n" +
                                           $"Error: {ex.Message}\n" +
                                           $"Last Check: {DateTime.Now:HH:mm:ss}\n" +
                                           $"System: Needs attention";
                }
            }
        }

        private void DashboardForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: // ESC to close dashboard
                    this.Close();
                    e.Handled = true;
                    break;
                    
                case Keys.F1: // F1 for Help
                    ShowDashboardHelp();
                    e.Handled = true;
                    break;
                    
                case Keys.F5: // F5 to refresh
                    LoadCompanyInformation();
                    UpdateSystemStatus();
                    e.Handled = true;
                    break;
            }
        }

        private void ShowDashboardHelp()
        {
            var helpMessage = @"Dashboard Navigation Help:

Dashboard Controls:
• Esc - Close dashboard and return to main menu
• F1 - Show this help
• F5 - Refresh dashboard data

Quick Actions:
• Alt+S - Select different company
• Alt+C - Manage companies
• Alt+F - Open File menu

Company Dashboard:
This is your main workspace for the selected company.
All company-specific operations can be accessed from here.

Status Information:
• API Connection: Shows if the system can connect to the server
• Company Details: Displays current company information
• Recent Activity: Shows latest system actions";

            MessageBox.Show(helpMessage, "Dashboard Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
