using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class EquipmentResponsibleHistory
    {
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }

        [ForeignKey("OldUser")]
        public int? OldUserId { get; set; }
        public virtual User OldUser { get; set; }

        public DateTime ChangeDate { get; set; }
        public string Comment { get; set; }
    }
}
