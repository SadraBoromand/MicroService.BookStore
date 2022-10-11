namespace Microservice.Accounting.Models.DTOs
{
    public class ListAccountingViewModel
    {
        public int OrderId { get; set; }
        public string UserSerial { get; set; }
        public bool IsPay { get; set; }
        public decimal Price { get; set; }
        
        public string Serial { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}