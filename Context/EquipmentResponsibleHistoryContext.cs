using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentResponsibleHistoryContext : EquipmentResponsibleHistory, IEquipmentResponsibleHistory
    {
        public List<EquipmentResponsibleHistory> AllEquipmentResponsibleHistorys()
        {
            List<EquipmentResponsibleHistory> allHistories = new List<EquipmentResponsibleHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query(
                    "SELECT * FROM EquipmentResponsibleHistories",
                    connection);

                while (dataHistories.Read())
                {
                    allHistories.Add(new EquipmentResponsibleHistory
                    {
                        Id = dataHistories.GetInt32(0),
                        EquipmentId = dataHistories.GetInt32(1),
                        OldUserId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2),
                        ChangeDate = dataHistories.GetDateTime(3),
                        Comment = dataHistories.GetString(4)
                    });
                }
            }
            return allHistories;
        }

        public void Save(bool Update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                    ? "UPDATE EquipmentResponsibleHistories SET " +
                      $"EquipmentId = {this.EquipmentId}, " +
                      $"OldUserId = {(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                      $"ChangeDate = '{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                      $"Comment = '{this.Comment}' " +
                      $"WHERE Id = {this.Id}"
                    : "INSERT INTO EquipmentResponsibleHistories " +
                      "(EquipmentId, OldUserId, ChangeDate, Comment) " +
                      "VALUES (" +
                      $"{this.EquipmentId}, " +
                      $"{(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                      $"'{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                      $"'{this.Comment}')";

                new DBConnection().Query(query, connection);
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query(
                    $"DELETE FROM EquipmentResponsibleHistories WHERE Id = {this.Id}",
                    connection);
            }
        }
    }
}