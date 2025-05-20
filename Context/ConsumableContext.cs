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
                MySqlCommand command = new MySqlCommand("SELECT * FROM Consumables", connection);
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
                            Image = (byte[])reader["Image"],
                            Quantity = reader.GetInt32(4),
                            ResponsibleId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            TempResponsibleId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                            ConsumableTypeId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7)
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
                    ? "UPDATE Consumables SET Name = @Name, Description = @Description, ReceiptDate = @ReceiptDate, Image = @Image, Quantity = @Quantity, ResponsibleId = @ResponsibleId, TempResponsibleId = @TempResponsibleId, ConsumableTypeId = @ConsumableTypeId WHERE Id = @Id"
                    : "INSERT INTO Consumables (Name, Description, ReceiptDate, Image, Quantity, ResponsibleId, TempResponsibleId, ConsumableTypeId) VALUES (@Name, @Description, @ReceiptDate, @Image, @Quantity, @ResponsibleId, @TempResponsibleId, @ConsumableTypeId)";

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
                MySqlCommand command = new MySqlCommand("DELETE FROM Consumables WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}