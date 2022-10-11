using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Models.DTOs
{
    public class SelesViewModel
    {
        public string BookSerial { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Auther { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }


        public string UserSerial { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
