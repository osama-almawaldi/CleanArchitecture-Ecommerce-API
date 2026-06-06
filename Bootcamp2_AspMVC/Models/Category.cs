using System.ComponentModel.DataAnnotations;

namespace Bootcamp2_AspMVC.Models
{
    public class Category 
    {
        [Key]
        public int Id { get; set; }

        public string uid { get; set; }=Guid.NewGuid().ToString();

        
        public string ImageUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string? Description { get; set; }


        public virtual ICollection<Product>? Products { get; set; }



    }
}
