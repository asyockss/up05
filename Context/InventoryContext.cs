using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using System;
using inventory.Context.Common;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class InventoryContext
    {
        public List<Inventory> AllInventories()
        {
            List<Inventory> allInventories = new List<Inventory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = @"
                    SELECT i.*, 
                           CONCAT(u.last_name, ' ', u.first_name, ' ', COALESCE(u.middle_name, '')) AS user_name
                    FROM inventories i
                    LEFT JOIN users u ON i.users_id = u.id";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allInventories.Add(new Inventory
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            StartDate = reader.GetDateTime("start_date"),
                            EndDate = reader.GetDateTime("end_date"),
                            UserId = reader.GetInt32("users_id"),
                            User = reader.IsDBNull(reader.GetOrdinal("user_name")) ? null : new User
                            {
                                Id = reader.GetInt32("users_id"),
                                LastName = reader.GetString("user_name").Split(' ')[0],
                                FirstName = reader.GetString("user_name").Split(' ')[1],
                                MiddleName = reader.GetString("user_name").Contains(" ") && reader.GetString("user_name").Split(' ').Length > 2 ? reader.GetString("user_name").Split(' ')[2] : null
                            },
                            Checks = new HashSet<InventoryCheck>()
                        });
                    }
                }
            }
            return allInventories;
        }

        public void Save(Inventory inventory, bool update = false)
        {
            if (inventory.StartDate > inventory.EndDate)
            {
                throw new ArgumentException("Дата начала не может быть позже даты окончания");
            }

            if (string.IsNullOrEmpty(inventory.Name))
            {
                throw new ArgumentException("Название инвентаризации обязательно");
            }
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE inventories SET name = @Name, start_date = @StartDate, end_date = @EndDate, users_id = @UserId WHERE id = @Id"
                    : "INSERT INTO inventories (name, start_date, end_date, users_id) VALUES (@Name, @StartDate, @EndDate, @UserId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", inventory.Id);
                command.Parameters.AddWithValue("@Name", inventory.Name);
                command.Parameters.AddWithValue("@StartDate", inventory.StartDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@EndDate", inventory.EndDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@UserId", (object)inventory.UserId ?? DBNull.Value);

                command.ExecuteNonQuery();
                if (!update)
                {
                    inventory.Id = (int)command.LastInsertedId;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM inventories WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
