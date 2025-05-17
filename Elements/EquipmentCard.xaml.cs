using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// <summary>
    /// Логика взаимодействия для EquipmentCard.xaml
    /// </summary>
    public partial class EquipmentCard : UserControl
    {
        public EquipmentCard()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Equipment equipment)
            {
                // Логика для определения цвета статуса (без рекурсивных шаблонов)
                Color statusColor = Colors.Gray;
                if (equipment.Status != null)
                {
                    switch (equipment.Status.Name)
                    {
                        case "Активен":
                            statusColor = Colors.Green;
                            break;
                        case "На ремонте":
                            statusColor = Colors.Orange;
                            break;
                        case "Списано":
                            statusColor = Colors.Red;
                            break;
                    }
                }

                // Добавляем свойство StatusColor в DataContext
                var dynamicContext = new DynamicViewModel(equipment);
                dynamicContext.AddProperty("StatusColor", new SolidColorBrush(statusColor));

                // Создаем превью изображения
                if (equipment.Photo != null && equipment.Photo.Length > 0)
                {
                    using (var ms = new MemoryStream(equipment.Photo))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        dynamicContext.AddProperty("ImagePreview", image);
                    }
                }

                DataContext = dynamicContext;
            }
        }
    }

    // Вспомогательный класс для динамических свойств
    public class DynamicViewModel : System.Dynamic.DynamicObject, INotifyPropertyChanged
    {
        private readonly object _original;
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public DynamicViewModel(object original)
        {
            _original = original;
        }

        public void AddProperty(string name, object value)
        {
            _properties[name] = value;
            OnPropertyChanged(name);
        }

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }

            var prop = _original.GetType().GetProperty(binder.Name);
            if (prop != null)
            {
                result = prop.GetValue(_original);
                return true;
            }

            result = null;
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}