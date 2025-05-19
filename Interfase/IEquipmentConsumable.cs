using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IEquipmentConsumable
    {
        List<EquipmentConsumable> AllEquipmentConsumables();
        void Save(bool Update = false);
        void Delete();
    }
}
