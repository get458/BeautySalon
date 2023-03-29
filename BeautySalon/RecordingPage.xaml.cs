using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BeautySalon
{
    /// <summary>
    /// Логика взаимодействия для RecordingPage.xaml
    /// </summary>
    public partial class RecordingPage : Page
    {
      public  ObservableCollection<TempDataOfRecording> ProceduresInRecording = new ObservableCollection<TempDataOfRecording>();
        public RecordingPage()
        {
            InitializeComponent();
            CmbClient.ItemsSource = DB.db.Client.OrderByDescending(x => x.Person.LastName).ToList();
            CmbProcedure.ItemsSource = DB.db.Procedure.OrderBy(x => x.ProceduresName).ToList();
            ProcedureOfMasterList.ItemsSource = ProceduresInRecording;
        }


        private void CmbProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var CurrentProcedure = CmbProcedure.SelectedItem as Procedure;
            CmbMaster.ItemsSource = DB.db.ProcedureOfMaster.Where(x => x.IDProcedure == CurrentProcedure.ProcedureID).OrderByDescending(x => x.Master.Employee.Person.LastName).ToList();
        }

        public class TempDataOfRecording 
        { 
            public ProcedureOfMaster ProcedureMaster { get; set; }
            public TimeSpan Time { get; set; }
        }


        private void BtnDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            ProceduresInRecording.Remove((sender as Button).DataContext as TempDataOfRecording);
            if (ProceduresInRecording.Count == 0)
            {
                CmbClient.IsEnabled = true;
                DaPDateRecording.IsEnabled = true;
            }
            int discount = (CmbClient.SelectedItem as Client).Discount.Procent;
            TxtTotalCost.Text = TotalCost(discount).ToString("F2") + "Скидка: " + discount;
        }


        public decimal TotalCost(int ProcentDiscount)
        {
            decimal TotalCost = 0;
            foreach (var item in ProceduresInRecording)
            {
                TotalCost += item.ProcedureMaster.Procedure.Cost;
            }
            TotalCost = TotalCost - (TotalCost * ProcentDiscount) / 100;
            return TotalCost;
        }

        private void BtnAddRecording_Click(object sender, RoutedEventArgs e)
        {
            if (CmbClient.SelectedItem == null ||
                DaPDateRecording.SelectedDate == null ||
                CmbProcedure.SelectedItem == null ||
                 CmbMaster.SelectedItem == null)
            {
                MessageBox.Show("Не все значения выбраны", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TimeSpan Time = new TimeSpan();
            if (TimeSpan.TryParse(TxtTime.Text, out Time) == false)
            {
                MessageBox.Show("Не корректное время", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TempDataOfRecording tempDataOfRecording = new TempDataOfRecording()
            {
                ProcedureMaster = CmbMaster.SelectedItem as ProcedureOfMaster,
                Time = Time
            };
            ProceduresInRecording.Add(tempDataOfRecording);
            int discount = (CmbClient.SelectedItem as Client).Discount.Procent;
            TxtTotalCost.Text = TotalCost(discount).ToString("F2") + " (₽ Скидка: " + discount + " %)";
            CmbClient.IsEnabled = false;
            DaPDateRecording.IsEnabled = false;
        }

        private void BtnCreateRecording_Click(object sender, RoutedEventArgs e)
        {
            if (ProceduresInRecording.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одной процедуры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Recording recording = new Recording();
            recording.Client = CmbClient.SelectedItem as Client;
            recording.Date = DaPDateRecording.SelectedDate.Value;
            DB.db.Recording.Add(recording);
            DB.db.SaveChanges();
            int recordingID = DB.db.Recording.Select(x => x.RecordingID).Max();
            foreach (var item in ProceduresInRecording)
            {
                ProcedureInRecording procedureInRecording = new ProcedureInRecording();
                procedureInRecording.ProcedureOfMaster = item.ProcedureMaster;
                procedureInRecording.Time = item.Time;
                procedureInRecording.IsVisited = false;
                procedureInRecording.IsCanceled = false;
                procedureInRecording.IDRecording = recordingID;
                DB.db.ProcedureInRecording.Add(procedureInRecording);
                DB.db.SaveChanges();
            }
            MessageBox.Show("Готово", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            CmbClient.IsEnabled = true;
            DaPDateRecording.IsEnabled = true;
            ProceduresInRecording.Clear();
            TxtTotalCost.Text = string.Empty;
        }
    }
}
