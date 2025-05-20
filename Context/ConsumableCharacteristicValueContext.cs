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
                MySqlDataReader dataValues = (MySqlDataReader)new DBConnection().Query("SELECT * FROM ConsumableCharacteristicValues", connection);
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
                    new DBConnection().Query("UPDATE ConsumableCharacteristicValues " +
                        "SET " +
                        $"CharacteristicId = {this.CharacteristicId}, " +
                        $"ConsumableId = {this.ConsumableId}, " +
                        $"CharacteristicValue = '{this.CharacteristicValue}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO ConsumableCharacteristicValues " +
                        "(CharacteristicId, ConsumableId, CharacteristicValue) " +
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
                new DBConnection().Query($"DELETE FROM ConsumableCharacteristicValues WHERE Id = {this.Id}", connection);
            }
        }
    }
}
