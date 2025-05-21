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
                            Photo = reader.IsDBNull(2) ? null : (byte[])reader["Photo"],
                            InventoryNumber = reader.GetString(3),
                            RoomId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            ResponsibleId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            TempResponsibleId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                            Cost = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Comment = reader.IsDBNull(8) ? null : reader.GetString(8),
                            StatusId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9), // Используем значение по умолчанию 0
                            ModelId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                            EquipmentTypeId = reader.IsDBNull(11) ? 0 : reader.GetInt32(11), // Используем значение по умолчанию 0
                            DirectionId = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
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
                    ? "UPDATE equipment SET name = @Name, photo = @Photo, inv_number = @InventoryNumber, room_id = @RoomId, responsible_id = @ResponsibleId, timeresponsible_id = @TimeresponsibleId, cost = @Cost, commentar = @Comment, status_id = @StatusId, model_id = @ModelId, equipment_type_id = @EquipmentTypeId, direction_id = @DirectionId WHERE id = @Id"
                    : "INSERT INTO equipment (name, photo, inv_number, room_id, responsible_id, timeresponsible_id, cost, commentar, status_id, model_id, equipment_type_id, direction_id) VALUES (@Name, @Photo, @InventoryNumber, @RoomId, @ResponsibleId, @TimeresponsibleId, @Cost, @Comment, @StatusId, @ModelId, @EquipmentTypeId, @DirectionId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", equipment.Id);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Photo", equipment.Photo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@InventoryNumber", equipment.InventoryNumber);
                command.Parameters.AddWithValue("@RoomId", equipment.RoomId.HasValue ? (object)equipment.RoomId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ResponsibleId", equipment.ResponsibleId.HasValue ? (object)equipment.ResponsibleId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@TimeresponsibleId", equipment.TempResponsibleId.HasValue ? (object)equipment.TempResponsibleId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Cost", equipment.Cost.HasValue ? (object)equipment.Cost.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Comment", equipment.Comment ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StatusId", equipment.StatusId);
                command.Parameters.AddWithValue("@ModelId", equipment.ModelId.HasValue ? (object)equipment.ModelId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@EquipmentTypeId", equipment.EquipmentTypeId);
                command.Parameters.AddWithValue("@DirectionId", equipment.DirectionId.HasValue ? (object)equipment.DirectionId.Value : DBNull.Value);

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
                // Сначала удаляем связанные записи из таблицы network
                MySqlCommand deleteNetworkCommand = new MySqlCommand("DELETE FROM network WHERE equipment_id = @Id", connection);
                deleteNetworkCommand.Parameters.AddWithValue("@Id", id);
                deleteNetworkCommand.ExecuteNonQuery();

                // Затем удаляем запись из таблицы equipment
                MySqlCommand deleteEquipmentCommand = new MySqlCommand("DELETE FROM equipment WHERE id = @Id", connection);
                deleteEquipmentCommand.Parameters.AddWithValue("@Id", id);
                deleteEquipmentCommand.ExecuteNonQuery();
            }
        }

    }
}
