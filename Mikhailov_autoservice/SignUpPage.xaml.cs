using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if(SelectedService != null)
            {
                this._currentService = SelectedService;
            }

            DataContext = _currentService;
            TBStart.PreviewTextInput += TBStart_PreviewTextInput;

            var _currentClient = Mikhailov_avtoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if(ComboClient.SelectedItem == null)
            {
                errors.AppendLine("Укажите ФИО клиента");
            }
            if(StartDate.Text == "")
            {
                errors.AppendLine("Укажите дату услуги");
            }
            if (TBStart.Text == "")
            {
                errors.AppendLine("Укажите время начала услуги");
            }
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if(_currentClientService.ID == 0)
            {
                Mikhailov_avtoserviceEntities.GetContext().ClientService.Add(_currentClientService);
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

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            DateTime tmp;

            if (!DateTime.TryParseExact(s, "HH:mm", null, System.Globalization.DateTimeStyles.None, out tmp))
            {
                TBEnd.Text = "";
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                int sum = startHour + startMin + _currentService.DurationlnSeconds;
                int EndHour = sum / 60;
                int EndMin = sum % 60;


                EndHour = EndHour % 24;

                TBEnd.Text = EndHour.ToString("D2") + ":" + EndMin.ToString("D2");
            }
        }
        private void TBStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.:]+");
            return !regex.IsMatch(text);
        }

    }
}
