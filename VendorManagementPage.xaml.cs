using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Services;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class VendorManagementPage : Page
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private readonly VendorService _vendorService;

        public VendorManagementPage(AppDbContext context, User currentUser, VendorService vendorService)
        {
            InitializeComponent();
            _context = context;
            _currentUser = currentUser;
            _vendorService = vendorService;
            LoadVendors();
        }

        private async void LoadVendors()
        {
            VendorListView.ItemsSource = await _vendorService.GetAllVendorsAsync();
        }

        private async void AddVendor_Click(object sender, RoutedEventArgs e)
        {
            var addVendorWindow = new AddEditVendorWindow(_context);
            if (addVendorWindow.ShowDialog() == true)
            {
                await _vendorService.AddVendorAsync(addVendorWindow.Vendor);
                LoadVendors();
            }
        }

        private async void EditVendor_Click(object sender, RoutedEventArgs e)
        {
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
            }
        }

        private async void DeleteVendor_Click(object sender, RoutedEventArgs e)
        {
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
            }
        }
    }
}