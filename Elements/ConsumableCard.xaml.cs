using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using inventory.Context.MySql;
using inventory.Context.Common;
using inventory.Models;
using inventory.Pages;
using MySql.Data.MySqlClient;

namespace inventory.Elements
{
    public partial class ConsumableCard : UserControl
    {
        public ConsumableCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Consumable consumable)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.NavigateToPage(new AddEditConsumablePage(consumable));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Consumable consumable &&
                MessageBox.Show("Вы уверены, что хотите удалить этот расходник?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    MySqlCommand charCommand = new MySqlCommand("DELETE FROM consumable_characteristic_values WHERE consumable_id = @Id", connection);
                    charCommand.Parameters.AddWithValue("@Id", consumable.Id);
                    charCommand.ExecuteNonQuery();

                    MySqlCommand historyCommand = new MySqlCommand("DELETE FROM consumable_responsible_history WHERE consumable_id = @Id", connection);
                    historyCommand.Parameters.AddWithValue("@Id", consumable.Id);
                    historyCommand.ExecuteNonQuery();

  
                    MySqlCommand equipmentCommand = new MySqlCommand("DELETE FROM equipment_consumables WHERE consumable_id = @Id", connection);
                    equipmentCommand.Parameters.AddWithValue("@Id", consumable.Id);
                    equipmentCommand.ExecuteNonQuery();

                    MySqlCommand command = new MySqlCommand("DELETE FROM consumables WHERE id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", consumable.Id);
                    command.ExecuteNonQuery();
                }
                RefreshParentPage();
            }
        }

        private void RefreshParentPage()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is ConsumablesPage))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is ConsumablesPage page) page.LoadData();
        }
    }
}