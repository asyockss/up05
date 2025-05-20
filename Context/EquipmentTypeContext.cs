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
                MySqlDataReader dataTypes = (MySqlDataReader)new DBConnection().Query("SELECT * FROM EquipmentTypes", connection);
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
                    new DBConnection().Query("UPDATE EquipmentTypes " +
                        "SET " +
                        $"Name = '{this.Name}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO EquipmentTypes " +
                        "(Name) " +
                        "VALUES (" +
                        $"'{this.Name}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM EquipmentTypes WHERE Id = {this.Id}", connection);
            }
        }
    }
}