using System.Linq;
using System;
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
            Loaded += EquipmentCard_Loaded;
        }

        private void EquipmentCard_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"EquipmentCard_Loaded Start: CurrentUser.Id={CurrentUser.Id}, CurrentUser.Role={CurrentUser.Role}, IsAdmin={CurrentUser.IsAdmin}");

            if (string.IsNullOrEmpty(CurrentUser.Role) || CurrentUser.Id == 0)
            {
                System.Diagnostics.Debug.WriteLine("EquipmentCard_Loaded: CurrentUser is not initialized. Attempting to re-initialize.");
                try
                {
                    var userContext = new UserContext();
                    string currentLogin = Application.Current.Properties["CurrentUserLogin"]?.ToString();
                    if (!string.IsNullOrEmpty(currentLogin))
                    {
                        var user = userContext.AllUsers().FirstOrDefault(u => u.Login == currentLogin);
                        if (user != null)
                        {
                            CurrentUser.Id = user.Id;
                            CurrentUser.Role = user.Role;
                            CurrentUser.Login = user.Login;
                            CurrentUser.FullName = user.FullName;
                            System.Diagnostics.Debug.WriteLine($"EquipmentCard_Loaded: Re-initialized CurrentUser - Id={user.Id}, Role={user.Role}, Login={user.Login}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"EquipmentCard_Loaded: No user found for login '{currentLogin}'. Hiding Edit button.");
                            EditButton.Visibility = Visibility.Collapsed;
                            return;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("EquipmentCard_Loaded: No CurrentUserLogin found. Hiding Edit button.");
                        EditButton.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"EquipmentCard_Loaded: Error re-initializing CurrentUser - {ex.Message}");
                    EditButton.Visibility = Visibility.Collapsed;
                    return;
                }
            }

            if (DataContext is Equipment equipment)
            {
                System.Diagnostics.Debug.WriteLine($"EquipmentCard Loaded: EquipmentId={equipment.Id}, ResponsibleId={equipment.ResponsibleId}, CurrentUser.Id={CurrentUser.Id}, CurrentUser.Role={CurrentUser.Role}, IsAdmin={CurrentUser.IsAdmin}");
                bool canEdit = CurrentUser.IsAdmin || (equipment.ResponsibleId.HasValue && equipment.ResponsibleId == CurrentUser.Id);
                EditButton.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
                System.Diagnostics.Debug.WriteLine($"EquipmentCard Loaded: EditButton Visibility={EditButton.Visibility}, canEdit={canEdit}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("EquipmentCard Loaded: DataContext is not Equipment");
                EditButton.Visibility = Visibility.Collapsed;
            }
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