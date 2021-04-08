using System;
using System.Collections.Generic;
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

namespace amonic
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public Auth()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TName.Text) && string.IsNullOrEmpty(TPass.Password))
                MessageBox.Show("Не введено имя или пароль!");
            else
            {
                var user = context.Информация_о_пользователе.Where(u => u.Email == TName.Text && 
                u.Password == TPass.Password).FirstOrDefault();
                if (user != null)
                {
                    if (user.Активность)
                    {
                        if (remember.IsChecked == true)
                            File.WriteAllText("authData", TName.Text + "\t" + TPass.Password);
                        if (user.Права_администратора)
                        {
                            Admin a = new Admin();
                            NavigationClass.id = user.id_пользователя;
                            NavigationClass.frame.Navigate(a);
                            NavigationClass.window.Title = "Admin Page";
                        }
                        else
                        {
                            Users u = new Users();
                            u.getValues(user);
                            NavigationClass.frame.Navigate(u);
                            NavigationClass.window.Title = "User Page";
                        }
                    }
                    else
                        MessageBox.Show("Данный пользователь не активен!");
                }
                else
                    MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText("authData");
            if (!string.IsNullOrEmpty(str))
            {
                TName.Text = str.Split('\t')[0];
                TPass.Password = str.Split('\t')[1];
            }
        }
    }
}
