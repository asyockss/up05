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

namespace inventory.Pages
{
    public partial class InventoryPage : Page, INotifyPropertyChanged
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
                        // Получаем все оборудование для данной инвентаризации
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

                            // Записываем результат проверки
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

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                // Заголовок акта
                worksheet.Cells[1, 1] = "АКТ приема-передачи оборудования";
                worksheet.Cells[2, 1] = $"г. Пермь, {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = "КГАПОУ Пермский Авиационный техникум им. А.Д. Швецова передает:";

                // Шапка таблицы
                worksheet.Cells[5, 1] = "№";
                worksheet.Cells[5, 2] = "Название";
                worksheet.Cells[5, 3] = "Инв. номер";
                worksheet.Cells[5, 4] = "Стоимость";
                worksheet.Cells[5, 5] = "Ответственный";

                int row = 6;
                var equipmentContext = new EquipmentContext();
                var equipments = equipmentContext.AllEquipment();
                foreach (var equip in equipments)
                {
                    worksheet.Cells[row, 1] = row - 5;
                    worksheet.Cells[row, 2] = equip.Name;
                    worksheet.Cells[row, 3] = equip.InventoryNumber;
                    worksheet.Cells[row, 4] = equip.Cost?.ToString("F2") ?? "Не указана";
                    worksheet.Cells[row, 5] = equip.Responsible?.FullName ?? "Не назначен";
                    row++;
                }

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();
                    MessageBox.Show("Акт успешно сгенерирован.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации акта: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void GenerateComsumables_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                // Заголовок акта
                worksheet.Cells[1, 1] = "АКТ приема-передачи оборудования";
                worksheet.Cells[2, 1] = $"г. Пермь, {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = "КГАПОУ Пермский Авиационный техникум им. А.Д. Швецова передает:";

                // Шапка таблицы
                worksheet.Cells[5, 1] = "№";
                worksheet.Cells[5, 2] = "Название";
                worksheet.Cells[5, 3] = "Описание";
                worksheet.Cells[5, 4] = "Дата поступления";
                worksheet.Cells[5, 5] = "Ответственный";
                worksheet.Cells[5, 5] = "Количество";

                int row = 6;
                var consumableContext = new ConsumableContext();
                var consumables = consumableContext.AllConsumables();
                foreach (var cons in consumables)
                {
                    worksheet.Cells[row, 1] = row - 5;
                    worksheet.Cells[row, 2] = cons.Name;
                    worksheet.Cells[row, 3] = cons.Description;
                    worksheet.Cells[row, 4] = cons.ReceiptDate;
                    worksheet.Cells[row, 5] = cons.ResponsibleId;
                    worksheet.Cells[row, 5] = cons.Quantity;
                    row++;
                }

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();
                    MessageBox.Show("Акт успешно сгенерирован.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации акта: {ex.Message}");
            }
        }
    }
}