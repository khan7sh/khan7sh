using System.Windows;
using BasicVendorInventoryPlatform.Models;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform
{
    public partial class EditVendorWindow : Window
    {
        public Vendor EditedVendor { get; private set; }

        public EditVendorWindow(Vendor vendor)
        {
            InitializeComponent();
            EditedVendor = new Vendor { Id = vendor.Id, Name = vendor.Name };
            NameTextBox.Text = EditedVendor.Name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditedVendor.Name = NameTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}