using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasicVendorInventoryPlatform.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Website { get; set; }

        public DateTime LastReviewDate { get; set; }

        public double Rating { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}