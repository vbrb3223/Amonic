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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public Admin()
        {
            InitializeComponent();
        }

        public List<data> tempLst = new List<data>();

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationClass.window.Title = "Login";
            NavigationClass.frame.Navigate(new Auth());
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
            {
                u.id_пользователя,
                u.Имя,
                u.Фамилия,
                u.Дата_рождения,
                u.Права_администратора,
                u.Email,
                g.Наименование_офиса,
                u.Активность
            }).Where(t => t.id_пользователя != NavigationClass.id).ToList();
            List<data> lst = new List<data>();
            foreach (var u in users)
            {
                if (!comboBox.Items.Contains(u.Наименование_офиса))
                    comboBox.Items.Add(u.Наименование_офиса);
                string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                string role = u.Права_администратора ? "administrator" : "office user";
                data d = new data()
                {
                    name = u.Имя,
                    lastName = u.Фамилия,
                    date = a,
                    adm = role,
                    email = u.Email,
                    nameOff = u.Наименование_офиса,
                    act = u.Активность
                };
                lst.Add(d);
            }
            tempLst = lst;
            dataGrid.ItemsSource = lst;
        }
        public class data
        {
            public string name { get; set; }
            public string lastName { get; set; }
            public string date { get; set; }
            public string adm { get; set; }
            public string email { get; set; }
            public string nameOff { get; set; }
            public bool act { get; set; }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                {
                    u.id_пользователя,
                    u.Имя,
                    u.Фамилия,
                    u.Дата_рождения,
                    u.Права_администратора,
                    u.Email,
                    g.Наименование_офиса,
                    u.Активность
                }).Where(t => t.id_пользователя != NavigationClass.id).ToList();
                List<data> lst = new List<data>();
                foreach (var u in users)
                {
                    string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                    string role = u.Права_администратора ? "administrator" : "office user";
                    data d = new data()
                    {
                        name = u.Имя,
                        lastName = u.Фамилия,
                        date = a,
                        adm = role,
                        email = u.Email,
                        nameOff = u.Наименование_офиса,
                        act = u.Активность
                    };
                    lst.Add(d);
                }
                tempLst = lst;
                dataGrid.ItemsSource = lst;
            }
            else
            {
                string offName = comboBox.SelectedItem.ToString();
                var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                {
                    u.id_пользователя,
                    u.Имя,
                    u.Фамилия,
                    u.Дата_рождения,
                    u.Права_администратора,
                    u.Email,
                    g.Наименование_офиса,
                    u.Активность
                }).Where(t => t.id_пользователя != NavigationClass.id && t.Наименование_офиса == offName).ToList();
                List<data> lst = new List<data>();
                foreach (var u in users)
                {
                    string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                    string role = u.Права_администратора ? "administrator" : "office user";
                    data d = new data()
                    {
                        name = u.Имя,
                        lastName = u.Фамилия,
                        date = a,
                        adm = role,
                        email = u.Email,
                        nameOff = u.Наименование_офиса,
                        act = u.Активность
                    };
                    lst.Add(d);
                }
                tempLst = lst;
                dataGrid.ItemsSource = lst;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
                string email = tempLst[dataGrid.SelectedIndex].email;
                var user = context.Информация_о_пользователе.Where(u => u.Email == email).FirstOrDefault();
                if (user.Активность)
                {
                    var quest = MessageBox.Show($"Вы точно хотите сделать пользователя {user.Имя + " " + user.Фамилия + " " + user.Email} не активным?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (quest == MessageBoxResult.Yes)
                    {
                        user.Активность = false;
                        context.SaveChanges();

                        if (comboBox.SelectedIndex == 0)
                        {
                            var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                            {
                                u.id_пользователя,
                                u.Имя,
                                u.Фамилия,
                                u.Дата_рождения,
                                u.Права_администратора,
                                u.Email,
                                g.Наименование_офиса,
                                u.Активность
                            }).Where(t => t.id_пользователя != NavigationClass.id).ToList();
                            List<data> lst = new List<data>();
                            foreach (var u in users)
                            {
                                string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                                string role = u.Права_администратора ? "administrator" : "office user";
                                data d = new data()
                                {
                                    name = u.Имя,
                                    lastName = u.Фамилия,
                                    date = a,
                                    adm = role,
                                    email = u.Email,
                                    nameOff = u.Наименование_офиса,
                                    act = u.Активность
                                };
                                lst.Add(d);
                            }
                            tempLst = lst;
                            dataGrid.ItemsSource = lst;
                        }
                        else
                        {
                            string offName = comboBox.SelectedItem.ToString();
                            var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                            {
                                u.id_пользователя,
                                u.Имя,
                                u.Фамилия,
                                u.Дата_рождения,
                                u.Права_администратора,
                                u.Email,
                                g.Наименование_офиса,
                                u.Активность
                            }).Where(t => t.id_пользователя != NavigationClass.id && t.Наименование_офиса == offName).ToList();
                            List<data> lst = new List<data>();
                            foreach (var u in users)
                            {
                                string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                                string role = u.Права_администратора ? "administrator" : "office user";
                                data d = new data()
                                {
                                    name = u.Имя,
                                    lastName = u.Фамилия,
                                    date = a,
                                    adm = role,
                                    email = u.Email,
                                    nameOff = u.Наименование_офиса,
                                    act = u.Активность
                                };
                                lst.Add(d);
                            }
                            tempLst = lst;
                            dataGrid.ItemsSource = lst;
                        }
                    }
                }
                else
                {
                    var quest = MessageBox.Show($"Вы точно хотите сделать пользователя {user.Имя + " " + user.Фамилия + " " + user.Email} активным?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (quest == MessageBoxResult.Yes)
                    {
                        user.Активность = true;
                        context.SaveChanges();
                    }
                    if (comboBox.SelectedIndex == 0)
                    {
                        var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                        {
                            u.id_пользователя,
                            u.Имя,
                            u.Фамилия,
                            u.Дата_рождения,
                            u.Права_администратора,
                            u.Email,
                            g.Наименование_офиса,
                            u.Активность
                        }).Where(t => t.id_пользователя != NavigationClass.id).ToList();
                        List<data> lst = new List<data>();
                        foreach (var u in users)
                        {
                            string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                            string role = u.Права_администратора ? "administrator" : "office user";
                            data d = new data()
                            {
                                name = u.Имя,
                                lastName = u.Фамилия,
                                date = a,
                                adm = role,
                                email = u.Email,
                                nameOff = u.Наименование_офиса,
                                act = u.Активность
                            };
                            lst.Add(d);
                        }
                        tempLst = lst;
                        dataGrid.ItemsSource = lst;
                    }
                    else
                    {
                        string offName = comboBox.SelectedItem.ToString();
                        var users = context.Информация_о_пользователе.Join(context.Офисы, u => u.id_офиса, g => g.id_офиса, (u, g) => new
                        {
                            u.id_пользователя,
                            u.Имя,
                            u.Фамилия,
                            u.Дата_рождения,
                            u.Права_администратора,
                            u.Email,
                            g.Наименование_офиса,
                            u.Активность
                        }).Where(t => t.id_пользователя != NavigationClass.id && t.Наименование_офиса == offName).ToList();
                        List<data> lst = new List<data>();
                        foreach (var u in users)
                        {
                            string a = (DateTime.Now.Year - u.Дата_рождения.Year).ToString();
                            string role = u.Права_администратора ? "administrator" : "office user";
                            data d = new data()
                            {
                                name = u.Имя,
                                lastName = u.Фамилия,
                                date = a,
                                adm = role,
                                email = u.Email,
                                nameOff = u.Наименование_офиса,
                                act = u.Активность
                            };
                            lst.Add(d);
                        }
                        tempLst = lst;
                        dataGrid.ItemsSource = lst;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Пользователь не выбран!");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            EditRole er = new EditRole();
            string email = tempLst[dataGrid.SelectedIndex].email;
            Информация_о_пользователе user = context.Информация_о_пользователе.Where(u => u.Email == email).FirstOrDefault();
            er.user = user;
            NavigationClass.frame.Navigate(er);
            NavigationClass.window.Title = "Edit Role";
        }

        private void textBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationClass.frame.Navigate(new AddUser());
            NavigationClass.window.Title = "Add user";
        }
    }
}
