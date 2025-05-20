using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentModelContext : EquipmentModel, IEquipmentModel
    {
        public List<EquipmentModel> AllEquipmentModels()
        {
            List<EquipmentModel> allModels = new List<EquipmentModel>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataModels = (MySqlDataReader)new DBConnection().Query("SELECT * FROM EquipmentModels", connection);
                while (dataModels.Read())
                {
                    allModels.Add(new EquipmentModel
                    {
                        Id = dataModels.GetInt32(0),
                        Name = dataModels.GetString(1),
                        EquipmentTypeId = dataModels.GetInt32(2)
                    });
                }
            }
            return allModels;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE EquipmentModels " +
                        "SET " +
                        $"Name = '{this.Name}', " +
                        $"EquipmentTypeId = {this.EquipmentTypeId} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO EquipmentModels " +
                        "(Name, EquipmentTypeId) " +
                        "VALUES (" +
                        $"'{this.Name}', " +
                        $"{this.EquipmentTypeId})", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM EquipmentModels WHERE Id = {this.Id}", connection);
            }
        }
    }
}