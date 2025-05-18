using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название оборудования обязательно")]
        [StringLength(255, ErrorMessage = "Название не должно превышать 255 символов")]
        public string Name { get; set; }

        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Инвентарный номер обязателен")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Инвентарный номер должен содержать только цифры")]
        public string InventoryNumber { get; set; }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("ResponsibleUser")]
        public int? ResponsibleId { get; set; }
        public virtual User ResponsibleUser { get; set; }

        [ForeignKey("TempResponsibleUser")]
        public int? TempResponsibleId { get; set; }
        public virtual User TempResponsibleUser { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Стоимость должна быть положительной")]
        public decimal Cost { get; set; }

        public string Comment { get; set; }

        [ForeignKey("Status")]
        public int? StatusId { get; set; }
        public virtual Status Status { get; set; }

        [ForeignKey("EquipmentModel")]
        public int? ModelId { get; set; }
        public virtual EquipmentModel EquipmentModel { get; set; }

        [ForeignKey("EquipmentType")]
        public int? EquipmentTypeId { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }

        [ForeignKey("Direction")]
        public int? DirectionId { get; set; }
        public virtual Direction Direction { get; set; }

        public virtual ICollection<Software> Software { get; set; }
        public virtual ICollection<Network> NetworkSettings { get; set; }
        public virtual ICollection<EquipmentConsumable> EquipmentConsumables { get; set; }
        public virtual ICollection<EquipmentLocationHistory> LocationHistory { get; set; }
        public virtual ICollection<EquipmentResponsibleHistory> ResponsibleHistory { get; set; }

        public Equipment()
        {
            Software = new HashSet<Software>();
            NetworkSettings = new HashSet<Network>();
            EquipmentConsumables = new HashSet<EquipmentConsumable>();
            LocationHistory = new HashSet<EquipmentLocationHistory>();
            ResponsibleHistory = new HashSet<EquipmentResponsibleHistory>();
        }
    }
}
}
