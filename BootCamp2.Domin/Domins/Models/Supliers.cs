using System.ComponentModel.DataAnnotations;

namespace BootCamp2.Domin.Domins.Models
{
    public class Supliers
    {

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
    }
}
