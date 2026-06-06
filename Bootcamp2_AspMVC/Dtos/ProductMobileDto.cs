namespace Bootcamp2_AspMVC.Dtos
{
    public class ProductMobileDto
    {
        public int Id { get; set; }
        public string uid { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int Qty { get; set; }
        public int ReservedQty { get; set; }
        public int AvailableQty { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
