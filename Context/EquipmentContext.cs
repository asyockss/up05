using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class EquipmentContext : Equipment, Interfaces.IEquipment
    {
        public List<EquipmentContext> AllEquipment()
        {
            List<EquipmentContext> allEquipment = new List<EquipmentContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataEquipment = Common.DBConnection.Query("SELECT * FROM Equipment", connection);
            while (dataEquipment.Read())
            {
                EquipmentContext newEquipment = new EquipmentContext();
                newEquipment.Id = dataEquipment.GetInt32(0);
                newEquipment.Name = dataEquipment.GetString(1);
                newEquipment.Photo = (byte[])dataEquipment["Photo"];
                newEquipment.InventoryNumber = dataEquipment.GetString(2);
                newEquipment.RoomId = dataEquipment.IsDBNull(3) ? (int?)null : dataEquipment.GetInt32(3);
                newEquipment.ResponsibleId = dataEquipment.IsDBNull(4) ? (int?)null : dataEquipment.GetInt32(4);
                newEquipment.TempResponsibleId = dataEquipment.IsDBNull(5) ? (int?)null : dataEquipment.GetInt32(5);
                newEquipment.Cost = dataEquipment.GetDecimal(6);
                newEquipment.Comment = dataEquipment.GetString(7);
                newEquipment.StatusId = dataEquipment.IsDBNull(8) ? (int?)null : dataEquipment.GetInt32(8);
                newEquipment.ModelId = dataEquipment.IsDBNull(9) ? (int?)null : dataEquipment.GetInt32(9);
                newEquipment.EquipmentTypeId = dataEquipment.IsDBNull(10) ? (int?)null : dataEquipment.GetInt32(10);
                newEquipment.DirectionId = dataEquipment.IsDBNull(11) ? (int?)null : dataEquipment.GetInt32(11);
                allEquipment.Add(newEquipment);
            }
            Common.DBConnection.CloseConnection(connection);
            return allEquipment;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE Equipment " +
                    "SET " +
                    $"Name = '{this.Name}', " +
                    $"Photo = @Photo, " +
                    $"InventoryNumber = '{this.InventoryNumber}', " +
                    $"RoomId = {(this.RoomId.HasValue ? this.RoomId.ToString() : "NULL")}, " +
                    $"ResponsibleId = {(this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL")}, " +
                    $"TempResponsibleId = {(this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL")}, " +
                    $"Cost = {this.Cost}, " +
                    $"Comment = '{this.Comment}', " +
                    $"StatusId = {(this.StatusId.HasValue ? this.StatusId.ToString() : "NULL")}, " +
                    $"ModelId = {(this.ModelId.HasValue ? this.ModelId.ToString() : "NULL")}, " +
                    $"EquipmentTypeId = {(this.EquipmentTypeId.HasValue ? this.EquipmentTypeId.ToString() : "NULL")}, " +
                    $"DirectionId = {(this.DirectionId.HasValue ? this.DirectionId.ToString() : "NULL")} " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO Equipment " +
                    "(Name, Photo, InventoryNumber, RoomId, ResponsibleId, TempResponsibleId, Cost, Comment, StatusId, ModelId, EquipmentTypeId, DirectionId) " +
                    "VALUES (" +
                    $"'{this.Name}', " +
                    "@Photo, " +
                    $"'{this.InventoryNumber}', " +
                    $"{this.RoomId.HasValue ? this.RoomId.ToString() : "NULL"}, " +
                    $"{this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL"}, " +
                    $"{this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL"}, " +
                    $"{this.Cost}, " +
                    $"'{this.Comment}', " +
                    $"{this.StatusId.HasValue ? this.StatusId.ToString() : "NULL"}, " +
                    $"{this.ModelId.HasValue ? this.ModelId.ToString() : "NULL"}, " +
                    $"{this.EquipmentTypeId.HasValue ? this.EquipmentTypeId.ToString() : "NULL"}, " +
                    $"{this.DirectionId.HasValue ? this.DirectionId.ToString() : "NULL"})", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM Equipment WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
