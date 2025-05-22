using inventory.Context.MySql;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace inventory
{
    /// <summary>
    /// Логика взаимодействия для InventoryCheckWindow.xaml
    /// </summary>
    public partial class InventoryCheckWindow : Window
    {
        public Inventory SelectedInventory { get; set; }
        public List<Equipment> EquipmentList { get; set; }
        private Equipment _selectedEquipment;
        private string _comment;

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }


        public InventoryCheckWindow(Inventory inventory)
        {
            InitializeComponent();
            SelectedInventory = inventory;
            EquipmentList = new EquipmentContext().AllEquipment();
            DataContext = this;
        }

        private void LoadEquipment()
        {
            // Загружаем оборудование, связанное с этой инвентаризацией
            EquipmentList = new EquipmentContext().AllEquipment()
                .Where(e => e.InventoryId == SelectedInventory.Id)
                .ToList();
        }

        private void SaveCheck_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите оборудование для проверки");
                return;
            }

            var check = new InventoryCheck
            {
                InventoryId = SelectedInventory.Id,
                EquipmentId = SelectedEquipment.Id,
                UserId = CurrentUser.Id,
                CheckDate = DateTime.Now,
                Comment = Comment
            };

            try
            {
                var context = new InventoryCheckContext();
                context.InventoryId = check.InventoryId;
                context.EquipmentId = check.EquipmentId;
                context.UserId = check.UserId;
                context.CheckDate = check.CheckDate;
                context.Comment = check.Comment;
                context.Save();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения проверки: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
