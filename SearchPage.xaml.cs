using System.Windows;
using System.Windows.Controls;
using BasicVendorInventoryPlatform.Services;
using System.Threading.Tasks;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Views
{
    public partial class SearchPage : Page
    {
        private readonly SearchService _searchService;

        public SearchPage(SearchService searchService)
        {
            InitializeComponent();
            _searchService = searchService;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Empty Search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            await PerformSearch(searchTerm);
        }

        private async Task PerformSearch(string searchTerm)
        {
            var vendors = await _searchService.SearchVendors(searchTerm);
            var products = await _searchService.SearchProducts(searchTerm);

            VendorResultsListView.ItemsSource = vendors;
            ProductResultsListView.ItemsSource = products;

            // Remove this line if ResultsTabControl doesn't exist
            // ResultsTabControl.SelectedIndex = 0;
        }
    }
}