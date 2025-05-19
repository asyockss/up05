using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace inventory.Context
{
    public abstract class BaseContext
    {
        protected abstract string ConnectionString { get; }

        protected virtual IDbConnection CreateConnection()
        {
            if (ConnectionString.Contains("Provider="))
                return new OleDbConnection(ConnectionString);
            else
                return new MySqlConnection(ConnectionString);
        }

        protected DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(connection, query, parameters))
                {
                    var dataTable = new DataTable();

                    if (connection is OleDbConnection)
                    {
                        using (var adapter = new OleDbDataAdapter((OleDbCommand)command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    else
                    {
                        using (var adapter = new MySqlDataAdapter((MySqlCommand)command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }
            }
        }

        protected int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(connection, query, parameters))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }

        protected object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = CreateCommand(connection, query, parameters))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        private IDbCommand CreateCommand(IDbConnection connection, string query, Dictionary<string, object> parameters)
        {
            IDbCommand command;

            if (connection is OleDbConnection)
                command = new OleDbCommand(query, (OleDbConnection)connection);
            else
                command = new MySqlCommand(query, (MySqlConnection)connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = param.Key;
                    parameter.Value = param.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}