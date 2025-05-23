using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Models;
using inventory.Context.Common;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class EquipmentConsumableContext
    {
        public List<EquipmentConsumable> AllEquipmentConsumables()
        {
            List<EquipmentConsumable> allEquipmentConsumables = new List<EquipmentConsumable>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    string query = "SELECT * FROM equipment_consumables";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allEquipmentConsumables.Add(new EquipmentConsumable
                            {
                                Id = reader.GetInt32("id"),
                                EquipmentId = reader.GetInt32("equipment_id"),
                                ConsumableId = reader.GetInt32("consumable_id")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при получении связей оборудование-расходники");
                    throw;
                }
            }
            return allEquipmentConsumables;
        }

        public void Save(EquipmentConsumable equipmentConsumable, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                ? "UPDATE equipment_consumables SET equipment_id = @EquipmentId, consumable_id = @ConsumableId WHERE id = @Id"
                : "INSERT INTO equipment_consumables (equipment_id, consumable_id) VALUES (@EquipmentId, @ConsumableId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", equipmentConsumable.Id);
                command.Parameters.AddWithValue("@EquipmentId", equipmentConsumable.EquipmentId);
                command.Parameters.AddWithValue("@ConsumableId", equipmentConsumable.ConsumableId);

                try
                {
                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        equipmentConsumable.Id = (int)command.LastInsertedId;
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при сохранении связи оборудование-расходник");
                    throw;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM equipment_consumables WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при удалении связи оборудование-расходник");
                    throw;
                }
            }
        }
    }
}