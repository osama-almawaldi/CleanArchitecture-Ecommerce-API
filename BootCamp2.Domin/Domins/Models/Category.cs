using System.ComponentModel.DataAnnotations;

namespace BootCamp2.Domin.Domins.Models
{
    public class Category 
    {
        [Key]
        public int Id { get; set; }

        public string uid { get; set; }=Guid.NewGuid().ToString();


        [Required]
        public string Name { get; set; }

        [Required]
        public string? Description { get; set; }


        public virtual ICollection<Product>? Products { get; set; }



    }
}
