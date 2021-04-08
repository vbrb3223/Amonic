using amonic.User;
using amonic.UsersPages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace amonic
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public Информация_о_пользователе user = new Информация_о_пользователе();
        List<Попытки_входа> logins = new List<Попытки_входа>();
        public DateTime timer = new DateTime(2020, 11, 7, 0, 0, 0);
        public DateTime loginTime = DateTime.Now;
        public Users()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            timer += TimeSpan.FromSeconds(1);
            TBtimer.Text = "Time spent on system: " + timer.ToString("HH:mm:ss");
        }

        public void getValues(Информация_о_пользователе u)
        {
            user = u;
            NavigationClass.id = user.id_пользователя;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TBhi.Text = $"Hi {user.Имя}, Welcome to AMONIC Airlines";
            logins = context.Попытки_входа.Where(u => u.id_пользователя == user.id_пользователя).ToList();
            TBcrashes.Text = $"Number of crashes: {logins.Count(u => u.Причина_выхода != null)}";
            TBtimer.Text = "Time spent on system: 00:00:00";
            dataGrid.ItemsSource = logins;
        }

        private void Exit_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TimeSpan login = new TimeSpan(0, loginTime.Hour, loginTime.Minute, loginTime.Second);
            var now = DateTime.Now;
            TimeSpan logout = new TimeSpan(0, now.Hour, now.Minute, now.Second);

            Попытки_входа loginAdd = new Попытки_входа() { Дата_входа = loginTime, Время_входа = login, Время_выхода = logout, Время_в_системе = logout - login, id_пользователя = user.id_пользователя, Причина_выхода = null };
            context.Попытки_входа.Add(loginAdd);
            context.SaveChanges();
            NavigationClass.window.Title = "Login";
            NavigationClass.frame.GoBack();
        }

        private void Exit_Copy_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationClass.frame.Navigate(new Flight());
        }

        private void Exit_Copy1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationClass.frame.Navigate(new SearchForFlights());
        }
    }
}
