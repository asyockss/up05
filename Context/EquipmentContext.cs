using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Models;
using inventory.Context.Common;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class EquipmentContext
    {
        public List<Equipment> AllEquipment()
        {
            List<Equipment> allEquipment = new List<Equipment>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand("SELECT id, name, photo, room_id, responsible_id, timeresponsible_id, cost, commentar, status_id, model_id, equipment_type_id, direction_id, inventory_id FROM equipment", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allEquipment.Add(new Equipment
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Photo = reader.IsDBNull(reader.GetOrdinal("photo")) ? null : (byte[])reader["photo"],
                                RoomId = reader.IsDBNull(reader.GetOrdinal("room_id")) ? (int?)null : reader.GetInt32("room_id"),
                                ResponsibleId = reader.IsDBNull(reader.GetOrdinal("responsible_id")) ? (int?)null : reader.GetInt32("responsible_id"),
                                TempResponsibleId = reader.IsDBNull(reader.GetOrdinal("timeresponsible_id")) ? (int?)null : reader.GetInt32("timeresponsible_id"),
                                Cost = reader.IsDBNull(reader.GetOrdinal("cost")) ? (decimal?)null : reader.GetDecimal("cost"),
                                Comment = reader.IsDBNull(reader.GetOrdinal("commentar")) ? null : reader.GetString("commentar"),
                                StatusId = reader.IsDBNull(reader.GetOrdinal("status_id")) ? (int?)null : reader.GetInt32("status_id"),
                                ModelId = reader.IsDBNull(reader.GetOrdinal("model_id")) ? (int?)null : reader.GetInt32("model_id"),
                                EquipmentTypeId = reader.IsDBNull(reader.GetOrdinal("equipment_type_id")) ? (int?)null : reader.GetInt32("equipment_type_id"),
                                DirectionId = reader.IsDBNull(reader.GetOrdinal("direction_id")) ? (int?)null : reader.GetInt32("direction_id"),
                                InventoryId = reader.GetInt32("inventory_id")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при загрузке оборудования: {ex.Message}", ex);
                }
            }
            return allEquipment;
        }

public void Save(Equipment equipment, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (update)
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT responsible_id, room_id FROM equipment WHERE id = @Id", connection, transaction);
                        cmd.Parameters.AddWithValue("@Id", equipment.Id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int? currentResponsibleId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                                int? currentRoomId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
                                reader.Close();

                                if (currentResponsibleId != equipment.ResponsibleId)
                                {
                                    var history = new EquipmentResponsibleHistory
                                    {
                                        EquipmentId = equipment.Id,
                                        OldUserId = currentResponsibleId,
                                        ChangeDate = DateTime.Now,
                                        Comment = "Ответственный пользователь изменен"
                                    };
                                    new EquipmentResponsibleHistoryContext().Save(history);
                                }
                                if (currentRoomId != equipment.RoomId)
                                {
                                    var locationHistory = new EquipmentLocationHistory
                                    {
                                        EquipmentId = equipment.Id,
                                        RoomId = currentRoomId,
                                        ChangeDate = DateTime.Now,
                                        Comment = "Комната изменена"
                                    };
                                    new EquipmentLocationHistoryContext().Save(locationHistory);
                                }
                            }
                            else
                            {
                                reader.Close();
                            }
                        }
                    }

                    string query = update
                    ? "UPDATE equipment SET name = @Name, photo = @Photo, room_id = @RoomId, responsible_id = @ResponsibleId, timeresponsible_id = @TempResponsibleId, cost = @Cost, commentar = @Comment, status_id = @StatusId, model_id = @ModelId, equipment_type_id = @EquipmentTypeId, direction_id = @DirectionId, inventory_id = @InventoryId WHERE id = @Id"
                    : "INSERT INTO equipment (name, photo, room_id, responsible_id, timeresponsible_id, cost, commentar, status_id, model_id, equipment_type_id, direction_id, inventory_id) VALUES (@Name, @Photo, @RoomId, @ResponsibleId, @TempResponsibleId, @Cost, @Comment, @StatusId, @ModelId, @EquipmentTypeId, @DirectionId, @InventoryId)";

                    MySqlCommand command = new MySqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", equipment.Id);
                    command.Parameters.AddWithValue("@Name", equipment.Name ?? throw new ArgumentException("Название оборудования обязательно"));
                    command.Parameters.AddWithValue("@Photo", (object)equipment.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RoomId", (object)equipment.RoomId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponsibleId", (object)equipment.ResponsibleId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TempResponsibleId", (object)equipment.TempResponsibleId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Cost", (object)equipment.Cost ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Comment", (object)equipment.Comment ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusId", (object)equipment.StatusId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ModelId", (object)equipment.ModelId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipmentTypeId", (object)equipment.EquipmentTypeId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DirectionId", (object)equipment.DirectionId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@InventoryId", equipment.InventoryId);

                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        equipment.Id = (int)command.LastInsertedId;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при сохранении оборудования: {ex.Message}", ex);
                }
            }
        }

        public bool Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    MySqlCommand deleteNetworkCommand = new MySqlCommand("DELETE FROM network WHERE equipment_id = @Id", connection, transaction);
                    deleteNetworkCommand.Parameters.AddWithValue("@Id", id);
                    deleteNetworkCommand.ExecuteNonQuery();

                    MySqlCommand deleteEquipmentCommand = new MySqlCommand("DELETE FROM equipment WHERE id = @Id", connection, transaction);
                    deleteEquipmentCommand.Parameters.AddWithValue("@Id", id);
                    deleteEquipmentCommand.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback();
                    Logging.LogError(ex, "Ошибка при удалении оборудования");
                    if (ex.Number == 1451)
                    {
                        return false;
                    }
                    throw new Exception($"Ошибка при удалении оборудования: {ex.Message}", ex);
                }
            }
        }
    }
}