using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class DeveloperContext : Developer, Interfaces.IDeveloper
    {
        public List<DeveloperContext> AllDevelopers()
        {
            List<DeveloperContext> allDevelopers = new List<DeveloperContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataDevelopers = Common.DBConnection.Query("SELECT * FROM Developers", connection);
            while (dataDevelopers.Read())
            {
                DeveloperContext newDeveloper = new DeveloperContext();
                newDeveloper.Id = dataDevelopers.GetInt32(0);
                newDeveloper.Name = dataDevelopers.GetString(1);
                allDevelopers.Add(newDeveloper);
            }
            Common.DBConnection.CloseConnection(connection);
            return allDevelopers;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE Developers " +
                    "SET " +
                    $"Name = '{this.Name}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO Developers " +
                    "(Name) " +
                    "VALUES (" +
                    $"'{this.Name}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM Developers WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
