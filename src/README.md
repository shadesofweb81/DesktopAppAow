# WinForms MDI Application with Authentication

This is a Windows Forms application that implements an MDI (Multiple Document Interface) with authentication against an external API.

## Features

- **Login Form**: Authenticates users against `auth.accountingonweb.com`
- **MDI Parent Form**: Main application window that can contain multiple child forms
- **JWT Token Management**: Stores and manages authentication tokens
- **Window Management**: Cascade, tile horizontal/vertical, and close all windows
- **Logout Functionality**: Secure logout that clears authentication tokens

## API Integration

The application integrates with the authentication API at `auth.accountingonweb.com`:

- **Endpoint**: `POST /api/user/login`
- **Request Format**: JSON with username and password
- **Response**: JWT token for authentication

## Project Structure

```
WinFormsApp1/
├── Models/
│   └── LoginModels.cs          # Login request/response models
├── Services/
│   └── AuthService.cs          # Authentication service
├── Forms/
│   ├── LoginForm.cs            # Login form
│   ├── MainMDIForm.cs          # MDI parent form
│   └── ChildForm.cs            # Base child form template
├── Program.cs                  # Application entry point
└── WinFormsApp1.csproj         # Project file
```

## How to Use

1. **Start the Application**: Run the application and the login form will appear
2. **Login**: Enter your username and password
3. **Main Application**: Upon successful login, the MDI form will open
4. **Create Child Forms**: Use the "New Form" menu item to create additional child forms
5. **Window Management**: Use the Window menu to arrange child forms
6. **Logout**: Use the File > Logout menu to securely log out

## Dependencies

- .NET 9.0 Windows
- Microsoft.Extensions.Http
- System.Text.Json

## Building and Running

1. Ensure you have .NET 9.0 SDK installed
2. Open the solution in Visual Studio or use the command line:
   ```bash
   dotnet build
   dotnet run
   ```

## Security Features

- JWT token storage for authentication
- Secure logout that clears tokens
- Password field masking
- Input validation
- Error handling for failed authentication

## Customization

To add new child forms:
1. Create a new form class inheriting from `ChildForm` or `Form`
2. Set `MdiParent` to the main form
3. Add menu items to open the new forms as needed

## API Configuration

The authentication API base URL is configured in `AuthService.cs`. Modify the `_baseUrl` field if needed:

```csharp
private readonly string _baseUrl = "https://auth.accountingonweb.com";
```
