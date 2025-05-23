using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using inventory.Context.MySql;
using inventory.Models;
using System.Linq;

namespace inventory.Pages
{
    public partial class AddEditEquipmentPage : Page, INotifyPropertyChanged
    {
        private Equipment _currentEquipment;
        public Equipment CurrentEquipment
        {
            get => _currentEquipment;
            set { _currentEquipment = value; OnPropertyChanged(nameof(CurrentEquipment)); }
        }
        public new string Title => CurrentEquipment.Id == 0 ? "Добавить оборудование" : "Редактировать оборудование";
        public List<EquipmentType> EquipmentTypes { get; set; }
        public List<EquipmentModel> EquipmentModels { get; set; }
        public List<Direction> Directions { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Room> Rooms { get; set; }
        public List<User> Users { get; set; }
        public List<Inventory> Inventory { get; set; }
        private BitmapImage _imagePreview;
        public BitmapImage ImagePreview
        {
            get => _imagePreview;
            set { _imagePreview = value; OnPropertyChanged(nameof(ImagePreview)); }
        }
        public bool IsMenuVisible => true;

        public AddEditEquipmentPage(Equipment equipment = null)
        {
            InitializeComponent();
            CurrentEquipment = equipment ?? new Equipment();
            LoadData();
            DataContext = this;
            if (CurrentEquipment.Photo != null)
                ImagePreview = ByteArrayToImage(CurrentEquipment.Photo);
        }

        private void LoadData()
        {
            EquipmentTypes = new EquipmentTypeContext().AllEquipmentTypes();
            EquipmentModels = new EquipmentModelContext().AllEquipmentModels();
            Directions = new DirectionContext().AllDirections();
            Statuses = new StatusContext().AllStatuses();
            Rooms = new RoomContext().AllRooms();
            Users = new UserContext().AllUsers();
            Inventory = new InventoryContext().AllInventories();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*" };
            if (dialog.ShowDialog() == true)
            {
                CurrentEquipment.Photo = System.IO.File.ReadAllBytes(dialog.FileName);
                ImagePreview = ByteArrayToImage(CurrentEquipment.Photo);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CurrentUser.IsAdmin && CurrentEquipment.ResponsibleId != CurrentUser.Id && CurrentEquipment.Id != 0)
                {
                    MessageBox.Show("У вас нет прав на редактирование этого оборудования.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(CurrentEquipment.Name))
                {
                    MessageBox.Show("Название оборудования обязательно.");
                    return;
                }
                if (CurrentEquipment.InventoryId == 0)
                {
                    MessageBox.Show("Пожалуйста, выберите инвентаризацию.");
                    return;
                }

                var inventoryContext = new InventoryContext();
                if (!inventoryContext.AllInventories().Any(i => i.Id == CurrentEquipment.InventoryId))
                {
                    MessageBox.Show("Выбранная инвентаризация не существует.");
                    return;
                }

                var equipmentContext = new EquipmentContext();
                equipmentContext.Save(CurrentEquipment, CurrentEquipment.Id != 0);
                NavigateBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();
        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new EquipmentPage());
        }

        private BitmapImage ByteArrayToImage(byte[] bytes)
        {
            using (var ms = new System.IO.MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}