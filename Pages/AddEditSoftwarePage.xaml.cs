using inventory.Context.MySql;
using inventory.Interfase;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace inventory.Pages
{
    public partial class AddEditSoftwarePage : Page, INotifyPropertyChanged
    {
        private Software _currentSoftware;
        private List<Developer> _developers;
        private List<Equipment> _equipmentList;

        public Software CurrentSoftware
        {
            get => _currentSoftware;
            set
            {
                _currentSoftware = value;
                OnPropertyChanged(nameof(CurrentSoftware));
            }
        }

        public List<Developer> Developer
        {
            get => _developers;
            set
            {
                _developers = value;
                OnPropertyChanged(nameof(Developer));
            }
        }

        public List<Equipment> Equipment
        {
            get => _equipmentList;
            set
            {
                _equipmentList = value;
                OnPropertyChanged(nameof(Equipment));
            }
        }


        public new string Title => CurrentSoftware.Id == 0 ? "Добавить программное обеспечение" : "Редактировать программное обеспечение";

        public AddEditSoftwarePage(Software software = null)
        {
            InitializeComponent();
            CurrentSoftware = software ?? new Software();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            try
            {
                Developer = new DeveloperContext().AllDevelopers();
                Equipment= new EquipmentContext().AllEquipment();

                //if (CurrentSoftware.Id != 0)
                //{
                //    if (CurrentSoftware.DeveloperId.HasValue)
                //    {
                //        CurrentSoftware.Developer = Developers.FirstOrDefault(d => d.Id == CurrentSoftware.DeveloperId);
                //        if (CurrentSoftware.Developer == null && CurrentSoftware.DeveloperId.HasValue)
                //        {
                //            MessageBox.Show($"Разработчик с ID {CurrentSoftware.DeveloperId} не найден в загруженных данных.");
                //            CurrentSoftware.DeveloperId = null;
                //        }
                //    }

                //    if (CurrentSoftware.EquipmentId.HasValue)
                //    {
                //        CurrentSoftware.Equipment = EquipmentList.FirstOrDefault(e => e.Id == CurrentSoftware.EquipmentId);
                //        if (CurrentSoftware.Equipment == null && CurrentSoftware.EquipmentId.HasValue)
                //        {
                //            MessageBox.Show($"Оборудование с ID {CurrentSoftware.EquipmentId} не найдено в загруженных данных.");
                //            CurrentSoftware.EquipmentId = null;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\nПодробности: {ex.InnerException?.Message}");
                Developer = new List<Developer>();
                Equipment= new List<Equipment>();
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentSoftware.Name))
            {
                MessageBox.Show("Заполните обязательное поле: Название");
                return;
            }

            try
            {
                new SoftwareContext().Save(CurrentSoftware, CurrentSoftware.Id != 0);
                MessageBox.Show("Программное обеспечение успешно сохранено.");
                NavigateBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new SoftwarePage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}