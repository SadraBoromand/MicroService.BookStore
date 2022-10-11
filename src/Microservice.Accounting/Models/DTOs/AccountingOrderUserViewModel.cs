using System;
using System.Collections.Generic;

namespace Microservice.Accounting.Models.DTOs
{
    public class AccountingOrderUserViewModel
    {
        public string UserSerial { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }



        public string OrderId { get; set; }
        public string OrderSerial { get; set; }
        public bool IsPay { get; set; }
        public decimal Price { get; set; }
        public DateTime PayDate { get; set; }
    }
}