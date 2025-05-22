using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class EquipmentResponsibleHistoryContext : EquipmentResponsibleHistory, IEquipmentResponsibleHistory
    {
        public List<EquipmentResponsibleHistory> AllEquipmentResponsibleHistorys()
        {
            List<EquipmentResponsibleHistory> allHistories = new List<EquipmentResponsibleHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataHistories = (MySqlDataReader)new DBConnection().Query(
                    "SELECT * FROM equipment_responsible_history",
                    connection);

                while (dataHistories.Read())
                {
                    allHistories.Add(new EquipmentResponsibleHistory
                    {
                        Id = dataHistories.GetInt32(0),
                        EquipmentId = dataHistories.GetInt32(1),
                        OldUserId = dataHistories.IsDBNull(2) ? (int?)null : dataHistories.GetInt32(2),
                        ChangeDate = dataHistories.GetDateTime(3),
                        Comment = dataHistories.GetString(4)
                    });
                }
            }
            return allHistories;
        }

        public void Save(EquipmentResponsibleHistory history, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE equipment_responsible_history SET equipment_id = @EquipmentId, old_user_id = @OldUserId, change_date = @ChangeDate, comment = @Comment WHERE id = @Id"
                    : "INSERT INTO equipment_responsible_history (equipment_id, old_user_id, change_date, comment) VALUES (@EquipmentId, @OldUserId, @ChangeDate, @Comment)";

                MySqlCommand command = new MySqlCommand(query, connection);
                if (update)
                    command.Parameters.AddWithValue("@Id", history.Id);
                command.Parameters.AddWithValue("@EquipmentId", history.EquipmentId);
                command.Parameters.AddWithValue("@OldUserId", (object)history.OldUserId ?? DBNull.Value);
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
                new DBConnection().Query(
                    $"DELETE FROM equipment_responsible_history WHERE id = {this.Id}",
                    connection);
            }
        }
    }
}