using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class ConsumableTypeContext : ConsumableType, Interfaces.IConsumableType
    {
        public List<ConsumableTypeContext> AllConsumableTypes()
        {
            List<ConsumableTypeContext> allTypes = new List<ConsumableTypeContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataTypes = Common.DBConnection.Query("SELECT * FROM ConsumableTypes", connection);
            while (dataTypes.Read())
            {
                ConsumableTypeContext newType = new ConsumableTypeContext();
                newType.Id = dataTypes.GetInt32(0);
                newType.Type = dataTypes.GetString(1);
                allTypes.Add(newType);
            }
            Common.DBConnection.CloseConnection(connection);
            return allTypes;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE ConsumableTypes " +
                    "SET " +
                    $"Type = '{this.Type}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO ConsumableTypes " +
                    "(Type) " +
                    "VALUES (" +
                    $"'{this.Type}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM ConsumableTypes WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
