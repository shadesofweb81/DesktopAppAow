using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Product
{
    public partial class ProductForm : Form
    {
        private readonly ProductService _productService;
        private readonly LocalStorageService _localStorageService;
        private WinFormsApp1.Models.Company? _company;
        private ListBox lstProducts = null!;
        private Button btnNew = null!;
        private Button btnEdit = null!;
        private Button btnView = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;
        private Label lblCompanyInfo = null!;
        private List<ProductModel> _products = new List<ProductModel>();
        private ProductModel? _selectedProduct;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount = 0;
        private int _totalPages = 0;

        public ProductForm(ProductService productService)
        {
            _productService = productService;
            _localStorageService = new LocalStorageService();
            InitializeComponent();
            SetupForm();
            LoadCompanyAndProducts();
        }

        private void InitializeComponent()
        {
            lstProducts = new ListBox();
            btnNew = new Button();
            btnEdit = new Button();
            btnView = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            lblCompanyInfo = new Label();
            SuspendLayout();
            
            // 
            // lblCompanyInfo
            // 
            lblCompanyInfo.Location = new Point(12, 9);
            lblCompanyInfo.Name = "lblCompanyInfo";
            lblCompanyInfo.Size = new Size(600, 25);
            lblCompanyInfo.Text = _company != null ? $"Products for: {_company.DisplayName}" : "No company selected";
            lblCompanyInfo.ForeColor = Color.DarkBlue;
            lblCompanyInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            
            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 40);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(600, 40);
            lblInstructions.Text = "Keyboard Navigation: ↑↓ to navigate, Enter to edit, V to view details, Insert for new, Delete to remove, F5 to refresh, Esc to close | Uses selected company from local storage";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // lstProducts
            // 
            lstProducts.Location = new Point(12, 85);
            lstProducts.Name = "lstProducts";
            lstProducts.Size = new Size(500, 350);
            lstProducts.TabIndex = 0;
            lstProducts.DisplayMember = "DisplayName";
            lstProducts.SelectedIndexChanged += new EventHandler(lstProducts_SelectedIndexChanged);
            lstProducts.DoubleClick += new EventHandler(lstProducts_DoubleClick);
            lstProducts.KeyDown += new KeyEventHandler(lstProducts_KeyDown);
            
            // 
            // btnNew
            // 
            btnNew.Location = new Point(530, 85);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(100, 30);
            btnNew.TabIndex = 1;
            btnNew.Text = "&New (Insert)";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += new EventHandler(btnNew_Click);
            
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(530, 125);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 30);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "&Edit (Enter)";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnEdit.Enabled = false;
            
            // 
            // btnView
            // 
            btnView.Location = new Point(530, 165);
            btnView.Name = "btnView";
            btnView.Size = new Size(100, 30);
            btnView.TabIndex = 3;
            btnView.Text = "&View (V)";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += new EventHandler(btnView_Click);
            btnView.Enabled = false;
            
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(530, 205);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "&Delete (Del)";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDelete.Enabled = false;
            
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(530, 245);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "&Refresh (F5)";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 450);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(600, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;
            
            // 
            // ProductForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 480);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(lstProducts);
            Controls.Add(lblInstructions);
            Controls.Add(lblCompanyInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProductForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _company != null ? $"Products - {_company.DisplayName}" : "Products - No Company Selected";
            KeyDown += new KeyEventHandler(ProductForm_KeyDown);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnEdit;
            CancelButton = btnRefresh;
            
            // Focus on product list
            lstProducts.Focus();
        }

        private async void LoadCompanyAndProducts()
        {
            try
            {
                // Load selected company from local storage
                _company = await _localStorageService.GetSelectedCompanyAsync();
                
                if (_company == null)
                {
                    lblStatus.Text = "No company selected. Please select a company first.";
                    lblStatus.ForeColor = Color.Orange;
                    lblCompanyInfo.Text = "No company selected";
                    Text = "Products - No Company Selected";
                    UpdateButtonStates();
                    return;
                }
                
                // Update UI with company info
                lblCompanyInfo.Text = $"Products for: {_company.DisplayName}";
                Text = $"Products - {_company.DisplayName}";
                
                // Load products
                await LoadProducts();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading company: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load company exception: {ex.Message}");
            }
        }

        private async Task LoadProducts()
        {
            try
            {
                if (_company == null)
                {
                    lblStatus.Text = "No company selected";
                    lblStatus.ForeColor = Color.Orange;
                    return;
                }
                
                lblStatus.Text = "Loading products...";
                lblStatus.ForeColor = Color.Blue;
                
                var companyId = Guid.Parse(_company.Id);
                var products = await _productService.GetProductsByCompanyAsync(companyId);
                
                _products = products;
                lstProducts.DataSource = null;
                lstProducts.DataSource = _products;
                lstProducts.DisplayMember = "DisplayName";
                
                lblStatus.Text = $"Loaded {_products.Count} products";
                lblStatus.ForeColor = Color.Green;
                
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading products: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                Console.WriteLine($"Load products exception: {ex.Message}");
            }
        }

        private void UpdateButtonStates()
        {
            bool hasCompany = _company != null;
            bool hasSelection = _selectedProduct != null;
            
            btnNew.Enabled = hasCompany;
            btnEdit.Enabled = hasCompany && hasSelection;
            btnView.Enabled = hasCompany && hasSelection;
            btnDelete.Enabled = hasCompany && hasSelection;
            btnRefresh.Enabled = hasCompany;
        }

        private void lstProducts_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _selectedProduct = lstProducts.SelectedItem as ProductModel;
            UpdateButtonStates();
        }

        private void lstProducts_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedProduct != null)
            {
                EditProduct();
            }
        }

        private void lstProducts_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_selectedProduct != null)
                    {
                        EditProduct();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.Insert:
                    NewProduct();
                    e.Handled = true;
                    break;
                    
                case Keys.Delete:
                    if (_selectedProduct != null)
                    {
                        DeleteProduct();
                        e.Handled = true;
                    }
                    break;
                    
                case Keys.V:
                    if (_selectedProduct != null)
                    {
                        ViewProduct();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void ProductForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    _ = Task.Run(async () => await LoadProducts());
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            NewProduct();
        }

        private async void btnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedProduct != null)
            {
                try
                {
                    await EditProduct();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing product: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnView_Click(object? sender, EventArgs e)
        {
            if (_selectedProduct != null)
            {
                try
                {
                    await ViewProduct();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error viewing product: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedProduct != null)
            {
                DeleteProduct();
            }
        }

        private async void btnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadProducts();
        }

        private void NewProduct()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // TODO: Implement ProductEditForm
            MessageBox.Show($"Create new product for company: {_company.DisplayName}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task EditProduct()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_selectedProduct != null)
            {
                Console.WriteLine($"EditProduct called for: ID='{_selectedProduct.Id}', Name='{_selectedProduct.Name}'");
                
                // TODO: Implement ProductEditForm
                MessageBox.Show($"Edit product: {_selectedProduct.Name} for company: {_company.DisplayName}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ViewProduct()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_selectedProduct != null)
            {
                Console.WriteLine($"ViewProduct called for: ID='{_selectedProduct.Id}', Name='{_selectedProduct.Name}'");
                
                // TODO: Implement ProductDetailsForm
                MessageBox.Show($"View product: {_selectedProduct.Name} for company: {_company.DisplayName}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void DeleteProduct()
        {
            if (_company == null)
            {
                MessageBox.Show("Please select a company first.", "No Company Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_selectedProduct != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the product '{_selectedProduct.Name}' from company '{_company.DisplayName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        lblStatus.Text = "Deleting product...";
                        lblStatus.ForeColor = Color.Blue;
                        
                        var productId = Guid.Parse(_selectedProduct.Id);
                        var success = await _productService.DeleteProductAsync(productId);
                        
                        if (success)
                        {
                            lblStatus.Text = "Product deleted successfully";
                            lblStatus.ForeColor = Color.Green;
                            await LoadProducts(); // Refresh the list
                        }
                        else
                        {
                            lblStatus.Text = "Failed to delete product";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = $"Error deleting product: {ex.Message}";
                        lblStatus.ForeColor = Color.Red;
                        Console.WriteLine($"Delete product exception: {ex.Message}");
                    }
                }
            }
        }
    }
}
