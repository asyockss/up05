using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace inventory.Elements
{
    public partial class InventoryCard : UserControl
    {
        public InventoryCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Inventory inventory)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditInventoryPage(inventory));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Inventory inventory &&
                MessageBox.Show("Вы уверены, что хотите удалить эту инвентаризацию?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    new InventoryContext().Delete(inventory.Id);
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
            while (parent != null && !(parent is InventoryPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is InventoryPage page) page.LoadInventories();
        }
    }
}