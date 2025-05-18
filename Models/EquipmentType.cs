using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class EquipmentType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа оборудования обязательно")]
        public string Name { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
        public virtual ICollection<EquipmentModel> Models { get; set; }

        public EquipmentType()
        {
            Equipment = new HashSet<Equipment>();
            Models = new HashSet<EquipmentModel>();
        }
    }
}
