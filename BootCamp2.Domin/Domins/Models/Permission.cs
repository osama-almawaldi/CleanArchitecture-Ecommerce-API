using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootCamp2.Domin.Domins.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public bool IsCategory { get; set; } = false;
        public bool IsProduct { get; set; } = false;
        public bool IsEmployee { get; set; } = false;


        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }



    }
}
