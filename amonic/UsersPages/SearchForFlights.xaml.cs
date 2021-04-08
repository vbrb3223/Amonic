using System;
using System.Collections;
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
    /// Логика взаимодействия для SearchForFlights.xaml
    /// </summary>
    public partial class SearchForFlights : Page
    {
        static СамолетыEnt context = new СамолетыEnt();
        List<Рейсы> flights = context.Рейсы.ToList();
        List<Аэропорты> airports = context.Аэропорты.ToList();
        List<data> tempFirst = new List<data>();
        List<data> tempSecond = new List<data>();
        public SearchForFlights()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var airport in airports)
            {
                if (!ComboBoxFrom.Items.Contains(airport.Код_Аэропорта))
                    ComboBoxFrom.Items.Add(airport.Код_Аэропорта);
                if (!ComboBoxTo.Items.Contains(airport.Код_Аэропорта))
                    ComboBoxTo.Items.Add(airport.Код_Аэропорта);
            }
            ComboBoxCabinType.ItemsSource = new List<string> { "Economy", "Business", "First Class" };

            OutboundTime.SelectedDate = DateTime.Parse("2017-10-04");

            dgFrist.ItemsSource = Ways("ADE", "AUH", DateTime.Parse("2017-10-04"), 1);
            tempFirst = Ways("ADE", "AUH", DateTime.Parse("2017-10-04"), 1);
            dgSecond.ItemsSource = WaysBack("AUH", "ADE", DateTime.Parse("2017-10-04"), 1, new DateTime());
            tempSecond = WaysBack("AUH", "ADE", DateTime.Parse("2017-10-04"), 1, new DateTime());
        }


        /// <summary>
        /// Поиск рейсов необходимых рейсов
        /// </summary>
        /// <param name="start">Стартовый аэропорт</param>
        /// <param name="finish">Конечный аэропорт</param>
        /// <param name="date">Дата вылета</param>
        /// <param name="price">Цена. 1 - Эконом, 2 - Бизнес класс, 3 - Первый класс</param>
        /// <returns></returns>
        private List<data> Ways(string start, string finish, DateTime date, int price)
        {
            List<data> data = new List<data>();

            var usualFlights = context.Рейсы.Where(u => u.Код_аэропорта == start && u.Код_аэропорта_посадки == finish && u.Дата_вылета == date).ToList();
            if (usualFlights == null)
            {
                MessageBox.Show("Не удалось найти рейсы!");
            }
            foreach (var flight in usualFlights)
            {
                var currentPrice = price == 1 ? flight.Цена : price == 2 ? flight.Бизнесс_цена : flight.Первый_класс_цена;
                var d = new data()
                {
                    from = start,
                    to = finish,
                    date = flight.Дата_вылета,
                    time = flight.Время_вылета,
                    flightNum = flight.Номер_рейса + "",
                    price = Convert.ToDecimal(currentPrice),
                    numOfStops = 0
                };
                data.Add(d);
            }
            return data;
        }


        /// <summary>
        /// Поиск рейсов необходимых рейсов в обратную сторону
        /// </summary>
        /// <param name="start">Стартовый аэропорт</param>
        /// <param name="finish">Конечный аэропорт</param>
        /// <param name="date">Дата вылета прошлого рейса</param>
        /// <param name="price">Цена. 1 - Эконом, 2 - Бизнес класс, 3 - Первый класс</param>
        /// <param name="dateBack">Дата возвращения</param>
        /// <returns></returns>
        private List<data> WaysBack(string start, string finish, DateTime date, int price, DateTime dateBack)
        {
            List<data> data = new List<data>();

            if(dateBack == new DateTime())
            {
                var usualFlights = context.Рейсы.Where(u => u.Код_аэропорта == start && u.Код_аэропорта_посадки == finish && u.Дата_вылета > date).ToList();
                if (usualFlights == null)
                {
                    MessageBox.Show("Не удалось найти рейсы в обратную сторону!");
                }
                foreach (var flight in usualFlights)
                {
                    var currentPrice = price == 1 ? flight.Цена : price == 2 ? flight.Бизнесс_цена : flight.Первый_класс_цена;
                    var d = new data()
                    {
                        from = start,
                        to = finish,
                        date = flight.Дата_вылета,
                        time = flight.Время_вылета,
                        flightNum = flight.Номер_рейса + "",
                        price = Convert.ToDecimal(currentPrice),
                        numOfStops = 0
                    };
                    data.Add(d);
                }
            }
            else
            {
                var usualFlights = context.Рейсы.Where(u => u.Код_аэропорта == start && u.Код_аэропорта_посадки == finish && u.Дата_вылета == dateBack).ToList();
                if(usualFlights == null)
                {
                    MessageBox.Show("Не удалось найти рейсы в обратную сторону!");
                }
                foreach (var flight in usualFlights)
                {
                    var currentPrice = price == 1 ? flight.Цена : price == 2 ? flight.Бизнесс_цена : flight.Первый_класс_цена;
                    var d = new data()
                    {
                        from = start,
                        to = finish,
                        date = flight.Дата_вылета,
                        time = flight.Время_вылета,
                        flightNum = flight.Номер_рейса + "",
                        price = Convert.ToDecimal(currentPrice),
                        numOfStops = 0
                    };
                    data.Add(d);
                }
            }
            return data;
        }




        /// <summary>
        /// Структура для добавления вычисляемых данных в dataGrid'ы
        /// </summary>
        class data
        {
            public string from { get; set; }
            public string to { get; set; }
            public DateTime date { get; set; }
            public TimeSpan time { get; set; }
            public string flightNum { get; set; }
            public decimal price { get; set; }
            public int numOfStops { get; set; }
        }


        /// <summary>
        /// Выборка рейсов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int price;
            if (ComboBoxCabinType.Text == "Economy")
                price = 1;
            else if (ComboBoxCabinType.Text == "Business")
                price = 2;
            else
                price = 3;
            if (OutboundTime.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату полета!");
                return;
            }

            if (ComboBoxFrom.Text != ComboBoxTo.Text)
            {
                dgFrist.ItemsSource = Ways(ComboBoxFrom.Text.Trim(), ComboBoxTo.Text.Trim(), Convert.ToDateTime(OutboundTime.SelectedDate), price);
                tempFirst = Ways(ComboBoxFrom.Text.Trim(), ComboBoxTo.Text.Trim(), Convert.ToDateTime(OutboundTime.SelectedDate), price);
                if (RBReturn.IsChecked == true)
                {
                    dgSecond.ItemsSource = WaysBack(ComboBoxTo.Text.Trim(), ComboBoxFrom.Text.Trim(),
                        Convert.ToDateTime(OutboundTime.SelectedDate), price, Convert.ToDateTime(ReturnTime.SelectedDate));
                    tempSecond = WaysBack(ComboBoxTo.Text.Trim(), ComboBoxFrom.Text.Trim(),
                        Convert.ToDateTime(OutboundTime.SelectedDate), price, Convert.ToDateTime(ReturnTime.SelectedDate));
                }
            }
            else
                MessageBox.Show("Выбран один и тот же аэропорт!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int countPass = 0;
            if (int.TryParse(TBNumberPassengers.Text, out countPass))
            {
                if (RBReturn.IsChecked == true)
                {
                    AddPassengers ap = new AddPassengers();
                    Рейсы outFlight = new Рейсы();
                    Рейсы returnFlight = new Рейсы();
                    if (dgFrist.SelectedIndex != -1 && dgSecond.SelectedIndex != -1)
                    {
                        outFlight.Код_аэропорта = tempFirst[dgFrist.SelectedIndex].from;
                        outFlight.Код_аэропорта_посадки = tempFirst[dgFrist.SelectedIndex].to;
                        outFlight.Дата_вылета = tempFirst[dgFrist.SelectedIndex].date;
                        outFlight.Номер_рейса = Convert.ToInt32(tempFirst[dgFrist.SelectedIndex].flightNum);
                        outFlight.Цена = tempFirst[dgFrist.SelectedIndex].price;


                        returnFlight.Код_аэропорта = tempSecond[dgSecond.SelectedIndex].from;
                        returnFlight.Код_аэропорта_посадки = tempSecond[dgSecond.SelectedIndex].to;
                        returnFlight.Дата_вылета = tempSecond[dgSecond.SelectedIndex].date;
                        returnFlight.Номер_рейса = Convert.ToInt32(tempSecond[dgSecond.SelectedIndex].flightNum);
                        returnFlight.Цена = tempSecond[dgSecond.SelectedIndex].price;

                        ap.GetValues(outFlight, returnFlight, countPass, ComboBoxCabinType.Text);
                        NavigationClass.frame.Navigate(ap);
                    }
                    else
                        MessageBox.Show("Выберите рейсы!");
                }
                else
                {
                    AddPassengers ap = new AddPassengers();
                    Рейсы outFlight = new Рейсы();
                    if (dgFrist.SelectedIndex != -1)
                    {
                        outFlight.Код_аэропорта = tempFirst[dgFrist.SelectedIndex].from;
                        outFlight.Код_аэропорта_посадки = tempFirst[dgFrist.SelectedIndex].to;
                        outFlight.Дата_вылета = tempFirst[dgFrist.SelectedIndex].date;
                        outFlight.Номер_рейса = Convert.ToInt32(tempFirst[dgFrist.SelectedIndex].flightNum);
                        outFlight.Цена = tempFirst[dgFrist.SelectedIndex].price;

                        ap.GetValues(outFlight, new Рейсы(), countPass, ComboBoxCabinType.Text);
                        NavigationClass.frame.Navigate(ap);
                    }
                    else
                        MessageBox.Show("Выберите рейс!");
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.Navigate(new Auth());
        }
    }
}
