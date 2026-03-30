using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_SystemSetting")]
    public class SystemSetting
    {
        [Key]
        public int ID { get; set; }

        public string SiteName { get; set; }

        public string Email { get; set; }

        public decimal ShippingFee { get; set; }

        public bool MaintenanceMode { get; set; }
    }
}