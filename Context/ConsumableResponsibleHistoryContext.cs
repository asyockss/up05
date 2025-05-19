using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class ConsumableResponsibleHistoryContext : ConsumableResponsibleHistory, Interfaces.IConsumableResponsibleHistory
    {
        public List<ConsumableResponsibleHistoryContext> AllConsumableResponsibleHistories()
        {
            List<ConsumableResponsibleHistoryContext> allHistories = new List<ConsumableResponsibleHistoryContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataHistories = Common.DBConnection.Query("SELECT * FROM ConsumableResponsibleHistories", connection);
            while (dataHistories.Read())
            {
                ConsumableResponsibleHistoryContext newHistory = new ConsumableResponsibleHistoryContext();
                newHistory.Id = dataHistories.GetInt32(0);
                newHistory.ConsumableId = dataHistories.GetInt32(1);
                newHistory.OldUserId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2);
                newHistory.ChangeDate = dataHistories.GetDateTime(3);
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
                Common.DBConnection.Query("UPDATE ConsumableResponsibleHistories " +
                    "SET " +
                    $"ConsumableId = {this.ConsumableId}, " +
                    $"OldUserId = {(this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL")}, " +
                    $"ChangeDate = '{this.ChangeDate.ToString("yyyy-MM-dd")}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO ConsumableResponsibleHistories " +
                    "(ConsumableId, OldUserId, ChangeDate) " +
                    "VALUES (" +
                    $"{this.ConsumableId}, " +
                    $"{this.OldUserId.HasValue ? this.OldUserId.ToString() : "NULL"}, " +
                    $"'{this.ChangeDate.ToString("yyyy-MM-dd")}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM ConsumableResponsibleHistories WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
