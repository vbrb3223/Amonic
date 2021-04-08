using amonic.User;
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

namespace amonic.UsersPages
{
    /// <summary>
    /// Логика взаимодействия для EditFlightsPage.xaml
    /// </summary>
    public partial class EditFlightsPage : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public Рейсы flight = new Рейсы();
        public EditFlightsPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Загрузка данных на страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var aircraft = context.Самолеты.Where(u=>u.id_Самолета == flight.id_Самолета).FirstOrDefault();
            labelFromNumber.Content = flight.Код_аэропорта;
            labelToNumber.Content = flight.Код_аэропорта_посадки;
            labelAircraftNumber.Content = aircraft.Тип_самолета;
            Date.SelectedDate = flight.Дата_вылета;
            TBTime.Text = flight.Время_вылета.Hours + ":" + flight.Время_вылета.Minutes;
            TBPrice.Text = Convert.ToInt32(flight.Цена).ToString();
        }


        /// <summary>
        /// Переход назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.GoBack();
        }


        /// <summary>
        /// Обновление данных и переход на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var flightFromDB = context.Рейсы.Where(u=>u.Номер_рейса == flight.Номер_рейса
                && u.Дата_вылета == flight.Дата_вылета).FirstOrDefault();
                //flightFromDB.Дата_вылета = Date.DisplayDate;
                flightFromDB.Время_вылета = TimeSpan.Parse(TBTime.Text);
                flightFromDB.Цена = Convert.ToDecimal(TBPrice.Text);

                context.SaveChanges();
                Flight f = new Flight();
                f.flights = context.Рейсы.ToList();
                NavigationClass.frame.Navigate(f);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
