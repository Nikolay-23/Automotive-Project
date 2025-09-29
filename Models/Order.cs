using Microsoft.EntityFrameworkCore;

namespace Automotive_Project.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string ClientId { get; set; } = null!;
        public UserAccount Client { get; set; } = null!;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Precision(16, 2)]
        public decimal ShippingFee { get; set; }

        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string PaymentDetails { get; set; } = null!; 
        public string OrderStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
