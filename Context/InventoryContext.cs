using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using System;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class InventoryContext
    {
        public List<Inventory> AllInventorys()
        {
            List<Inventory> allInventories = new List<Inventory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM inventories", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allInventories.Add(new Inventory
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            StartDate = reader.GetDateTime(2),
                            EndDate = reader.GetDateTime(3),
                            UserId = reader.GetInt32(4)
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
                command.Parameters.AddWithValue("@UserId", inventory.UserId);

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