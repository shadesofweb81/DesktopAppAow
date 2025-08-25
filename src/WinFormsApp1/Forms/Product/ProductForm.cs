using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Product
{
    public partial class ProductForm : Form
    {
        private readonly ProductService _productService;
        private readonly LocalStorageService _localStorageService;
        private WinFormsApp1.Models.Company? _company;
        private DataGridView dgvProducts = null!;
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
            dgvProducts = new DataGridView();
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
            lblInstructions.Text = "Keyboard Navigation: ↑↓ to navigate rows, Enter to edit, V to view details, Insert for new, Delete to remove, F5 to refresh, Esc to close | Uses selected company from local storage";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);
            
            // 
            // dgvProducts
            // 
            dgvProducts.Location = new Point(12, 85);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Size = new Size(800, 350);
            dgvProducts.TabIndex = 0;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionChanged += new EventHandler(dgvProducts_SelectionChanged);
            dgvProducts.DoubleClick += new EventHandler(dgvProducts_DoubleClick);
            dgvProducts.KeyDown += new KeyEventHandler(dgvProducts_KeyDown);
            
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
            ClientSize = new Size(800, 600);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
            Controls.Add(btnEdit);
            Controls.Add(btnNew);
            Controls.Add(dgvProducts);
            Controls.Add(lblInstructions);
            Controls.Add(lblCompanyInfo);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            CancelButton = null; // Ensure no default cancel button interferes
            Name = "ProductForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _company != null ? $"Products - {_company.DisplayName}" : "Products - No Company Selected";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(ProductForm_KeyDown);
            Load += new EventHandler(ProductForm_Load);
            Resize += new EventHandler(ProductForm_Resize);
            Activated += new EventHandler(ProductForm_Activated);
            FormClosing += new FormClosingEventHandler(ProductForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnEdit;
            CancelButton = null; // Remove default cancel button to prevent conflicts
            
            // Focus on product grid
            dgvProducts.Focus();
        }

        private void ProductForm_Load(object? sender, EventArgs e)
        {
            // Open as maximized child form within MDI parent
            WindowState = FormWindowState.Maximized;
            
            // Resize controls to fit the maximized form
            ResizeControls();
            
            // Focus on the data grid
            dgvProducts.Focus();
            
            // Hide MDI navigation panel when this form is maximized
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
            // Ensure the form stays maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void ResizeControls()
        {
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            // Reserve space for buttons on the right side
            int buttonAreaWidth = 150;
            int availableWidth = clientWidth - buttonAreaWidth - 30; // 30px margin
            int availableHeight = clientHeight - 150;
            
            // Ensure minimum grid width
            if (availableWidth < 600)
            {
                availableWidth = 600;
            }
            
            // Resize the data grid to use most of the available space
            dgvProducts.Size = new Size(availableWidth, availableHeight);
            
            // Reposition buttons on the right side with 30px margin
            int buttonX = availableWidth + 30;
            btnNew.Location = new Point(buttonX, 85);
            btnEdit.Location = new Point(buttonX, 125);
            btnView.Location = new Point(buttonX, 165);
            btnDelete.Location = new Point(buttonX, 205);
            btnRefresh.Location = new Point(buttonX, 245);
            
            // Reposition status label at the bottom
            lblStatus.Location = new Point(12, clientHeight - 30);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            // Resize company info and instructions labels
            lblCompanyInfo.Size = new Size(clientWidth - 24, 25);
            lblInstructions.Size = new Size(clientWidth - 24, 40);
        }

        private void ProductForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void ProductForm_Activated(object? sender, EventArgs e)
        {
            // When ProductForm is activated, ensure it's maximized and navigation is hidden
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            // Hide navigation panel when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
            
            // Ensure the form takes focus and maintains its state
            this.BringToFront();
            this.Activate();
            
            // Force the form to stay maximized
            this.BeginInvoke(new Action(() =>
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }));
        }

        private void ProductForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When ProductForm is closing, ensure navigation panel is shown again
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.BeginInvoke(new Action(() =>
                {
                    mdiForm.ShowNavigationPanel();
                    mdiForm.SetFocusToNavigation();
                }));
            }
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
                
                // Validate company ID
                if (string.IsNullOrWhiteSpace(_company.Id))
                {
                    lblStatus.Text = "Invalid company data. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company data";
                    Text = "Products - Invalid Company Data";
                    UpdateButtonStates();
                    return;
                }
                
                // Try to parse the company ID as GUID
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format. Please select a company again.";
                    lblStatus.ForeColor = Color.Red;
                    lblCompanyInfo.Text = "Invalid company ID format";
                    Text = "Products - Invalid Company ID";
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
                
                // Validate company ID
                if (string.IsNullOrWhiteSpace(_company.Id))
                {
                    lblStatus.Text = "Invalid company data";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                // Try to parse the company ID as GUID
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                lblStatus.Text = "Loading products...";
                lblStatus.ForeColor = Color.Blue;
                
                var products = await _productService.GetProductsByCompanyAsync(companyId);
                
                _products = products;
                
                // Configure DataGridView columns
                dgvProducts.Columns.Clear();
                dgvProducts.Columns.Add("ProductCode", "Product Code");
                dgvProducts.Columns.Add("Name", "Product Name");
                dgvProducts.Columns.Add("Category", "Category");
                dgvProducts.Columns.Add("SKU", "SKU");
                dgvProducts.Columns.Add("Unit", "Unit");
                dgvProducts.Columns.Add("PurchasePrice", "Purchase Price");
                dgvProducts.Columns.Add("SellingPrice", "Selling Price");
                dgvProducts.Columns.Add("StockQuantity", "Stock Qty");
                dgvProducts.Columns.Add("IsActive", "Active");
                
                // Set column widths
                if (dgvProducts.Columns["ProductCode"] != null) dgvProducts.Columns["ProductCode"].Width = 100;
                if (dgvProducts.Columns["Name"] != null) dgvProducts.Columns["Name"].Width = 200;
                if (dgvProducts.Columns["Category"] != null) dgvProducts.Columns["Category"].Width = 120;
                if (dgvProducts.Columns["SKU"] != null) dgvProducts.Columns["SKU"].Width = 100;
                if (dgvProducts.Columns["Unit"] != null) dgvProducts.Columns["Unit"].Width = 80;
                if (dgvProducts.Columns["PurchasePrice"] != null) dgvProducts.Columns["PurchasePrice"].Width = 100;
                if (dgvProducts.Columns["SellingPrice"] != null) dgvProducts.Columns["SellingPrice"].Width = 100;
                if (dgvProducts.Columns["StockQuantity"] != null) dgvProducts.Columns["StockQuantity"].Width = 80;
                if (dgvProducts.Columns["IsActive"] != null) dgvProducts.Columns["IsActive"].Width = 60;
                
                // Populate data
                foreach (var product in _products)
                {
                    dgvProducts.Rows.Add(
                        product.ProductCode ?? "",
                        product.Name ?? "",
                        product.Category ?? "",
                        product.SKU ?? "",
                        product.Unit ?? "",
                        product.PurchasePrice.ToString("C"),
                        product.SellingPrice.ToString("C"),
                        product.StockQuantity.ToString(),
                        product.IsActive ? "Yes" : "No"
                    );
                }
                
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

        private void dgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.Index >= 0 && dgvProducts.CurrentRow.Index < _products.Count)
            {
                _selectedProduct = _products[dgvProducts.CurrentRow.Index];
            }
            else
            {
                _selectedProduct = null;
            }
            UpdateButtonStates();
        }

        private void dgvProducts_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedProduct != null)
            {
                EditProduct();
            }
        }

        private void dgvProducts_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"dgvProducts_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
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
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadProducts());
                    e.Handled = true;
                    break;
                    
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in dgvProducts_KeyDown");
                    // When closing with Escape, ensure navigation panel is shown
                    if (MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.BeginInvoke(new Action(() =>
                        {
                            mdiForm.ShowNavigationPanel();
                            mdiForm.SetFocusToNavigation();
                        }));
                    }
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private void ProductForm_KeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"ProductForm_KeyDown: KeyCode={e.KeyCode}, KeyData={e.KeyData}, Alt={e.Alt}, Control={e.Control}, Shift={e.Shift}");
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Console.WriteLine("Escape key pressed in ProductForm_KeyDown");
                    // When closing with Escape, ensure navigation panel is shown
                    if (MdiParent is MainMDIForm mdiForm)
                    {
                        mdiForm.BeginInvoke(new Action(() =>
                        {
                            mdiForm.ShowNavigationPanel();
                            mdiForm.SetFocusToNavigation();
                        }));
                    }
                    Close();
                    e.Handled = true;
                    break;
                    
                case Keys.F5:
                    _ = Task.Run(async () => await LoadProducts());
                    e.Handled = true;
                    break;
                    
                case Keys.Insert:
                    NewProduct();
                    e.Handled = true;
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
            
            // Check if ProductEditForm is already open
            foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
            {
                if (childForm is ProductEditForm editForm && !editForm.IsEditMode)
                {
                    childForm.BringToFront();
                    childForm.Activate();
                    return;
                }
            }

            // Validate company ID
            if (!Guid.TryParse(_company.Id, out Guid companyId))
            {
                MessageBox.Show("Invalid company ID format. Please select a company again.", "Invalid Company", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Create new product edit form as MDI child
            var productEditForm = new ProductEditForm(_productService, null, companyId)
            {
                MdiParent = this.MdiParent,
                Text = "New Product",
                WindowState = FormWindowState.Maximized
            };

            productEditForm.Show();
            
            // Refresh the product list when the edit form is closed
            productEditForm.FormClosed += (s, e) => LoadProducts();
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
                
                // Check if ProductEditForm is already open for this product
                foreach (Form childForm in this.MdiParent?.MdiChildren ?? new Form[0])
                {
                    if (childForm is ProductEditForm editForm && editForm.IsEditMode && editForm.ProductId == _selectedProduct.Id)
                    {
                        childForm.BringToFront();
                        childForm.Activate();
                        return;
                    }
                }

                // Validate company ID
                if (!Guid.TryParse(_company.Id, out Guid companyId))
                {
                    MessageBox.Show("Invalid company ID format. Please select a company again.", "Invalid Company", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Create new product edit form as MDI child
                var productEditForm = new ProductEditForm(_productService, _selectedProduct, companyId)
                {
                    MdiParent = this.MdiParent,
                    Text = $"Edit Product - {_selectedProduct.DisplayName}",
                    WindowState = FormWindowState.Maximized
                };

                productEditForm.Show();
                
                // Refresh the product list when the edit form is closed
                productEditForm.FormClosed += (s, e) => LoadProducts();
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
                        
                        // Validate product ID
                        if (!Guid.TryParse(_selectedProduct.Id, out Guid productId))
                        {
                            lblStatus.Text = "Invalid product ID format";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        
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
