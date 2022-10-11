namespace Microservice.Seles.Models.DTOs
{
    public class OrderItemViewModel
    {
        public int OrderItemId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}