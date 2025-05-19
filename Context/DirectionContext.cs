using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class DirectionContext : Direction, IDirection
    {
        public List<Direction> AllDirections()
        {
            List<Direction> allDirections = new List<Direction>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataDirections = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Directions", connection);
                while (dataDirections.Read())
                {
                    Direction newDirection = new Direction();
                    newDirection.Id = dataDirections.GetInt32(0);
                    newDirection.Name = dataDirections.GetString(1);
                    allDirections.Add(newDirection);
                }
            }
            return allDirections;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Directions " +
                        "SET " +
                        $"Name = '{this.Name}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Directions " +
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
                new DBConnection().Query($"DELETE FROM Directions WHERE Id = {this.Id}", connection);
            }
        }
    }
}
