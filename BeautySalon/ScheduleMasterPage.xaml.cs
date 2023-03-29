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

namespace BeautySalon
{
    /// <summary>
    /// Логика взаимодействия для ScheduleMasterPage.xaml
    /// </summary>
    public partial class ScheduleMasterPage : Page
    {
        public ScheduleMasterPage()
        {
            InitializeComponent();
            MasterID = 0;
            DaPDate.SelectedDate = DateTime.Today;
            CmbMaster.ItemsSource = DB.db.Master.Where(x => x.Employee.IsWorking == true && x.Employee.IDRole == 2).OrderBy(x => x.Employee.Person.LastName).ToList();
        }

        public int MasterID { get; set; }
        public ScheduleMasterPage(int IDMaster)
        {
            InitializeComponent();
            DaPDate.SelectedDate = DateTime.Today;
            MasterPanel.Visibility = Visibility.Collapsed;
            MasterID = IDMaster;
        }

        public void ShowShedule()
        {
            try
            {
                if (MasterID == 0)
                {
                    var Master = CmbMaster.SelectedItem as Master;
                    SheduleList.ItemsSource = DB.db.ProcedureInRecording
                        .Where(x => x.Recording.Date == DaPDate.SelectedDate.Value 
                        && x.ProcedureOfMaster.IDMaster == Master.MasterID && x.IsCanceled == false)
                        .OrderBy(x => x.Time).ToList();
                }
                else
                {
                    SheduleList.ItemsSource = DB.db.ProcedureInRecording
                        .Where(x => x.Recording.Date == DaPDate.SelectedDate.Value 
                        && x.ProcedureOfMaster.IDMaster == MasterID && x.IsCanceled == false)
                        .OrderBy(x => x.Time).ToList();
                }
            }
            catch
            {

            }

        }

        private void BtnCanceled_Click(object sender, RoutedEventArgs e)
        {
            ProcedureInRecording CurrentProcedure = SheduleList.SelectedItem as ProcedureInRecording;
            CanceledProcedure(CurrentProcedure);
        }


        public bool CanceledProcedure(ProcedureInRecording CurrentProcedure)
        {
            if (CurrentProcedure != null)
            {
                var result = MessageBox.Show("Отменить процедуру?", "Отмена", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var FindProcedure = DB.db.ProcedureInRecording.Find(CurrentProcedure.ProcedureInRecordingID);
                    FindProcedure.IsCanceled = true;
                    DB.db.SaveChanges();
                    ShowShedule();
                    return true;
                }
            }
            return false;
        }

        private void BtnVisit_Click(object sender, RoutedEventArgs e)
        {
            ProcedureInRecording CurrnetProcedure = SheduleList.SelectedItem as ProcedureInRecording;
            if (CurrnetProcedure != null)
            {
                var result = MessageBox.Show("Отметить посещение?", "Посещение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var FindProcedure = DB.db.ProcedureInRecording.Find(CurrnetProcedure.ProcedureInRecordingID);
                    FindProcedure.IsVisited = true;
                    DB.db.SaveChanges();
                    ShowShedule();
                }
            }
        }

        private void CmbMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowShedule();
        }

        private void DaPDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowShedule();
        }
    }
}
