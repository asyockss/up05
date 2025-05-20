using inventory.Context.MySql;
using inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public List<UserContext> userContexts = UserContext.AllUsers();
        public UsersPage()
        {
            InitializeComponent();
            CreateUI();
        }

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (UserContext item in userContexts)
            {
                parent.Children.Add(new Elements.UserCard(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления нового пользователя
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования пользователя
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления пользователя
        }
    }

}
