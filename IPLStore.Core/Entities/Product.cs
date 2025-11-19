using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLStore.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Franchise { get; set; } = string.Empty; // CSK, MI, RCB, etc.
        public string Type { get; set; } = string.Empty;      // Jersey, Cap, Flag, Autograph, etc.
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
