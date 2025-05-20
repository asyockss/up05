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
using inventory.Context;
using inventory.Models;

namespace inventory.Pages
{
    public partial class ConsumablesPage : Page
    {
        public List<Consumable> consumables = ConsumableContext.AllConsumables().Cast<Consumable>().ToList();
        public ConsumablesPage()
        {
            InitializeComponent();
            CreateUI();
        }

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (Consumable item in consumables)
            {
                parent.Children.Add(new Elements.ConsumableCard(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления нового расходника
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования расходника
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Логика для удаления расходника
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            consumables = ConsumableContext.AllConsumables().Cast<Consumable>().ToList();
            CreateUI();
        }
    }
}
