using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class EquipmentLocationHistory
    {
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public DateTime ChangeDate { get; set; }
        public string Comment { get; set; }
    }
}
