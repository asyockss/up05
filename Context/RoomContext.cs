using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using System;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class RoomContext
    {
        public List<Room> AllRooms()
        {
            List<Room> allRooms = new List<Room>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM rooms", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allRooms.Add(new Room
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ShortName = reader.GetString(2),
                            ResponsibleId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                        });
                    }
                }
            }
            return allRooms;
        }

        public void Save(Room room, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE rooms SET name = @Name, short_name = @ShortName, responsible_id = @ResponsibleId WHERE id = @Id"
                    : "INSERT INTO rooms (name, short_name, responsible_id) VALUES (@Name, @ShortName, @ResponsibleId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", room.Id);
                command.Parameters.AddWithValue("@Name", room.Name);
                command.Parameters.AddWithValue("@ShortName", room.ShortName);
                command.Parameters.AddWithValue("@ResponsibleId", room.ResponsibleId ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
                if (!update)
                {
                    room.Id = (int)command.LastInsertedId;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM rooms WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}