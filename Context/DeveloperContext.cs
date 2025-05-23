using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Models;
using inventory.Context.Common;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class DeveloperContext
    {
        public List<Developer> AllDevelopers()
        {
            List<Developer> allDevelopers = new List<Developer>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand("SELECT id, name FROM developers", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allDevelopers.Add(new Developer
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при загрузке разработчиков: {ex.Message}", ex);
                }
            }
            return allDevelopers;
        }

public void Save(Developer developer, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    string query = update
                    ? "UPDATE developers SET name = @Name WHERE id = @Id"
                    : "INSERT INTO developers (name) VALUES (@Name)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", developer.Id);
                    command.Parameters.AddWithValue("@Name", developer.Name ?? throw new ArgumentException("Название разработчика обязательно"));

                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        developer.Id = (int)command.LastInsertedId;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при сохранении разработчика: {ex.Message}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand("DELETE FROM developers WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451)
                    {
                        throw new Exception("Невозможно удалить разработчика, так как он связан с программным обеспечением.");
                    }
                    throw new Exception($"Ошибка при удалении разработчика: {ex.Message}", ex);
                }
            }
        }
    }
}