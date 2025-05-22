using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;

namespace inventory.Pages
{
    public partial class EquipmentPage : Page, INotifyPropertyChanged
    {
        private List<Equipment> _equipmentList;
        private string _searchText;

        public List<Equipment> EquipmentList
        {
            get => _equipmentList;
            set { _equipmentList = value; OnPropertyChanged(nameof(EquipmentList)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterEquipment(); }
        }

        public bool IsMenuVisible => true;

        public EquipmentPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadEquipment();
        }

        public void LoadEquipment()
        {
            var equipment = new EquipmentContext().AllEquipment();
            var types = new EquipmentTypeContext().AllEquipmentTypes().ToDictionary(t => t.Id, t => t);
            var statuses = new StatusContext().AllStatuses().ToDictionary(s => s.Id, s => s);

            foreach (var item in equipment)
            {
                if (types.TryGetValue(item.EquipmentTypeId, out var equipmentType))
                    item.EquipmentType = equipmentType;
                if (statuses.TryGetValue(item.StatusId, out var status))
                    item.Status = status;
            }
            EquipmentList = equipment;
        }

        private void FilterEquipment()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                LoadEquipment();
            }
            else
            {
                EquipmentList = EquipmentList
                    .Where(e => e.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                e.InventoryNumber.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditEquipmentPage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadEquipment();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void ImportFromExcel_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog { Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx" };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Open(openFileDialog.FileName);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    var range = worksheet.UsedRange;

                    var equipmentContext = new EquipmentContext();
                    var inventoryContext = new InventoryContext();
                    var defaultInventory = inventoryContext.AllInventorys().FirstOrDefault(); // Get a default inventory
                    if (defaultInventory == null)
                        throw new Exception("No inventory available for import.");

                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        var equipment = new Equipment
                        {
                            Name = range.Cells[row, 2].Value?.ToString() ?? "Unknown",
                            InventoryNumber = range.Cells[row, 3].Value?.ToString() ?? Guid.NewGuid().ToString(),
                            StatusId = 1, // Default status (adjust as needed)
                            EquipmentTypeId = 1, // Default type (adjust as needed)
                            InventoryId = defaultInventory.Id // Required field
                        };
                        equipmentContext.Save(equipment);
                    }

                    workbook.Close();
                    excelApp.Quit();
                    LoadEquipment();
                    MessageBox.Show("Импорт успешно завершен.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при импорте: {ex.Message}");
                }
            }
        }
    }
}