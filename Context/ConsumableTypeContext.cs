using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;
using inventory.Models.inventory.Models;

namespace inventory.Context.MySql
{
    public class ConsumableTypeContext : ConsumableType
    {
        public List<ConsumableType> AllConsumableTypes()
        {
            List<ConsumableType> allTypes = new List<ConsumableType>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM type_consumables", connection);
                using (MySqlDataReader dataTypes = command.ExecuteReader())
                {
                    while (dataTypes.Read())
                    {
                        allTypes.Add(new ConsumableType
                        {
                            Id = dataTypes.GetInt32("id"),
                            Type = dataTypes.GetString("type")
                        });
                    }
                }
            }
            return allTypes;
        }

        public void Save(bool Update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                    ? "UPDATE type_consumables SET type = @Type WHERE id = @Id"
                    : "INSERT INTO type_consumables (type) VALUES (@Type)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", this.Id);
                command.Parameters.AddWithValue("@Type", this.Type);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM type_consumables WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}