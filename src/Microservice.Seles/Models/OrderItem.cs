using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int BookId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }
    }
}
