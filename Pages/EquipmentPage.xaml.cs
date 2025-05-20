using inventory.Context.MySql;
using inventory.Models;
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
using System.Xml.Linq;

namespace inventory.Pages
{
    public partial class EquipmentPage : Page
    {
        public List<Equipment> equipment = EquipmentContext.AllEquipment().Cast<Equipment>().ToList();
        public EquipmentPage()
        {
            InitializeComponent();
            CreateUI();
        }

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (Equipment item in equipment)
            {
                parent.Children.Add(new Elements.EquipmentCard(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления нового оборудования
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования оборудования
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления оборудования
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            equipment = EquipmentContext.AllEquipment().Cast<Equipment>().ToList();
            CreateUI();
        }
    }
}
