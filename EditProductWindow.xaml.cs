using System.Windows;
using BasicVendorInventoryPlatform.Models;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform
{
    public partial class EditProductWindow : Window
    {
        public Product EditedProduct { get; private set; }

        public EditProductWindow(Product product)
        {
            InitializeComponent();
            EditedProduct = new Product { Id = product.Id, Name = product.Name };
            NameTextBox.Text = EditedProduct.Name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditedProduct.Name = NameTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}