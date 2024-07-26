using System;
using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using BasicVendorInventoryPlatform.Services;
using Microsoft.Win32;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class VendorDetailsPage : Page
    {
        private readonly AppDbContext _context;
        private readonly Vendor _vendor;
        private readonly PdfService _pdfService;

        public VendorDetailsPage(AppDbContext context, Vendor vendor, PdfService pdfService)
        {
            InitializeComponent();
            _context = context;
            _vendor = vendor;
            _pdfService = pdfService;
            DataContext = _vendor;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName = $"{_vendor.Name}_Report.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _pdfService.GenerateVendorReport(_vendor, saveFileDialog.FileName);
                    MessageBox.Show("Report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}