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
                string query = @"
                    SELECT r.*, 
                           CONCAT(u1.last_name, ' ', u1.first_name, ' ', COALESCE(u1.middle_name, '')) AS responsible_name,
                           CONCAT(u2.last_name, ' ', u2.first_name, ' ', COALESCE(u2.middle_name, '')) AS timeresponsible_name,
                           (SELECT COUNT(*) FROM equipment e WHERE e.room_id = r.id) AS equipment_count
                    FROM rooms r
                    LEFT JOIN users u1 ON r.responsible_id = u1.id
                    LEFT JOIN users u2 ON r.timeresponsible_id = u2.id";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allRooms.Add(new Room
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            ShortName = reader.GetString("short_name"),
                            ResponsibleId = reader.IsDBNull(reader.GetOrdinal("responsible_id")) ? (int?)null : reader.GetInt32("responsible_id"),
                            TempResponsibleId = reader.IsDBNull(reader.GetOrdinal("timeresponsible_id")) ? (int?)null : reader.GetInt32("timeresponsible_id"),
                            ResponsibleUser = reader.IsDBNull(reader.GetOrdinal("responsible_name")) ? null : new User
                            {
                                Id = reader.GetInt32("responsible_id"),
                                LastName = reader.GetString("responsible_name").Split(' ')[0],
                                FirstName = reader.GetString("responsible_name").Split(' ')[1],
                                MiddleName = reader.GetString("responsible_name").Contains(" ") && reader.GetString("responsible_name").Split(' ').Length > 2 ? reader.GetString("responsible_name").Split(' ')[2] : null
                            },
                            TempResponsibleUser = reader.IsDBNull(reader.GetOrdinal("timeresponsible_name")) ? null : new User
                            {
                                Id = reader.GetInt32("timeresponsible_id"),
                                LastName = reader.GetString("timeresponsible_name").Split(' ')[0],
                                FirstName = reader.GetString("timeresponsible_name").Split(' ')[1],
                                MiddleName = reader.GetString("timeresponsible_name").Contains(" ") && reader.GetString("timeresponsible_name").Split(' ').Length > 2 ? reader.GetString("timeresponsible_name").Split(' ')[2] : null
                            },
                            Equipment = new List<Equipment> { /* Placeholder for count */ }
                        });
                        // Set Equipment.Count using the equipment_count from the query
                        allRooms[allRooms.Count - 1].Equipment = new List<Equipment>(new Equipment[reader.GetInt32("equipment_count")]);
                    }
                }
            }
            return allRooms;
        }

        public void Save(Room room, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (string.IsNullOrEmpty(room.Name) || string.IsNullOrEmpty(room.ShortName))
                        throw new ArgumentException("Name and ShortName are required.");

                    string query = update
                        ? "UPDATE rooms SET name = @Name, short_name = @ShortName, responsible_id = @ResponsibleId, timeresponsible_id = @TempResponsibleId WHERE id = @Id"
                        : "INSERT INTO rooms (name, short_name, responsible_id, timeresponsible_id) VALUES (@Name, @ShortName, @ResponsibleId, @TempResponsibleId)";

                    MySqlCommand command = new MySqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", room.Id);
                    command.Parameters.AddWithValue("@Name", room.Name);
                    command.Parameters.AddWithValue("@ShortName", room.ShortName);
                    command.Parameters.AddWithValue("@ResponsibleId", room.ResponsibleId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TempResponsibleId", room.TempResponsibleId ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        room.Id = (int)command.LastInsertedId;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Failed to save room: {ex.Message}", ex);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM equipment WHERE room_id = @Id", connection, transaction);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    long equipmentCount = (long)checkCommand.ExecuteScalar();
                    if (equipmentCount > 0)
                        throw new InvalidOperationException("Cannot delete room with associated equipment.");

                    MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM rooms WHERE id = @Id", connection, transaction);
                    deleteCommand.Parameters.AddWithValue("@Id", id);
                    deleteCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Failed to delete room: {ex.Message}", ex);
                }
            }
        }
    }
}