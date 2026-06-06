namespace BootCamp2.Domin.Domins.Models
{
  public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!; // مثل: ORD-2025-00001
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total => Subtotal - Discount + Shipping;

        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending, Paid, Cancelled, Expired
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        // صلاحية الحجز (مثلاً 30 دقيقة من الإنشاء)
        public DateTime ReservationExpiresAt { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        Paid = 1,
        Cancelled = 2,
        Expired = 3
    }

    //public class OrderDetails
    //{
    //    public int Id { get; set; }
    //    public int OrderId { get; set; }
    //    public virtual Order Order { get; set; } = null!;
    //    public int ProductId { get; set; }
    //    public virtual Product Product { get; set; } = null!;
    //    public int Quantity { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public decimal LineTotal => UnitPrice * Quantity;
    //}
}
