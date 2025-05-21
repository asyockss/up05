using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class ConsumableContext
    {
        public List<Consumable> AllConsumables()
        {
            List<Consumable> allConsumables = new List<Consumable>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM consumables", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allConsumables.Add(new Consumable
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            ReceiptDate = reader.GetDateTime(3),
                            Image = reader.IsDBNull(4) ? null : (byte[])reader["Image"],
                            Quantity = reader.GetInt32(5),
                            ResponsibleId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                            TempResponsibleId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                            ConsumableTypeId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8)
                        });
                    }
                }
            }
            return allConsumables;
        }


        public void Save(Consumable consumable, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE consumables SET name = @Name, description = @Description, receipt_date = @ReceiptDate, image = @Image, quantity = @Quantity, responsible_id  = @ResponsibleId, timeresponsible_id  = @TempResponsibleId, type_consumables_id  = @ConsumableTypeId WHERE id = @Id"
                    : "INSERT INTO consumables (name, description, receipt_date, image, quantity, responsible_id , timeresponsible_id , type_consumables_id ) VALUES (@Name, @Description, @ReceiptDate, @Image, @Quantity, @ResponsibleId, @TempResponsibleId, @ConsumableTypeId)";

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

                command.ExecuteNonQuery();
                if (!update)
                {
                    consumable.Id = (int)command.LastInsertedId;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM consumables WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}