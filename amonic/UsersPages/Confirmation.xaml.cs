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
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public Confirmation()
        {
            InitializeComponent();
        }

        List<Пассажиры> passengersToAdd = new List<Пассажиры>();
        decimal ticketsPrice = 0;
        Билеты ticketToAdd = new Билеты();

        public void GetValues(List<Пассажиры> passengers, Билеты ticket, decimal price)
        {
            ticketsPrice = price;
            passengersToAdd = passengers;
            ticketToAdd = ticket;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            context.Пассажиры.AddRange(passengersToAdd);
            context.Билеты.Add(ticketToAdd);
            context.SaveChanges();
            MessageBox.Show("Билеты куплены!");
            NavigationClass.frame.Navigate(new SearchForFlights());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Price.Content = $"[ $ {ticketsPrice * passengersToAdd.Count} ]";
        }
    }
}
