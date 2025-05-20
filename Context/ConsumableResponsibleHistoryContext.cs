using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableResponsibleHistoryContext : ConsumableResponsibleHistory, IConsumableResponsibleHistory
    {
        public List<ConsumableResponsibleHistory> AllConsumableResponsibleHistories()
        {
            List<ConsumableResponsibleHistory> allHistories = new List<ConsumableResponsibleHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query("SELECT * FROM consumable_responsible_history", connection);
                while (dataHistories.Read())
                {
                    ConsumableResponsibleHistory newHistory = new ConsumableResponsibleHistory();
                    newHistory.Id = dataHistories.GetInt32(0);
                    newHistory.ConsumableId = dataHistories.GetInt32(1);
                    newHistory.OldUserId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2);
                    newHistory.ChangeDate = dataHistories.GetDateTime(3);
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
                    new DBConnection().Query("UPDATE consumable_responsible_history " +
                        "SET " +
                        $"consumable_id  = {this.ConsumableId}, " +
                        $"old_user_id  = {(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                        $"change_date = '{this.ChangeDate.ToString("yyyy-MM-dd")}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO consumable_responsible_history " +
                        "(consumable_id , old_user_id, change_date) " +
                        "VALUES (" +
                        $"{this.ConsumableId}, " +
                        $"{(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                        $"'{this.ChangeDate.ToString("yyyy-MM-dd")}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM consumable_responsible_history WHERE id = {this.Id}", connection);
            }
        }
    }
}
