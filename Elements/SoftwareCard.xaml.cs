using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace inventory.Elements
{
    public partial class SoftwareCard : UserControl
    {
        public SoftwareCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Software software)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditSoftwarePage(software));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Software software &&
                MessageBox.Show("Вы уверены, что хотите удалить это программное обеспечение?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    new SoftwareContext().Delete(software.Id);
                    RefreshParentPage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is SoftwarePage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is SoftwarePage page) page.LoadSoftware();
        }
    }
}
