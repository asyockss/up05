using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentTypeContext : EquipmentType, IEquipmentType
    {
        public List<EquipmentType> AllEquipmentTypes()
        {
            List<EquipmentType> allTypes = new List<EquipmentType>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataTypes = (MySqlDataReader)new DBConnection().Query("SELECT * FROM equipment_type", connection);
                while (dataTypes.Read())
                {
                    EquipmentType newType = new EquipmentType
                    {
                        Id = dataTypes.GetInt32(0),
                        Name = dataTypes.GetString(1)
                    };
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
                    new DBConnection().Query("UPDATE equipment_type " +
                        "SET " +
                        $"name = '{this.Name}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO equipment_type " +
                        "(name) " +
                        "VALUES (" +
                        $"'{this.Name}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM equipment_type WHERE id = {this.Id}", connection);
            }
        }
    }
}