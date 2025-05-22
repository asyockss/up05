using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Models.inventory.Models;

namespace inventory.Pages
{
    public partial class AddEditConsumablePage : Page, INotifyPropertyChanged
    {
        private Consumable _currentConsumable;
        public Consumable CurrentConsumable
        {
            get => _currentConsumable;
            set { _currentConsumable = value; OnPropertyChanged(nameof(CurrentConsumable)); }
        }
        public new string Title => CurrentConsumable.Id == 0 ? "Добавить расходник" : "Редактировать расходник";
        public List<ConsumableType> ConsumableTypes { get; set; }
        public List<User> Users { get; set; }
        private BitmapImage _imagePreview;
        public BitmapImage ImagePreview
        {
            get => _imagePreview;
            set { _imagePreview = value; OnPropertyChanged(nameof(ImagePreview)); }
        }
        public bool IsMenuVisible => true;

        public AddEditConsumablePage(Consumable consumable = null)
        {
            InitializeComponent();
            CurrentConsumable = consumable ?? new Consumable { ReceiptDate = DateTime.Now };
            LoadData();
            DataContext = this;
            if (CurrentConsumable.Image != null)
                ImagePreview = ByteArrayToImage(CurrentConsumable.Image);
        }

        private void LoadData()
        {
            ConsumableTypes = new ConsumableTypeContext().AllConsumableTypes();
            Users = new UserContext().AllUsers();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*" };
            if (dialog.ShowDialog() == true)
            {
                CurrentConsumable.Image = System.IO.File.ReadAllBytes(dialog.FileName);
                ImagePreview = ByteArrayToImage(CurrentConsumable.Image);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentConsumable.Name) || CurrentConsumable.Quantity < 0)
            {
                MessageBox.Show("Заполните обязательные поля корректно");
                return;
            }
            new ConsumableContext().Save(CurrentConsumable, CurrentConsumable.Id != 0);
            NavigateBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigateBack();

        private void NavigateBack()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new ConsumablesPage());
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}