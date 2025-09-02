using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class TaxSelectionDialog : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvTaxes = null!;
        private DataGridView dgvComponents = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;
        private Label lblComponents = null!;
        private List<TaxListDto> _allTaxes;
        private List<TaxListDto> _filteredTaxes;
        private TaxListDto? _selectedTax;

        public List<TaxSelectionResult> SelectedTaxes { get; private set; } = new List<TaxSelectionResult>();

        public TaxSelectionDialog(List<TaxListDto> taxes)
        {
            _allTaxes = taxes;
            _filteredTaxes = new List<TaxListDto>(taxes);
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Text = "Select Taxes and Components";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;
            
            txtSearch = new TextBox();
            dgvTaxes = new DataGridView();
            dgvComponents = new DataGridView();
            btnOK = new Button();
            btnCancel = new Button();
            lblComponents = new Label();

            SuspendLayout();

            // Search textbox
            txtSearch.Location = new Point(20, 20);
            txtSearch.Size = new Size(940, 25);
            txtSearch.PlaceholderText = "Type to search taxes...";
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Taxes grid (left side)
            dgvTaxes.Location = new Point(20, 55);
            dgvTaxes.Size = new Size(460, 480);
            dgvTaxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaxes.MultiSelect = false;
            dgvTaxes.AllowUserToAddRows = false;
            dgvTaxes.AllowUserToDeleteRows = false;
            dgvTaxes.ReadOnly = true;
            dgvTaxes.RowHeadersVisible = false;
            dgvTaxes.AutoGenerateColumns = false;
            dgvTaxes.SelectionChanged += DgvTaxes_SelectionChanged;
            
            SetupTaxGrid();

            // Components label
            lblComponents.Location = new Point(500, 55);
            lblComponents.Size = new Size(460, 25);
            lblComponents.Text = "Select Tax Components (for the selected tax):";
            lblComponents.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Add keyboard navigation hint
            var lblKeyboardHint = new Label
            {
                Location = new Point(500, 25),
                Size = new Size(460, 20),
                Text = "Navigation: Tab → Components, Shift+Tab → Back, Space → Select, Enter → OK",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.DarkBlue
            };

            // Components grid (right side)
            dgvComponents.Location = new Point(500, 85);
            dgvComponents.Size = new Size(460, 450);
            dgvComponents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvComponents.MultiSelect = true;
            dgvComponents.AllowUserToAddRows = false;
            dgvComponents.AllowUserToDeleteRows = false;
            dgvComponents.ReadOnly = true;
            dgvComponents.RowHeadersVisible = false;
            dgvComponents.AutoGenerateColumns = false;
            dgvComponents.SelectionChanged += DgvComponents_SelectionChanged;
            
            SetupComponentGrid();

            // Buttons
            btnOK.Location = new Point(800, 545);
            btnOK.Size = new Size(75, 30);
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += BtnOK_Click;

            btnCancel.Location = new Point(885, 545);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            Controls.AddRange(new Control[] { txtSearch, dgvTaxes, lblComponents, lblKeyboardHint, dgvComponents, btnOK, btnCancel });

            LoadTaxes();
            txtSearch.Focus();

            ResumeLayout(false);
            PerformLayout();

            // Event handlers
            KeyDown += TaxSelectionDialog_KeyDown;
            dgvTaxes.KeyDown += DgvTaxes_KeyDown;
            dgvTaxes.DoubleClick += DgvTaxes_DoubleClick;
            dgvComponents.KeyDown += DgvComponents_KeyDown;
        }

        private void SetupTaxGrid()
        {
            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Tax Name",
                DataPropertyName = "Name",
                Width = 200
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DefaultRate",
                HeaderText = "Rate (%)",
                DataPropertyName = "DefaultRate",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvTaxes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HSNCode",
                HeaderText = "HSN Code",
                DataPropertyName = "HSNCode",
                Width = 100
            });
        }

        private void SetupComponentGrid()
        {
            dgvComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Component Name",
                DataPropertyName = "Name",
                Width = 200
            });

            dgvComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rate",
                HeaderText = "Rate (%)",
                DataPropertyName = "Rate",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvComponents.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 160
            });

            // Set up visual styling for better selection visibility
            dgvComponents.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvComponents.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvComponents.GridColor = Color.LightGray;
            dgvComponents.BorderStyle = BorderStyle.Fixed3D;
        }

        private void LoadTaxes()
        {
            dgvTaxes.DataSource = _filteredTaxes;
            if (_filteredTaxes.Any())
            {
                dgvTaxes.Rows[0].Selected = true;
                LoadComponentsForSelectedTax();
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredTaxes = new List<TaxListDto>(_allTaxes);
            }
            else
            {
                _filteredTaxes = _allTaxes.Where(t => 
                    t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Category.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (t.HSNCode?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }
            
            LoadTaxes();
        }

        private void DgvTaxes_SelectionChanged(object? sender, EventArgs e)
        {
            LoadComponentsForSelectedTax();
        }

        private void LoadComponentsForSelectedTax()
        {
            if (dgvTaxes.SelectedRows.Count > 0)
            {
                _selectedTax = dgvTaxes.SelectedRows[0].DataBoundItem as TaxListDto;
                if (_selectedTax?.Components != null && _selectedTax.Components.Any())
                {
                    dgvComponents.DataSource = _selectedTax.Components.ToList();
                    lblComponents.Text = $"Select Tax Components for '{_selectedTax.Name}' (Click to select):";
                    
                    // Do NOT select all components by default - user must manually select
                    dgvComponents.ClearSelection();
                }
                else
                {
                    dgvComponents.DataSource = new List<TaxComponentDto>();
                    lblComponents.Text = $"No components available for '{_selectedTax?.Name ?? "selected tax"}':";
                }
            }
            else
            {
                dgvComponents.DataSource = new List<TaxComponentDto>();
                lblComponents.Text = "Select Tax Components:";
            }
        }

        private void TaxSelectionDialog_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnCancel_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvTaxes_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                // When Tab is pressed in tax list, move focus to component list
                dgvComponents.Focus();
                if (dgvComponents.Rows.Count > 0)
                {
                    // Select the first row in components grid if available
                    dgvComponents.Rows[0].Selected = true;
                    dgvComponents.CurrentCell = dgvComponents.Rows[0].Cells[0];
                }
                e.Handled = true;
            }
        }

        private void DgvComponents_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (e.Modifiers.HasFlag(Keys.Shift))
                {
                    // Shift+Tab: Go back to tax list
                    dgvTaxes.Focus();
                    if (dgvTaxes.SelectedRows.Count > 0)
                    {
                        dgvTaxes.CurrentCell = dgvTaxes.SelectedRows[0].Cells[0];
                    }
                }
                else
                {
                    // Tab: Move to OK button
                    btnOK.Focus();
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Enter: Move to OK button
                btnOK.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Space)
            {
                // Space: Toggle selection of current component row
                if (dgvComponents.CurrentRow != null)
                {
                    dgvComponents.CurrentRow.Selected = !dgvComponents.CurrentRow.Selected;
                }
                e.Handled = true;
            }
        }

        private void DgvComponents_SelectionChanged(object? sender, EventArgs e)
        {
            // Update the components label to show selection count
            if (_selectedTax != null)
            {
                var selectedCount = dgvComponents.SelectedRows.Count;
                var totalCount = _selectedTax.Components?.Count ?? 0;
                lblComponents.Text = $"Select Tax Components for '{_selectedTax.Name}' ({selectedCount}/{totalCount} selected):";
            }
        }

        private void DgvTaxes_DoubleClick(object? sender, EventArgs e)
        {
            BtnOK_Click(null, EventArgs.Empty);
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (dgvTaxes.SelectedRows.Count > 0)
            {
                var selectedTax = dgvTaxes.SelectedRows[0].DataBoundItem as TaxListDto;
                if (selectedTax != null)
                {
                    var selectedComponents = dgvComponents.SelectedRows.Cast<DataGridViewRow>()
                        .Select(row => row.DataBoundItem as TaxComponentDto)
                        .Where(component => component != null)
                        .Cast<TaxComponentDto>()
                        .ToList();

                    // Validate that at least one component is selected if components are available
                    if (selectedTax.Components?.Any() == true && !selectedComponents.Any())
                    {
                        MessageBox.Show("Please select at least one tax component for the selected tax.", "No Component Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var componentsDisplay = selectedComponents.Any() 
                        ? string.Join(", ", selectedComponents.Select(c => c.DisplayName))
                        : "No Components";

                    var taxResult = new TaxSelectionResult
                    {
                        Tax = selectedTax,
                        SelectedComponents = selectedComponents,
                        ComponentsDisplay = componentsDisplay
                    };

                    SelectedTaxes.Add(taxResult);
                    
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Please select a tax.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
