using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class ConsumableResponsibleHistory
    {
        public int Id { get; set; }

        [ForeignKey("Consumable")]
        public int ConsumableId { get; set; }
        public virtual Consumable Consumable { get; set; }

        [ForeignKey("OldUser")]
        public int? OldUserId { get; set; }
        public virtual User OldUser { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
