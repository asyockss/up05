using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class UserContext : User, IUser
    {
        public List<User> AllUsers()
        {
            List<User> allUsers = new List<User>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataUsers = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Users", connection);
                while (dataUsers.Read())
                {
                    User newUser = new User();
                    newUser.Id = dataUsers.GetInt32(0);
                    newUser.Login = dataUsers.GetString(1);
                    newUser.Password = dataUsers.GetString(2);
                    newUser.Role = dataUsers.GetString(3);
                    newUser.Email = dataUsers.GetString(4);
                    newUser.LastName = dataUsers.GetString(5);
                    newUser.FirstName = dataUsers.GetString(6);
                    newUser.MiddleName = dataUsers.GetString(7);
                    newUser.Phone = dataUsers.GetString(8);
                    newUser.Address = dataUsers.GetString(9);
                    allUsers.Add(newUser);
                }
            }
            return allUsers;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Users " +
                        "SET " +
                        $"Login = '{this.Login}', " +
                        $"Password = '{this.Password}', " +
                        $"Role = '{this.Role}', " +
                        $"Email = '{this.Email}', " +
                        $"LastName = '{this.LastName}', " +
                        $"FirstName = '{this.FirstName}', " +
                        $"MiddleName = '{this.MiddleName}', " +
                        $"Phone = '{this.Phone}', " +
                        $"Address = '{this.Address}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Users " +
                        "(Login, Password, Role, Email, LastName, FirstName, MiddleName, Phone, Address) " +
                        "VALUES (" +
                        $"'{this.Login}', " +
                        $"'{this.Password}', " +
                        $"'{this.Role}', " +
                        $"'{this.Email}', " +
                        $"'{this.LastName}', " +
                        $"'{this.FirstName}', " +
                        $"'{this.MiddleName}', " +
                        $"'{this.Phone}', " +
                        $"'{this.Address}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Users WHERE Id = {this.Id}", connection);
            }
        }
    }
}