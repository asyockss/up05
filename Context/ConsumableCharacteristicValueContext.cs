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
                MySqlCommand command = new MySqlCommand("SELECT * FROM consumable_characteristic_values", connection);
                using (MySqlDataReader dataValues = command.ExecuteReader())
                {
                    while (dataValues.Read())
                    {
                        allValues.Add(new ConsumableCharacteristicValue
                        {
                            Id = dataValues.GetInt32("id"),
                            CharacteristicId = dataValues.GetInt32("characteristic_id"),
                            ConsumableId = dataValues.GetInt32("consumable_id"),
                            CharacteristicValue = dataValues.GetString("characteristic_value")
                        });
                    }
                }
            }
            return allValues;
        }

        public void Save(bool Update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                    ? "UPDATE consumable_characteristic_values SET characteristic_id = @CharacteristicId, consumable_id = @ConsumableId, characteristic_value = @CharacteristicValue WHERE id = @Id"
                    : "INSERT INTO consumable_characteristic_values (characteristic_id, consumable_id, characteristic_value) VALUES (@CharacteristicId, @ConsumableId, @CharacteristicValue)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", this.Id);
                command.Parameters.AddWithValue("@CharacteristicId", this.CharacteristicId);
                command.Parameters.AddWithValue("@ConsumableId", this.ConsumableId);
                command.Parameters.AddWithValue("@CharacteristicValue", this.CharacteristicValue);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM consumable_characteristic_values WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
