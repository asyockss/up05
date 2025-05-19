using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IConsumableResponsibleHistory
    {
        List<ConsumableResponsibleHistory> AllConsumableResponsibleHistories();
        void Save(bool Update = false);
        void Delete();
    }
}
