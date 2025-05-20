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
                MySqlDataReader dataChecks = (MySqlDataReader)new DBConnection().Query("SELECT * FROM InventoryChecks", connection);
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
                    new DBConnection().Query("UPDATE InventoryChecks " +
                        "SET " +
                        $"InventoryId = {this.InventoryId}, " +
                        $"EquipmentId = {this.EquipmentId}, " +
                        $"UserId = {this.UserId}, " +
                        $"CheckDate = '{this.CheckDate.ToString("yyyy-MM-dd")}', " +
                        $"Comment = '{this.Comment}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO InventoryChecks " +
                        "(InventoryId, EquipmentId, UserId, CheckDate, Comment) " +
                        "VALUES (" +
                        $"{this.InventoryId}, " +
                        $"{this.EquipmentId}, " +
                        $"{this.UserId}, " +
                        $"'{this.CheckDate.ToString("yyyy-MM-dd")}', " +
                        $"'{this.Comment}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM InventoryChecks WHERE Id = {this.Id}", connection);
            }
        }
    }
}