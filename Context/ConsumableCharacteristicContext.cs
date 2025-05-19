using System;
using System.Collections.Generic;
using System.Data.OleDb;
using inventory.Models;

namespace inventory.Context.OleDb
{
    public class ConsumableCharacteristicContext : ConsumableCharacteristic, Interfaces.IConsumableCharacteristic
    {
        public List<ConsumableCharacteristicContext> AllConsumableCharacteristics()
        {
            List<ConsumableCharacteristicContext> allCharacteristics = new List<ConsumableCharacteristicContext>();
            OleDbConnection connection = Common.DBConnection.Connection();
            OleDbDataReader dataCharacteristics = Common.DBConnection.Query("SELECT * FROM ConsumableCharacteristics", connection);
            while (dataCharacteristics.Read())
            {
                ConsumableCharacteristicContext newCharacteristic = new ConsumableCharacteristicContext();
                newCharacteristic.Id = dataCharacteristics.GetInt32(0);
                newCharacteristic.ConsumableTypeId = dataCharacteristics.GetInt32(1);
                newCharacteristic.CharacteristicName = dataCharacteristics.GetString(2);
                allCharacteristics.Add(newCharacteristic);
            }
            Common.DBConnection.CloseConnection(connection);
            return allCharacteristics;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("UPDATE ConsumableCharacteristics " +
                    "SET " +
                    $"ConsumableTypeId = {this.ConsumableTypeId}, " +
                    $"CharacteristicName = '{this.CharacteristicName}' " +
                    $"WHERE Id = {this.Id}", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            else
            {
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query("INSERT INTO ConsumableCharacteristics " +
                    "(ConsumableTypeId, CharacteristicName) " +
                    "VALUES (" +
                    $"{this.ConsumableTypeId}, " +
                    $"'{this.CharacteristicName}')", connection);
                Common.DBConnection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            OleDbConnection connection = Common.DBConnection.Connection();
            Common.DBConnection.Query($"DELETE FROM ConsumableCharacteristics WHERE Id = {this.Id}", connection);
            Common.DBConnection.CloseConnection(connection);
        }
    }
}
