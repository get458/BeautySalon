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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            ShowClient();
        }
        public void ShowClient()
        {
            ClientList.ItemsSource = DB.db.Client.Where(item =>
            (item.Person.LastName + " " +
            item.Person.Name + " "
            + item.Person.MiddleName).Contains(TxtSearch.Text)).OrderByDescending(x => x.Person.LastName).ToList();
            CmbDiscount.ItemsSource = DB.db.Discount.ToList();
        }
        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.MainFrame.Navigate(new WorkOfClientPage());
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            Client CurrentClient = (Client)ClientList.SelectedItem;
            if (CurrentClient != null)
            {
                ManagerFrame.MainFrame.Navigate(new WorkOfClientPage(CurrentClient.ClientID));
            }

        }

        public bool DeleteClient(Client CurrentClient)
        {
            var result = MessageBox.Show("Удалить клиента?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DB.db.Client.Remove(CurrentClient);
                DB.db.SaveChanges();
                return true;

            }
            return false;
        }

        private void BtnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            Client CurrentClient = (Client)ClientList.SelectedItem;
            if (CurrentClient != null)
            {
                DeleteClient(CurrentClient);
            }
            ShowClient();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowClient();
        }

        private void ClientList_CurrentCellChanged(object sender, EventArgs e)
        {
            DB.db.SaveChanges();
        }
    }
}
