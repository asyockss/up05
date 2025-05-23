﻿using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Interfase
{
    public interface IEquipmentLocationHistory
    {
        List<EquipmentLocationHistory> AllEquipmentLocationHistories();
        void Save(EquipmentLocationHistory history, bool update = false);
        void Delete(int id);
    }
}
