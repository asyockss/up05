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
using System.Text;

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
                MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM inventories WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", inventory.Id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void LogDiscrepancy(Inventory inventory)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                // Проверяем, существует ли inventory_id в таблице inventories
                MySqlCommand checkInventoryCommand = new MySqlCommand("SELECT COUNT(*) FROM inventories WHERE id = @Id", connection);
                checkInventoryCommand.Parameters.AddWithValue("@Id", inventory.Id);
                int inventoryCount = Convert.ToInt32(checkInventoryCommand.ExecuteScalar());

                // Проверяем, существует ли equipment_id в таблице equipment
                MySqlCommand checkEquipmentCommand = new MySqlCommand("SELECT COUNT(*) FROM equipment WHERE id = @Id", connection);
                checkEquipmentCommand.Parameters.AddWithValue("@Id", inventory.Id);
                int equipmentCount = Convert.ToInt32(checkEquipmentCommand.ExecuteScalar());

                // Проверяем, существует ли user_id в таблице users
                MySqlCommand checkUserCommand = new MySqlCommand("SELECT COUNT(*) FROM users WHERE id = @Id", connection);
                checkUserCommand.Parameters.AddWithValue("@Id", CurrentUser.Id);
                int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                if (inventoryCount > 0 && equipmentCount > 0 && userCount > 0)
                {
                    InventoryCheck check = new InventoryCheck
                    {
                        InventoryId = inventory.Id,
                        EquipmentId = inventory.Id,
                        UserId = CurrentUser.Id, 
                        CheckDate = DateTime.Now,
                        Comment = "Что-то это значит"
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
                else
                {
                    MessageBox.Show($"Ошибка: inventory_id, equipment_id или user_id не существует в соответствующих таблицах.");
                }
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;

                Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Sheets[1];

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Start Date";
                worksheet.Cells[1, 4].Value = "End Date";
                worksheet.Cells[1, 5].Value = "User ID";

                int row = 2;
                foreach (var inventory in InventoryList)
                {
                    worksheet.Cells[row, 1].Value = inventory.Id;
                    worksheet.Cells[row, 2].Value = inventory.Name;
                    worksheet.Cells[row, 3].Value = inventory.StartDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 4].Value = inventory.EndDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = inventory.UserId;
                    row++;
                }

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();
                    MessageBox.Show("Отчет успешно сгенерирован.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}