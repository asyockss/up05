using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название аудитории обязательно")]
        public string Name { get; set; }

        public string ShortName { get; set; }

        [ForeignKey("ResponsibleUser")]
        public int? ResponsibleId { get; set; }
        public virtual User ResponsibleUser { get; set; }

        [ForeignKey("TempResponsibleUser")]
        public int? TempResponsibleId { get; set; }
        public virtual User TempResponsibleUser { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public Room()
        {
            Equipment = new HashSet<Equipment>();
        }
    }
}

