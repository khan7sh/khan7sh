using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Services;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class ProductPage : Page
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private readonly ProductService _productService;

        public ProductPage(AppDbContext context, User currentUser, ProductService productService)
        {
            InitializeComponent();
            _context = context;
            _currentUser = currentUser;
            _productService = productService;
            LoadProducts();
            UpdateButtonVisibility();
        }

        private async void LoadProducts()
        {
            ProductListView.ItemsSource = await _productService.GetAllProductsAsync();
        }

        private void UpdateButtonVisibility()
        {
            AddProductButton.Visibility = _currentUser.HasPermission(Permissions.CreateProduct) ? Visibility.Visible : Visibility.Collapsed;
            EditProductButton.Visibility = _currentUser.HasPermission(Permissions.EditProduct) ? Visibility.Visible : Visibility.Collapsed;
            DeleteProductButton.Visibility = _currentUser.HasPermission(Permissions.DeleteProduct) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.CreateProduct))
            {
                MessageBox.Show("You don't have permission to add products.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addProductWindow = new AddEditProductWindow(_context);
            if (addProductWindow.ShowDialog() == true)
            {
                await _productService.AddProductAsync(addProductWindow.Product);
                LoadProducts();
                StatusTextBlock.Text = "Product added successfully.";
            }
        }

        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.EditProduct))
            {
                MessageBox.Show("You don't have permission to edit products.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedProduct = ProductListView.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editProductWindow = new AddEditProductWindow(_context, selectedProduct);
            if (editProductWindow.ShowDialog() == true)
            {
                await _productService.UpdateProductAsync(editProductWindow.Product);
                LoadProducts();
                StatusTextBlock.Text = "Product updated successfully.";
            }
        }

        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.HasPermission(Permissions.DeleteProduct))
            {
                MessageBox.Show("You don't have permission to delete products.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedProduct = ProductListView.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {selectedProduct.Name}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _productService.DeleteProductAsync(selectedProduct.Id);
                LoadProducts();
                StatusTextBlock.Text = "Product deleted successfully.";
            }
        }
    }
}