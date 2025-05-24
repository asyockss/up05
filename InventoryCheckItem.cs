using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory
{
    public class InventoryCheckItem
    {
        public Equipment Equipment { get; set; }
        public bool IsSelected { get; set; }
        public string CheckComment { get; set; }
    }
}
