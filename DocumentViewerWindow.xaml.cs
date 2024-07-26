using System;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class DocumentViewerWindow : Window
    {
        public DocumentViewerWindow(string pdfPath)
        {
            InitializeComponent();
            PdfViewer.Navigate(new Uri(pdfPath));
        }
    }
}