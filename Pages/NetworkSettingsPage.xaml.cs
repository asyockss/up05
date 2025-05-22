using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using inventory.Context.MySql;
using inventory.Models;

namespace inventory.Pages
{
    public partial class NetworkSettingsPage : Page, INotifyPropertyChanged
    {
        private NetworkContext networkContext = new NetworkContext();
        private EquipmentContext equipmentContext = new EquipmentContext();
        private List<Equipment> _equipments;
        private int _selectedEquipmentId;
        private string _ipAddress;
        private string _subnetMask;
        private string _gateway;
        private string _dnsServers;
        private string _checkIpAddress;
        private string _checkResult;

        public List<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value; OnPropertyChanged(nameof(Equipments)); }
        }
        public int SelectedEquipmentId
        {
            get => _selectedEquipmentId;
            set { _selectedEquipmentId = value; OnPropertyChanged(nameof(SelectedEquipmentId)); }
        }
        public string IpAddress
        {
            get => _ipAddress;
            set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        }
        public string SubnetMask
        {
            get => _subnetMask;
            set { _subnetMask = value; OnPropertyChanged(nameof(SubnetMask)); }
        }
        public string Gateway
        {
            get => _gateway;
            set { _gateway = value; OnPropertyChanged(nameof(Gateway)); }
        }
        public string DnsServers
        {
            get => _dnsServers;
            set { _dnsServers = value; OnPropertyChanged(nameof(DnsServers)); }
        }
        public string CheckIpAddress
        {
            get => _checkIpAddress;
            set { _checkIpAddress = value; OnPropertyChanged(nameof(CheckIpAddress)); }
        }
        public string CheckResult
        {
            get => _checkResult;
            set { _checkResult = value; OnPropertyChanged(nameof(CheckResult)); }
        }

        public NetworkSettingsPage()
        {
            InitializeComponent();
            DataContext = this;
            Equipments = equipmentContext.AllEquipment();
        }

        private void SaveNetworkSettings_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipmentId == 0 || string.IsNullOrEmpty(IpAddress) || string.IsNullOrEmpty(SubnetMask))
            {
                MessageBox.Show("Заполните обязательные поля: Оборудование, IP адрес, Маска подсети");
                return;
            }

            bool equipmentExists = Equipments.Any(eq => eq.Id == SelectedEquipmentId);
            if (!equipmentExists)
            {
                MessageBox.Show("Выбранное оборудование не существует в базе данных.");
                return;
            }

            Network network = new Network
            {
                EquipmentId = SelectedEquipmentId,
                IpAddress = IpAddress,
                SubnetMask = SubnetMask,
                Gateway = Gateway,
                DnsServers = DnsServers
            };

            bool isUpdate = network.Id != 0;
            networkContext.Save(isUpdate);
            MessageBox.Show("Сетевые настройки сохранены");
        }

        private void CheckIpAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CheckIpAddress))
            {
                CheckResult = "Введите IP адрес для проверки";
                return;
            }
            try
            {
                bool isAvailable = IsIpAvailable(CheckIpAddress);
                CheckResult = isAvailable ? "IP доступен" : "IP не доступен";
            }
            catch (Exception ex)
            {
                CheckResult = $"Ошибка: {ex.Message}";
            }
        }

        private bool IsIpAvailable(string ipAddress)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("arp", "-a " + ipAddress)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    return output.Contains(ipAddress);
                }
            }
            catch
            {
                return false;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}