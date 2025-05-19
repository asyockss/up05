using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class DirectionContext : Direction, Interfaces.IDirection
    {
        public List<DirectionContext> AllDirections()
        {
            List<DirectionContext> allDirections = new List<DirectionContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataDirections = Common.DBConnection.Query("SELECT * FROM Directions", connection);
            while (dataDirections.Read())
            {
                DirectionContext newDirection = new DirectionContext();
                newDirection.Id = dataDirections.GetInt32(0);
                newDirection.Name = dataDirections.GetString(1);
                allDirections.Add(newDirection);
            }
            Common.DBConnection.CloseConnection(connection);
            return allDirections;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE Directions " +
                    "SET " +
                    $"Name = '{this.Name}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO Directions " +
                    "(Name) " +
                    "VALUES (" +
                    $"'{this.Name}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM Directions WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
