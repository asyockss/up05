using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using System;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentContext
    {
        public List<Equipment> AllEquipment()
        {
            List<Equipment> allEquipment = new List<Equipment>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM Equipment", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allEquipment.Add(new Equipment
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Photo = reader.IsDBNull(2) ? null : (byte[])reader["Photo"],
                            InventoryNumber = reader.GetString(3),
                            RoomId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            ResponsibleId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            Cost = reader.GetDecimal(6),
                            Comment = reader.GetString(7)
                        });
                    }
                }
            }
            return allEquipment;
        }

        public void Save(Equipment equipment, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE Equipment SET Name = @Name, Photo = @Photo, InventoryNumber = @InventoryNumber, RoomId = @RoomId, ResponsibleId = @ResponsibleId, Cost = @Cost, Comment = @Comment WHERE Id = @Id"
                    : "INSERT INTO Equipment (Name, Photo, InventoryNumber, RoomId, ResponsibleId, Cost, Comment) VALUES (@Name, @Photo, @InventoryNumber, @RoomId, @ResponsibleId, @Cost, @Comment)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", equipment.Id);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Photo", equipment.Photo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@InventoryNumber", equipment.InventoryNumber);
                command.Parameters.AddWithValue("@RoomId", equipment.RoomId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ResponsibleId", equipment.ResponsibleId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Cost", equipment.Cost);
                command.Parameters.AddWithValue("@Comment", equipment.Comment);

                command.ExecuteNonQuery();
                if (!update)
                {
                    equipment.Id = (int)command.LastInsertedId;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM Equipment WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}