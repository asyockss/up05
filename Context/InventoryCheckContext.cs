using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class InventoryCheckContext : InventoryCheck, IInventoryCheck
    {
        public List<InventoryCheck> AllInventoryChecks()
        {
            List<InventoryCheck> allChecks = new List<InventoryCheck>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataChecks = (MySqlDataReader)new DBConnection().Query("SELECT * FROM inventory_checks", connection);
                while (dataChecks.Read())
                {
                    InventoryCheck newCheck = new InventoryCheck();
                    newCheck.Id = dataChecks.GetInt32(0);
                    newCheck.InventoryId = dataChecks.GetInt32(1);
                    newCheck.EquipmentId = dataChecks.GetInt32(2);
                    newCheck.UserId = dataChecks.GetInt32(3);
                    newCheck.CheckDate = dataChecks.GetDateTime(4);
                    newCheck.Comment = dataChecks.GetString(5);
                    allChecks.Add(newCheck);
                }
            }
            return allChecks;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE inventory_checks " +
                        "SET " +
                        $"inventory_id = {this.InventoryId}, " +
                        $"equipment_id = {this.EquipmentId}, " +
                        $"users_id = {this.UserId}, " +
                        $"check_date = '{this.CheckDate.ToString("yyyy-MM-dd")}', " +
                        $"comment = '{this.Comment}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO inventory_checks " +
                        "(inventory_id, equipment_id, users_id, check_date, comment) " +
                        "VALUES (" +
                        $"{this.InventoryId}, " +
                        $"{this.EquipmentId}, " +
                        $"{this.UserId}, " +
                        $"'{this.CheckDate.ToString("yyyy-MM-dd")}', " +
                        $"'{this.Comment}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM inventory_checks WHERE id = {this.Id}", connection);
            }
        }
    }
}