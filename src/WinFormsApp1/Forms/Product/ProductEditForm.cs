using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1.Forms.Product
{
    public partial class ProductEditForm : BaseForm
    {
        private readonly ProductService _productService;
        private readonly AttributeService _attributeService;
        private readonly ProductModel? _product;
        private readonly bool _isEditMode;
        private readonly string _companyId;
        private List<AttributeModel> _availableAttributes = new List<AttributeModel>();
        private List<string> _productAttributeIds = new List<string>(); // Store product's associated attribute IDs

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
        private GroupBox grpAttributes = null!;
        private FlowLayoutPanel flpAttributes = null!;
        private Button btnLoadAttributes = null!;

        public ProductEditForm(ProductService productService, AttributeService attributeService, ProductModel? product, string companyId)
        {
            _productService = productService;
            _attributeService = attributeService;
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
            grpAttributes = new GroupBox();
            flpAttributes = new FlowLayoutPanel();
            btnLoadAttributes = new Button();
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

            // Load Attributes Button (hidden since attributes load automatically)
            btnLoadAttributes.Location = new Point(100, 320);
            btnLoadAttributes.Name = "btnLoadAttributes";
            btnLoadAttributes.Size = new Size(150, 30);
            btnLoadAttributes.TabIndex = 12;
            btnLoadAttributes.Text = "&Load Attributes";
            btnLoadAttributes.UseVisualStyleBackColor = true;
            btnLoadAttributes.Visible = false; // Hide the button since attributes load automatically
            btnLoadAttributes.Click += new EventHandler(btnLoadAttributes_Click);

            // Attributes Group Box
            grpAttributes.Location = new Point(12, 360);
            grpAttributes.Name = "grpAttributes";
            grpAttributes.Size = new Size(800, 400);
            grpAttributes.TabIndex = 13;
            grpAttributes.Text = "Product Attributes";
            grpAttributes.Visible = false;

            // Attributes Flow Layout Panel
            flpAttributes.Location = new Point(10, 20);
            flpAttributes.Name = "flpAttributes";
            flpAttributes.Size = new Size(780, 370);
            flpAttributes.TabIndex = 0;
            flpAttributes.AutoScroll = true;
            flpAttributes.FlowDirection = FlowDirection.TopDown;
            flpAttributes.WrapContents = false;

            // Save Button
            btnSave.Location = new Point(200, 580);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 14;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new EventHandler(btnSave_Click);

            // Cancel Button
            btnCancel.Location = new Point(310, 580);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 15;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);

            // Status Label
            lblStatus.Location = new Point(12, 620);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(450, 20);
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Green;

            // 
            // ProductEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 650);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpAttributes);
            Controls.Add(btnLoadAttributes);
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
            
            // Add flow layout panel to group box
            grpAttributes.Controls.Add(flpAttributes);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MaximizeBox = true;
            MinimizeBox = true;
            Name = "ProductEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = _isEditMode ? "Edit Product" : "New Product";
            WindowState = FormWindowState.Maximized;
            // KeyDown is handled by BaseForm
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
            
            // Focus on product code field - BaseForm will handle this automatically
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
            
            // Adjust attributes group box position since load button is hidden
            if (grpAttributes.Visible)
            {
                grpAttributes.Location = new Point(12, 360); // Keep original position
            }
            
            // Resize attributes group box
            if (grpAttributes.Visible)
            {
                // Calculate dynamic height based on number of attributes with smaller panels
                int attributeCount = flpAttributes.Controls.Count;
                int panelHeight = 120; // Smaller height per panel
                int margin = 10; // 5px top + 5px bottom margin
                int totalAttributeHeight = attributeCount * (panelHeight + margin);
                
                // Double the group box height and add some padding
                int groupBoxHeight = Math.Min(Math.Max(totalAttributeHeight * 2 + 80, 800), availableHeight - 200);
                int groupBoxWidth = Math.Min(availableWidth - 24, availableWidth - 24); // Use full available width
                grpAttributes.Size = new Size(groupBoxWidth, groupBoxHeight);
                flpAttributes.Size = new Size(grpAttributes.Width - 20, grpAttributes.Height - 30);
            }
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
                    
                    // Fetch fresh product data from the server
                    var freshProduct = await _productService.GetProductByIdAsync(_product.Id);
                    
                    if (freshProduct != null)
                    {
                        // Load the fresh product data into the form
                        LoadProductDataFromModel(freshProduct);
                        
                        lblStatus.Text = "Product data loaded successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        // Fallback to the original product data if server fetch fails
                        LoadProductDataFromModel(_product);
                        
                        lblStatus.Text = "Product data loaded (using cached data)";
                        lblStatus.ForeColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    // Fallback to the original product data if there's an error
                    LoadProductDataFromModel(_product);
                    
                    lblStatus.Text = $"Error loading product data: {ex.Message} (using cached data)";
                    lblStatus.ForeColor = Color.Orange;
                }
            }
            else
            {
                // For new product, just clear the form
                ClearForm();
                lblStatus.Text = "Ready to create new product";
                lblStatus.ForeColor = Color.Green;
            }
            
            // Load attributes automatically after product data is loaded
            await LoadAttributesAsync();
        }

        private async Task LoadAttributesAsync()
        {
            try
            {
                lblStatus.Text = "Loading attributes...";
                lblStatus.ForeColor = Color.Blue;
                
                // Parse company ID
                if (!Guid.TryParse(_companyId, out Guid companyId))
                {
                    lblStatus.Text = "Invalid company ID format";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                // Load attributes from the server
                _availableAttributes = await _attributeService.GetAttributesByCompanyAsync(companyId);
                
                if (_availableAttributes.Any())
                {
                    // Log the product's associated attribute IDs for debugging
                    Console.WriteLine($"Product has {_productAttributeIds.Count} associated attributes:");
                    foreach (var attrId in _productAttributeIds)
                    {
                        Console.WriteLine($"  - Attribute ID: {attrId}");
                    }
                    
                    Console.WriteLine($"Available attributes ({_availableAttributes.Count}):");
                    foreach (var attr in _availableAttributes)
                    {
                        Console.WriteLine($"  - {attr.Name} (ID: {attr.Id})");
                    }
                    
                    // Populate the attribute selection controls
                    PopulateAttributeControls();
                    
                    // Show the attributes group box
                    grpAttributes.Visible = true;
                    
                    lblStatus.Text = $"Loaded {_availableAttributes.Count} attributes";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "No attributes found for this company";
                    lblStatus.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error loading attributes: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void PopulateAttributeControls()
        {
            // Clear existing controls
            flpAttributes.Controls.Clear();
            
            // Get the available width from the flow layout panel
            int availableWidth = flpAttributes.Width - 20; // Account for margins
            
            foreach (var attribute in _availableAttributes.Where(a => a.IsActive))
            {
                // Create a panel for each attribute with smaller size
                var attributePanel = new Panel
                {
                    Width = availableWidth, // Use full available width
                    Height = 120, // Smaller height
                    Margin = new Padding(5, 5, 5, 5), // Smaller margin
                    BorderStyle = BorderStyle.FixedSingle
                };
                
                // Create checkbox for the attribute
                var chkAttribute = new CheckBox
                {
                    Text = attribute.Name,
                    Location = new Point(10, 10),
                    Width = 200,
                    Font = new Font(Font.FontFamily, 9, FontStyle.Bold),
                    Tag = attribute, // Store the attribute object in the tag
                    Checked = _productAttributeIds.Contains(attribute.Id) // Pre-select if this attribute is associated with the product
                };
                
                // Create label for description
                var lblDescription = new Label
                {
                    Text = attribute.Description,
                    Location = new Point(10, 30),
                    Width = availableWidth - 20, // Use full width minus margins
                    ForeColor = Color.Gray,
                    Font = new Font(Font.FontFamily, 8)
                };
                
                // Add controls to panel
                attributePanel.Controls.Add(chkAttribute);
                attributePanel.Controls.Add(lblDescription);
                
                // Add attribute options if they exist
                if (attribute.AttributeOptions?.Any() == true)
                {
                    var lblOptions = new Label
                    {
                        Text = "Options:",
                        Location = new Point(10, 50),
                        Width = 60,
                        ForeColor = Color.DarkBlue,
                        Font = new Font(Font.FontFamily, 8, FontStyle.Bold)
                    };
                    attributePanel.Controls.Add(lblOptions);
                    
                    // Create a horizontal flow layout panel for options
                    var optionsFlowPanel = new FlowLayoutPanel
                    {
                        Location = new Point(70, 50),
                        Width = availableWidth - 80, // Use full width minus label and margins
                        Height = 60, // Smaller height
                        AutoScroll = true,
                        FlowDirection = FlowDirection.LeftToRight,
                        WrapContents = true, // Allow wrapping to next line
                        Margin = new Padding(0, 0, 0, 0) // No margin to prevent clipping
                    };
                    
                    // Add each option as a label in horizontal layout
                    foreach (var option in attribute.AttributeOptions.Where(o => o.IsActive))
                    {
                        var lblOption = new Label
                        {
                            Text = $"{option.DisplayName}",
                            AutoSize = true,
                            ForeColor = Color.DarkGreen,
                            Font = new Font(Font.FontFamily, 8),
                            Margin = new Padding(3, 2, 3, 2),
                            Padding = new Padding(2, 1, 2, 1)
                        };
                        optionsFlowPanel.Controls.Add(lblOption);
                    }
                    
                    attributePanel.Controls.Add(optionsFlowPanel);
                }
                
                // Add panel to flow layout
                flpAttributes.Controls.Add(attributePanel);
            }
        }

        private List<string> GetSelectedAttributeIds()
        {
            var selectedAttributeIds = new List<string>();
            
            foreach (Control control in flpAttributes.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is CheckBox chk && chk.Checked && chk.Tag is AttributeModel attribute)
                        {
                            selectedAttributeIds.Add(attribute.Id);
                        }
                    }
                }
            }
            
            return selectedAttributeIds;
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
            
            // Store the product's associated attribute IDs
            _productAttributeIds = product.AttributeIds ?? new List<string>();
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

        protected override bool HandleEnterKey()
        {
            // Ctrl+Enter to save
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                SaveProduct();
                return true;
            }
            
            // Enter on save button
            if (ActiveControl == btnSave)
            {
                SaveProduct();
                return true;
            }
            
            // Enter on cancel button
            if (ActiveControl == btnCancel)
            {
                Close();
                return true;
            }
            
            return false; // Let BaseForm handle navigation
        }

        protected override void HandleEscapeKey()
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

        protected override void ShowHelp()
        {
            var helpMessage = @"Product Edit Form - Keyboard Navigation Help:

Navigation:
• Tab - Move to next field
• Backspace - Move to previous field
• Enter - Confirm action or move to next field
• Ctrl+Enter - Save product
• Escape - Close form or go back
• F1 - Show this help

Field Order:
1. Product Code
2. Product Name (required)
3. Category
4. Description
5. Unit
6. SKU
7. Purchase Price
8. Selling Price
9. Stock Quantity
10. Reorder Level
11. Barcode
12. Is Active (checkbox)
13. Product Attributes (if available)
14. Save Button
15. Cancel Button

Tips:
• Use Tab/Backspace for fast field navigation
• Ctrl+Enter to save from any field
• Escape to cancel and close
• All fields are highlighted when focused
• Product attributes load automatically";

            MessageBox.Show(helpMessage, "Product Edit Form Help", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnLoadAttributes_Click(object? sender, EventArgs e)
        {
            await LoadAttributesAsync();
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
                    CompanyId = _companyId,
                    AttributeIds = GetSelectedAttributeIds()
                };

                bool success = false;
                
                if (_isEditMode && _product != null)
                {
                    // Update the product model with the form data
                    _product.Name = request.Name;
                    _product.ProductCode = request.ProductCode;
                    _product.Category = request.Category;
                    _product.Description = request.Description;
                    _product.Unit = request.Unit;
                    _product.SKU = request.SKU;
                    _product.PurchasePrice = request.PurchasePrice;
                    _product.SellingPrice = request.SellingPrice;
                    _product.StockQuantity = request.StockQuantity;
                    _product.ReorderLevel = request.ReorderLevel;
                    _product.Barcode = request.Barcode;
                    _product.IsActive = request.IsActive;
                    _product.CompanyId = request.CompanyId;
                    _product.AttributeIds = request.AttributeIds;
                    
                    success = await _productService.UpdateProductAsync(_product.Id, _product);
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

            //if (string.IsNullOrWhiteSpace(txtProductCode.Text))
            //{
            //    MessageBox.Show("Product code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtProductCode.Focus();
            //    return false;
            //}

          
            return true;
        }
    }
}


