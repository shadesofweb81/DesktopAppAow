using System.ComponentModel;

namespace WinFormsApp1.Forms
{
    /// <summary>
    /// Base form class that provides keyboard-based navigation functionality.
    /// Implements Tab for forward navigation and Backspace for backward navigation.
    /// </summary>
    public class BaseForm : Form
    {
        private List<Control> _navigableControls = new List<Control>();
        private int _currentControlIndex = 0;

        public BaseForm()
        {
            // Enable key preview for global key handling
            KeyPreview = true;
            
            // Set up the form
            SetupForm();
            
            // Add event handlers
            Load += BaseForm_Load;
            KeyDown += BaseForm_KeyDown;
        }

        private void SetupForm()
        {
            // Set default form properties for keyboard navigation
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void BaseForm_Load(object? sender, EventArgs e)
        {
            // Build the navigation list when form loads
            BuildNavigationList();
            
            // Focus the first control
            if (_navigableControls.Count > 0)
            {
                _navigableControls[0].Focus();
                _currentControlIndex = 0;
            }
        }

        private void BaseForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Allow arrow keys to work normally for DataGridView and other list controls
            if (ActiveControl is DataGridView || ActiveControl is ListBox || ActiveControl is ListView)
            {
                // Let arrow keys pass through for list controls
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    return; // Don't handle arrow keys for list controls
                }
            }

