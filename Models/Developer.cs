using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class Developer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название разработчика обязательно")]
        public string Name { get; set; }

        public virtual ICollection<Software> Software { get; set; }

        public Developer()
        {
            Software = new HashSet<Software>();
        }
    }
}

