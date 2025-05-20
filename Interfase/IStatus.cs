using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IStatus
    {
        List<Status> AllStatuses();
        void Save(bool Update = false);
        void Delete(int id);
    }
}
