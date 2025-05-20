using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class StatusContext : Status, IStatus
    {
        public List<Status> AllStatuses()
        {
            List<Status> allStatuses = new List<Status>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataStatuses = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Statuses", connection);
                while (dataStatuses.Read())
                {
                    Status newStatus = new Status();
                    newStatus.Id = dataStatuses.GetInt32(0);
                    newStatus.Name = dataStatuses.GetString(1);
                    allStatuses.Add(newStatus);
                }
            }
            return allStatuses;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Statuses " +
                        "SET " +
                        $"Name = '{this.Name}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Statuses " +
                        "(Name) " +
                        "VALUES (" +
                        $"'{this.Name}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Statuses WHERE Id = {this.Id}", connection);
            }
        }
    }
}