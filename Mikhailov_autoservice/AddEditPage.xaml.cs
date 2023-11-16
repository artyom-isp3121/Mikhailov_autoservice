using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Mikhailov_autoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if(SelectedService != null)
            {
                _currentService = SelectedService; 
            }

            DataContext = _currentService;
        }

          
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
            {
                errors.AppendLine("Укажите название услуги");
            }
            var context = Mikhailov_avtoserviceEntities.GetContext();

            if(context.Service.Any(service => service.Title == _currentService.Title && service.ID != _currentService.ID))
            {
                errors.AppendLine("Уже существует такая услуга");
            }
            if (_currentService.Cost == 0 || _currentService.Cost < 0)
            {
                errors.AppendLine("Укажите стоимость услуги");
            }
            if (string.IsNullOrWhiteSpace(_currentService.Discount.ToString()))
            {
                errors.AppendLine("Укажите скидку");
            }
            if (_currentService.DurationlnSeconds == 0)
            {
                errors.AppendLine("Укажите длительность услуги");
            }
            if (_currentService.DurationlnSeconds >240)
            {
                errors.AppendLine("Длительность не может быть больше 240 минут");
            }
            if ( _currentService.DurationlnSeconds < 0)
            {
                errors.AppendLine("Длительность не может быть меньше 0 минут");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(_currentService.ID == 0) 
            {
                Mikhailov_avtoserviceEntities.GetContext().Service.Add(_currentService);
            }

            try
            {
                Mikhailov_avtoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
