using amonic.UsersPages;
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

namespace amonic.User
{
    public partial class Flight : Page
    {
        static СамолетыEnt context = new СамолетыEnt();

        //Выгрузка данных
        public List<Аэропорты> airports = context.Аэропорты.ToList();
        public List<Рейсы> flights = context.Рейсы.OrderByDescending(u=> u.Дата_вылета).ToList();

        public Flight()
        {
            InitializeComponent();
        }


        //Рейсы, которые находяться в dataGrid
        public List<Рейсы> tempFlights;


        /// <summary>
        /// Фильтрация записей
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Сбор ошибок в введенных данных
            StringBuilder errors = new StringBuilder();
            if (FromComboBox.Text != "All airports" &&
                FromComboBox.Text == ToComboBox.Text)
            {
                errors.Append("Аэропорты вылета и посадки должны отличаться");
            }
            try
            {
                if (!string.IsNullOrEmpty(FlighNumberTB.Text))
                    Convert.ToInt32(FlighNumberTB.Text);
            }
            catch
            {
                errors.Append("Введенный номер рейса неверного формата");
            }

            //Фильтрация
            if (errors.Length > 0)
            {
                MessageBox.Show("Найденные ошибки:\n" + errors);
            }
            else
            {
                string from = FromComboBox.Text;
                string to = ToComboBox.Text;
                string date = OutboundTime.Text;
                List<Рейсы> filteredFlights = flights;
                switch(SortByComboBox.Text)
                {
                    case "Date-Time":
                        filteredFlights = filteredFlights.OrderByDescending(u => u.Дата_вылета).ThenByDescending(u => u.Время_вылета).ToList();
                        break;
                    case "Economy price":
                        filteredFlights = filteredFlights.OrderByDescending(u => u.Цена).ToList();
                        break;
                    case "Confirmed":
                        filteredFlights = filteredFlights.OrderByDescending(u => u.Подтверждение).ToList();
                        break;
                }
                if (!string.IsNullOrEmpty(from) && from != "All airports")
                {
                    filteredFlights = filteredFlights.Where(u => u.Код_аэропорта == from).ToList();
                }    
                if (!string.IsNullOrEmpty(to) && to != "All airports")
                {
                    filteredFlights = filteredFlights.Where(u => u.Код_аэропорта_посадки == to).ToList();
                }
                if (!string.IsNullOrEmpty(date))
                {
                    DateTime dt = Convert.ToDateTime(date);
                    filteredFlights = filteredFlights.Where(u => u.Дата_вылета == dt).ToList();
                }
                if (!string.IsNullOrEmpty(FlighNumberTB.Text))
                {
                    int flightNumber = Convert.ToInt32(FlighNumberTB.Text);
                    filteredFlights = filteredFlights.Where(u => u.Номер_рейса == flightNumber).ToList();
                }
                dataGridAppend(filteredFlights);
            }
        }

        
        /// <summary>
        /// Заполнение dataGrid и временного списка данными
        /// </summary>
        /// <param name="flights">Список, которым нужно заполнить dataGrid</param>
        private void dataGridAppend(List<Рейсы> flights)
        {
            //Заполнение dataGrid
            dg.ItemsSource = flights;

            //Добавление данных во временный список
            tempFlights = flights;
        }


        /// <summary>
        /// Выгрузка данных в dataGrid и поля фильтрации
        /// </summary>
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Заполнение combobox Аэропортами
            foreach (var airport in airports)
            {

                if (!FromComboBox.Items.Contains(airport.Код_Аэропорта))
                {
                    FromComboBox.Items.Add(airport.Код_Аэропорта);
                }
                if (!ToComboBox.Items.Contains(airport.Код_Аэропорта))
                {
                    ToComboBox.Items.Add(airport.Код_Аэропорта);
                }
            }

            //Параметры для sortBy
            SortByComboBox.ItemsSource = new List<string>() { "Date-Time", "Economy price", "Confirmed" };

            dataGridAppend(flights);
        }


        /// <summary>
        /// Обновление данных в dataGrid
        /// </summary>
        private void DataGridSet()
        {
            dg.ClearValue(ItemsControl.ItemsSourceProperty);
            dg.ItemsSource = tempFlights;
        }


        /// <summary>
        /// Отключение и включение рейсов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dg.SelectedIndex != -1)
                {
                    if (tempFlights[dg.SelectedIndex].Подтверждение == "CANCELED")
                    {
                        var msgQuest = MessageBox.Show("Вы уверены, что хотите сделать рейс активным?", 
                            "Подтверждение действия", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (msgQuest == MessageBoxResult.Yes)
                        {
                            var selectedFlight = tempFlights[dg.SelectedIndex];
                            var setFlight = context.Рейсы.Where(u=> u.Номер_рейса 
                            == selectedFlight.Номер_рейса && 
                            u.Дата_вылета == selectedFlight.Дата_вылета).FirstOrDefault();

                            setFlight.Подтверждение = "OK";
                            context.SaveChanges();
                            tempFlights[dg.SelectedIndex].Подтверждение = "OK";
                            DataGridSet();
                        }
                    }
                    else
                    {
                        var msgQuest = MessageBox.Show("Вы уверены, что хотите отменить рейс?",
                            "Подтверждение действия", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (msgQuest == MessageBoxResult.Yes)
                        {
                            var selectedFlight = tempFlights[dg.SelectedIndex];
                            var setFlight = context.Рейсы.Where(u => u.Номер_рейса
                            == selectedFlight.Номер_рейса &&
                            u.Дата_вылета == selectedFlight.Дата_вылета).FirstOrDefault();

                            setFlight.Подтверждение = "CANCELED";
                            context.SaveChanges();
                            tempFlights[dg.SelectedIndex].Подтверждение = "CANCELED";
                            DataGridSet();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Рейс не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Редактирование текста кнопки отмены/подтверждения рейса в зависимости от выбранного элемента dataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg.SelectedIndex != -1)
            {
                if (tempFlights[dg.SelectedIndex].Подтверждение == "OK")
                    CancelFlightsBTN.Content = "✖Cancel Flight";
                else
                    CancelFlightsBTN.Content = "✔Confirm Flight";
            }
        }


        /// <summary>
        /// Переход на страницу редактирования записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedIndex != -1)
            {
                EditFlightsPage efp = new EditFlightsPage();
                efp.flight = tempFlights[dg.SelectedIndex];
                NavigationClass.frame.Navigate(efp);
            }
            else
            {
                MessageBox.Show("Выберите запись, которую хотите редактировать");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.Navigate(new ImportPage());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.Navigate(new Auth());
        }
    }
}
