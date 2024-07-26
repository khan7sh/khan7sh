using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Services;
using System.Linq;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class LoginPage : Page
    {
        private readonly AuthService _authService;
        private readonly LoggingService _loggingService;
        private readonly MainWindow _mainWindow;

        public LoginPage(AuthService authService, LoggingService loggingService, MainWindow mainWindow)
        {
            InitializeComponent();
            _authService = authService;
            _loggingService = loggingService;
            _mainWindow = mainWindow;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            _loggingService.LogInfo($"Attempting login for user: {username}");

            var user = await _authService.AuthenticateUser(username, password);
            if (user != null)
            {
                _loggingService.LogInfo($"User {username} logged in successfully");
                (_mainWindow as MainWindow)?.SetCurrentUser(user);
                _mainWindow.NavigateToDashboard(); // Remove the arguments here
            }
            else
            {
                _loggingService.LogWarning($"Failed login attempt for user {username}");
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}