using System;

namespace Microservice.Accounting.Models.DTOs
{
    public class AccountingListViewModel
    {
        // for order
        public int OrderId { get; set; }
        public string OrderSerial { get; set; }
        public string UserSerial { get; set; }
        public bool IsPay { get; set; }
        public decimal Price { get; set; }
        public DateTime PayDate { get; set; }

        // for user
        public string FullName { get; set; }
        //public string Phone { get; set; }
        public string Role { get; set; }
        //public string Password { get; set; }
    }
}
