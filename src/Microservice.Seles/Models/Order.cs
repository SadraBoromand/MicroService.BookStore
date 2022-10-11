using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderSerial { get; set; }
        public string UserSerial { get; set; }
        public bool IsPay { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
