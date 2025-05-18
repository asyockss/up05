using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class EquipmentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название модели обязательно")]
        public string Name { get; set; }

        [ForeignKey("EquipmentType")]
        public int EquipmentTypeId { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public EquipmentModel()
        {
            Equipment = new HashSet<Equipment>();
        }
    }
}
}
