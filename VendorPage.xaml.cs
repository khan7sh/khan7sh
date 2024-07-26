using System;
using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Services;
using Microsoft.Win32;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class VendorPage : Page
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private readonly VendorService _vendorService;
        private readonly ReportService _reportService;
        private readonly PdfService _pdfService;

        public VendorPage(AppDbContext context, User currentUser, VendorService vendorService, ReportService reportService, PdfService pdfService)
        {
            InitializeComponent();
            _context = context;
            _currentUser = currentUser;
            _vendorService = vendorService;
            _reportService = reportService;
            _pdfService = pdfService;
            LoadVendors();
            UpdateButtonVisibility();
        }

        private async void LoadVendors()
        {
            VendorListView.ItemsSource = await _vendorService.GetAllVendorsAsync();
        }

        private void UpdateButtonVisibility()
        {
            bool canManageVendors = _currentUser.HasPermission(Permissions.ManageVendors);
            AddVendorButton.Visibility = canManageVendors ? Visibility.Visible : Visibility.Collapsed;
            EditVendorButton.Visibility = canManageVendors ? Visibility.Visible : Visibility.Collapsed;
            DeleteVendorButton.Visibility = canManageVendors ? Visibility.Visible : Visibility.Collapsed;
            GenerateReportButton.Visibility = Visibility.Visible; // Always make it visible
        }

        private async void AddVendor_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.ManageVendors))
            {
                MessageBox.Show("You don't have permission to add vendors.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addVendorWindow = new AddEditVendorWindow(_context);
            if (addVendorWindow.ShowDialog() == true)
            {
                await _vendorService.AddVendorAsync(addVendorWindow.Vendor);
                LoadVendors();
                StatusTextBlock.Text = "Vendor added successfully.";
            }
        }

        private async void EditVendor_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.ManageVendors))
            {
                MessageBox.Show("You don't have permission to edit vendors.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedVendor = VendorListView.SelectedItem as Vendor;
            if (selectedVendor == null)
            {
                MessageBox.Show("Please select a vendor to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editVendorWindow = new AddEditVendorWindow(_context, selectedVendor);
            if (editVendorWindow.ShowDialog() == true)
            {
                await _vendorService.UpdateVendorAsync(editVendorWindow.Vendor);
                LoadVendors();
                StatusTextBlock.Text = "Vendor updated successfully.";
            }
        }

        private async void DeleteVendor_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.ManageVendors))
            {
                MessageBox.Show("You don't have permission to delete vendors.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedVendor = VendorListView.SelectedItem as Vendor;
            if (selectedVendor == null)
            {
                MessageBox.Show("Please select a vendor to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {selectedVendor.Name}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _vendorService.DeleteVendorAsync(selectedVendor.Id);
                LoadVendors();
                StatusTextBlock.Text = "Vendor deleted successfully.";
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var selectedVendor = VendorListView.SelectedItem as Vendor;
            if (selectedVendor != null)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    DefaultExt = "pdf",
                    FileName = $"{selectedVendor.Name}_Report.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        _pdfService.GenerateVendorReport(selectedVendor, saveFileDialog.FileName);
                        MessageBox.Show("Report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ViewVendorDetails_Click(object sender, RoutedEventArgs e)
        {
            var selectedVendor = VendorListView.SelectedItem as Vendor;
            if (selectedVendor == null)
            {
                MessageBox.Show("Please select a vendor to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Navigate to VendorDetailsPage
            ((MainWindow)Application.Current.MainWindow).NavigateToVendorDetails(selectedVendor);
        }
    }
}