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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ManagerFrame.MainFrame = this.MainFrame;
            ManagerFrame.MainFrame.Navigate(new AuthorizationPage());
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            BtnBack.Visibility = ManagerFrame.MainFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;
            MainLogoPanel.Visibility = ManagerFrame.MainFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;
        }

        public static void Logaut()
        {
            if (ManagerFrame.MainFrame.CanGoBack)
            {
                ManagerFrame.MainFrame.GoBack();
                Logaut();
            }
            else
            {
                return;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.MainFrame.GoBack();
        }
    }
}
