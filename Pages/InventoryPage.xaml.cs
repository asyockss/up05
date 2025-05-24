using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;
using MySql.Data.MySqlClient;
using inventory.Context.Common;
using Microsoft.Office.Interop.Excel;

namespace inventory.Pages
{
    public partial class InventoryPage : System.Windows.Controls.Page, INotifyPropertyChanged
    {

        private InventoryContext inventoryContext;
        private List<Inventory> _inventoryList;
        private DateTime? _startDateFilter;
        private DateTime? _endDateFilter;
        private Inventory _selectedInventory;
        public Inventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged(nameof(SelectedInventory));
            }
        }
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
            DataContext = this;
            inventoryContext = new InventoryContext();
            LoadInventories();
        }

        public void LoadInventories()
        {
            InventoryList = inventoryContext.AllInventories().ToList();
        }

        private void FilterInventories()
        {
            IEnumerable<Inventory> inventories = inventoryContext.AllInventories();
            if (StartDateFilter.HasValue)
                inventories = inventories.Where(i => i.StartDate >= StartDateFilter.Value);
            if (EndDateFilter.HasValue)
                inventories = inventories.Where(i => i.EndDate <= EndDateFilter.Value);
            InventoryList = inventories.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("У вас нет прав на добавление инвентаризацией.");
                return;
            }
            else
            {
                var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditInventoryPage());
            }
            
        }

        private void PerformCheck_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInventory == null)
            {
                MessageBox.Show("Выберите инвентаризацию для проверки");
                return;
            }
            var checkWindow = new InventoryCheckWindow(SelectedInventory);
            if (checkWindow.ShowDialog() == true)
            {
                LoadInventories();
            }
            if (InventoryList == null || InventoryList.Count == 0)
            {
                MessageBox.Show("Нет доступных инвентаризаций для проверки.");
                return;
            }
            PerformInventoryCheck();
        }

        private void PerformInventoryCheck()
        {
            try
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    foreach (var inventory in InventoryList)
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT id, status_id FROM equipment WHERE inventory_id = @InventoryId", connection);
                        cmd.Parameters.AddWithValue("@InventoryId", inventory.Id);
                        List<(int id, int statusId)> equipmentList = new List<(int, int)>();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipmentList.Add((reader.GetInt32(0), reader.GetInt32(1)));
                            }
                        }

                        foreach (var equipment in equipmentList)
                        {
                            int equipmentId = equipment.id;
                            int statusId = equipment.statusId;
                            Status status = new StatusContext().AllStatuses().FirstOrDefault(s => s.Id == statusId);
                            string comment = (status != null && status.Name == "Активен")
                                ? "Оборудование в порядке"
                                : $"Оборудование не активно, статус: {status?.Name ?? "Неизвестен"}";

                            InventoryCheck check = new InventoryCheck
                            {
                                InventoryId = inventory.Id,
                                EquipmentId = equipmentId,
                                UserId = CurrentUser.Id,
                                CheckDate = DateTime.Now,
                                Comment = comment
                            };
                            InventoryCheckContext checkContext = new InventoryCheckContext();
                            checkContext.Id = check.Id;
                            checkContext.InventoryId = check.InventoryId;
                            checkContext.EquipmentId = check.EquipmentId;
                            checkContext.UserId = check.UserId;
                            checkContext.CheckDate = check.CheckDate;
                            checkContext.Comment = check.Comment;
                            checkContext.Save();
                        }
                    }
                }
                MessageBox.Show("Проверка инвентаризации выполнена успешно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении проверки: {ex.Message}");
            }
        }

        private void GenerateEquipmentTransferTemp_Click(object sender, RoutedEventArgs e)
        {
            new GenerateActWindow("Акт оборудования (врем.)").ShowDialog();
        }

        private void GenerateConsumablesTransfer_Click(object sender, RoutedEventArgs e)
        {
            new GenerateActWindow("Акт расходников").ShowDialog();
        }

        private void GenerateEquipmentTransferPermanent_Click(object sender, RoutedEventArgs e)
        {
            new GenerateActWindow("Акт оборудования (пост.)").ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}