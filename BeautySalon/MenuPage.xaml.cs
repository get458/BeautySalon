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
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
            MasterID = null;
            ManagerFrame.ContentFrame = this.ContentFrame;
            ManagerFrame.ContentFrame.Navigate(new RecordingPage());
        }

        public int? MasterID { get; set; }
        public MenuPage(int IdMaster)
        {
            InitializeComponent();
            MasterID = IdMaster;
            AdminPanel.Visibility = Visibility.Collapsed;
            ManagerFrame.ContentFrame = this.ContentFrame;
            ManagerFrame.ContentFrame.Navigate(new ScheduleMasterPage(IdMaster));
        }
        private void BtnRecording_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new RecordingPage());
        }
        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new EmployeePage());
        }

        private void BtnProcedure_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new ProcedurePage());
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new ClientPage());
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (MasterID == null)
            {
                ManagerFrame.ContentFrame.Navigate(new ScheduleMasterPage());
            }
            else
            {
                ManagerFrame.ContentFrame.Navigate(new ScheduleMasterPage(MasterID.Value));
            }
        }


    }
}
