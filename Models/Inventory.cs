using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название инвентаризации обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateTime EndDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<InventoryCheck> Checks { get; set; }

        public Inventory()
        {
            Checks = new HashSet<InventoryCheck>();
        }
    }
}

