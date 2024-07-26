using BasicVendorInventoryPlatform.Data;
using BasicVendorInventoryPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BasicVendorInventoryPlatform.Services
{
    public class DocumentService
    {
        private readonly AppDbContext _context;
        private readonly LoggingService _loggingService;

        public DocumentService(AppDbContext context, LoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }


        public async Task<Document> AttachDocument(string filePath, string name, int? vendorId = null, int? productId = null)
        {
            try
            {
                var document = new Document
                {
                    Name = name,
                    FilePath = filePath,
                    VendorId = vendorId,
                    ProductId = productId,
                    ContentType = Path.GetExtension(filePath).ToLowerInvariant()
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
                _loggingService.LogInfo($"Document {name} attached successfully");
                return document;
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error attaching document", ex);
                throw;
            }
        }

        public async Task<Document> GetDocument(int documentId)
        {
            return await _context.Documents.FindAsync(documentId);
        }
    }
}