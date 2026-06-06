namespace Bootcamp2_AspMVC.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }            // المخزون المتاح

    
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }


    }
}
