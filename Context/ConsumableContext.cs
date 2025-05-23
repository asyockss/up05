using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Models;
using inventory.Context.Common;
using inventory.Models.inventory.Models;
using inventory.Interfase;

namespace inventory.Context.MySql
{
    public class ConsumableContext
    {
        public List<Consumable> AllConsumables()
        {
            List<Consumable> allConsumables = new List<Consumable>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                try
                {
                    string query = @"
SELECT c.*, t.type,
CONCAT(u1.last_name, ' ', u1.first_name, ' ', COALESCE(u1.middle_name, '')) AS responsible_name,
CONCAT(u2.last_name, ' ', u2.first_name, ' ', COALESCE(u2.middle_name, '')) AS temp_responsible_name
FROM consumables c
LEFT JOIN type_consumables t ON c.type_consumables_id = t.id
LEFT JOIN users u1 ON c.responsible_id = u1.id
LEFT JOIN users u2 ON c.timeresponsible_id = u2.id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allConsumables.Add(new Consumable
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description"),
                                ReceiptDate = reader.GetDateTime("receipt_date"),
                                Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : (byte[])reader["image"],
                                Quantity = reader.GetInt32("quantity"),
                                ResponsibleId = reader.IsDBNull(reader.GetOrdinal("responsible_id")) ? (int?)null : reader.GetInt32("responsible_id"),
                                TempResponsibleId = reader.IsDBNull(reader.GetOrdinal("timeresponsible_id")) ? (int?)null : reader.GetInt32("timeresponsible_id"),
                                ConsumableTypeId = reader.IsDBNull(reader.GetOrdinal("type_consumables_id")) ? (int?)null : reader.GetInt32("type_consumables_id"),
                                ConsumableType = reader.IsDBNull(reader.GetOrdinal("type")) ? null : new ConsumableType
                                {
                                    Id = reader.GetInt32("type_consumables_id"),
                                    Type = reader.GetString("type")
                                },
                                ResponsibleUser = reader.IsDBNull(reader.GetOrdinal("responsible_name")) ? null : new User
                                {
                                    Id = reader.GetInt32("responsible_id"),
                                    FirstName = reader.GetString("responsible_name").Trim()
                                },
                                TempResponsibleUser = reader.IsDBNull(reader.GetOrdinal("temp_responsible_name")) ? null : new User
                                {
                                    Id = reader.GetInt32("timeresponsible_id"),
                                    FirstName = reader.GetString("temp_responsible_name").Trim()
                                }
                            });
                        }
                    }
                
foreach (var consumable in allConsumables)
                    {
                        string charQuery = @"
SELECT ccv.*, cc.characteristic_name
FROM consumable_characteristic_values ccv
JOIN consumable_characteristics cc ON ccv.characteristic_id = cc.id
WHERE ccv.consumable_id = @ConsumableId";
                        MySqlCommand charCommand = new MySqlCommand(charQuery, connection);
                        charCommand.Parameters.AddWithValue("@ConsumableId", consumable.Id);
                        using (MySqlDataReader charReader = charCommand.ExecuteReader())
                        {
                            while (charReader.Read())
                            {
                                consumable.Characteristics.Add(new ConsumableCharacteristicValue
                                {
                                    Id = charReader.GetInt32("id"),
                                    CharacteristicId = charReader.GetInt32("characteristic_id"),
                                    ConsumableId = charReader.GetInt32("consumable_id"),
                                    CharacteristicValue = charReader.GetString("characteristic_value"),
                                    Characteristic = new ConsumableCharacteristic
                                    {
                                        Id = charReader.GetInt32("characteristic_id"),
                                        CharacteristicName = charReader.GetString("characteristic_name")
                                    }
                                });
                            }
                        }

                        string historyQuery = @"
SELECT crh.*, CONCAT(u.last_name, ' ', u.first_name, ' ', COALESCE(u.middle_name, '')) AS user_name
FROM consumable_responsible_history crh
LEFT JOIN users u ON crh.old_user_id = u.id
WHERE crh.consumable_id = @ConsumableId";
                        MySqlCommand historyCommand = new MySqlCommand(historyQuery, connection);
                        historyCommand.Parameters.AddWithValue("@ConsumableId", consumable.Id);
                        using (MySqlDataReader historyReader = historyCommand.ExecuteReader())
                        {
                            while (historyReader.Read())
                            {
                                consumable.ResponsibleHistory.Add(new ConsumableResponsibleHistory
                                {
                                    Id = historyReader.GetInt32("id"),
                                    ConsumableId = historyReader.GetInt32("consumable_id"),
                                    OldUserId = historyReader.IsDBNull(historyReader.GetOrdinal("old_user_id")) ? (int?)null : historyReader.GetInt32("old_user_id"),
                                    ChangeDate = historyReader.GetDateTime("change_date"),
                                    OldUser = historyReader.IsDBNull(historyReader.GetOrdinal("user_name")) ? null : new User
                                    {
                                        Id = historyReader.GetInt32("old_user_id"),
                                        FirstName = historyReader.GetString("user_name").Trim()
                                    }
                                });
                            }
                        }

                        string equipmentQuery = @"
SELECT ec.*, e.name AS equipment_name
FROM equipment_consumables ec
LEFT JOIN equipment e ON ec.equipment_id = e.id
WHERE ec.consumable_id = @ConsumableId";
                        MySqlCommand equipmentCommand = new MySqlCommand(equipmentQuery, connection);
                        equipmentCommand.Parameters.AddWithValue("@ConsumableId", consumable.Id);
                        using (MySqlDataReader equipmentReader = equipmentCommand.ExecuteReader())
                        {
                            while (equipmentReader.Read())
                            {
                                consumable.EquipmentConsumables.Add(new EquipmentConsumable
                                {
                                    Id = equipmentReader.GetInt32("id"),
                                    EquipmentId = equipmentReader.GetInt32("equipment_id"),
                                    ConsumableId = equipmentReader.GetInt32("consumable_id"),
                                    Equipment = equipmentReader.IsDBNull(equipmentReader.GetOrdinal("equipment_name")) ? null : new Equipment
                                    {
                                        Id = equipmentReader.GetInt32("equipment_id"),
                                        Name = equipmentReader.GetString("equipment_name")
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при получении списка расходников");
                    throw;
                }
            }
            return allConsumables;
        }

        public void Save(Consumable consumable, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                if (update)
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT responsible_id, timeresponsible_id FROM consumables WHERE id = @Id", connection);
                    cmd.Parameters.AddWithValue("@Id", consumable.Id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int? currentResponsibleId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                            int? currentTempResponsibleId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
                            reader.Close();

                            if (currentResponsibleId != consumable.ResponsibleId)
                            {
                                var historyContext = new ConsumableResponsibleHistoryContext
                                {
                                    ConsumableId = consumable.Id,
                                    OldUserId = currentResponsibleId,
                                    ChangeDate = DateTime.Now
                                };
                                historyContext.Save();
                            }

                            if (currentTempResponsibleId != consumable.TempResponsibleId)
                            {
                                var historyContext = new ConsumableResponsibleHistoryContext
                                {
                                    ConsumableId = consumable.Id,
                                    OldUserId = currentTempResponsibleId,
                                    ChangeDate = DateTime.Now
                                };
                                historyContext.Save();
                            }
                        }
                    }
                }

                string query = update
                ? "UPDATE consumables SET name = @Name, description = @Description, receipt_date = @ReceiptDate, image = @Image, quantity = @Quantity, responsible_id = @ResponsibleId, timeresponsible_id = @TempResponsibleId, type_consumables_id = @ConsumableTypeId WHERE id = @Id"
                : "INSERT INTO consumables (name, description, receipt_date, image, quantity, responsible_id, timeresponsible_id, type_consumables_id) VALUES (@Name, @Description, @ReceiptDate, @Image, @Quantity, @ResponsibleId, @TempResponsibleId, @ConsumableTypeId)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", consumable.Id);
                command.Parameters.AddWithValue("@Name", consumable.Name);
                command.Parameters.AddWithValue("@Description", consumable.Description);
                command.Parameters.AddWithValue("@ReceiptDate", consumable.ReceiptDate);
                command.Parameters.AddWithValue("@Image", consumable.Image ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Quantity", consumable.Quantity);
                command.Parameters.AddWithValue("@ResponsibleId", consumable.ResponsibleId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TempResponsibleId", consumable.TempResponsibleId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ConsumableTypeId", consumable.ConsumableTypeId ?? (object)DBNull.Value);

                try
                {
                    command.ExecuteNonQuery();
                    if (!update)
                    {
                        consumable.Id = (int)command.LastInsertedId;
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при сохранении расходника");
                    throw;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM consumables WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при удалении расходника");
                    throw;
                }
            }
        }
    }
}