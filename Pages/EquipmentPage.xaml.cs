using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
            var rooms = new RoomContext().AllRooms().ToDictionary(r => r.Id, r => r);
            var users = new UserContext().AllUsers().ToDictionary(u => u.Id, u => u);
            var inventories = new InventoryContext().AllInventorys().ToDictionary(i => i.Id, i => i);
            var models = new EquipmentModelContext().AllEquipmentModels().ToDictionary(m => m.Id, m => m);
            var directions = new DirectionContext().AllDirections().ToDictionary(d => d.Id, d => d);

            foreach (var item in equipment)
            {
                if (item.EquipmentTypeId.HasValue && types.TryGetValue(item.EquipmentTypeId.Value, out var equipmentType))
                    item.EquipmentType = equipmentType;
                if (item.StatusId.HasValue && statuses.TryGetValue(item.StatusId.Value, out var status))
                    item.Status = status;
                if (item.RoomId.HasValue && rooms.TryGetValue(item.RoomId.Value, out var room))
                    item.Room = room;
                if (item.ResponsibleId.HasValue && users.TryGetValue(item.ResponsibleId.Value, out var responsible))
                    item.Responsible = responsible;
                if (item.TempResponsibleId.HasValue && users.TryGetValue(item.TempResponsibleId.Value, out var tempResponsible))
                    item.TempResponsible = tempResponsible;
                if (item.ModelId.HasValue && models.TryGetValue(item.ModelId.Value, out var model))
                    item.Model = model;
                if (item.DirectionId.HasValue && directions.TryGetValue(item.DirectionId.Value, out var direction))
                    item.Direction = direction;
                if (inventories.TryGetValue(item.InventoryId, out var inventory))
                    item.Inventory = inventory;
            }
            EquipmentList = equipment;
        }

        private void FilterEquipment()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                EquipmentList = new EquipmentContext().AllEquipment().OrderBy(e => e.Name).ToList();
            }
            else
            {
                EquipmentList = EquipmentList
                    .Where(e => e.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderBy(e => e.Name)
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

                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        string name = range.Cells[row, 1].Value?.ToString();
                        byte[] photo = null; // You need to handle image import separately

                        int rid, resid, trid, sid, mid, etid, did;
                        decimal c;

                        int? roomId = int.TryParse(range.Cells[row, 2].Value?.ToString(), out rid) ? rid : (int?)null;
                        int? responsibleId = int.TryParse(range.Cells[row, 3].Value?.ToString(), out resid) ? resid : (int?)null;
                        int? tempResponsibleId = int.TryParse(range.Cells[row, 4].Value?.ToString(), out trid) ? trid : (int?)null;
                        decimal? cost = decimal.TryParse(range.Cells[row, 5].Value?.ToString(), out c) ? c : (decimal?)null;
                        string comment = range.Cells[row, 6].Value?.ToString();
                        int? statusId = int.TryParse(range.Cells[row, 7].Value?.ToString(), out sid) ? sid : (int?)null;
                        int? modelId = int.TryParse(range.Cells[row, 8].Value?.ToString(), out mid) ? mid : (int?)null;
                        int? equipmentTypeId = int.TryParse(range.Cells[row, 9].Value?.ToString(), out etid) ? etid : (int?)null;
                        int? directionId = int.TryParse(range.Cells[row, 10].Value?.ToString(), out did) ? did : (int?)null;
                        string inventoryId = range.Cells[row, 11].Value?.ToString();

                        var equipment = new Equipment
                        {
                            Name = name,
                            Photo = photo,
                            RoomId = roomId,
                            ResponsibleId = responsibleId,
                            TempResponsibleId = tempResponsibleId,
                            Cost = cost,
                            Comment = comment,
                            StatusId = statusId,
                            ModelId = modelId,
                            EquipmentTypeId = equipmentTypeId,
                            DirectionId = directionId,
                            InventoryId = int.Parse(inventoryId)
                        };
                        equipmentContext.Save(equipment);
                    }

                    Marshal.ReleaseComObject(range);
                    Marshal.ReleaseComObject(worksheet);
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

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