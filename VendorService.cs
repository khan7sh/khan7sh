using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;

namespace BasicVendorInventoryPlatform.Services
{
    public class VendorService
    {
        private readonly AppDbContext _context;

        public VendorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            return await _context.Vendors.Include(v => v.Products).ToListAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(int id)
        {
            return await _context.Vendors
                .Include(v => v.Products)
                .Include(v => v.Documents)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vendor> AddVendorAsync(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }

        public async Task<Vendor> UpdateVendorAsync(Vendor vendor)
        {
            _context.Entry(vendor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return vendor;
        }

        public async Task DeleteVendorAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vendor>> GetVendorsNeedingReviewAsync()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return await _context.Vendors
                .Where(v => v.LastReviewDate < thirtyDaysAgo)
                .OrderBy(v => v.LastReviewDate)
                .ToListAsync();
        }
    }
}