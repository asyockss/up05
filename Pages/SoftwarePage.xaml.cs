using inventory.Context.MySql;
using inventory.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace inventory.Pages
{
    public partial class SoftwarePage : Page, INotifyPropertyChanged
    {
        private SoftwareContext softwareContext;
        private List<Software> _softwareList;
        private string _searchText;

        public List<Software> SoftwareList
        {
            get => _softwareList;
            set { _softwareList = value; OnPropertyChanged(nameof(SoftwareList)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterSoftware(); }
        }

        public bool IsMenuVisible => true;

        public SoftwarePage()
        {
            InitializeComponent();
            DataContext = this;
            softwareContext = new SoftwareContext();
            LoadSoftware();
        }

        public void LoadSoftware()
        {
            try
            {
                SoftwareList = softwareContext.AllSoftwares();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки программного обеспечения: {ex.Message}");
                SoftwareList = new List<Software>();
            }
        }

        private void FilterSoftware()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                LoadSoftware();
            }
            else
            {
                try
                {
                    SoftwareList = softwareContext.AllSoftwares()
                        .Where(s => s.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    s.Version.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка фильтрации программного обеспечения: {ex.Message}");
                    SoftwareList = new List<Software>();
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToPage(new AddEditSoftwarePage());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadSoftware();
            public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
