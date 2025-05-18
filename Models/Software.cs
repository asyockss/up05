using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Software
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название программы обязательно")]
        public string Name { get; set; }

        public string Version { get; set; }

        [ForeignKey("Developer")]
        public int? DeveloperId { get; set; }
        public virtual Developer Developer { get; set; }

        [ForeignKey("Equipment")]
        public int? EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
}
