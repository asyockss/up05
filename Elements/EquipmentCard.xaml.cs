using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using inventory.Context.MySql;
using inventory.Models;
using inventory.Pages;

namespace inventory.Elements
{
    public partial class EquipmentCard : UserControl
    {
        public EquipmentCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Equipment equipment)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditEquipmentPage(equipment));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Equipment equipment &&
                MessageBox.Show("Вы уверены, что хотите удалить это оборудование?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var context = new EquipmentContext();
                if (context.Delete(equipment.Id))
                {
                    RefreshParentPage();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить оборудование, так как оно связано с другими данными.");
                }
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is EquipmentPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is EquipmentPage page) page.LoadEquipment();
        }
    }
}