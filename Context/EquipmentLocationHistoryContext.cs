using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentLocationHistoryContext : EquipmentLocationHistory, IEquipmentLocationHistory
    {
        public List<EquipmentLocationHistory> AllEquipmentLocationHistories()
        {
            List<EquipmentLocationHistory> allHistories = new List<EquipmentLocationHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query("SELECT * FROM equipment_location_history", connection);
                while (dataHistories.Read())
                {
                    EquipmentLocationHistory newHistory = new EquipmentLocationHistory();
                    newHistory.Id = dataHistories.GetInt32(0);
                    newHistory.EquipmentId = dataHistories.GetInt32(1);
                    newHistory.RoomId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2);
                    newHistory.ChangeDate = dataHistories.GetDateTime(3);
                    newHistory.Comment = dataHistories.GetString(4);
                    allHistories.Add(newHistory);
                }
            }
            return allHistories;
        }

        public void Save(EquipmentLocationHistory history, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE equipment_location_history SET equipment_id = @EquipmentId, room_id = @RoomId, change_date = @ChangeDate, comment = @Comment WHERE id = @Id"
                    : "INSERT INTO equipment_location_history (equipment_id, room_id, change_date, comment) VALUES (@EquipmentId, @RoomId, @ChangeDate, @Comment)";

                MySqlCommand command = new MySqlCommand(query, connection);
                if (update)
                    command.Parameters.AddWithValue("@Id", history.Id);
                command.Parameters.AddWithValue("@EquipmentId", history.EquipmentId);
                command.Parameters.AddWithValue("@RoomId", (object)history.RoomId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ChangeDate", history.ChangeDate);
                command.Parameters.AddWithValue("@Comment", history.Comment ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
                if (!update)
                    history.Id = (int)command.LastInsertedId;
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM equipment_location_history WHERE id = {this.Id}", connection);
            }
        }
    }
}
