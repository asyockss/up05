using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class InventoryCheckContext : InventoryCheck, IInventoryCheck
    {
        public List<InventoryCheck> AllInventoryChecks()
        {
            List<InventoryCheck> allChecks = new List<InventoryCheck>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataChecks = (MySqlDataReader)new DBConnection().Query("SELECT * FROM inventory_checks", connection);
                while (dataChecks.Read())
                {
                    InventoryCheck newCheck = new InventoryCheck();
                    newCheck.Id = dataChecks.GetInt32(0);
                    newCheck.InventoryId = dataChecks.GetInt32(1);
                    newCheck.EquipmentId = dataChecks.GetInt32(2);
                    newCheck.UserId = dataChecks.GetInt32(3);
                    newCheck.CheckDate = dataChecks.GetDateTime(4);
                    newCheck.Comment = dataChecks.GetString(5);
                    allChecks.Add(newCheck);
                }
            }
            return allChecks;
        }
        public void Save(bool Update = false)
        {

            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                ? "UPDATE inventory_checks SET inventory_id = @InventoryId, equipment_id = @EquipmentId, users_id = @UserId, check_date = @CheckDate, comment = @Comment WHERE id = @Id"
                : "INSERT INTO inventory_checks (inventory_id, equipment_id, users_id, check_date, comment) VALUES (@InventoryId, @EquipmentId, @UserId, @CheckDate, @Comment)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", this.Id);
                command.Parameters.AddWithValue("@InventoryId", this.InventoryId);
                command.Parameters.AddWithValue("@EquipmentId", this.EquipmentId);
                command.Parameters.AddWithValue("@UserId", this.UserId);
                command.Parameters.AddWithValue("@CheckDate", this.CheckDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Comment", this.Comment);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogError(ex, "Ошибка при сохранении проверки инвентаризации");
                    throw;
                }
            }
        }



        private void LogError(Exception ex, string message)
        {
            //Logging.LogError(new Exception("Тестовая ошибка"), "Тест логирования");
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM inventory_checks WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
