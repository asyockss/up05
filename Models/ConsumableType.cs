using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class ConsumableType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Тип расходника обязателен")]
        public string Type { get; set; }

        public virtual ICollection<Consumable> Consumables { get; set; }
        public virtual ICollection<ConsumableCharacteristic> Characteristics { get; set; }

        public ConsumableType()
        {
            Consumables = new HashSet<Consumable>();
            Characteristics = new HashSet<ConsumableCharacteristic>();
        }
    }
}