            switch (e.KeyCode)
            {
                case Keys.Tab:
                    if (!e.Shift)
                    {
                        // Tab - Move to next control
                        NavigateToNextControl();
                        e.Handled = true;
                    }
                    // Shift+Tab is handled by default Windows behavior
                    break;

                case Keys.Back:
                    if (!e.Control && !e.Alt)
                    {
                        // Check if the current control is a text input control that's being edited
                        if (IsTextInputControl(ActiveControl))
                        {
                            // For text input controls, check if there are characters to delete
                            if (HasCharactersToDelete(ActiveControl))
                            {
                                // Let the default Backspace behavior work (delete characters)
                                return; // Don't handle the key, let it bubble up
                            }
                            else
                            {
                                // No characters to delete, move to previous control
                                NavigateToPreviousControl();
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            // Backspace - Move to previous control (for non-text controls)
                            NavigateToPreviousControl();
                            e.Handled = true;
                        }
                    }
                    break;

                case Keys.Escape:
                    // ESC - Close form or go back
                    HandleEscapeKey();
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    // Enter - Trigger default action or move to next control
                    if (!HandleEnterKey())
                    {
                        NavigateToNextControl();
                        e.Handled = true;
                    }
                    break;

                case Keys.F1:
                    // F1 - Show help
                    ShowHelp();
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Builds the list of navigable controls in tab order
        /// </summary>
        private void BuildNavigationList()
        {
            _navigableControls.Clear();
            
            // Get all controls recursively
            var allControls = GetAllControls(this);
            
            // Filter for navigable controls
            foreach (var control in allControls)
            {
                if (IsNavigableControl(control))
                {
                    _navigableControls.Add(control);
                }
            }
            
            // Sort by tab index
            _navigableControls = _navigableControls
                .OrderBy(c => c.TabIndex)
                .ToList();
        }

        /// <summary>
        /// Gets all controls recursively from a container
        /// </summary>
        private List<Control> GetAllControls(Control container)
        {
            var controls = new List<Control>();
            
            foreach (Control control in container.Controls)
            {
                controls.Add(control);
                controls.AddRange(GetAllControls(control));
            }
            
            return controls;
        }

        /// <summary>
        /// Determines if a control should be included in navigation
        /// </summary>
        private bool IsNavigableControl(Control control)
        {
            // Skip invisible or disabled controls
            if (!control.Visible || !control.Enabled)
                return false;

            // Include common input controls
            return control is TextBox ||
                   control is ComboBox ||
                   control is CheckBox ||
                   control is RadioButton ||
                   control is DateTimePicker ||
                   control is NumericUpDown ||
                   control is RichTextBox ||
                   control is MaskedTextBox ||
                   control is Button ||
                   control is DataGridView;
        }

        /// <summary>
        /// Determines if a control is a text input control that should allow default Backspace behavior
        /// </summary>
        private bool IsTextInputControl(Control? control)
        {
            if (control == null) return false;

            // Text input controls that should allow default Backspace behavior
            return control is TextBox ||
                   control is RichTextBox ||
                   control is MaskedTextBox ||
                   control is NumericUpDown ||
                   (control is ComboBox comboBox && comboBox.DropDownStyle == ComboBoxStyle.DropDown);
        }

        /// <summary>
        /// Determines if a text input control has characters that can be deleted
        /// </summary>
        private bool HasCharactersToDelete(Control? control)
        {
            if (control == null) return false;

            if (control is TextBox textBox)
            {
                // Check if there's text and cursor is not at the beginning
                return !string.IsNullOrEmpty(textBox.Text) && textBox.SelectionStart > 0;
            }
            else if (control is RichTextBox richTextBox)
            {
                // Check if there's text and cursor is not at the beginning
                return !string.IsNullOrEmpty(richTextBox.Text) && richTextBox.SelectionStart > 0;
            }
            else if (control is MaskedTextBox maskedTextBox)
            {
                // Check if there's text and cursor is not at the beginning
                return !string.IsNullOrEmpty(maskedTextBox.Text) && maskedTextBox.SelectionStart > 0;
            }
            else if (control is NumericUpDown numericUpDown)
            {
                // For NumericUpDown, check if value is not at minimum
                return numericUpDown.Value > numericUpDown.Minimum;
            }
            else if (control is ComboBox comboBox && comboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                // Check if there's text and cursor is not at the beginning
                return !string.IsNullOrEmpty(comboBox.Text) && comboBox.SelectionStart > 0;
            }

            return false;
        }

        /// <summary>
        /// Navigate to the next control in the list
        /// </summary>
        private void NavigateToNextControl()
        {
            if (_navigableControls.Count == 0) return;

            _currentControlIndex = (_currentControlIndex + 1) % _navigableControls.Count;
            var nextControl = _navigableControls[_currentControlIndex];
            
            // Ensure the control is visible and focusable
            EnsureControlVisible(nextControl);
            nextControl.Focus();
            
            // Highlight the focused control
            HighlightFocusedControl(nextControl);
        }

        /// <summary>
        /// Navigate to the previous control in the list
        /// </summary>
        private void NavigateToPreviousControl()
        {
            if (_navigableControls.Count == 0) return;

            // Find the current control in the list
            var currentControl = ActiveControl;
            var currentIndex = -1;
            
            for (int i = 0; i < _navigableControls.Count; i++)
            {
                if (_navigableControls[i] == currentControl)
                {
                    currentIndex = i;
                    break;
                }
            }
            
            // If current control not found, use the stored index
            if (currentIndex == -1)
            {
                currentIndex = _currentControlIndex;
            }
            
            // Calculate previous index (don't wrap around)
            int previousIndex = currentIndex - 1;
            if (previousIndex < 0)
            {
                // Stay at the first control instead of wrapping to the last
                previousIndex = 0;
            }
            
            _currentControlIndex = previousIndex;
            var previousControl = _navigableControls[_currentControlIndex];
            
            // Ensure the control is visible and focusable
            EnsureControlVisible(previousControl);
            previousControl.Focus();
            
            // Highlight the focused control
            HighlightFocusedControl(previousControl);
        }

        /// <summary>
        /// Ensures a control is visible by scrolling to it if necessary
        /// </summary>
        private void EnsureControlVisible(Control control)
        {
            // If the control is in a scrollable container, make sure it's visible
            var parent = control.Parent;
            while (parent != null)
            {
                if (parent is ScrollableControl scrollable)
                {
                    scrollable.ScrollControlIntoView(control);
                }
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// Highlights the currently focused control
        /// </summary>
        private void HighlightFocusedControl(Control control)
        {
            // Clear previous highlights
            ClearAllHighlights();
            
            // Highlight the current control
            if (control is TextBox textBox)
            {
                textBox.SelectAll(); // Select all text for easy editing
                textBox.BackColor = Color.FromArgb(255, 255, 200); // Light yellow background
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = Color.FromArgb(255, 255, 200);
            }
            else if (control is Button button)
            {
                button.BackColor = Color.FromArgb(0, 102, 204);
                button.ForeColor = Color.White;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.BackColor = Color.FromArgb(255, 255, 200);
            }
            else if (control is DateTimePicker dateTimePicker)
            {
                dateTimePicker.BackColor = Color.FromArgb(255, 255, 200);
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.BackColor = Color.FromArgb(255, 255, 200);
            }
        }

        /// <summary>
        /// Clears all control highlights
        /// </summary>
        private void ClearAllHighlights()
        {
            foreach (var control in _navigableControls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BackColor = SystemColors.Window;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = SystemColors.Window;
                }
                else if (control is Button button)
                {
                    button.BackColor = SystemColors.Control;
                    button.ForeColor = SystemColors.ControlText;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.BackColor = SystemColors.Control;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = SystemColors.Window;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.BackColor = SystemColors.Window;
                }
            }
        }

        /// <summary>
        /// Handles the Enter key press - can be overridden by derived classes
        /// </summary>
        /// <returns>True if the Enter key was handled, false otherwise</returns>
        protected virtual bool HandleEnterKey()
        {
            // Default implementation - let derived classes override
            return false;
        }

        /// <summary>
        /// Handles the Escape key press - can be overridden by derived classes
        /// </summary>
        protected virtual void HandleEscapeKey()
        {
            // Default implementation - close the form
            Close();
        }

        /// <summary>
        /// Shows help information - can be overridden by derived classes
        /// </summary>
        protected virtual void ShowHelp()
        {
            var helpMessage = @"Keyboard Navigation Help:

Navigation:
• Tab - Move to next field
• Backspace - Move to previous field
• Enter - Confirm action or move to next field
• Escape - Close form or go back
• F1 - Show this help

Tips:
• Use Tab/Backspace for fast field navigation
• Enter to save or confirm actions
• Escape to cancel or close forms
• All fields are highlighted when focused";

            MessageBox.Show(helpMessage, "Keyboard Navigation Help", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gets the current focused control index
        /// </summary>
        protected int CurrentControlIndex => _currentControlIndex;

        /// <summary>
        /// Gets the list of navigable controls
        /// </summary>
        protected List<Control> NavigableControls => _navigableControls;

        /// <summary>
        /// Refreshes the navigation list (call this when controls are added/removed dynamically)
        /// </summary>
        protected void RefreshNavigationList()
        {
            BuildNavigationList();
        }

        /// <summary>
        /// Sets focus to a specific control by index
        /// </summary>
        protected void SetFocusToControl(int index)
        {
            if (index >= 0 && index < _navigableControls.Count)
            {
                _currentControlIndex = index;
                var control = _navigableControls[index];
                EnsureControlVisible(control);
                control.Focus();
                HighlightFocusedControl(control);
            }
        }

        /// <summary>
        /// Sets focus to a specific control by reference
        /// </summary>
        protected void SetFocusToControl(Control control)
        {
            var index = _navigableControls.IndexOf(control);
            if (index >= 0)
            {
                SetFocusToControl(index);
            }
        }
    }
}


