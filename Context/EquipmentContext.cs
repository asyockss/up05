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
                MySqlCommand command = new MySqlCommand("SELECT * FROM equipment", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allEquipment.Add(new Equipment
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Photo = reader.IsDBNull(2) ? null : (byte[])reader["photo"],
                            RoomId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            ResponsibleId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            TempResponsibleId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            Cost = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                            Comment = reader.IsDBNull(7) ? null : reader.GetString(7),
                            StatusId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                            ModelId = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                            EquipmentTypeId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                            DirectionId = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11),
                            InventoryId = reader.GetInt32(12)
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
                if (update)
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT responsible_id, room_id FROM equipment WHERE id = @Id", connection);
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
                    }
                }

                string query = update
                    ? "UPDATE equipment SET name = @Name, photo = @Photo, room_id = @RoomId, responsible_id = @ResponsibleId, timeresponsible_id = @TempResponsibleId, cost = @Cost, commentar = @Comment, status_id = @StatusId, model_id = @ModelId, equipment_type_id = @EquipmentTypeId, direction_id = @DirectionId, inventory_id = @InventoryId WHERE id = @Id"
                    : "INSERT INTO equipment (name, photo, room_id, responsible_id, timeresponsible_id, cost, commentar, status_id, model_id, equipment_type_id, direction_id, inventory_id) VALUES (@Name, @Photo, @RoomId, @ResponsibleId, @TempResponsibleId, @Cost, @Comment, @StatusId, @ModelId, @EquipmentTypeId, @DirectionId, @InventoryId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", equipment.Id);
                command.Parameters.AddWithValue("@Name", equipment.Name);
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
                    equipment.Id = (int)command.LastInsertedId;
            }
        }

        public bool Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    MySqlCommand deleteNetworkCommand = new MySqlCommand("DELETE FROM network WHERE equipment_id = @Id", connection);
                    deleteNetworkCommand.Parameters.AddWithValue("@Id", id);
                    deleteNetworkCommand.ExecuteNonQuery();

                    MySqlCommand deleteEquipmentCommand = new MySqlCommand("DELETE FROM equipment WHERE id = @Id", connection);
                    deleteEquipmentCommand.Parameters.AddWithValue("@Id", id);
                    deleteEquipmentCommand.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {
                    Logging.LogError(ex, "Ошибки при сохранение данных об оборудовании");
                    if (ex.Number == 1451)
                    {
                        return false;
                    }
                    throw;
                }
            }
        }

    }
}
