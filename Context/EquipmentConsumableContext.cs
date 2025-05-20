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
                MySqlDataReader dataEquipmentConsumables = (MySqlDataReader)new DBConnection().Query("SELECT * FROM EquipmentConsumables", connection);
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
                    new DBConnection().Query("UPDATE EquipmentConsumables " +
                        "SET " +
                        $"EquipmentId = {this.EquipmentId}, " +
                        $"ConsumableId = {this.ConsumableId} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO EquipmentConsumables " +
                        "(EquipmentId, ConsumableId) " +
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
                new DBConnection().Query($"DELETE FROM EquipmentConsumables WHERE Id = {this.Id}", connection);
            }
        }
    }
}
