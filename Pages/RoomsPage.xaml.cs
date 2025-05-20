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
    /// Логика взаимодействия для RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : Page
    {
        public List<RoomContext> roomContexts = RoomContext.AllRooms();
        public RoomsPage()
        {
            InitializeComponent();
            CreateUI();
        }

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (RoomContext item in roomContexts)
            {
                parent.Children.Add(new Elements.RoomCard(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления новой аудитории
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования аудитории
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления аудитории
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            roomContexts = RoomContext.AllRooms();
            CreateUI();
        }
    }

}
