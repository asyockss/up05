using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class InventoryContext : Inventory, IInventory
    {
        public List<Inventory> AllInventorys()
        {
            List<Inventory> allInventories = new List<Inventory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataInventories = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Inventories", connection);
                while (dataInventories.Read())
                {
                    Inventory newInventory = new Inventory();
                    newInventory.Id = dataInventories.GetInt32(0);
                    newInventory.Name = dataInventories.GetString(1);
                    newInventory.StartDate = dataInventories.GetDateTime(2);
                    newInventory.EndDate = dataInventories.GetDateTime(3);
                    newInventory.UserId = dataInventories.GetInt32(4);
                    allInventories.Add(newInventory);
                }
            }
            return allInventories;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Inventories " +
                        "SET " +
                        $"Name = '{this.Name}', " +
                        $"StartDate = '{this.StartDate.ToString("yyyy-MM-dd")}', " +
                        $"EndDate = '{this.EndDate.ToString("yyyy-MM-dd")}', " +
                        $"UserId = {this.UserId} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Inventories " +
                        "(Name, StartDate, EndDate, UserId) " +
                        "VALUES (" +
                        $"'{this.Name}', " +
                        $"'{this.StartDate.ToString("yyyy-MM-dd")}', " +
                        $"'{this.EndDate.ToString("yyyy-MM-dd")}', " +
                        $"{this.UserId})", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Inventories WHERE Id = {this.Id}", connection);
            }
        }
    }
}