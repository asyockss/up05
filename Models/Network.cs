using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory.Models
{
    public class Network
    {
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }

        [Required(ErrorMessage = "IP-адрес обязателен")]
        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Неверный формат IP-адреса")]
        public string IpAddress { get; set; }

        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Неверный формат маски подсети")]
        public string SubnetMask { get; set; }

        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Неверный формат шлюза")]
        public string Gateway { get; set; }

        public string DnsServers { get; set; }
    }
}
