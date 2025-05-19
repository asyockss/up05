using System;
using MySql.Data.MySqlClient;
using inventory.Interfase;

namespace inventory.Context.Common
{
    public class DBConnection : IDatabaseConnection
    {
        public static string MySqlConfig = "server=127.0.0.1;database=up05;port=3307;user=root;password=;";

        public object OpenConnection()
        {
            return OpenConnection("MySql");
        }

        public object OpenConnection(string provider)
        {
            if (provider == "MySql")
            {
                MySqlConnection connection = new MySqlConnection(MySqlConfig);
                connection.Open();
                return connection;
            }
            else
            {
                throw new ArgumentException("Неподдерживаемый провайдер базы данных");
            }
        }

        public object Query(string sql, object connection)
        {
            if (connection is MySqlConnection)
            {
                MySqlCommand command = new MySqlCommand(sql, (MySqlConnection)connection);
                return command.ExecuteReader();
            }
            else
            {
                throw new ArgumentException("Неподдерживаемый тип соединения");
            }
        }

        public void CloseConnection(object connection)
        {
            if (connection is MySqlConnection)
            {
                ((MySqlConnection)connection).Close();
                MySqlConnection.ClearPool((MySqlConnection)connection);
            }
            else
            {
                throw new ArgumentException("Неподдерживаемый тип соединения");
            }
        }
    }
}
