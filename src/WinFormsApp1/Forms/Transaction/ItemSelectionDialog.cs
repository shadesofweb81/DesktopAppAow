using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class ItemSelectionDialog : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvProducts = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;
        private List<ProductListDto> _allProducts;
        private List<ProductListDto> _filteredProducts;

        public List<ProductListDto> SelectedProducts { get; private set; } = new List<ProductListDto>();

        public ItemSelectionDialog(List<ProductListDto> products)
        {
            _allProducts = products;
            _filteredProducts = new List<ProductListDto>(products);
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Text = "Select Products";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;
            
            txtSearch = new TextBox();
            dgvProducts = new DataGridView();
            btnOK = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Search textbox
            txtSearch.Location = new Point(20, 20);
            txtSearch.Size = new Size(740, 25);
            txtSearch.PlaceholderText = "Type to search products...";
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Products grid
            dgvProducts.Location = new Point(20, 55);
            dgvProducts.Size = new Size(740, 480);
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.AutoGenerateColumns = false;
            
            SetupProductGrid();

            // Buttons
            btnOK.Location = new Point(600, 545);
            btnOK.Size = new Size(75, 30);
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += BtnOK_Click;

            btnCancel.Location = new Point(685, 545);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;

            Controls.AddRange(new Control[] { txtSearch, dgvProducts, btnOK, btnCancel });

            LoadProducts();
            txtSearch.Focus();

            ResumeLayout(false);
            PerformLayout();

            // Event handlers
            KeyDown += ItemSelectionDialog_KeyDown;
            dgvProducts.KeyDown += DgvProducts_KeyDown;
            dgvProducts.DoubleClick += DgvProducts_DoubleClick;
        }

        private void SetupProductGrid()
        {
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductCode",
                HeaderText = "Code",
                DataPropertyName = "ProductCode",
                Width = 100
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Product Name",
                DataPropertyName = "Name",
                Width = 300
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Category",
                HeaderText = "Category",
                DataPropertyName = "Category",
                Width = 120
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "Unit",
                DataPropertyName = "Unit",
                Width = 80
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SellingPrice",
                HeaderText = "Price",
                DataPropertyName = "SellingPrice",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StockQuantity",
                HeaderText = "Stock",
                DataPropertyName = "StockQuantity",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private void LoadProducts()
        {
            dgvProducts.DataSource = _filteredProducts;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredProducts = new List<ProductListDto>(_allProducts);
            }
            else
            {
                _filteredProducts = _allProducts.Where(p => 
                    p.ProductCode.ToLower().Contains(searchText) ||
                    p.Name.ToLower().Contains(searchText) ||
                    p.Category.ToLower().Contains(searchText)
                ).ToList();
            }
            
            LoadProducts();
        }

        private void ItemSelectionDialog_KeyDown(object? sender, KeyEventArgs e)
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

        private void DgvProducts_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(null, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void DgvProducts_DoubleClick(object? sender, EventArgs e)
        {
            BtnOK_Click(null, EventArgs.Empty);
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                SelectedProducts = dgvProducts.SelectedRows.Cast<DataGridViewRow>()
                    .Select(row => row.DataBoundItem as ProductListDto)
                    .Where(product => product != null)
                    .ToList()!;
                
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please select at least one product.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
