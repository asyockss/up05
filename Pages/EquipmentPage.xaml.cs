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
using System.Xml.Linq;

namespace inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public List<EquipmentContext> equipmentContexts = EquipmentContext.AllEquipment();
        public EquipmentPage()
        {
            InitializeComponent();
            CreateUI();
        }

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (EquipmentContext item in equipmentContexts)
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
            equipmentContexts = EquipmentContext.AllEquipment();
            CreateUI();
        }
    }

}
