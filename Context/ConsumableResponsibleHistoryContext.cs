using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableResponsibleHistoryContext : ConsumableResponsibleHistory, IConsumableResponsibleHistory
    {
        public List<ConsumableResponsibleHistory> AllConsumableResponsibleHistories()
        {
            List<ConsumableResponsibleHistory> allHistories = new List<ConsumableResponsibleHistory>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM consumable_responsible_history", connection);
                using (MySqlDataReader dataHistories = command.ExecuteReader())
                {
                    while (dataHistories.Read())
                    {
                        allHistories.Add(new ConsumableResponsibleHistory
                        {
                            Id = dataHistories.GetInt32("id"),
                            ConsumableId = dataHistories.GetInt32("consumable_id"),
                            OldUserId = dataHistories.IsDBNull(dataHistories.GetOrdinal("old_user_id")) ? (int?)null : dataHistories.GetInt32("old_user_id"),
                            ChangeDate = dataHistories.GetDateTime("change_date")
                        });
                    }
                }
            }
            return allHistories;
        }

        public void Save(bool Update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                    ? "UPDATE consumable_responsible_history SET consumable_id = @ConsumableId, old_user_id = @OldUserId, change_date = @ChangeDate WHERE id = @Id"
                    : "INSERT INTO consumable_responsible_history (consumable_id, old_user_id, change_date) VALUES (@ConsumableId, @OldUserId, @ChangeDate)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", this.Id);
                command.Parameters.AddWithValue("@ConsumableId", this.ConsumableId);
                command.Parameters.AddWithValue("@OldUserId", this.OldUserId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ChangeDate", this.ChangeDate);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM consumable_responsible_history WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}