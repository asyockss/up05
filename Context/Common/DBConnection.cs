using System;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using inventory.Interfase;
namespace inventory.Context.Common
{
    public class DBConnection : IDatabaseConnection
    {
        public static string MySqlConfig = "server=127.0.0.1;database=up05;port=3307;user=root;password=;";
        public static string OleDbConfig = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=yourDatabasePath.mdb;";

        public object OpenConnection(string provider = "MySql")
        {
            if (provider == "MySql")
            {
                MySqlConnection connection = new MySqlConnection(MySqlConfig);
                connection.Open();
                return connection;
            }
            else if (provider == "OleDb")
            {
                OleDbConnection connection = new OleDbConnection(OleDbConfig);
                connection.Open();
                return connection;
            }
            else
            {
                throw new ArgumentException("Unsupported database provider");
            }
        }

        public object Query(string sql, object connection)
        {
            if (connection is MySqlConnection)
            {
                MySqlCommand command = new MySqlCommand(sql, (MySqlConnection)connection);
                return command.ExecuteReader();
            }
            else if (connection is OleDbConnection)
            {
                OleDbCommand command = new OleDbCommand(sql, (OleDbConnection)connection);
                return command.ExecuteReader();
            }
            else
            {
                throw new ArgumentException("Unsupported connection type");
            }
        }

        public void CloseConnection(object connection)
        {
            if (connection is MySqlConnection)
            {
                ((MySqlConnection)connection).Close();
                MySqlConnection.ClearPool((MySqlConnection)connection);
            }
            else if (connection is OleDbConnection)
            {
                ((OleDbConnection)connection).Close();
            }
            else
            {
                throw new ArgumentException("Unsupported connection type");
            }
        }
    }
}
