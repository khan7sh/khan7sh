using System;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows; // Add this for FrameworkElement

namespace BasicVendorInventoryPlatform.Views
{
    public partial class DashboardPage : Page
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;

        public DashboardPage(AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _context = context;
            _currentUser = currentUser;
            LoadDashboard();
            RemoveUnwantedButtons();
        }

        private void LoadDashboard()
        {
            WelcomeTextBlock.Text = $"Welcome, {_currentUser.Username}!";

            var recentVendors = _context.Vendors.OrderByDescending(v => v.LastReviewDate).Take(5).ToList();
            RecentVendorsListView.ItemsSource = recentVendors;

            var recentProducts = _context.Products.OrderByDescending(p => p.Id).Take(5).ToList();
            RecentProductsListView.ItemsSource = recentProducts;

            var vendorsNeedingReview = _context.Vendors
                .Where(v => v.LastReviewDate < DateTime.Now.AddDays(-30))
                .OrderBy(v => v.LastReviewDate)
                .Take(5)
                .ToList();
            VendorsNeedingReviewListView.ItemsSource = vendorsNeedingReview;

            int totalVendors = _context.Vendors.Count();
            int totalProducts = _context.Products.Count();
            double averageVendorRating = _context.Vendors.Any() ? _context.Vendors.Average(v => v.Rating) : 0;

            TotalVendorsTextBlock.Text = $"Total Vendors: {totalVendors}";
            TotalProductsTextBlock.Text = $"Total Products: {totalProducts}";
            AverageVendorRatingTextBlock.Text = $"Average Vendor Rating: {averageVendorRating:F2}";
        }

        private void RemoveUnwantedButtons()
        {
            // Explicitly use WPF Button and Panel
            if (FindName("VendorManagementButton") is System.Windows.Controls.Button vendorManagementButton)
            {
                (vendorManagementButton.Parent as System.Windows.Controls.Panel)?.Children.Remove(vendorManagementButton);
            }

            if (FindName("GenerateReportButton") is System.Windows.Controls.Button generateReportButton)
            {
                (generateReportButton.Parent as System.Windows.Controls.Panel)?.Children.Remove(generateReportButton);
            }

            // If you need to adjust layout, you might want to update the layout of the parent panel
            if (FindName("MainMenu") is System.Windows.Controls.Panel mainMenu)
            {
                mainMenu.UpdateLayout();
            }
        }

        // Keep any existing event handlers or methods below this line

        // Example: 
        // private void SomeButton_Click(object sender, RoutedEventArgs e)
        // {
        //     // Event handler logic
        // }
    }
}

