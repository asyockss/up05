using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название статуса обязательно")]
        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public Status()
        {
            Equipment = new HashSet<Equipment>();
        }
    }
}
