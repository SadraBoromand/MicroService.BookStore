using System.Collections.Generic;

namespace Microservice.Seles.Models.DTOs
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserSerial { get; set; }
        public bool IsPay { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}