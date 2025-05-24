using inventory.Context.MySql;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace inventory
{
    public partial class InventoryCheckWindow : Window
    {
        public Inventory SelectedInventory { get; set; }
        public List<InventoryCheckItem> CheckItems { get; set; }

        public InventoryCheckWindow(Inventory inventory)
        {
            InitializeComponent();
            SelectedInventory = inventory;

            var equipmentList = new EquipmentContext().AllEquipment()
                .Where(e => e.InventoryId == SelectedInventory.Id)
                .ToList();

            // Диагностика
            Console.WriteLine($"Найдено оборудования: {equipmentList.Count} для InventoryId: {SelectedInventory.Id}");

            if (CurrentUser.IsAdmin)
            {
                CheckItems = equipmentList.Select(e => new InventoryCheckItem { Equipment = e }).ToList();
            }
            else
            {
                CheckItems = equipmentList
                    .Where(e => e.ResponsibleId == CurrentUser.Id || e.TempResponsibleId == CurrentUser.Id)
                    .Select(e => new InventoryCheckItem { Equipment = e })
                    .ToList();
            }

            // Диагностика
            Console.WriteLine($"CheckItems после фильтрации: {CheckItems.Count}, IsAdmin: {CurrentUser.IsAdmin}, CurrentUser.Id: {CurrentUser.Id}");
            if (!CheckItems.Any())
            {
                MessageBox.Show("Нет оборудования, доступного для проверки.");
            }

            DataContext = this;
        }

        private void SaveCheck_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = CheckItems.Where(item => item.IsSelected).ToList();
            if (!selectedItems.Any())
            {
                MessageBox.Show("Выберите оборудование для проверки");
                return;
            }

            try
            {
                foreach (var item in selectedItems)
                {
                    if (!CurrentUser.IsAdmin && item.Equipment.ResponsibleId != CurrentUser.Id && item.Equipment.TempResponsibleId != CurrentUser.Id)
                    {
                        MessageBox.Show($"У вас нет прав на проверку оборудования: {item.Equipment.Name}");
                        continue;
                    }

                    var check = new InventoryCheck
                    {
                        InventoryId = SelectedInventory.Id,
                        EquipmentId = item.Equipment.Id,
                        UserId = CurrentUser.Id,
                        CheckDate = DateTime.Now,
                        Comment = item.CheckComment
                    };
                    var context = new InventoryCheckContext();
                    context.InventoryId = check.InventoryId;
                    context.EquipmentId = check.EquipmentId;
                    context.UserId = check.UserId;
                    context.CheckDate = check.CheckDate;
                    context.Comment = check.Comment;
                    context.Save();
                    Console.WriteLine($"Сохранена проверка для оборудования: {item.Equipment.Name}");
                }
                MessageBox.Show("Проверка успешно сохранена.");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения проверки: {ex.Message}");
            }
        }
    }

}