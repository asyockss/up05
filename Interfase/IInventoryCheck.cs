using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IInventoryCheck
    {
        List<InventoryCheck> AllInventoryChecks();
        void Save(bool Update = false);
        void Delete();
    }
}
