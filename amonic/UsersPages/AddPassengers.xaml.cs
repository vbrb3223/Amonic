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
    /// Логика взаимодействия для AddPassengers.xaml
    /// </summary>
    public partial class AddPassengers : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public AddPassengers()
        {
            InitializeComponent();
        }

        Рейсы outFlight = new Рейсы();
        Рейсы returnFlight = new Рейсы();
        int countPassengers = 0;
        string cabin = "";
        List<data> passengers = new List<data>();

        public void GetValues(Рейсы outFl, Рейсы returnFl, int countPass, string cabinType)
        {
            outFlight = outFl;
            returnFlight = returnFl;
            countPassengers = countPass;
            cabin = cabinType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var country in context.Страны.ToList())
            {
                if (!PCCB.Items.Contains(country.Название))
                    PCCB.Items.Add(country.Название);
            }
            OutFrom.Content = outFlight.Код_аэропорта;
            OutTo.Content = outFlight.Код_аэропорта_посадки;
            OutCabin.Content = cabin;
            OutDate.Content = outFlight.Дата_вылета;
            OutFlight.Content = outFlight.Номер_рейса;

            if (returnFlight != new Рейсы())
            {
                ReturnFrom.Content = returnFlight.Код_аэропорта;
                ReturnTo.Content = returnFlight.Код_аэропорта_посадки;
                ReturnCabin.Content = cabin;
                ReturnDate.Content = returnFlight.Дата_вылета;
                ReturnFlight.Content = returnFlight.Номер_рейса;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FNTB.Text != "" && LNTB.Text != "" && BDDP.Text != ""
                && PNTB.Text != "" && PCCB.SelectedIndex != -1 && PTB.Text != "")
            {
                data passenger = new data
                {
                    firstname = FNTB.Text,
                    lastname = LNTB.Text,
                    date = DateTime.Parse(BDDP.Text),
                    passportnum = PNTB.Text,
                    country = PCCB.Text,
                    phone = PTB.Text
                };
                if (!passengers.Contains(passenger))
                {
                    passengers.Add(passenger);
                    dg.Items.Add(passenger);
                }
                else
                    MessageBox.Show("Такой пассажир уже существует!");
            }
            else
                MessageBox.Show("Не все поля заполнены!");
        }

        class data
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public DateTime date { get; set; }
            public string passportnum { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedIndex != -1)
            {
                passengers.Remove(passengers[dg.SelectedIndex]);
                dg.Items.Remove(dg.SelectedItem);
            }
            else
                MessageBox.Show("Не выбран пассажир!");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (dg.Items.Count == countPassengers)
            {
                decimal price = outFlight.Цена + (returnFlight != new Рейсы() ? returnFlight.Цена : 0);
                List<Пассажиры> passengersToAdd = new List<Пассажиры>();
                var countryNames = context.Страны.ToList();
                foreach (var pass in passengers)
                {
                    int countryNum = countryNames.Where(u=>u.Название == pass.country).FirstOrDefault().id_страны;
                    var passenger = new Пассажиры()
                    {
                        Firstname = pass.firstname,
                        Lastname = pass.lastname,
                        Birthdate = pass.date,
                        Passport_number = pass.passportnum,
                        id_Country = countryNum,
                        Phone = pass.phone
                    };
                    passengersToAdd.Add(passenger);
                }
                var numbersTickets = context.Билеты.Select(u => u.Код_билета).ToList();
                int number = 0;
                Random r = new Random();
                while(true)
                {
                    number = r.Next(0, 9) + r.Next(0, 9) * 10 + r.Next(0, 9) * 100 + r.Next(0, 9) * 1000;
                    if (!numbersTickets.Contains(number))
                        break;
                }
                var ticket = new Билеты()
                {
                    Код_билета = number,
                    Номер_рейса = outFlight.Номер_рейса,
                    Номер_обратного_рейса = returnFlight.Номер_рейса,
                    Дата_полета = outFlight.Дата_вылета,
                    Дата_обратного_полета = returnFlight.Дата_вылета
                };
                Confirmation c = new Confirmation();
                c.GetValues(passengersToAdd, ticket, price);
                NavigationClass.frame.Navigate(c);
            }
            else
                MessageBox.Show("Количество добавленных пассажиров не соответствует указанному!");
        }
    }
}
