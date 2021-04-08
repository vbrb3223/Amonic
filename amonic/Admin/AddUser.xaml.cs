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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public AddUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.window.Title = "Admin Page";
            NavigationClass.frame.GoBack();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox.Text) && !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && textBox3.SelectedIndex != -1 && !string.IsNullOrEmpty(textBox4.Text)
                     && !string.IsNullOrEmpty(textBox5.Text))
                {

                    Информация_о_пользователе user = new Информация_о_пользователе();
                    string officeName = textBox3.SelectedItem.ToString();
                    var office = context.Офисы.Where(u => u.Наименование_офиса == officeName).FirstOrDefault();
                    user.Email = textBox.Text;
                    user.Имя = textBox1.Text;
                    user.Фамилия = textBox2.Text;
                    user.id_офиса = office.id_офиса;
                    user.Дата_рождения = new DateTime(Convert.ToInt32(textBox4.Text.Split('/')[2]), Convert.ToInt32(textBox4.Text.Split('/')[1]), Convert.ToInt32(textBox4.Text.Split('/')[0]));
                    user.Password = textBox5.Text;
                    user.Активность = true;
                    user.Права_администратора = false;
                    context.Информация_о_пользователе.Add(user);
                    context.SaveChanges();
                    NavigationClass.frame.Navigate(new Admin());
                    NavigationClass.window.Title = "Admin Page";
                }
                else
                    MessageBox.Show("Не все поля заполнены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var offices = context.Офисы.ToList();
            foreach (var o in offices)
            {
                if (!textBox3.Items.Contains(o.Наименование_офиса))
                    textBox3.Items.Add(o.Наименование_офиса);
            }
        }

        private void textBox4_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
                textBox4.Text = "[dd/mm/yyyy]";
        }

        private void textBox4_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBox4.Text == "[dd/mm/yyyy]")
                textBox4.Text = "";
        }
    }
}
