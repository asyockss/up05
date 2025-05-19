using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class ConsumableCharacteristicValueContext : ConsumableCharacteristicValue, Interfaces.IConsumableCharacteristicValue
    {
        public List<ConsumableCharacteristicValueContext> AllConsumableCharacteristicValues()
        {
            List<ConsumableCharacteristicValueContext> allValues = new List<ConsumableCharacteristicValueContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataValues = Common.DBConnection.Query("SELECT * FROM ConsumableCharacteristicValues", connection);
            while (dataValues.Read())
            {
                ConsumableCharacteristicValueContext newValue = new ConsumableCharacteristicValueContext();
                newValue.Id = dataValues.GetInt32(0);
                newValue.CharacteristicId = dataValues.GetInt32(1);
                newValue.ConsumableId = dataValues.GetInt32(2);
                newValue.CharacteristicValue = dataValues.GetString(3);
                allValues.Add(newValue);
            }
            Common.DBConnection.CloseConnection(connection);
            return allValues;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE ConsumableCharacteristicValues " +
                    "SET " +
                    $"CharacteristicId = {this.CharacteristicId}, " +
                    $"ConsumableId = {this.ConsumableId}, " +
                    $"CharacteristicValue = '{this.CharacteristicValue}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO ConsumableCharacteristicValues " +
                    "(CharacteristicId, ConsumableId, CharacteristicValue) " +
                    "VALUES (" +
                    $"{this.CharacteristicId}, " +
                    $"{this.ConsumableId}, " +
                    $"'{this.CharacteristicValue}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM ConsumableCharacteristicValues WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
