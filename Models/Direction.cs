using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Direction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название направления обязательно")]
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public Direction()
        {
            Equipment = new HashSet<Equipment>();
        }
    }
}
