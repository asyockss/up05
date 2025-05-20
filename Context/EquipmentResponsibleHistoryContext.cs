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
                    "SELECT * FROM equipment_responsible_history",
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
                    ? "UPDATE equipment_responsible_history SET " +
                      $"equipment_id = {this.EquipmentId}, " +
                      $"old_user_id = {(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                      $"change_date = '{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                      $"comment = '{this.Comment}' " +
                      $"WHERE Id = {this.Id}"
                    : "INSERT INTO equipment_responsible_history " +
                      "(equipment_id, old_user_id , change_date, comment) " +
                      "VALUES (" +
                      $"{this.EquipmentId}, " +
                      $"{(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                      $"'{this.ChangeDate.ToString("yyyy-MM-dd")}', " +
                      $"'{this.Comment}')";

                new DBConnection().Query(query, connection);
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query(
                    $"DELETE FROM equipment_responsible_history WHERE id = {this.Id}",
                    connection);
            }
        }
    }
}