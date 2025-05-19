using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableContext : Consumable, IConsumable
    {
        public List<Consumable> AllConsumables()
        {
            List<Consumable> allConsumables = new List<Consumable>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataConsumables = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Consumables", connection);
                while (dataConsumables.Read())
                {
                    Consumable newConsumable = new Consumable();
                    newConsumable.Id = dataConsumables.GetInt32(0);
                    newConsumable.Name = dataConsumables.GetString(1);
                    newConsumable.Description = dataConsumables.GetString(2);
                    newConsumable.ReceiptDate = dataConsumables.GetDateTime(3);
                    newConsumable.Image = (byte[])dataConsumables["Image"];
                    newConsumable.Quantity = dataConsumables.GetInt32(4);
                    newConsumable.ResponsibleId = dataConsumables.IsDBNull(5) ? (int?)null : dataConsumables.GetInt32(5);
                    newConsumable.TempResponsibleId = dataConsumables.IsDBNull(6) ? (int?)null : dataConsumables.GetInt32(6);
                    newConsumable.ConsumableTypeId = dataConsumables.IsDBNull(7) ? (int?)null : dataConsumables.GetInt32(7);
                    allConsumables.Add(newConsumable);
                }
            }
            return allConsumables;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Consumables " +
                        "SET " +
                        $"Name = '{this.Name}', " +
                        $"Description = '{this.Description}', " +
                        $"ReceiptDate = '{this.ReceiptDate.ToString("yyyy-MM-dd")}', " +
                        $"Image = @Image, " +
                        $"Quantity = {this.Quantity}, " +
                        $"ResponsibleId = {(this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL")}, " +
                        $"TempResponsibleId = {(this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL")}, " +
                        $"ConsumableTypeId = {(this.ConsumableTypeId.HasValue ? this.ConsumableTypeId.ToString() : "NULL")} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Consumables " +
                        "(Name, Description, ReceiptDate, Image, Quantity, ResponsibleId, TempResponsibleId, ConsumableTypeId) " +
                        "VALUES (" +
                        $"'{this.Name}', " +
                        $"'{this.Description}', " +
                        $"'{this.ReceiptDate.ToString("yyyy-MM-dd")}', " +
                        "@Image, " +
                        $"{this.Quantity}, " +
                        $"{(this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL")}, " +
                        $"{(this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL")}, " +
                        $"{(this.ConsumableTypeId.HasValue ? this.ConsumableTypeId.ToString() : "NULL")})", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Consumables WHERE Id = {this.Id}", connection);
            }
        }
    }
}
