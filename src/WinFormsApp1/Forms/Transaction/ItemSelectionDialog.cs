using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Services;
using WinFormsApp1.Forms.Product;

namespace WinFormsApp1.Forms.Transaction
{
    public partial class ItemSelectionDialog : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvProducts = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;
        private Button btnAddNewProduct = null!;
        private List<ProductListDto> _allProducts;
        private List<ProductListDto> _filteredProducts;
        private readonly ProductService? _productService;
        private readonly AttributeService? _attributeService;
        private readonly string? _companyId;

        public List<ProductListDto> SelectedProducts { get; private set; } = new List<ProductListDto>();

        public ItemSelectionDialog(List<ProductListDto> products)
        {
            _allProducts = products;
            _filteredProducts = new List<ProductListDto>(products);
            _productService = null;
            _attributeService = null;
            _companyId = null;
            InitializeDialog();
        }

        public ItemSelectionDialog(List<ProductListDto> products, ProductService productService, AttributeService attributeService, string companyId)
        {
            _allProducts = products;
            _filteredProducts = new List<ProductListDto>(products);
            _productService = productService;
            _attributeService = attributeService;
            _companyId = companyId;
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
            btnAddNewProduct = new Button();

            SuspendLayout();

            // Search textbox
            txtSearch.Location = new Point(20, 20);
            txtSearch.Size = new Size(740, 25);
            txtSearch.PlaceholderText = "Type to search products...";
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Products grid
            dgvProducts.Location = new Point(20, 55);
            dgvProducts.Size = new Size(740, 440);
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.AutoGenerateColumns = false;
            
            SetupProductGrid();

            // Add New Product Button
            btnAddNewProduct.Location = new Point(20, 505);
            btnAddNewProduct.Size = new Size(120, 30);
            btnAddNewProduct.Text = "&Add New Product";
            btnAddNewProduct.UseVisualStyleBackColor = true;
            btnAddNewProduct.BackColor = Color.LightGreen;
            btnAddNewProduct.ForeColor = Color.DarkGreen;
            btnAddNewProduct.Enabled = _productService != null && _attributeService != null && !string.IsNullOrEmpty(_companyId);
            btnAddNewProduct.Click += BtnAddNewProduct_Click;

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

            Controls.AddRange(new Control[] { txtSearch, dgvProducts, btnAddNewProduct, btnOK, btnCancel });

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

        private async void BtnAddNewProduct_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_productService == null || _attributeService == null || string.IsNullOrEmpty(_companyId))
                {
                    MessageBox.Show("Product creation is not available. Missing required services.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine("Opening new product form...");
                
                // Create and show the product edit form as a modal dialog
                using var productForm = new ProductEditForm(_productService, _attributeService, null, _companyId);
                productForm.Text = "Add New Product";
                productForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                productForm.MaximizeBox = false;
                productForm.MinimizeBox = false;
                productForm.StartPosition = FormStartPosition.CenterParent;
                productForm.WindowState = FormWindowState.Normal;
                productForm.Size = new Size(820, 650); // Larger size to accommodate all fields
                productForm.MinimumSize = new Size(700, 500); // Set minimum size
                productForm.AutoScroll = true; // Enable scrolling if content exceeds visible area
                productForm.MaximumSize = new Size(1000, 800); // Set reasonable maximum size
                productForm.ShowInTaskbar = false; // Don't show in taskbar since it's a modal dialog
                
                var result = productForm.ShowDialog(this);
                
                if (result == DialogResult.OK)
                {
                    Console.WriteLine("Product was successfully created, refreshing product list...");
                    
                    // Show a temporary status message
                    Text = "Refreshing product list...";
                    
                    // Refresh the product list from the server
                    await RefreshProductList();
                    
                    // Restore original title with updated count
                    Text = $"Select Products - {_filteredProducts.Count} products available - New product added!";
                    
                    // Focus on the grid so user can see the updated list
                    dgvProducts.Focus();
                    
                    // Optionally try to find and select the newly created product
                    TrySelectNewlyCreatedProduct();
                }
                else
                {
                    Console.WriteLine("Product creation was cancelled");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BtnAddNewProduct_Click: {ex.Message}");
                MessageBox.Show($"Error opening new product form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RefreshProductList()
        {
            try
            {
                if (_productService == null || string.IsNullOrEmpty(_companyId))
                    return;

                Console.WriteLine("Refreshing product list from server...");
                
                // Parse company ID and fetch fresh product list
                if (Guid.TryParse(_companyId, out var companyId))
                {
                    var freshProducts = await _productService.GetProductsByCompanyAsync(companyId);
                    
                    if (freshProducts != null && freshProducts.Any())
                    {
                        // Convert to ProductListDto format
                        var productDtos = freshProducts.Select(p => new ProductListDto
                        {
                            Id = p.Id,
                            ProductCode = p.ProductCode,
                            Name = p.Name,
                            Category = p.Category,
                            Unit = p.Unit,
                            SellingPrice = p.SellingPrice,
                            StockQuantity = p.StockQuantity,
                            IsActive = p.IsActive
                        }).ToList();
                        
                        // Update the product lists
                        _allProducts = productDtos;
                        _filteredProducts = new List<ProductListDto>(_allProducts);
                        
                        // Apply current search filter if any
                        if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                        {
                            TxtSearch_TextChanged(null, EventArgs.Empty);
                        }
                        else
                        {
                            LoadProducts();
                        }
                        
                        Console.WriteLine($"Product list refreshed successfully with {_allProducts.Count} products");
                    }
                    else
                    {
                        Console.WriteLine("No products returned from server");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid company ID format: {_companyId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing product list: {ex.Message}");
                MessageBox.Show($"Error refreshing product list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TrySelectNewlyCreatedProduct()
        {
            try
            {
                // Try to find the most recently created product by looking for the one with the highest row index
                if (dgvProducts.Rows.Count > 0)
                {
                    // For now, just scroll to the bottom where new products are likely to appear
                    var lastRowIndex = dgvProducts.Rows.Count - 1;
                    dgvProducts.ClearSelection();
                    dgvProducts.Rows[lastRowIndex].Selected = true;
                    dgvProducts.CurrentCell = dgvProducts.Rows[lastRowIndex].Cells[1]; // Focus on the name column
                    dgvProducts.FirstDisplayedScrollingRowIndex = Math.Max(0, lastRowIndex - 5); // Scroll to show the selected row
                    
                    Console.WriteLine($"Auto-selected row {lastRowIndex + 1} (likely the new product)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-selecting newly created product: {ex.Message}");
                // Don't show error to user as this is a nice-to-have feature
            }
        }
    }
}
