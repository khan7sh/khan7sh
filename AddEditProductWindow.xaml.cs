using System;
using System.Linq;
using System.Windows;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class AddEditProductWindow : Window
    {
        private readonly AppDbContext _context;
        public Product Product { get; private set; }

        public AddEditProductWindow(AppDbContext context, Product product = null)
        {
            InitializeComponent();
            _context = context;
            Product = product ?? new Product();

            LoadVendors();
            PopulateFields();
        }

        private void LoadVendors()
        {
            VendorComboBox.ItemsSource = _context.Vendors.ToList();
            VendorComboBox.SelectedItem = Product.Vendor;
        }

        private void PopulateFields()
        {
            NameTextBox.Text = Product.Name;
            DescriptionTextBox.Text = Product.Description;
            RatingSlider.Value = Product.Rating;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Product.Name = NameTextBox.Text;
            Product.Description = DescriptionTextBox.Text;
            Product.Rating = RatingSlider.Value;
            Product.Vendor = (Vendor)VendorComboBox.SelectedItem;

            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}