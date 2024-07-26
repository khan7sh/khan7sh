using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicVendorInventoryPlatform.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public int? VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public Product()
        {
            Documents = new HashSet<Document>();
        }
    }
}