using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableCharacteristicContext : ConsumableCharacteristic, IConsumableCharacteristic
    {
        public List<ConsumableCharacteristic> AllConsumableCharacteristics()
        {
            List<ConsumableCharacteristic> allCharacteristics = new List<ConsumableCharacteristic>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataCharacteristics = (MySqlDataReader)new DBConnection().Query("SELECT * FROM ConsumableCharacteristics", connection);
                while (dataCharacteristics.Read())
                {
                    ConsumableCharacteristic newCharacteristic = new ConsumableCharacteristic();
                    newCharacteristic.Id = dataCharacteristics.GetInt32(0);
                    newCharacteristic.ConsumableTypeId = dataCharacteristics.GetInt32(1);
                    newCharacteristic.CharacteristicName = dataCharacteristics.GetString(2);
                    allCharacteristics.Add(newCharacteristic);
                }
                dataCharacteristics.Close();
            }
            return allCharacteristics;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE ConsumableCharacteristics " +
                        "SET " +
                        $"ConsumableTypeId = {this.ConsumableTypeId}, " +
                        $"CharacteristicName = '{this.CharacteristicName}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO ConsumableCharacteristics " +
                        "(ConsumableTypeId, CharacteristicName) " +
                        "VALUES (" +
                        $"{this.ConsumableTypeId}, " +
                        $"'{this.CharacteristicName}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM ConsumableCharacteristics WHERE Id = {this.Id}", connection);
            }
        }
    }
}
