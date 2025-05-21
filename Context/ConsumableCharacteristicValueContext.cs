using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableCharacteristicValueContext : ConsumableCharacteristicValue, IConsumableCharacteristicValue
    {
        public List<ConsumableCharacteristicValue> AllConsumableCharacteristicValues()
        {
            List<ConsumableCharacteristicValue> allValues = new List<ConsumableCharacteristicValue>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataValues = (MySqlDataReader)new DBConnection().Query("SELECT * FROM consumable_characteristic_values", connection);
                while (dataValues.Read())
                {
                    ConsumableCharacteristicValue newValue = new ConsumableCharacteristicValue();
                    newValue.Id = dataValues.GetInt32(0);
                    newValue.CharacteristicId = dataValues.GetInt32(1);
                    newValue.ConsumableId = dataValues.GetInt32(2);
                    newValue.CharacteristicValue = dataValues.GetString(3);
                    allValues.Add(newValue);
                }
            }
            return allValues;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE consumable_characteristic_values " +
                        "SET " +
                        $"characteristic_id = {this.CharacteristicId}, " +
                        $"consumable_id  = {this.ConsumableId}, " +
                        $"characteristic_value = '{this.CharacteristicValue}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO consumable_characteristic_values " +
                        "(characteristic_id, consumable_id, CharacteristicValue) " +
                        "VALUES (" +
                        $"{this.CharacteristicId}, " +
                        $"{this.ConsumableId}, " +
                        $"'{this.CharacteristicValue}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM consumable_characteristic_values WHERE id = {this.Id}", connection);
            }
        }
    }
}
