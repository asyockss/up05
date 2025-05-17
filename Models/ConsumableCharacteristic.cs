using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class ConsumableCharacteristic
    {
        public int Id { get; set; }

        [ForeignKey("ConsumableType")]
        public int ConsumableTypeId { get; set; }
        public virtual ConsumableType ConsumableType { get; set; }

        [Required(ErrorMessage = "Название характеристики обязательно")]
        public string CharacteristicName { get; set; }

        public virtual ICollection<ConsumableCharacteristicValue> Values { get; set; }

        public ConsumableCharacteristic()
        {
            Values = new HashSet<ConsumableCharacteristicValue>();
        }
    }
}
