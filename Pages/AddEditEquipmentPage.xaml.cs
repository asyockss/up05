using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using inventory.Models;
using inventory;

namespace inventory.ViewModels
{
    public class AddEditEquipmentViewModel //INotifyPropertyChanged
    {
        //private readonly DataService _dataService;
        //private Equipment _currentEquipment;
        //private BitmapImage _imagePreview;

        //public Equipment CurrentEquipment
        //{
        //    get => _currentEquipment;
        //    set
        //    {
        //        _currentEquipment = value;
        //        OnPropertyChanged(nameof(CurrentEquipment));
        //        LoadImagePreview();
        //    }
        //}

        //public string Title => CurrentEquipment.Id == 0 ? "Добавить оборудование" : "Редактировать оборудование";

        //public BitmapImage ImagePreview
        //{
        //    get => _imagePreview;
        //    set
        //    {
        //        _imagePreview = value;
        //        OnPropertyChanged(nameof(ImagePreview));
        //    }
        //}

        //public List<EquipmentType> EquipmentTypes { get; set; }
        //public List<EquipmentModel> EquipmentModels { get; set; }
        //public List<Status> Statuses { get; set; }
        //public List<Room> Rooms { get; set; }
        //public List<User> Users { get; set; }
        //public List<Direction> Directions { get; set; }
        //public List<Consumable> AvailableConsumables { get; set; }
        //public List<Software> AvailableSoftware { get; set; }

        //public ICommand SaveCommand { get; }
        //public ICommand CancelCommand { get; }
        //public ICommand SelectImageCommand { get; }
        //public ICommand AddSoftwareCommand { get; }
        //public ICommand RemoveSoftwareCommand { get; }
        //public ICommand AddNetworkSettingsCommand { get; }
        //public ICommand RemoveNetworkSettingsCommand { get; }
        //public ICommand AddConsumableCommand { get; }
        //public ICommand RemoveConsumableCommand { get; }

        //public AddEditEquipmentViewModel(Equipment equipment)
        //{
        //    _dataService = new DataService();
        //    CurrentEquipment = equipment ?? new Equipment();

        //    SaveCommand = new RelayCommand(Save);
        //    CancelCommand = new RelayCommand(Cancel);
        //    SelectImageCommand = new RelayCommand(SelectImage);
        //    AddSoftwareCommand = new RelayCommand(AddSoftware);
        //    RemoveSoftwareCommand = new RelayCommand(RemoveSoftware);
        //    AddNetworkSettingsCommand = new RelayCommand(AddNetworkSettings);
        //    RemoveNetworkSettingsCommand = new RelayCommand(RemoveNetworkSettings);
        //    AddConsumableCommand = new RelayCommand(AddConsumable);
        //    RemoveConsumableCommand = new RelayCommand(RemoveConsumable);

        //    LoadDataAsync();
        //}

        //private async void LoadDataAsync()
        //{
        //    try
        //    {
        //        EquipmentTypes = await _dataService.GetAllEquipmentTypesAsync();
        //        EquipmentModels = await _dataService.GetAllEquipmentModelsAsync();
        //        Statuses = await _dataService.GetAllStatusesAsync();
        //        Rooms = await _dataService.GetAllRoomsAsync();
        //        Users = await _dataService.GetAllUsersAsync();
        //        Directions = await _dataService.GetAllDirectionsAsync();
        //        AvailableConsumables = await _dataService.GetAllConsumablesAsync();
        //        AvailableSoftware = await _dataService.GetAllSoftwareAsync();

        //        OnPropertyChanged(nameof(EquipmentTypes));
        //        OnPropertyChanged(nameof(EquipmentModels));
        //        OnPropertyChanged(nameof(Statuses));
        //        OnPropertyChanged(nameof(Rooms));
        //        OnPropertyChanged(nameof(Users));
        //        OnPropertyChanged(nameof(Directions));
        //        OnPropertyChanged(nameof(AvailableConsumables));
        //        OnPropertyChanged(nameof(AvailableSoftware));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
        //    }
        //}

        //private void LoadImagePreview()
        //{
        //    if (CurrentEquipment?.Photo != null && CurrentEquipment.Photo.Length > 0)
        //    {
        //        try
        //        {
        //            using (var ms = new MemoryStream(CurrentEquipment.Photo))
        //            {
        //                var image = new BitmapImage();
        //                image.BeginInit();
        //                image.CacheOption = BitmapCacheOption.OnLoad;
        //                image.StreamSource = ms;
        //                image.EndInit();
        //                ImagePreview = image;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
        //        }
        //    }
        //}

        //private void SelectImage()
        //{
        //    var openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
        //        Title = "Выберите изображение оборудования"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
        //            CurrentEquipment.Photo = imageBytes;
        //            LoadImagePreview();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
        //        }
        //    }
        //}

        //private void AddSoftware()
        //{
        //    // Реализация выбора и добавления ПО
        //}

        //private void RemoveSoftware()
        //{
        //    // Реализация удаления выбранного ПО
        //}

        //private void AddNetworkSettings()
        //{
        //    // Реализация добавления сетевых настроек
        //}

        //private void RemoveNetworkSettings()
        //{
        //    // Реализация удаления сетевых настроек
        //}

        //private void AddConsumable()
        //{
        //    // Реализация выбора и добавления расходника
        //}

        //private void RemoveConsumable()
        //{
        //    // Реализация удаления расходника
        //}

        //private async void Save()
        //{
        //    try
        //    {
        //        // Валидация
        //        if (string.IsNullOrWhiteSpace(CurrentEquipment.Name))
        //        {
        //            MessageBox.Show("Название оборудования обязательно");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(CurrentEquipment.InventoryNumber))
        //        {
        //            MessageBox.Show("Инвентарный номер обязателен");
        //            return;
        //        }

        //        if (!CurrentEquipment.InventoryNumber.All(char.IsDigit))
        //        {
        //            MessageBox.Show("Инвентарный номер должен содержать только цифры");
        //            return;
        //        }

        //        if (CurrentEquipment.Id == 0)
        //        {
        //            await _dataService.AddEquipmentAsync(CurrentEquipment);
        //            MessageBox.Show("Оборудование успешно добавлено");
        //        }
        //        else
        //        {
        //            await _dataService.UpdateEquipmentAsync(CurrentEquipment);
        //            MessageBox.Show("Оборудование успешно обновлено");
        //        }

        //        // Закрыть страницу
        //        if (Application.Current.MainWindow is MainWindow mainWindow)
        //        {
        //            mainWindow.NavigateBack();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка сохранения: {ex.Message}");
        //    }
        //}

        //private void Cancel()
        //{
        //    // Закрыть страницу
        //    if (Application.Current.MainWindow is MainWindow mainWindow)
        //    {
        //        mainWindow.NavigateBack();
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}