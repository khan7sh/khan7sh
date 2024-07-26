using System;
using System.Windows;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class AddEditVendorWindow : Window
    {
        private readonly AppDbContext _context;
        public Vendor Vendor { get; private set; }

        public AddEditVendorWindow(AppDbContext context, Vendor vendor = null)
        {
            InitializeComponent();
            _context = context;
            Vendor = vendor ?? new Vendor();

            PopulateFields();
        }

        private void PopulateFields()
        {
            NameTextBox.Text = Vendor.Name;
            WebsiteTextBox.Text = Vendor.Website;
            LastReviewDatePicker.SelectedDate = Vendor.LastReviewDate;
            RatingSlider.Value = Vendor.Rating;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Vendor.Name = NameTextBox.Text;
            Vendor.Website = WebsiteTextBox.Text;
            Vendor.LastReviewDate = LastReviewDatePicker.SelectedDate ?? DateTime.Now;
            Vendor.Rating = RatingSlider.Value;

            if (string.IsNullOrWhiteSpace(Vendor.Name))
            {
                MessageBox.Show("Vendor name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
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