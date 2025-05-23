using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class EquipmentConsumable
    {
        public int Id { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }

        [ForeignKey("Consumable")]
        public int ConsumableId { get; set; }
        public virtual Consumable Consumable { get; set; }
    }
}