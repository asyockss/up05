using System;
using System.Windows;
using System.Windows.Controls;
using inventory.Models;
using System.Linq;
using inventory.Context.Common;
using inventory.Context;

namespace inventory.Pages
{
    public partial class LoginPage : Page
    {
        public bool IsMenuVisible => false;
        private readonly InventoryContext _context;

        public LoginPage()
        {
            InitializeComponent();
            _context = new InventoryContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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
                var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user == null)
                {
                    ShowError("Неверный логин или пароль");
                    return;
                }

                // Сохраняем данные пользователя
                CurrentUser.Id = user.Id;
                CurrentUser.Login = user.Login;
                CurrentUser.Role = user.Role;
                CurrentUser.FullName = user.FullName;

                // Переходим на главную страницу
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToMainPage();
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

    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static string Login { get; set; }
        public static string Role { get; set; }
        public static string FullName { get; set; }

        public static bool IsAdmin => Role?.ToLower() == "admin";
    }
}