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
        public int EquipmentId { get; set; }
        public int? RoomId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Comment { get; set; }
    }
}
