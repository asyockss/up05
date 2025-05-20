using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;
using inventory.ViewModels;

namespace inventory.Pages
{
    public partial class InventoryPage : Page
    {
        public List<Inventory> inventories = InventoryContext.AllInventorys().Cast<Inventory>().ToList();
        public InventoryPage()
        {
            InitializeComponent();
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Доступ запрещен. Требуются права администратора.");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToMainPage();
                return;
            }
            DataContext = new MainPageViewModel();
            CreateUI();
        }

        public bool IsMenuVisible => true;

        public void CreateUI()
        {
            parent.Children.Clear();
            foreach (Inventory item in inventories)
            {
                parent.Children.Add(new Elements.InventoryCard(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления новой инвентаризации
        }

        private void PerformCheck_Click(object sender, RoutedEventArgs e)
        {
            // Логика для проведения проверки
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // Логика для генерации отчета
        }
    }
}
