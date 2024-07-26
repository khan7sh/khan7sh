using System;
using System.Windows;
using BasicVendorInventoryPlatform.Services;
using BasicVendorInventoryPlatform.Views;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Data;

namespace BasicVendorInventoryPlatform
{
    public partial class MainWindow : Window
    {
        private readonly AuthService _authService;
        private readonly SearchService _searchService;
        private readonly LoggingService _loggingService;
        private readonly VendorService _vendorService;
        private readonly ProductService _productService;
        private readonly DocumentService _documentService;
        private readonly ReportService _reportService;
        private readonly AppDbContext _dbContext;
        private readonly PdfService _pdfService;

        private User _currentUser;

        public MainWindow(
            AuthService authService,
            SearchService searchService,
            LoggingService loggingService,
            VendorService vendorService,
            ProductService productService,
            DocumentService documentService,
            ReportService reportService,
            AppDbContext dbContext,
            PdfService pdfService)
        {
            InitializeComponent();

            _authService = authService;
            _searchService = searchService;
            _loggingService = loggingService;
            _vendorService = vendorService;
            _productService = productService;
            _documentService = documentService;
            _reportService = reportService;
            _dbContext = dbContext;
            _pdfService = pdfService;

            NavigateToLogin();
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
            UserNameText.Text = user.Username;
            UpdateUIForUser();
            NavigateToVendors();
        }

        private void UpdateUIForUser()
        {
            // Enable/disable menu items based on user permissions
            // This is a basic implementation. Adjust according to your permission system.
            bool isAdmin = _currentUser.HasPermission(Permissions.ManageUsers);
            // Update menu items visibility here if needed
        }

        #region Navigation Methods

        public void NavigateToLogin()
        {
            MainFrame.Navigate(new LoginPage(_authService, _loggingService, this));
            StatusText.Text = "Please log in";
        }

        public void NavigateToDashboard()
        {
            if (_currentUser != null)
            {
                MainFrame.Navigate(new DashboardPage(_dbContext, _currentUser));
                StatusText.Text = "Dashboard";
            }
            else
            {
                _loggingService.LogError("Attempted to navigate to Dashboard without a logged-in user", null);
                NavigateToLogin();
            }
        }

        public void NavigateToVendors()
        {
            if (_currentUser != null)
            {
                MainFrame.Navigate(new VendorPage(_dbContext, _currentUser, _vendorService, _reportService, _pdfService));
                StatusText.Text = "Vendor Management";
            }
            else
            {
                _loggingService.LogError("Attempted to navigate to Vendors without a logged-in user", null);
                NavigateToLogin();
            }
        }

        public void NavigateToProducts()
        {
            if (_currentUser != null)
            {
                MainFrame.Navigate(new ProductPage(_dbContext, _currentUser, _productService));
                StatusText.Text = "Product Management";
            }
            else
            {
                _loggingService.LogError("Attempted to navigate to Products without a logged-in user", null);
                NavigateToLogin();
            }
        }

        public void NavigateToSearch()
        {
            if (_currentUser != null)
            {
                MainFrame.Navigate(new SearchPage(_searchService));
                StatusText.Text = "Search";
            }
            else
            {
                _loggingService.LogError("Attempted to navigate to Search without a logged-in user", null);
                NavigateToLogin();
            }
        }

        public void NavigateToVendorDetails(Vendor vendor)
        {
            if (_currentUser != null)
            {
                MainFrame.Navigate(new VendorDetailsPage(_dbContext, vendor, _pdfService));
                StatusText.Text = $"Vendor Details: {vendor.Name}";
            }
            else
            {
                _loggingService.LogError("Attempted to navigate to Vendor Details without a logged-in user", null);
                NavigateToLogin();
            }
        }
        #endregion

        #region Event Handlers

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DashboardMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashboard();
        }

        private void VendorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToVendors();
        }

        private void ProductsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToProducts();
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSearch();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vendor Inventory Platform\nVersion 1.0\n© 2023 Your Company", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashboard();
        }

        private void VendorsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToVendors();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToProducts();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSearch();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }

        #endregion

        public void LogOut()
        {
            _currentUser = null;
            UserNameText.Text = "Not logged in";
            _loggingService.LogInfo("User logged out");
            UpdateUIForUser();
            NavigateToLogin();
        }
    }
}