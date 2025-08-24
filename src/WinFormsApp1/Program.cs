using WinFormsApp1.Forms;
using WinFormsApp1.Forms.Auth;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Create the authentication service
            using var authService = new AuthService();
            
            // Check if user is already authenticated with a valid token
            if (authService.HasValidSavedToken)
            {
                Console.WriteLine("Valid saved token found, skipping login");
                // User is already authenticated, show main form directly
                var mainForm = new MainMDIForm(authService);
                Application.Run(mainForm);
            }
            else
            {
                // Show login form
                using var loginForm = new LoginForm(authService);
                var loginResult = loginForm.ShowDialog();
                
                // If login was successful, show the main MDI form
                if (loginResult == DialogResult.OK && authService.IsAuthenticated)
                {
                    var mainForm = new MainMDIForm(authService);
                    Application.Run(mainForm);
                }
            }
        }
    }
}