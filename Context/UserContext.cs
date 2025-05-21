using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using inventory.Context.Common;
using System;

namespace inventory.Context.MySql
{
    public class UserContext
    {
        public List<User> AllUsers()
        {
            List<User> allUsers = new List<User>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM users", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allUsers.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            LastName = reader.GetString(5),
                            FirstName = reader.GetString(6),
                            MiddleName = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Phone = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Address = reader.IsDBNull(9) ? null : reader.GetString(9)
                        });
                    }
                }
            }
            return allUsers;
        }

        public void Save(User user, bool update = false)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = update
                    ? "UPDATE users SET login = @Login, password = @Password, role = @Role, email = @Email, last_name = @LastName, first_name = @FirstName, middle_name = @MiddleName, phone = @Phone, address = @Address WHERE id = @Id"
                    : "INSERT INTO users (login, password, role, email, last_name, first_name, middle_name, phone, address) VALUES (@Login, @Password, @Role, @Email, @LastName, @FirstName, @MiddleName, @Phone, @Address)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Login", user.Login);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@MiddleName", user.MiddleName);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Address", user.Address);

                command.ExecuteNonQuery();
                if (!update)
                {
                    user.Id = (int)command.LastInsertedId;
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    MySqlCommand updateConsumablesCommand = new MySqlCommand(
                        "UPDATE consumables SET timeresponsible_id = NULL, responsible_id = NULL WHERE timeresponsible_id = @Id OR responsible_id = @Id", connection, transaction);
                    updateConsumablesCommand.Parameters.AddWithValue("@Id", id);
                    updateConsumablesCommand.ExecuteNonQuery();

                    MySqlCommand updateCharacteristicValuesCommand = new MySqlCommand(
                        "UPDATE consumable_characteristic_values SET consumable_id = NULL WHERE consumable_id = @Id", connection, transaction);
                    updateCharacteristicValuesCommand.Parameters.AddWithValue("@Id", id);
                    updateCharacteristicValuesCommand.ExecuteNonQuery();

                    MySqlCommand updateEquipmentCommand = new MySqlCommand(
                        "UPDATE equipment SET responsible_id = NULL, timeresponsible_id = NULL WHERE responsible_id = @Id OR timeresponsible_id = @Id", connection, transaction);
                    updateEquipmentCommand.Parameters.AddWithValue("@Id", id);
                    updateEquipmentCommand.ExecuteNonQuery();

                    MySqlCommand deleteInventoriesCommand = new MySqlCommand(
                        "DELETE FROM inventories WHERE users_id = @Id", connection, transaction);
                    deleteInventoriesCommand.Parameters.AddWithValue("@Id", id);
                    deleteInventoriesCommand.ExecuteNonQuery();

                    MySqlCommand updateRoomsCommand = new MySqlCommand(
                        "UPDATE rooms SET responsible_id = NULL, timeresponsible_id = NULL WHERE responsible_id = @Id OR timeresponsible_id = @Id", connection, transaction);
                    updateRoomsCommand.Parameters.AddWithValue("@Id", id);
                    updateRoomsCommand.ExecuteNonQuery();

                    MySqlCommand deleteUserCommand = new MySqlCommand("DELETE FROM users WHERE id = @Id", connection, transaction);
                    deleteUserCommand.Parameters.AddWithValue("@Id", id);
                    deleteUserCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

    }
}