﻿using System;
using System.Windows;
using System.Windows.Controls;
using inventory.Models;
using inventory.Context.Common;
using MySql.Data.MySqlClient;

namespace inventory.Pages
{
    public partial class LoginPage : Page
    {
        public bool IsMenuVisible => false;
        private readonly DBConnection _dbConnection;

        public LoginPage()
        {
            InitializeComponent();
            _dbConnection = new DBConnection();
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }

            try
            {
                using (var connection = (MySqlConnection)_dbConnection.OpenConnection("MySql"))
                {
                    var query = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            CurrentUser.Id = dataReader.GetInt32(0);
                            CurrentUser.Login = dataReader.GetString(1);
                            CurrentUser.Role = dataReader.GetString(3);
                            string lastName = dataReader.GetString(5);
                            string firstName = dataReader.GetString(6);
                            string middleName = dataReader.IsDBNull(7) ? "" : dataReader.GetString(7);
                            CurrentUser.FullName = $"{lastName} {firstName} {middleName}".Trim();
                            Application.Current.Properties["CurrentUserLogin"] = CurrentUser.Login;
                            System.Diagnostics.Debug.WriteLine($"Login successful: Id={CurrentUser.Id}, Role={CurrentUser.Role}, CurrentUserLogin={Application.Current.Properties["CurrentUserLogin"]}");
                            var mainWindow = (MainWindow)Application.Current.MainWindow;
                            mainWindow.NavigateToMainPage();
                        }
                        else
                        {
                            ShowError("Неверный логин или пароль");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка авторизации: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
//public static class CurrentUser
//{
//    public static int Id { get; set; }
//    public static string Login { get; set; }
//    public static string Role { get; set; }
//    public static string FullName { get; set; }
//    public static bool IsAdmin => Role?.ToLower() == "admin";
//}