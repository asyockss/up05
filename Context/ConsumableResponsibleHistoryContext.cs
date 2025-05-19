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
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query("SELECT * FROM ConsumableResponsibleHistories", connection);
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
                    new DBConnection().Query("UPDATE ConsumableResponsibleHistories " +
                        "SET " +
                        $"ConsumableId = {this.ConsumableId}, " +
                        $"OldUserId = {(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                        $"ChangeDate = '{this.ChangeDate.ToString("yyyy-MM-dd")}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO ConsumableResponsibleHistories " +
                        "(ConsumableId, OldUserId, ChangeDate) " +
                        "VALUES (" +
                        $"{this.ConsumableId}, " +
                        $"{(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                        $"'{this.ChangeDate.ToString("yyyy-MM-dd")}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM ConsumableResponsibleHistories WHERE Id = {this.Id}", connection);
            }
        }
    }
}
