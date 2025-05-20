using MySql.Data.MySqlClient;
using System.Collections.Generic;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class UserContext
    {
        public List<User> AllUsers()
        {
            List<User> allUsers = new List<User>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM Users", connection);
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
                            Email = reader.GetString(4),
                            LastName = reader.GetString(5),
                            FirstName = reader.GetString(6),
                            MiddleName = reader.GetString(7),
                            Phone = reader.GetString(8),
                            Address = reader.GetString(9)
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
                    ? "UPDATE Users SET Login = @Login, Password = @Password, Role = @Role, Email = @Email, LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, Phone = @Phone, Address = @Address WHERE Id = @Id"
                    : "INSERT INTO Users (Login, Password, Role, Email, LastName, FirstName, MiddleName, Phone, Address) VALUES (@Login, @Password, @Role, @Email, @LastName, @FirstName, @MiddleName, @Phone, @Address)";

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
                MySqlCommand command = new MySqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}