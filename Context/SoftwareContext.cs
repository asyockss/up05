using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Models;
using inventory.Context.Common;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class SoftwareContext
    {
        public List<Software> AllSoftwares()
        {
            List<Software> allSoftwares = new List<Software>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = @"
SELECT s.id, s.name, s.version, s.developer_id, s.equipment_id,
d.id AS developer_id, d.name AS developer_name,
e.id AS equipment_id, e.name AS equipment_name
FROM software s
LEFT JOIN developers d ON s.developer_id = d.id
LEFT JOIN equipment e ON s.equipment_id = e.id"
                ;

                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader dataSoftwares = command.ExecuteReader())
                {
                    while (dataSoftwares.Read())
                    {
                        Software newSoftware = new Software
                        {
                            Id = dataSoftwares.GetInt32("id"),
                            Name = dataSoftwares.GetString("name"),
                            Version = dataSoftwares.IsDBNull(dataSoftwares.GetOrdinal("version")) ? null : dataSoftwares.GetString("version"),
                            DeveloperId = dataSoftwares.IsDBNull(dataSoftwares.GetOrdinal("developer_id")) ? (int?)null : dataSoftwares.GetInt32("developer_id"),
                            EquipmentId = dataSoftwares.IsDBNull(dataSoftwares.GetOrdinal("equipment_id")) ? (int?)null : dataSoftwares.GetInt32("equipment_id")
                        };

                        if (!dataSoftwares.IsDBNull(dataSoftwares.GetOrdinal("developer_id")))
                        {
                            newSoftware.Developer = new Developer
                            {
                                Id = dataSoftwares.GetInt32("developer_id"),
                                Name = dataSoftwares.GetString("developer_name")
                            };
                        }

                        if (!dataSoftwares.IsDBNull(dataSoftwares.GetOrdinal("equipment_id")))
                        {
                            newSoftware.Equipment = new Equipment
                            {
                                Id = dataSoftwares.GetInt32("equipment_id"),
                                Name = dataSoftwares.GetString("equipment_name")
                            };
                        }

                        allSoftwares.Add(newSoftware);
                    }
                }
            }
            return allSoftwares;
        }

        public void Save(Software software, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (string.IsNullOrEmpty(software.Name))
                        throw new ArgumentException("Name is required.");

                    string query = update
                    ? "UPDATE software SET name = @Name, version = @Version, developer_id = @DeveloperId, equipment_id = @EquipmentId WHERE id = @Id"
                    : "INSERT INTO software (name, version, developer_id, equipment_id) VALUES (@Name, @Version, @DeveloperId, @EquipmentId)";

                    MySqlCommand command = new MySqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", software.Id);
                    command.Parameters.AddWithValue("@Name", software.Name);
                    command.Parameters.AddWithValue("@Version", software.Version ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DeveloperId", software.DeveloperId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EquipmentId", software.EquipmentId ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        software.Id = (int)command.LastInsertedId;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Failed to save software: {ex.Message}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand("DELETE FROM software WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451)
                    {
                        throw new Exception("Невозможно удалить программное обеспечение, так как оно связано с другими модулями.");
                    }
                    throw;
                }
            }
        }
    }
}