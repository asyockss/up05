using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Context.Common;
using inventory.Models;
using MySql.Data.MySqlClient;
using inventory.Interfase;

namespace inventory.Pages
{
    public partial class InventoryPage : Page, INotifyPropertyChanged
    {
        private InventoryContext inventoryContext;
        private EquipmentContext equipmentContext;
        private List<Inventory> _inventoryList;
        private DateTime? _startDateFilter;
        private DateTime? _endDateFilter;

        public List<Inventory> InventoryList
        {
            get => _inventoryList;
            set { _inventoryList = value; OnPropertyChanged(nameof(InventoryList)); }
        }

        public DateTime? StartDateFilter
        {
            get => _startDateFilter;
            set { _startDateFilter = value; OnPropertyChanged(nameof(StartDateFilter)); FilterInventories(); }
        }

        public DateTime? EndDateFilter
        {
            get => _endDateFilter;
            set { _endDateFilter = value; OnPropertyChanged(nameof(EndDateFilter)); FilterInventories(); }
        }

        public bool IsMenuVisible => true;

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
            DataContext = this;
            inventoryContext = new InventoryContext();
            LoadInventories();
        }

        public void LoadInventories()
        {
            InventoryList = inventoryContext.AllInventorys().ToList(); 
        }

        private void FilterInventories()
        {
            IEnumerable<Inventory> inventories = inventoryContext.AllInventorys();
            if (StartDateFilter.HasValue)
                inventories = inventories.Where(i => i.StartDate >= StartDateFilter.Value);
            if (EndDateFilter.HasValue)
                inventories = inventories.Where(i => i.EndDate <= EndDateFilter.Value);

            InventoryList = inventories.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditInventoryPage());
        }

        private void PerformCheck_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryList == null || InventoryList.Count == 0)
            {
                MessageBox.Show("Нет доступных инвентаризаций для проверки.");
                return;
            }

            PerformInventoryCheck();
            MessageBox.Show("Проверка инвентаризации выполнена успешно.");
        }


        private void PerformInventoryCheck()
        {
            try
            {
                foreach (var inventory in InventoryList)
                {
                    bool isValid = CheckInventoryItem(inventory);
                    if (!isValid)
                    {
                        LogDiscrepancy(inventory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении проверки: {ex.Message}");
            }
        }


        private bool CheckInventoryItem(Inventory inventory)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM equipment WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", inventory.Id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void LogDiscrepancy(Inventory inventory)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                // Убедитесь, что inventory.Id существует в таблице equipment
                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM equipment WHERE id = @Id", connection);
                checkCommand.Parameters.AddWithValue("@Id", inventory.Id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    // Создаем объект InventoryCheck и заполняем его данными
                    InventoryCheck check = new InventoryCheck
                    {
                        InventoryId = inventory.Id,
                        EquipmentId = inventory.Id, // Предполагаем, что equipment_id совпадает с inventory.Id
                        UserId = CurrentUser.Id, // Предполагаем, что CurrentUser.Id - это ID текущего пользователя
                        CheckDate = DateTime.Now,
                        Comment = "Discrepancy found during inventory check"
                    };

                    // Сохраняем запись в таблицу inventory_checks
                    InventoryCheckContext checkContext = new InventoryCheckContext();
                    checkContext.Id = check.Id;
                    checkContext.InventoryId = check.InventoryId;
                    checkContext.EquipmentId = check.EquipmentId;
                    checkContext.UserId = check.UserId;
                    checkContext.CheckDate = check.CheckDate;
                    checkContext.Comment = check.Comment;
                    checkContext.Save();
                }
                else
                {
                    MessageBox.Show($"Ошибка: equipment_id {inventory.Id} не существует в таблице equipment.");
                }
            }
        }



        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Генерация отчета еще не реализована.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}