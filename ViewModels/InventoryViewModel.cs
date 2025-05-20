using System.Collections.ObjectModel;
using System.Windows.Input;
using inventory.Models;

namespace inventory.ViewModels
{
    public class InventoryViewModel
    {
        public ObservableCollection<Inventory> Inventories { get; set; }
        public ICommand AddCommand { get; }
        public ICommand PerformCheckCommand { get; }
        public ICommand GenerateReportCommand { get; }

        public InventoryViewModel()
        {
            Inventories = new ObservableCollection<Inventory>();
            LoadInventories();

            AddCommand = new RelayCommand(AddInventory);
            PerformCheckCommand = new RelayCommand(PerformCheck);
            GenerateReportCommand = new RelayCommand(GenerateReport);
        }

        private void LoadInventories()
        {
            // Загрузка инвентаризаций из базы данных
        }

        private void AddInventory()
        {
            // Логика добавления новой инвентаризации
        }

        private void PerformCheck()
        {
            // Логика проведения проверки
        }

        private void GenerateReport()
        {
            // Логика генерации отчета
        }
    }
}
