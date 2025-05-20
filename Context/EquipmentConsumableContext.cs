using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentConsumableContext : EquipmentConsumable, IEquipmentConsumable
    {
        public List<EquipmentConsumable> AllEquipmentConsumables()
        {
            List<EquipmentConsumable> allEquipmentConsumables = new List<EquipmentConsumable>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataEquipmentConsumables = (MySqlDataReader)new DBConnection().Query("SELECT * FROM equipment_consumables", connection);
                while (dataEquipmentConsumables.Read())
                {
                    EquipmentConsumable newEquipmentConsumable = new EquipmentConsumable();
                    newEquipmentConsumable.Id = dataEquipmentConsumables.GetInt32(0);
                    newEquipmentConsumable.EquipmentId = dataEquipmentConsumables.GetInt32(1);
                    newEquipmentConsumable.ConsumableId = dataEquipmentConsumables.GetInt32(2);
                    allEquipmentConsumables.Add(newEquipmentConsumable);
                }
            }
            return allEquipmentConsumables;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE equipment_consumables " +
                        "SET " +
                        $"equipment_id  = {this.EquipmentId}, " +
                        $"consumable_id  = {this.ConsumableId} " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO equipment_consumables " +
                        "(equipment_id , consumable_id ) " +
                        "VALUES (" +
                        $"{this.EquipmentId}, " +
                        $"{this.ConsumableId})", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM equipment_consumables WHERE id = {this.Id}", connection);
            }
        }
    }
}
