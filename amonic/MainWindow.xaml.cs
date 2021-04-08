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

namespace amonic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        СамолетыEnt context = new СамолетыEnt();
        public MainWindow()
        {
            InitializeComponent();
            NavigationClass.frame = frame;
            NavigationClass.window = this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (NavigationClass.window.Title == "User Page")
            {
                Users u = new Users();
                TimeSpan login = new TimeSpan(0, u.loginTime.Hour, u.loginTime.Minute, u.loginTime.Second);
                var now = DateTime.Now;
                TimeSpan logout = new TimeSpan(0, now.Hour, now.Minute, now.Second);

                Попытки_входа loginAdd = new Попытки_входа() { Дата_входа = u.loginTime, Время_входа = login, Время_выхода = logout, Время_в_системе = logout - login, id_пользователя = NavigationClass.id, Причина_выхода = "Некорректное закрытие программы" };
                context.Попытки_входа.Add(loginAdd);
                context.SaveChanges();
            }
        }
    }
}
