using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_SystemLog")]
    public class SystemLog
    {
        [Key]
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}