using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class InventoryCheck
    {
        public int Id { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CheckDate { get; set; }
        public string Comment { get; set; }
    }
}
