using System;
using MySql.Data.MySqlClient;
using inventory.Context.Common;
using System.IO;

namespace inventory.Context.Common
{
    public class Logging
    {
        private static readonly string LogFilePath = "error_log.txt";

        public static void LogError(Exception ex, string message)
        {
            // Логирование в базу данных
            try
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    string query = "INSERT INTO error_logs (message, stack_trace, log_date) VALUES (@Message, @StackTrace, @LogDate)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Message", $"{message}: {ex.Message}");
                    command.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception logEx)
            {
                // Если не удалось записать в базу, записать в файл
                File.AppendAllText(LogFilePath, $"{DateTime.Now}: Не удалось записать в базу: {logEx.Message}\n");
            }

            // Логирование в файл
            try
            {
                File.AppendAllText(LogFilePath, $"{DateTime.Now}: {message}: {ex.Message}\nStackTrace: {ex.StackTrace}\n\n");
            }
            catch (Exception)
            {
                // Игнорировать ошибки записи в файл
            }
        }
    }
}