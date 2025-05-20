using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentLocationHistoryContext : EquipmentLocationHistory, IEquipmentLocationHistory
    {
        public List<EquipmentLocationHistory> AllEquipmentLocationHistories()
        {
            List<EquipmentLocationHistory> allHistories = new List<EquipmentLocationHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query("SELECT * FROM EquipmentLocationHistories", connection);
                while (dataHistories.Read())
                {
                    EquipmentLocationHistory newHistory = new EquipmentLocationHistory();
                    newHistory.Id = dataHistories.GetInt32(0);
                    newHistory.EquipmentId = dataHistories.GetInt32(1);
                    newHistory.RoomId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2);
                    newHistory.ChangeDate = dataHistories.GetDateTime(3);
                    newHistory.Comment = dataHistories.GetString(4);
                    allHistories.Add(newHistory);
                }
            }
            return allHistories;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE EquipmentLocationHistories " +
                        "SET " +
                        $"EquipmentId = {this.EquipmentId}, " +
                        $"RoomId = {(this.RoomId.HasValue ? this.RoomId.ToString() : "NULL")}, " +
                        $"ChangeDate = '{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                        $"Comment = '{this.Comment}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO EquipmentLocationHistories " +
                        "(EquipmentId, RoomId, ChangeDate, Comment) " +
                        "VALUES (" +
                        $"{this.EquipmentId}, " +
                        $"{(this.RoomId.HasValue ? this.RoomId.ToString() : "NULL")}, " +
                        $"'{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                        $"'{this.Comment}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM EquipmentLocationHistories WHERE Id = {this.Id}", connection);
            }
        }
    }
}
