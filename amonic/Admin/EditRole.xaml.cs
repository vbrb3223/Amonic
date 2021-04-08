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
    /// Логика взаимодействия для EditRole.xaml
    /// </summary>
    public partial class EditRole : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public EditRole()
        {
            InitializeComponent();
        }

        public Информация_о_пользователе user = new Информация_о_пользователе();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var offices = context.Офисы.ToList();
            foreach (var o in offices)
            {
                if (!textBox3.Items.Contains(o.Наименование_офиса))
                    textBox3.Items.Add(o.Наименование_офиса);
            }
            textBox.Text = user.Email;
            textBox1.Text = user.Имя;
            textBox2.Text = user.Фамилия;
            var office = context.Офисы.Where(u => u.id_офиса == user.id_офиса).FirstOrDefault();
            textBox3.SelectedItem = office.Наименование_офиса;
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.window.Title = "Admin Page";
            NavigationClass.frame.GoBack();
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox.Text) && !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && textBox3.SelectedIndex != -1)
                {
                    var editUser = context.Информация_о_пользователе.Where(u => u.id_пользователя == user.id_пользователя).FirstOrDefault();
                    string officeName = textBox3.SelectedItem.ToString();
                    var office = context.Офисы.Where(u => u.Наименование_офиса == officeName).FirstOrDefault();
                    editUser.Email = textBox.Text;
                    editUser.Имя = textBox1.Text;
                    editUser.Фамилия = textBox2.Text;
                    editUser.id_офиса = office.id_офиса;
                    if (rb1.IsChecked == true)
                        editUser.Права_администратора = false;
                    else
                        editUser.Права_администратора = true;
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
    }
}
