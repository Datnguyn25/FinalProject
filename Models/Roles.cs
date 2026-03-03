using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Roles
    {
        [Key] // Primary Key
        public int RoleId { get; set; }

        [Required] // Required field
        [StringLength(50)]
        public string RoleName { get; set; }
        //khglygglglglglyg


    }
}
