using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class SoftwareContext : Software, ISoftware
    {
        public List<Software> AllSoftwares()
        {
            List<Software> allSoftwares = new List<Software>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataSoftwares = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Softwares", connection);
                while (dataSoftwares.Read())
                {
                    Software newSoftware = new Software();
                    newSoftware.Id = dataSoftwares.GetInt32(0);
                    newSoftware.Name = dataSoftwares.GetString(1);
                    newSoftware.Version = dataSoftwares.IsDBNull(2) ? null : dataSoftwares.GetString(2);
                    newSoftware.DeveloperId = dataSoftwares.IsDBNull(3) ? (int?)null : dataSoftwares.GetInt32(3);
                    newSoftware.EquipmentId = dataSoftwares.IsDBNull(4) ? (int?)null : dataSoftwares.GetInt32(4);
                    allSoftwares.Add(newSoftware);
                }
            }
            return allSoftwares;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Softwares " +
                        "SET " +
                        $"Name = '{this.Name}', " +
                        $"Version = {(this.Version != null ? $"'{this.Version}'" : "NULL")}, " +
                        $"DeveloperId = {(this.DeveloperId.HasValue ? this.DeveloperId.ToString() : "NULL")}, " +
                        $"EquipmentId = {(this.EquipmentId.HasValue ? this.EquipmentId.ToString() : "NULL")} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Softwares " +
                        "(Name, Version, DeveloperId, EquipmentId) " +
                        "VALUES (" +
                        $"'{this.Name}', " +
                        $"{(this.Version != null ? $"'{this.Version}'" : "NULL")}, " +
                        $"{(this.DeveloperId.HasValue ? this.DeveloperId.ToString() : "NULL")}, " +
                        $"{(this.EquipmentId.HasValue ? this.EquipmentId.ToString() : "NULL")})", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Softwares WHERE Id = {this.Id}", connection);
            }
        }
    }
}