using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class ConsumableCharacteristicValue
    {
        public int Id { get; set; }

        [ForeignKey("Characteristic")]
        public int CharacteristicId { get; set; }
        public virtual ConsumableCharacteristic Characteristic { get; set; }

        [ForeignKey("Consumable")]
        public int ConsumableId { get; set; }
        public virtual Consumable Consumable { get; set; }

        [Required(ErrorMessage = "Значение характеристики обязательно")]
        public string CharacteristicValue { get; set; }
    }
}
