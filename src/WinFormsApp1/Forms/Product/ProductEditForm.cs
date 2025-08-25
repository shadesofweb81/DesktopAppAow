using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Product
{
    public partial class ProductEditForm : Form
    {
        private readonly ProductService _productService;
        private readonly ProductModel? _product;
        private readonly bool _isEditMode;
        private readonly Guid _companyId;

        // Public properties for MDI child form management
        public bool IsEditMode => _isEditMode;
        public string ProductId => _product?.Id ?? string.Empty;

        private TextBox txtProductCode = null!;
        private TextBox txtName = null!;
        private TextBox txtCategory = null!;
        private TextBox txtDescription = null!;
        private TextBox txtUnit = null!;
        private TextBox txtSKU = null!;
        private TextBox txtPurchasePrice = null!;
        private TextBox txtSellingPrice = null!;
        private TextBox txtStockQuantity = null!;
        private TextBox txtReorderLevel = null!;
        private TextBox txtBarcode = null!;
        private CheckBox chkIsActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblStatus = null!;
        private Label lblInstructions = null!;

        public ProductEditForm(ProductService productService, ProductModel? product, Guid companyId)
        {
            _productService = productService;
            _product = product;
            _isEditMode = product != null;
            _companyId = companyId;
            
            InitializeComponent();
            SetupForm();
            LoadProductDataAsync();
        }

        private void InitializeComponent()
        {
            txtProductCode = new TextBox();
            txtName = new TextBox();
            txtCategory = new TextBox();
            txtDescription = new TextBox();
            txtUnit = new TextBox();
            txtSKU = new TextBox();
            txtPurchasePrice = new TextBox();
            txtSellingPrice = new TextBox();
            txtStockQuantity = new TextBox();
            txtReorderLevel = new TextBox();
            txtBarcode = new TextBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            lblInstructions = new Label();
            SuspendLayout();

            // 
            // lblInstructions
            // 
            lblInstructions.Location = new Point(12, 9);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(500, 25);
            lblInstructions.Text = "Keyboard Navigation: Tab/Shift+Tab to navigate, Enter to save, Esc to cancel";
            lblInstructions.ForeColor = Color.Blue;
            lblInstructions.Font = new Font("Arial", 9, FontStyle.Regular);

            // Product Code
            var lblProductCode = new Label();
            lblProductCode.Location = new Point(12, 45);
            lblProductCode.Size = new Size(80, 20);
            lblProductCode.Text = "&Code:";
            lblProductCode.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblProductCode);

            txtProductCode.Location = new Point(100, 42);
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Size = new Size(150, 23);
            txtProductCode.TabIndex = 0;

            // Product Name
            var lblName = new Label();
            lblName.Location = new Point(12, 75);
            lblName.Size = new Size(80, 20);
            lblName.Text = "&Name:";
            lblName.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblName);

            txtName.Location = new Point(100, 72);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 23);
            txtName.TabIndex = 1;

            // Category
            var lblCategory = new Label();
            lblCategory.Location = new Point(12, 105);
            lblCategory.Size = new Size(80, 20);
            lblCategory.Text = "&Category:";
            lblCategory.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblCategory);

            txtCategory.Location = new Point(100, 102);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(150, 23);
            txtCategory.TabIndex = 2;

            // Description
            var lblDescription = new Label();
            lblDescription.Location = new Point(12, 135);
            lblDescription.Size = new Size(80, 20);
            lblDescription.Text = "&Description:";
            lblDescription.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblDescription);

            txtDescription.Location = new Point(100, 132);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(350, 23);
            txtDescription.TabIndex = 3;

            // Unit
            var lblUnit = new Label();
            lblUnit.Location = new Point(12, 165);
            lblUnit.Size = new Size(80, 20);
            lblUnit.Text = "&Unit:";
            lblUnit.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblUnit);

            txtUnit.Location = new Point(100, 162);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(100, 23);
            txtUnit.TabIndex = 4;

            // SKU
            var lblSKU = new Label();
            lblSKU.Location = new Point(210, 165);
            lblSKU.Size = new Size(40, 20);
            lblSKU.Text = "&SKU:";
            lblSKU.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblSKU);

            txtSKU.Location = new Point(258, 162);
            txtSKU.Name = "txtSKU";
            txtSKU.Size = new Size(120, 23);
            txtSKU.TabIndex = 5;

            // Purchase Price
            var lblPurchasePrice = new Label();
            lblPurchasePrice.Location = new Point(12, 195);
            lblPurchasePrice.Size = new Size(80, 20);
            lblPurchasePrice.Text = "&Purchase Price:";
            lblPurchasePrice.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblPurchasePrice);

            txtPurchasePrice.Location = new Point(100, 192);
            txtPurchasePrice.Name = "txtPurchasePrice";
            txtPurchasePrice.Size = new Size(120, 23);
            txtPurchasePrice.TabIndex = 6;

            // Selling Price
            var lblSellingPrice = new Label();
            lblSellingPrice.Location = new Point(230, 195);
            lblSellingPrice.Size = new Size(80, 20);
            lblSellingPrice.Text = "&Selling Price:";
            lblSellingPrice.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblSellingPrice);

            txtSellingPrice.Location = new Point(318, 192);
            txtSellingPrice.Name = "txtSellingPrice";
            txtSellingPrice.Size = new Size(120, 23);
            txtSellingPrice.TabIndex = 7;

            // Stock Quantity
            var lblStockQuantity = new Label();
            lblStockQuantity.Location = new Point(12, 225);
            lblStockQuantity.Size = new Size(80, 20);
            lblStockQuantity.Text = "&Stock Qty:";
            lblStockQuantity.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblStockQuantity);

            txtStockQuantity.Location = new Point(100, 222);
            txtStockQuantity.Name = "txtStockQuantity";
            txtStockQuantity.Size = new Size(100, 23);
            txtStockQuantity.TabIndex = 8;

            // Reorder Level
            var lblReorderLevel = new Label();
            lblReorderLevel.Location = new Point(210, 225);
            lblReorderLevel.Size = new Size(80, 20);
            lblReorderLevel.Text = "&Reorder Level:";
            lblReorderLevel.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblReorderLevel);

            txtReorderLevel.Location = new Point(298, 222);
            txtReorderLevel.Name = "txtReorderLevel";
            txtReorderLevel.Size = new Size(100, 23);
            txtReorderLevel.TabIndex = 9;

            // Barcode
            var lblBarcode = new Label();
            lblBarcode.Location = new Point(12, 255);
            lblBarcode.Size = new Size(80, 20);
            lblBarcode.Text = "&Barcode:";
            lblBarcode.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lblBarcode);

            txtBarcode.Location = new Point(100, 252);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(200, 23);
            txtBarcode.TabIndex = 10;

            // Is Active
            chkIsActive.Location = new Point(100, 285);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(120, 24);
            chkIsActive.TabIndex = 11;
            chkIsActive.Text = "Is &Active";
            chkIsActive.UseVisualStyleBackColor = true;
            chkIsActive.Checked = true;

            // Save Button
            btnSave.Location = new Point(200, 320);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 12;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            // Cancel Button
            btnCancel.Location = new Point(310, 320);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // Status Label
            lblStatus.Location = new Point(12, 360);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(450, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // ProductEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 390);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(txtBarcode);
            Controls.Add(txtReorderLevel);
            Controls.Add(txtStockQuantity);
            Controls.Add(txtSellingPrice);
            Controls.Add(txtPurchasePrice);
            Controls.Add(txtSKU);
            Controls.Add(txtUnit);
            Controls.Add(txtDescription);
            Controls.Add(txtCategory);
            Controls.Add(txtName);
            Controls.Add(txtProductCode);
            Controls.Add(lblInstructions);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "ProductEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _isEditMode ? "Edit Product" : "New Product";
            WindowState = FormWindowState.Maximized;
            KeyDown += new KeyEventHandler(ProductEditForm_KeyDown);
            Load += new EventHandler(ProductEditForm_Load);
            Resize += new EventHandler(ProductEditForm_Resize);
            Activated += new EventHandler(ProductEditForm_Activated);
            FormClosing += new FormClosingEventHandler(ProductEditForm_FormClosing);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupForm()
        {
            // Set default button
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            
            // Focus on product code field
            txtProductCode.Focus();
        }

        private void ProductEditForm_Load(object? sender, EventArgs e)
        {
            // Check if this is an MDI child form or a dialog
            if (MdiParent != null)
            {
                // This is an MDI child form - maximize it
                WindowState = FormWindowState.Maximized;
                
                // Hide MDI navigation panel when this form is maximized
                if (MdiParent is MainMDIForm mdiForm)
                {
                    mdiForm.HideNavigationPanel();
                }
            }
            else
            {
                // This is a dialog - center it and make it a reasonable size
                WindowState = FormWindowState.Normal;
                StartPosition = FormStartPosition.CenterParent;
                Size = new Size(800, 600);
            }
            
            // Resize controls to fit the form
            ResizeControls();
            
            // Focus on product code field
            txtProductCode.Focus();
        }

        private void ResizeControls()
        {
            // Get the client area size
            int clientWidth = ClientSize.Width;
            int clientHeight = ClientSize.Height;
            
            // Use appropriate spacing for normal child form
            int availableWidth = clientWidth - 50;
            int availableHeight = clientHeight - 100;
            
            // Reposition and resize controls for better layout
            // Instructions label
            lblInstructions.Size = new Size(availableWidth - 24, 25);
            
            // Status label at the bottom
            lblStatus.Location = new Point(12, clientHeight - 35);
            lblStatus.Size = new Size(clientWidth - 24, 20);
            
            // Buttons at the bottom
            btnSave.Location = new Point(availableWidth - 200, clientHeight - 80);
            btnCancel.Location = new Point(availableWidth - 100, clientHeight - 80);
        }

        private void ProductEditForm_Resize(object? sender, EventArgs e)
        {
            // Resize controls when form is resized
            ResizeControls();
        }

        private void ProductEditForm_Activated(object? sender, EventArgs e)
        {
            // When ProductEditForm is activated, ensure it's maximized and navigation is hidden
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            
            // Hide navigation panel when this form is activated
            if (MdiParent is MainMDIForm mdiForm)
            {
                mdiForm.HideNavigationPanel();
            }
        }

        private void ProductEditForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // When ProductEditForm is closing, check if any ProductForm is open
            if (MdiParent is MainMDIForm mdiForm)
            {
                // Check if any ProductForm is open among MDI children
                bool hasProductForm = false;
                foreach (Form childForm in mdiForm.MdiChildren)
                {
                    if (childForm is ProductForm)
                    {
                        hasProductForm = true;
                        break;
                    }
                }
                
                if (hasProductForm)
                {
                    // If ProductForm is open, don't show navigation panel
                    // Let the ProductForm handle its own state
                    return;
                }
                else
                {
                    // No ProductForm is open, show navigation panel
                    mdiForm.BeginInvoke(new Action(() =>
                    {
                        mdiForm.ShowNavigationPanel();
                        mdiForm.SetFocusToNavigation();
                    }));
                }
            }
        }

        private async void LoadProductDataAsync()
        {
            if (_isEditMode && _product != null)
            {
                try
                {
                    lblStatus.Text = "Loading product data...";
                    lblStatus.ForeColor = Color.Blue;
                    
                    // Load the product data into the form
                    LoadProductDataFromModel(_product);
                    
                    lblStatus.Text = "Product data loaded successfully";
                    lblStatus.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Error loading product data: {ex.Message}";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                // For new product, just clear the form
                ClearForm();
                lblStatus.Text = "Ready to create new product";
                lblStatus.ForeColor = Color.Green;
            }
        }

        private void LoadProductDataFromModel(ProductModel product)
        {
            txtProductCode.Text = product.ProductCode;
            txtName.Text = product.Name;
            txtCategory.Text = product.Category;
            txtDescription.Text = product.Description;
            txtUnit.Text = product.Unit;
            txtSKU.Text = product.SKU;
            txtPurchasePrice.Text = product.PurchasePrice.ToString();
            txtSellingPrice.Text = product.SellingPrice.ToString();
            txtStockQuantity.Text = product.StockQuantity.ToString();
            txtReorderLevel.Text = product.ReorderLevel?.ToString() ?? "";
            txtBarcode.Text = product.Barcode;
            chkIsActive.Checked = product.IsActive;
        }

        private void ClearForm()
        {
            txtProductCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtCategory.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtSKU.Text = string.Empty;
            txtPurchasePrice.Text = string.Empty;
            txtSellingPrice.Text = string.Empty;
            txtStockQuantity.Text = string.Empty;
            txtReorderLevel.Text = string.Empty;
            txtBarcode.Text = string.Empty;
            chkIsActive.Checked = true;
        }

        private void ProductEditForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    // Check if this is an MDI child form
                    if (MdiParent != null)
                    {
                        // This is an MDI child form - just close it
                        Close();
                    }
                    else
                    {
                        // This is a dialog - set result and close
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                    e.Handled = true;
                    break;
                    
                case Keys.Enter:
                    if (e.Control) // Ctrl+Enter to save
                    {
                        SaveProduct();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            await SaveProduct();
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            // Check if this is an MDI child form
            if (MdiParent != null)
            {
                // This is an MDI child form - just close it
                Close();
            }
            else
            {
                // This is a dialog - set result and close
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private async Task SaveProduct()
        {
            if (!ValidateForm())
                return;

            try
            {
                lblStatus.Text = _isEditMode ? "Updating product..." : "Creating product...";
                lblStatus.ForeColor = Color.Blue;
                
                var request = new ProductCreateRequest
                {
                    ProductCode = txtProductCode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Unit = txtUnit.Text.Trim(),
                    SKU = txtSKU.Text.Trim(),
                    PurchasePrice = decimal.TryParse(txtPurchasePrice.Text, out var purchasePrice) ? purchasePrice : 0,
                    SellingPrice = decimal.TryParse(txtSellingPrice.Text, out var sellingPrice) ? sellingPrice : 0,
                    StockQuantity = int.TryParse(txtStockQuantity.Text, out var stockQty) ? stockQty : 0,
                    ReorderLevel = int.TryParse(txtReorderLevel.Text, out var reorderLevel) ? reorderLevel : null,
                    Barcode = txtBarcode.Text.Trim(),
                    IsActive = chkIsActive.Checked,
                    CompanyId = _companyId
                };

                bool success = false;
                
                if (_isEditMode && _product != null)
                {
                    success = await _productService.UpdateProductAsync(Guid.Parse(_product.Id), request);
                }
                else
                {
                    var createdProduct = await _productService.CreateProductAsync(request);
                    success = createdProduct != null;
                }

                if (success)
                {
                    lblStatus.Text = _isEditMode ? "Product updated successfully!" : "Product created successfully!";
                    lblStatus.ForeColor = Color.Green;
                    
                    // Check if this is an MDI child form
                    if (MdiParent != null)
                    {
                        // This is an MDI child form - stay open and show success message
                        // The parent form will handle refreshing the list
                        await Task.Delay(2000); // Show success message for 2 seconds
                        lblStatus.Text = "Ready";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        // This is a dialog - close after a short delay
                        await Task.Delay(1000);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    lblStatus.Text = _isEditMode ? "Failed to update product" : "Failed to create product";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProductCode.Text))
            {
                MessageBox.Show("Product code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductCode.Focus();
                return false;
            }

            // Validate numeric fields
            if (!decimal.TryParse(txtPurchasePrice.Text, out _))
            {
                MessageBox.Show("Please enter a valid purchase price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPurchasePrice.Focus();
                return false;
            }

            if (!decimal.TryParse(txtSellingPrice.Text, out _))
            {
                MessageBox.Show("Please enter a valid selling price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSellingPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtStockQuantity.Text, out _))
            {
                MessageBox.Show("Please enter a valid stock quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockQuantity.Focus();
                return false;
            }

            return true;
        }
    }
}
