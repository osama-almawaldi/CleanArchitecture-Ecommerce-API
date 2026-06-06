using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootCamp2.Domin.Domins.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string uid { get; set; } = Guid.NewGuid().ToString();


        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public int Qty { get; set; }

      //  public int StockQty { get; set; }         // المخزون المتاح
        public int ReservedQty { get; set; }      // المحجوز مؤقتاً
        public byte[]? RowVersion { get; set; }   // للمنافسة المتزامنة (Concurrency)
        public string? Description { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
    }
}
