using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLStore.Core.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty; // simple identifier like "user1"
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product? Product { get; set; }
    }
}
