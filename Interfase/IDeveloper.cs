using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IDeveloper
    {
        List<Developer> AllDevelopers();
        void Save(bool Update = false);
        void Delete();
    }
}
