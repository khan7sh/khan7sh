using BasicVendorInventoryPlatform.Models;
using System.IO;
using System.Text;

namespace BasicVendorInventoryPlatform.Services
{
    public class ReportService
    {
        private readonly LoggingService _loggingService;

        public ReportService(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void GenerateVendorReport(Vendor vendor, string outputPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Vendor Report: {vendor.Name}");
            sb.AppendLine($"Website: {vendor.Website}");
            sb.AppendLine($"Last Review Date: {vendor.LastReviewDate}");
            sb.AppendLine($"Rating: {vendor.Rating}");
            sb.AppendLine("\nProducts:");
            foreach (var product in vendor.Products)
            {
                sb.AppendLine($"- {product.Name} (Rating: {product.Rating})");
            }

            File.WriteAllText(outputPath, sb.ToString());
            _loggingService.LogInfo($"Generated report for vendor {vendor.Name} at {outputPath}");
        }
    }
}