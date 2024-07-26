using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicVendorInventoryPlatform.Services
{
    public class SearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vendor>> SearchVendors(string searchTerm)
        {
            return await _context.Vendors
                .Where(v => v.Name.Contains(searchTerm) || v.Products.Any(p => p.Name.Contains(searchTerm)))
                .Include(v => v.Products)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProducts(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .Include(p => p.Vendor)
                .ToListAsync();
        }
    }
}