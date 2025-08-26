using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.FinancialYear
{
    public partial class FinancialYearEditForm : Form
    {
        private readonly FinancialYearService _financialYearService;
        private readonly LocalStorageService _localStorageService;
        private FinancialYearModel? _financialYear;
        private Guid _companyId;
        private bool _isEditMode;
        
        // Controls
        private TextBox txtYearLabel = null!;
        private DateTimePicker dtpStartDate = null!;
        private DateTimePicker dtpEndDate = null!;
        private CheckBox chkIsActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private Label lblYearLabel = null!;
        private Label lblStartDate = null!;
        private Label lblEndDate = null!;

        public bool IsEditMode => _isEditMode;
        public Guid FinancialYearId => _financialYear?.Id ?? Guid.Empty;

        public FinancialYearEditForm(FinancialYearService financialYearService, FinancialYearModel? financialYear, Guid companyId)
        {
            _financialYearService = financialYearService;
            _localStorageService = new LocalStorageService();
            _financialYear = financialYear;
            _companyId = companyId;
            _isEditMode = financialYear != null;
            
            InitializeComponent();
            SetupForm();
            LoadFinancialYearData();
        }

        private void InitializeComponent()
        {
            txtYearLabel = new TextBox();
            dtpStartDate = new DateTimePicker();
            dtpEndDate = new DateTimePicker();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            lblYearLabel = new Label();
            lblStartDate = new Label();
            lblEndDate = new Label();
            SuspendLayout();
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(600, 40);
            lblInstructions.Text = "Financial Year Details: Fill in the required fields and press Ctrl+Enter to save, Esc to cancel";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // lblYearLabel
            // 
            lblYearLabel.Location = new Point(12, 60);
            lblYearLabel.Name = "lblYearLabel";
            lblYearLabel.Size = new Size(150, 20);
            lblYearLabel.Text = "Year Label:";
            lblYearLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            
            // 
            // txtYearLabel
            // 
            txtYearLabel.Location = new Point(170, 60);
            txtYearLabel.Name = "txtYearLabel";
            txtYearLabel.Size = new Size(200, 23);
            txtYearLabel.TabIndex = 0;
            txtYearLabel.PlaceholderText = "e.g. 2025-2026";
            
            // 
            // lblStartDate
            // 
            lblStartDate.Location = new Point(12, 100);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(150, 20);
            lblStartDate.Text = "Start Date:";
            lblStartDate.Font = new Font("Arial", 9, FontStyle.Bold);
            
            // 
            // dtpStartDate
            // 
            dtpStartDate.Location = new Point(170, 100);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(200, 23);
            dtpStartDate.TabIndex = 1;
            dtpStartDate.Format = DateTimePickerFormat.Short;
            
            // 
            // lblEndDate
            // 
            lblEndDate.Location = new Point(12, 140);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(150, 20);
            lblEndDate.Text = "End Date:";
            lblEndDate.Font = new Font("Arial", 9, FontStyle.Bold);
            
            // 
            // dtpEndDate
            // 
            dtpEndDate.Location = new Point(170, 140);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(200, 23);
            dtpEndDate.TabIndex = 2;
            dtpEndDate.Format = DateTimePickerFormat.Short;
            
            // 
            // chkIsActive
            // 
            chkIsActive.Location = new Point(170, 180);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(200, 20);
            chkIsActive.TabIndex = 3;
            chkIsActive.Text = "Is Active";
            chkIsActive.Checked = true;
            
            // 
            // btnSave
            // 
            btnSave.Location = new Point(170, 220);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);
            
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(260, 220);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 270);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // FinancialYearEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(dtpEndDate);
            Controls.Add(lblEndDate);
            Controls.Add(dtpStartDate);
            Controls.Add(lblStartDate);
            Controls.Add(txtYearLabel);
            Controls.Add(lblYearLabel);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "FinancialYearEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _isEditMode ? "Edit Financial Year" : "New Financial Year";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(FinancialYearEditForm_KeyDown);
            Load += new EventHandler(FinancialYearEditForm_Load);
            Resize += new EventHandler(FinancialYearEditForm_Resize);
            Activated += new EventHandler(FinancialYearEditForm_Activated);
            FormClosing += new FormClosingEventHandler(FinancialYearEditForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            txtYearLabel.Focus();
        }

        private void FinancialYearEditForm_Load(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            ResizeControls();
            txtYearLabel.Focus();
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }

        private void ResizeControls()
        {
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            lblInstructions.Size = new Size(clientWidth - 24, 40);
            
            lblYearLabel.Location = new Point(12, 60);
            txtYearLabel.Location = new Point(170, 60);
            txtYearLabel.Size = new Size(300, 23);
            
            lblStartDate.Location = new Point(12, 100);
            dtpStartDate.Location = new Point(170, 100);
            dtpStartDate.Size = new Size(200, 23);
            
            lblEndDate.Location = new Point(12, 140);
            dtpEndDate.Location = new Point(170, 140);
            dtpEndDate.Size = new Size(200, 23);
            
            chkIsActive.Location = new Point(170, 180);
            
            btnSave.Location = new Point(170, 220);
            btnCancel.Location = new Point(260, 220);
            
            lblStatus.Location = new Point(12, clientHeight - 30);
            lblStatus.Size = new Size(clientWidth - 24, 20);
        }

        private void FinancialYearEditForm_Resize(object? sender, EventArgs e)
        {
            ResizeControls();
        }

        private void FinancialYearEditForm_Activated(object? sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }

        private void FinancialYearEditForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
        }

        private async void LoadFinancialYearData()
        {
            try
            {
                if (_isEditMode && _financialYear != null)
                {
                    await LoadFinancialYearDataFromModel();
                }
                else
                {
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading financial year data: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load financial year data exception: {ex.Message}");
            }
        }

        private async Task LoadFinancialYearDataFromModel()
        {
            if (_financialYear == null) return;

            txtYearLabel.Text = _financialYear.YearLabel ?? "";
            dtpStartDate.Value = _financialYear.StartDate;
            dtpEndDate.Value = _financialYear.EndDate;
            chkIsActive.Checked = _financialYear.IsActive;
        }

        private void ClearForm()
        {
            txtYearLabel.Text = "";
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddYears(1);
            chkIsActive.Checked = true;
        }

        private void FinancialYearEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.BeginInvoke(new Action(() =>
                        {
                            mdiForm.ShowNavigationPanel();
                            mdiForm.SetFocusToNavigation();
                        }));
                    }
                    DialogResult = DialogResult.Cancel;
                    Close();
                    e.Handled = true;
                    break;
                    
                case Keys.Enter:
                    if (e.Control)
                    {
                        SaveFinancialYear();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            SaveFinancialYear();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void SaveFinancialYear()
        {
            try
            {
                if (!ValidateForm())
                {
                    return;
                }

                lblStatus.Text = "Saving financial year...";
                lblStatus.ForeColor = Color.Blue;

                var financialYear = new FinancialYearModel
                {
                    CompanyId = _companyId,
                    YearLabel = txtYearLabel.Text.Trim(),
                    StartDate = dtpStartDate.Value.Date,
                    EndDate = dtpEndDate.Value.Date,
                    IsActive = chkIsActive.Checked
                };

                FinancialYearModel? savedFinancialYear;

                if (_isEditMode && _financialYear != null)
                {
                    financialYear.Id = _financialYear.Id;
                    savedFinancialYear = await _financialYearService.UpdateFinancialYearAsync(_financialYear.Id, financialYear);
                }
                else
                {
                    savedFinancialYear = await _financialYearService.CreateFinancialYearAsync(financialYear);
                }

                if (savedFinancialYear != null)
                {
                    lblStatus.Text = $"Financial year '{savedFinancialYear.YearLabel}' saved successfully!";
                    lblStatus.ForeColor = Color.Green;
                    
                    await Task.Delay(1000);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    lblStatus.Text = "Failed to save financial year";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving financial year: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Save financial year exception: {ex.Message}");
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtYearLabel.Text))
            {
                lblStatus.Text = "Year Label is required";
                lblStatus.ForeColor = Color.Red;
                txtYearLabel.Focus();
                return false;
            }

            if (dtpStartDate.Value >= dtpEndDate.Value)
            {
                lblStatus.Text = "End Date must be after Start Date";
                lblStatus.ForeColor = Color.Red;
                dtpEndDate.Focus();
                return false;
            }

            return true;
        }
    }
}


