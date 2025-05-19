using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class EquipmentConsumableContext : EquipmentConsumable, Interfaces.IEquipmentConsumable
    {
        public List<EquipmentConsumableContext> AllEquipmentConsumables()
        {
            List<EquipmentConsumableContext> allEquipmentConsumables = new List<EquipmentConsumableContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataEquipmentConsumables = Common.DBConnection.Query("SELECT * FROM EquipmentConsumables", connection);
            while (dataEquipmentConsumables.Read())
            {
                EquipmentConsumableContext newEquipmentConsumable = new EquipmentConsumableContext();
                newEquipmentConsumable.Id = dataEquipmentConsumables.GetInt32(0);
                newEquipmentConsumable.EquipmentId = dataEquipmentConsumables.GetInt32(1);
                newEquipmentConsumable.ConsumableId = dataEquipmentConsumables.GetInt32(2);
                allEquipmentConsumables.Add(newEquipmentConsumable);
            }
            Common.DBConnection.CloseConnection(connection);
            return allEquipmentConsumables;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE EquipmentConsumables " +
                    "SET " +
                    $"EquipmentId = {this.EquipmentId}, " +
                    $"ConsumableId = {this.ConsumableId} " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO EquipmentConsumables " +
                    "(EquipmentId, ConsumableId) " +
                    "VALUES (" +
                    $"{this.EquipmentId}, " +
                    $"{this.ConsumableId})", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM EquipmentConsumables WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
