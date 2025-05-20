using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class DeveloperContext : Developer, IDeveloper
    {
        public  List<Developer> AllDevelopers()
        {
            List<Developer> allDevelopers = new List<Developer>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataDevelopers = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Developers", connection);
                while (dataDevelopers.Read())
                {
                    Developer newDeveloper = new Developer();
                    newDeveloper.Id = dataDevelopers.GetInt32(0);
                    newDeveloper.Name = dataDevelopers.GetString(1);
                    allDevelopers.Add(newDeveloper);
                }
            }
            return allDevelopers;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Developers " +
                        "SET " +
                        $"Name = '{this.Name}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Developers " +
                        "(Name) " +
                        "VALUES (" +
                        $"'{this.Name}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Developers WHERE Id = {this.Id}", connection);
            }
        }
    }
}
