using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class EquipmentLocationHistoryContext : EquipmentLocationHistory, Interfaces.IEquipmentLocationHistory
    {
        public List<EquipmentLocationHistoryContext> AllEquipmentLocationHistories()
        {
            List<EquipmentLocationHistoryContext> allHistories = new List<EquipmentLocationHistoryContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataHistories = Common.DBConnection.Query("SELECT * FROM EquipmentLocationHistories", connection);
            while (dataHistories.Read())
            {
                EquipmentLocationHistoryContext newHistory = new EquipmentLocationHistoryContext();
                newHistory.Id = dataHistories.GetInt32(0);
                newHistory.EquipmentId = dataHistories.GetInt32(1);
                newHistory.RoomId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2);
                newHistory.ChangeDate = dataHistories.GetDateTime(3);
                newHistory.Comment = dataHistories.GetString(4);
                allHistories.Add(newHistory);
            }
            Common.DBConnection.CloseConnection(connection);
            return allHistories;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE EquipmentLocationHistories " +
                    "SET " +
                    $"EquipmentId = {this.EquipmentId}, " +
                    $"RoomId = {(this.RoomId.HasValue ? this.RoomId.ToString() : "NULL")}, " +
                    $"ChangeDate = '{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                    $"Comment = '{this.Comment}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO EquipmentLocationHistories " +
                    "(EquipmentId, RoomId, ChangeDate, Comment) " +
                    "VALUES (" +
                    $"{this.EquipmentId}, " +
                    $"{this.RoomId.HasValue ? this.RoomId.ToString() : "NULL"}, " +
                    $"'{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                    $"'{this.Comment}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM EquipmentLocationHistories WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
