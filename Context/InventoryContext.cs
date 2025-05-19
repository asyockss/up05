using inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Context
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() : base("name=InventoryContext")
        {
        }

        public DbSet<Consumable> Consumables { get; set; }
        public DbSet<ConsumableCharacteristic> ConsumableCharacteristics { get; set; }
        public DbSet<ConsumableCharacteristicValue> ConsumableCharacteristicValues { get; set; }
        public DbSet<ConsumableResponsibleHistory> ConsumableResponsibleHistories { get; set; }
        public DbSet<ConsumableType> ConsumableTypes { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentConsumable> EquipmentConsumables { get; set; }
        public DbSet<EquipmentLocationHistory> EquipmentLocationHistories { get; set; }
        public DbSet<EquipmentModel> EquipmentModels { get; set; }
        public DbSet<EquipmentResponsibleHistory> EquipmentResponsibleHistories { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryCheck> InventoryChecks { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
