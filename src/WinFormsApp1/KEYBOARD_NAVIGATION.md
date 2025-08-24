# Keyboard Navigation Guide

This WinForms application is designed for efficient keyboard-only operation, allowing users to navigate and perform all operations without using a mouse.

## Core Navigation Principles

- **ESC Key**: Always goes back or closes the current form/dialog
- **ENTER Key**: Confirms actions or opens selected items
- **TAB/Shift+TAB**: Navigate between controls
- **Arrow Keys**: Navigate within lists and data grids
- **Alt+Letter**: Access menu items and controls with mnemonics

## Application Structure

### Main Application (MainMDIForm)
**Global Shortcuts:**
- `Alt+C` - Open Company Management
- `Alt+F4` - Exit Application
- `F1` - Show keyboard navigation help
- `Esc` - Close active child form
- `Ctrl+Tab` - Cycle through open child forms
- `Ctrl+W` - Close active child form

**Menu Navigation:**
- `Alt+F` - Open File menu
- `Alt+W` - Open Window menu
- Use arrow keys to navigate menu items
- `Enter` to select, `Esc` to close menu

### Login Form
**Navigation:**
- `Tab/Shift+Tab` - Move between fields and buttons
- `Enter` - Login (from any field)
- `Esc` - Cancel and close
- `F1` - Show login help

**Fields:**
- Username field (auto-focused on form load)
- Password field
- Login button
- Test API button

### Company Management Form
**List Navigation:**
- `↑↓` Arrow keys - Navigate company list
- `Enter` - Edit selected company
- `Insert` - Create new company
- `Delete` - Delete selected company
- `F5` - Refresh company list
- `Esc` - Close form

**Button Shortcuts:**
- `Alt+N` - New company (Insert key)
- `Alt+E` - Edit company (Enter key)
- `Alt+D` - Delete company (Delete key)
- `Alt+R` - Refresh list (F5 key)

### Company Edit Form
**Field Navigation:**
- `Tab/Shift+Tab` - Navigate between fields
- `Enter` - Save (when not in text field)
- `Ctrl+Enter` - Save from anywhere
- `Esc` - Cancel and close

**Field Order:**
1. Company Name (required)
2. Company Code (required)
3. Address
4. City
5. State
6. Zip Code
7. Country
8. Phone
9. Email (validated format)
10. Website
11. Tax ID
12. Active checkbox
13. Save button
14. Cancel button

## API Integration

### Authentication Service
- **Base URL**: `https://auth.accountingonweb.com`
- **Endpoint**: `/api/auth/User/login`
- **Method**: POST
- **Authentication**: JWT token based

### Company Service
- **Base URL**: `https://erp.accountingonweb.com`
- **Endpoints**:
  - `GET /api/v1/company/all` - Get all companies
  - `GET /api/v1/company/{id}` - Get company by ID
  - `POST /api/v1/company` - Create new company
  - `PUT /api/v1/company/{id}` - Update company
  - `DELETE /api/v1/company/{id}` - Delete company
- **Authentication**: Bearer token from login

## Form Flow

```
Login Form (with ESC to exit)
     ↓ (successful login)
Main MDI Form
     ↓ (Alt+C or File → Company)
Company Management Form
     ↓ (Enter to edit, Insert for new)
Company Edit Form
     ↓ (Enter to save, ESC to cancel)
Back to Company Management Form
     ↓ (ESC to close)
Back to Main MDI Form
```

## Error Handling

- All API calls include error handling with user-friendly messages
- Validation is performed on required fields before API calls
- Connection issues are reported with diagnostic information
- Form state is preserved during error conditions

## Keyboard Accessibility Features

1. **Tab Order**: Logical tab sequence for all forms
2. **Mnemonics**: Alt+Letter shortcuts for all major controls
3. **Visual Feedback**: Clear indication of focused controls
4. **Help System**: F1 help available on all forms
5. **Status Messages**: Real-time feedback for all operations
6. **Escape Routes**: ESC key always provides a way back

## Best Practices for Users

1. **Learn the Shortcuts**: Memorize Alt+C for company management
2. **Use F1**: Get context-sensitive help anytime
3. **Navigate with Purpose**: Use ESC to back out, Enter to proceed
4. **Check Status Messages**: Always read the status bar for feedback
5. **Use Tab Navigation**: More reliable than mouse in data entry forms

## Technical Implementation

- `KeyPreview = true` on all forms for global key handling
- Consistent event handlers for ESC/Enter across all forms
- Proper tab order setup for logical navigation
- Mnemonic characters assigned to all actionable controls
- Status feedback for all asynchronous operations

This keyboard navigation system ensures the application can be used efficiently without a mouse, providing a fast and accessible user experience.

