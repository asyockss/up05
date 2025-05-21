using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableTypeContext : ConsumableType, IConsumableType
    {
        public  List<ConsumableType> AllConsumableTypes()
        {
            List<ConsumableType> allTypes = new List<ConsumableType>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataTypes = (MySqlDataReader)new DBConnection().Query("SELECT * FROM type_consumables", connection);
                while (dataTypes.Read())
                {
                    ConsumableType newType = new ConsumableType();
                    newType.Id = dataTypes.GetInt32(0);
                    newType.Type = dataTypes.GetString(1);
                    allTypes.Add(newType);
                }
            }
            return allTypes;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE type_consumables " +
                        "SET " +
                        $"type = '{this.Type}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO type_consumables " +
                        "(type) " +
                        "VALUES (" +
                        $"'{this.Type}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM type_consumables WHERE id = {this.Id}", connection);
            }
        }
    }
}
