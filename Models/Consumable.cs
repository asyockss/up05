using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Consumable
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Название расходника обязательно")]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ReceiptDate { get; set; }

        public byte[] Image { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть положительным")]
        public int Quantity { get; set; }

        [ForeignKey("ResponsibleUser")]
        public int? ResponsibleId { get; set; }
        public virtual User ResponsibleUser { get; set; }

        [ForeignKey("TempResponsibleUser")]
        public int? TempResponsibleId { get; set; }
        public virtual User TempResponsibleUser { get; set; }

        [ForeignKey("ConsumableType")]
        public int? ConsumableTypeId { get; set; }
        public virtual ConsumableType ConsumableType { get; set; }

        public virtual ICollection<EquipmentConsumable> EquipmentConsumables { get; set; }
        public virtual ICollection<ConsumableCharacteristicValue> Characteristics { get; set; }
        public virtual ICollection<ConsumableResponsibleHistory> ResponsibleHistory { get; set; }

        public Consumable()
        {
            EquipmentConsumables = new HashSet<EquipmentConsumable>();
            Characteristics = new HashSet<ConsumableCharacteristicValue>();
            ResponsibleHistory = new HashSet<ConsumableResponsibleHistory>();
            ReceiptDate = DateTime.Now;
        }
    }
}
