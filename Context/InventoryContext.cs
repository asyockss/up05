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
        public InventoryContext() : base("name=up05")
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка связей между таблицами
            modelBuilder.Entity<Consumable>()
                .HasMany(c => c.Characteristics)
                .WithRequired(c => c.Consumable)
                .HasForeignKey(c => c.ConsumableId);

            modelBuilder.Entity<Consumable>()
                .HasMany(c => c.ResponsibleHistory)
                .WithRequired(c => c.Consumable)
                .HasForeignKey(c => c.ConsumableId);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.EquipmentConsumables)
                .WithRequired(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.LocationHistory)
                .WithRequired(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.ResponsibleHistory)
                .WithRequired(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.NetworkSettings)
                .WithRequired(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Software)
                .WithOptional(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Checks)
                .WithRequired(i => i.Inventory)
                .HasForeignKey(i => i.InventoryId);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Equipment)
                .WithOptional(e => e.Room)
                .HasForeignKey(e => e.RoomId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ResponsibleForEquipment)
                .WithOptional(e => e.ResponsibleUser)
                .HasForeignKey(e => e.ResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TempResponsibleForEquipment)
                .WithOptional(e => e.TempResponsibleUser)
                .HasForeignKey(e => e.TempResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ResponsibleForRooms)
                .WithOptional(r => r.ResponsibleUser)
                .HasForeignKey(r => r.ResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TempResponsibleForRooms)
                .WithOptional(r => r.TempResponsibleUser)
                .HasForeignKey(r => r.TempResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ResponsibleForConsumables)
                .WithOptional(c => c.ResponsibleUser)
                .HasForeignKey(c => c.ResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TempResponsibleForConsumables)
                .WithOptional(c => c.TempResponsibleUser)
                .HasForeignKey(c => c.TempResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Inventories)
                .WithRequired(i => i.User)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.InventoryChecks)
                .WithRequired(i => i.User)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.EquipmentResponsibleHistory)
                .WithOptional(e => e.OldUser)
                .HasForeignKey(e => e.OldUserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ConsumableResponsibleHistory)
                .WithOptional(c => c.OldUser)
                .HasForeignKey(c => c.OldUserId);
        }
    }
}