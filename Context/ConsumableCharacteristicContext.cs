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
        public  List<ConsumableCharacteristic> AllConsumableCharacteristics()
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
                    new DBConnection().Query("UPDATE consumable_characteristics " +
                        "SET " +
                        $"type_consumables_id = {this.ConsumableTypeId}, " +
                        $"characteristic_name = '{this.CharacteristicName}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO consumable_characteristics " +
                        "(type_consumables_id, characteristic_name) " +
                        "VALUES (" +
                        $"{this.ConsumableTypeId}, " +
                        $"'{this.CharacteristicName}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM consumable_characteristics WHERE id = {this.Id}", connection);
            }
        }
    }
}
