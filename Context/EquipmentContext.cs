using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using inventory.Context.MySql;
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
                            Photo = (byte[])reader["Photo"],
                            InventoryNumber = reader.GetString(2),
                            RoomId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                            ResponsibleId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            TempResponsibleId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            Cost = reader.GetDecimal(6),
                            Comment = reader.GetString(7),
                            StatusId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                            ModelId = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                            EquipmentTypeId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                            DirectionId = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11)
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
                    ? "UPDATE Equipment SET Name = @Name, Photo = @Photo, InventoryNumber = @InventoryNumber, RoomId = @RoomId, ResponsibleId = @ResponsibleId, TempResponsibleId = @TempResponsibleId, Cost = @Cost, Comment = @Comment, StatusId = @StatusId, ModelId = @ModelId, EquipmentTypeId = @EquipmentTypeId, DirectionId = @DirectionId WHERE Id = @Id"
                    : "INSERT INTO Equipment (Name, Photo, InventoryNumber, RoomId, ResponsibleId, TempResponsibleId, Cost, Comment, StatusId, ModelId, EquipmentTypeId, DirectionId) VALUES (@Name, @Photo, @InventoryNumber, @RoomId, @ResponsibleId, @TempResponsibleId, @Cost, @Comment, @StatusId, @ModelId, @EquipmentTypeId, @DirectionId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", equipment.Id);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Photo", equipment.Photo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@InventoryNumber", equipment.InventoryNumber);
                command.Parameters.AddWithValue("@RoomId", equipment.RoomId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ResponsibleId", equipment.ResponsibleId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TempResponsibleId", equipment.TempResponsibleId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Cost", equipment.Cost);
                command.Parameters.AddWithValue("@Comment", equipment.Comment);
                command.Parameters.AddWithValue("@StatusId", equipment.StatusId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModelId", equipment.ModelId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EquipmentTypeId", equipment.EquipmentTypeId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DirectionId", equipment.DirectionId ?? (object)DBNull.Value);

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