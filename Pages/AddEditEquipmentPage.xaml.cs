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
using inventory.Context;

namespace inventory.Pages
{
    public partial class AddEditEquipmentPage : Page, INotifyPropertyChanged
    {
        public Equipment CurrentEquipment { get; set; }
        public string Title => CurrentEquipment.Id == 0 ? "Добавить оборудование" : "Редактировать оборудование";
        public List<EquipmentType> EquipmentTypes { get; set; }
        public List<EquipmentModel> EquipmentModels { get; set; }
        public List<Direction> Directions { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Room> Rooms { get; set; }
        public List<User> Users { get; set; }
        public BitmapImage ImagePreview { get; set; }
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
            EquipmentTypes = EquipmentTypeContext.AllEquipmentTypes().Cast<EquipmentType>().ToList();
            EquipmentModels = EquipmentModelContext.AllEquipmentModels().Cast<EquipmentModel>().ToList();
            Directions = DirectionContext.AllDirections().Cast<Direction>().ToList();
            Statuses = StatusContext.AllStatuses().Cast<Status>().ToList();
            Rooms = RoomContext.AllRooms().Cast<Room>().ToList();
            Users = UserContext.AllUsers().Cast<User>().ToList();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*" };
            if (dialog.ShowDialog() == true)
            {
                CurrentEquipment.Photo = System.IO.File.ReadAllBytes(dialog.FileName);
                ImagePreview = ByteArrayToImage(CurrentEquipment.Photo);
                OnPropertyChanged(nameof(ImagePreview));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentEquipment.Name) || string.IsNullOrEmpty(CurrentEquipment.InventoryNumber))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }
            EquipmentContext.Save(CurrentEquipment, CurrentEquipment.Id != 0);
            NavigateBack();
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
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}