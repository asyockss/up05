using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class ConsumableContext : Consumable//, Interfaces.IConsumable
    {
        public List<ConsumableContext> AllConsumables()
        {
            List<ConsumableContext> allConsumables = new List<ConsumableContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataConsumables = Common.DBConnection.Query("SELECT * FROM Consumables", connection);
            while (dataConsumables.Read())
            {
                ConsumableContext newConsumable = new ConsumableContext();
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
            Common.DBConnection.CloseConnection(connection);
            return allConsumables;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE Consumables " +
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
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO Consumables " +
                    "(Name, Description, ReceiptDate, Image, Quantity, ResponsibleId, TempResponsibleId, ConsumableTypeId) " +
                    "VALUES (" +
                    $"'{this.Name}', " +
                    $"'{this.Description}', " +
                    $"'{this.ReceiptDate.ToString("yyyy-MM-dd")}', " +
                    "@Image, " +
                    $"{this.Quantity}, " +
                    $"{this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL"}, " +
                    $"{this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL"}, " +
                    $"{this.ConsumableTypeId.HasValue ? this.ConsumableTypeId.ToString() : "NULL"})", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM Consumables WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
