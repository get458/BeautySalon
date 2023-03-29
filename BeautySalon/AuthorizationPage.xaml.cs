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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        public bool Autorization(string login, string password)
        {
            if (login == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Ключевые поля не заполнены");
                return false;
            }
            Employee user = DB.db.Employee.FirstOrDefault(x => x.Login == login);
            if (user == null)
            {
                MessageBox.Show("Пользователя нет в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.Password != password)
            {
                MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.IsWorking == false)
            {
                MessageBox.Show("Сотрудник уволен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.IDRole == 1)
            {
               ManagerFrame.MainFrame.Navigate(new MenuPage());
               return true;
            }
            if (user.IDRole == 2)
            {
                int MasterID = DB.db.Master.FirstOrDefault(x => x.IDEmployee == user.EmployeeID).MasterID;
                ManagerFrame.MainFrame.Navigate(new MenuPage(MasterID));
                return true;
            }
            return false;

        }

        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            Autorization(TxtLogin.Text, TxtPass.Password);
        }
    }
}
