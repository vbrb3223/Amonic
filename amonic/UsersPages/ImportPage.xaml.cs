using Microsoft.Win32;
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

namespace amonic.UsersPages
{
    /// <summary>
    /// Логика взаимодействия для ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        СамолетыEnt context = new СамолетыEnt();
        public ImportPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Переходит на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationClass.frame.GoBack();
        }


        /// <summary>
        /// Открытие диалогового окна для выбора файла .csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath.Text = openFileDialog.FileName;
            }
        }


        /// <summary>
        /// Загрузка данных в БД из файла .csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int countSuccess = 0;
            int countSkipped = 0;
            int countDuplicates = 0;
            int countMissFields = 0;
            List<string> addedLines = new List<string>();
            var records = File.ReadAllLines(FilePath.Text);
            foreach (var record in records)
            {

                if (record.Split(',').Length == 9)
                {
                    if (addedLines.Contains(record))
                    {
                        countDuplicates++;
                    }
                    else
                    {
                        addedLines.Add(record);
                        if (record.Split(',')[0] == "ADD")
                        {
                            try
                            {
                                var values = record.Split(',');
                                decimal ecoClass = Convert.ToDecimal(values[7].Replace('.', ','));
                                decimal businessClass = Math.Round(ecoClass + ecoClass * 35 / 100, 2);
                                decimal firstClass = Math.Round(businessClass + businessClass * 30 / 100, 2);

                                var flight = new Рейсы()
                                {
                                    Дата_вылета = DateTime.Parse(values[1]),
                                    Время_вылета = TimeSpan.Parse(values[2]),
                                    Номер_рейса = Convert.ToInt32(values[3]),
                                    Код_аэропорта = values[4],
                                    Код_аэропорта_посадки = values[5],
                                    id_Самолета = Convert.ToInt32(values[6]),
                                    Цена = ecoClass,
                                    Бизнесс_цена = businessClass,
                                    Первый_класс_цена = firstClass,
                                    Подтверждение = values[8]
                                };
                                context.Рейсы.Add(flight);
                                context.SaveChanges();
                                countSuccess++;
                            }
                            catch
                            {
                                countSkipped++;
                            }
                        }
                        else if (record.Split(',')[0] == "EDIT")
                        {
                            try
                            {
                                var values = record.Split(',');
                                decimal ecoClass = Convert.ToDecimal(values[7].Replace('.', ','));
                                decimal businessClass = Math.Round(ecoClass + ecoClass * 35 / 100, 2);
                                decimal firstClass = Math.Round(businessClass + businessClass * 30 / 100, 2);

                                int id_flight = Convert.ToInt32(values[3]);
                                DateTime date = DateTime.Parse(values[1]);
                                var flight = context.Рейсы.Where(u => u.Номер_рейса == id_flight &&
                                u.Дата_вылета == date).FirstOrDefault();
                                if (flight != null)
                                {
                                    flight.Время_вылета = TimeSpan.Parse(values[2]);
                                    flight.Номер_рейса = Convert.ToInt32(values[3]);
                                    flight.Код_аэропорта = values[4];
                                    flight.Код_аэропорта_посадки = values[5];
                                    flight.id_Самолета = Convert.ToInt32(values[6]);
                                    flight.Цена = ecoClass;
                                    flight.Бизнесс_цена = businessClass;
                                    flight.Первый_класс_цена = firstClass;
                                    flight.Подтверждение = values[8];
                                    context.SaveChanges();
                                    countSuccess++;
                                }
                                else
                                    countSkipped++;
                            }
                            catch
                            {
                                countSkipped++;
                            }
                        }
                        else
                            countSkipped++;
                    }
                }
                else
                    countMissFields++;
            }

            SuccessLabel.Content = $"[{countSuccess}]";
            DuplicateLabel.Content = $"[{countDuplicates}]";
            MissingLabel.Content = $"[{countMissFields}]";
            SkippedRecords.Content = $"[{countSkipped}]";
        }
    }
}
