using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using inventory.Context.MySql;
using inventory.Models;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace inventory
{
    /// <summary>
    /// Логика взаимодействия для GenerateActWindow.xaml
    /// </summary>
    public partial class GenerateActWindow : System.Windows.Window
    {
        private List<User> employees;
        private List<Equipment> equipmentList;
        private List<TransferConsumable> transferConsumables;
        private string selectedActType;
        public GenerateActWindow(string actType)
        {
            InitializeComponent();
            selectedActType = actType;
            LoadEmployees();
            ActTypeComboBox.SelectedItem = ActTypeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == actType);
        }
        private void LoadEmployees()
        {
            employees = new UserContext().AllUsers().Where(u => u.Role == "employee" || u.Role == "teacher").ToList();
            EmployeeComboBox.ItemsSource = employees;
            EmployeeComboBox.DisplayMemberPath = "FullName";
        }

        private void ActTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedActType = (ActTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DynamicContentPanel.Children.Clear();

            if (selectedActType == "Акт оборудования (врем.)" || selectedActType == "Акт оборудования (пост.)")
            {
                LoadEquipment();
                var listView = new ListView { SelectionMode = SelectionMode.Multiple };
                listView.View = new GridView
                {
                    Columns =
                    {
                        new GridViewColumn { Header = "Название", DisplayMemberBinding = new Binding("Name") },
                        new GridViewColumn { Header = "Инв. номер", DisplayMemberBinding = new Binding("InventoryId") },
                        new GridViewColumn { Header = "Стоимость", DisplayMemberBinding = new Binding("Cost") }
                    }
                };
                listView.ItemsSource = equipmentList;
                DynamicContentPanel.Children.Add(listView);

                if (selectedActType == "Акт оборудования (врем.)")
                {
                    var returnDateLabel = new System.Windows.Controls.Label { Content = "Дата возврата:" };
                    var returnDatePicker = new DatePicker();
                    DynamicContentPanel.Children.Add(returnDateLabel);
                    DynamicContentPanel.Children.Add(returnDatePicker);
                }
            }
            else if (selectedActType == "Акт расходников")
            {
                LoadConsumables();
                var dataGrid = new DataGrid { AutoGenerateColumns = false };
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Consumable.Name"), IsReadOnly = true });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Доступно", Binding = new Binding("Consumable.Quantity"), IsReadOnly = true });
                dataGrid.Columns.Add(new DataGridTextColumn { Header = "Передача", Binding = new Binding("TransferQuantity") });
                dataGrid.ItemsSource = transferConsumables;
                DynamicContentPanel.Children.Add(dataGrid);
            }
        }

        private void LoadEquipment()
        {
            equipmentList = new EquipmentContext().AllEquipment();
        }

        private void LoadConsumables()
        {
            var consumables = new ConsumableContext().AllConsumables();
            transferConsumables = consumables.Select(c => new TransferConsumable { Consumable = c, TransferQuantity = 0 }).ToList();
        }

        private void GenerateAct_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            var employee = (User)EmployeeComboBox.SelectedItem;

            if (selectedActType == "Акт оборудования (врем.)")
            {
                var listView = DynamicContentPanel.Children.OfType<ListView>().FirstOrDefault();
                var selectedEquipment = listView?.SelectedItems.Cast<Equipment>().ToList();
                var returnDatePicker = DynamicContentPanel.Children.OfType<DatePicker>().FirstOrDefault();

                if (selectedEquipment == null || !selectedEquipment.Any() || returnDatePicker?.SelectedDate == null)
                {
                    MessageBox.Show("Выберите оборудование и дату возврата.");
                    return;
                }

                GenerateEquipmentTransferAct(employee, selectedEquipment, returnDatePicker.SelectedDate.Value, true);
            }
            else if (selectedActType == "Акт расходников")
            {
                var dataGrid = DynamicContentPanel.Children.OfType<DataGrid>().FirstOrDefault();
                var selectedConsumables = transferConsumables.Where(tc => tc.TransferQuantity > 0).ToList();

                if (!selectedConsumables.Any())
                {
                    MessageBox.Show("Укажите количество для передачи расходников.");
                    return;
                }

                GenerateConsumablesTransferAct(employee, selectedConsumables);
            }
            else if (selectedActType == "Акт оборудования (пост.)")
            {
                var listView = DynamicContentPanel.Children.OfType<ListView>().FirstOrDefault();
                var selectedEquipment = listView?.SelectedItems.Cast<Equipment>().ToList();

                if (selectedEquipment == null || !selectedEquipment.Any())
                {
                    MessageBox.Show("Выберите оборудование.");
                    return;
                }

                GenerateEquipmentTransferAct(employee, selectedEquipment, DateTime.MinValue, false);
            }
        }

        private void GenerateEquipmentTransferAct(User employee, List<Equipment> equipments, DateTime returnDate, bool isTemporary)
        {
            try
            {
                var excelApp = new Excel.Application { Visible = false };
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Worksheet)workbook.Sheets[1];

                string actTitle = isTemporary ? "АКТ приема-передачи оборудования на временное пользование" : "АКТ приема-передачи оборудования";
                worksheet.Cells[1, 1] = actTitle;
                worksheet.Cells[2, 1] = $"г. Пермь, {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = "КГАПОУ Пермский Авиационный техникум им. А.Д. Швецова передает сотруднику";
                worksheet.Cells[4, 1] = $"{employee.FullName} следующее оборудование:";

                int row = 6;
                worksheet.Cells[row, 1] = "№";
                worksheet.Cells[row, 2] = "Название";
                worksheet.Cells[row, 3] = "Серийный номер";
                worksheet.Cells[row, 4] = "Стоимость";
                row++;

                int index = 1;
                foreach (var equip in equipments)
                {
                    worksheet.Cells[row, 1] = index++;
                    worksheet.Cells[row, 2] = equip.Name;
                    worksheet.Cells[row, 3] = equip.InventoryId.ToString();
                    worksheet.Cells[row, 4] = equip.Cost?.ToString("F2") ?? "Не указана";
                    row++;
                }

                if (isTemporary)
                {
                    worksheet.Cells[row, 1] = $"Дата возврата: {returnDate:dd.MM.yyyy}";
                }

                worksheet.Cells[row + 2, 1] = employee.FullName;

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();
                    MessageBox.Show("Акт успешно сгенерирован.");
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void GenerateConsumablesTransferAct(User employee, List<TransferConsumable> consumables)
        {
            try
            {
                var excelApp = new Excel.Application { Visible = false };
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Worksheet)workbook.Sheets[1];

                worksheet.Cells[1, 1] = "АКТ приема-передачи расходных материалов";
                worksheet.Cells[2, 1] = $"г. Пермь, {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = "КГАПОУ Пермский Авиационный техникум им. А.Д. Швецова передает сотруднику";
                worksheet.Cells[4, 1] = $"{employee.FullName} следующие расходные материалы:";

                int row = 6;
                worksheet.Cells[row, 1] = "№";
                worksheet.Cells[row, 2] = "Название";
                worksheet.Cells[row, 3] = "Количество";
                worksheet.Cells[row, 4] = "Стоимость";
                row++;

                int index = 1;
                decimal totalCost = 0;
                foreach (var tc in consumables)
                {
                    decimal cost = (tc.Consumable.PricePerUnit ?? 0) * tc.TransferQuantity;
                    totalCost += cost;
                    worksheet.Cells[row, 1] = index++;
                    worksheet.Cells[row, 2] = tc.Consumable.Name;
                    worksheet.Cells[row, 3] = tc.TransferQuantity;
                    worksheet.Cells[row, 4] = $"{cost:F2} руб.";
                    row++;
                }

                worksheet.Cells[row, 1] = $"Итого: {totalCost:F2} руб.";
                worksheet.Cells[row + 2, 1] = employee.FullName;

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();
                    MessageBox.Show("Акт успешно сгенерирован.");
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    public class TransferConsumable
    {
        public Consumable Consumable { get; set; }
        public int TransferQuantity { get; set; }
    }
}
