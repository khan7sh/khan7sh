using System;
using System.IO;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using BasicVendorInventoryPlatform.Models;
using System.Linq;

namespace BasicVendorInventoryPlatform.Services
{
    public class PdfService
    {
        public void GenerateVendorReport(Vendor vendor, string outputPath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var width = page.Width;
                var height = page.Height;

                // Define fonts
                var titleFont = new XFont("Verdana", 22);
                var headingFont = new XFont("Verdana", 16);
                var normalFont = new XFont("Verdana", 10);

                // Draw title
                gfx.DrawString("Vendor Report", titleFont, XBrushes.Black,
                    new XRect(0, 40, width, 0), XStringFormats.TopCenter);

                // Draw vendor details
                double yPosition = 100;
                DrawText(gfx, "Vendor Name:", vendor.Name, headingFont, normalFont, 50, ref yPosition);
                DrawText(gfx, "Website:", vendor.Website, headingFont, normalFont, 50, ref yPosition);
                DrawText(gfx, "Last Review Date:", vendor.LastReviewDate.ToShortDateString(), headingFont, normalFont, 50, ref yPosition);
                DrawText(gfx, "Rating:", vendor.Rating.ToString("0.0"), headingFont, normalFont, 50, ref yPosition);

                yPosition += 20;

                // Draw products table
                gfx.DrawString("Products", headingFont, XBrushes.Black, 50, yPosition);
                yPosition += 20;

                if (vendor.Products != null && vendor.Products.Any())
                {
                    foreach (var product in vendor.Products)
                    {
                        DrawText(gfx, "Name:", product.Name, normalFont, normalFont, 70, ref yPosition);
                        DrawText(gfx, "Description:", product.Description, normalFont, normalFont, 70, ref yPosition);
                        DrawText(gfx, "Rating:", product.Rating.ToString("0.0"), normalFont, normalFont, 70, ref yPosition);
                        yPosition += 10;

                        if (yPosition > height - 50)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yPosition = 50;
                        }
                    }
                }
                else
                {
                    gfx.DrawString("No products available", normalFont, XBrushes.Black, 70, yPosition);
                }

                document.Save(outputPath);
            }

            try
            {
                Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening PDF: {ex.Message}");
            }
        }

        private void DrawText(XGraphics gfx, string label, string value, XFont labelFont, XFont valueFont, double x, ref double y)
        {
            gfx.DrawString(label, labelFont, XBrushes.Black, x, y);
            gfx.DrawString(value, valueFont, XBrushes.Black, x + 150, y);
            y += 20;
        }
    }
}