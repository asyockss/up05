﻿using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IEquipmentResponsibleHistory
    {
        List<EquipmentResponsibleHistory> AllEquipmentResponsibleHistorys();
        void Save(EquipmentResponsibleHistory history, bool update = false);
        void Delete(int id);
    }
}
