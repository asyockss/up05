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
                MySqlDataReader dataStatuses = (MySqlDataReader)new DBConnection().Query("SELECT * FROM status", connection);
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
                    new DBConnection().Query("UPDATE status " +
                        "SET " +
                        $"name = '{this.Name}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO status " +
                        "(name) " +
                        "VALUES (" +
                        $"'{this.Name}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM status WHERE id = {this.Id}", connection);
            }
        }
    }
}