using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Логика взаимодействия для WorkOfClientPage.xaml
    /// </summary>
    public partial class WorkOfClientPage : Page
    {
        public WorkOfClientPage()
        {
            InitializeComponent();
            DataContext = new Client();
            PersonData.DataContext = new Person();
        }

        public WorkOfClientPage(int IDClient)
        {
            InitializeComponent();
            var FindClient = DB.db.Client.Find(IDClient);
            DataContext = FindClient;
            PersonData.DataContext = FindClient.Person;
            Title = "Редатирование клиента";
            BtnAddOrEditClient.Content = "Редактировать";
        }

        public bool CheckClient(Client NewClient)
        {
            if (NewClient.Person.LastName == string.Empty ||
                NewClient.Person.Name == string.Empty ||
                NewClient.Person.MiddleName == string.Empty ||
                NewClient.Phone == null
                )
            {
                MessageBox.Show("Не все значения введены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (NewClient.Person.LastName.All(char.IsLetter) == false ||
                NewClient.Person.Name.All(char.IsLetter) == false ||
                (NewClient.Person.MiddleName != string.Empty && NewClient.Person.MiddleName.All(char.IsLetter) == false))
            {
                MessageBox.Show("Не корректное ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!new PhoneAttribute().IsValid(NewClient.Phone))
            {
                MessageBox.Show("Не корректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool AddClient(Client NewClient)
        {
            try
            {
                if (CheckClient(NewClient) == false)
                {
                    return false;
                }
                DB.db.Client.Add(NewClient);
                MessageBox.Show("Клиент добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DB.db.SaveChanges();
                DataContext = new Employee();
                PersonData.DataContext = new Person();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool EditClient()
        {
            var CurrentClient = DataContext as Client;
            CurrentClient.IDDiscount = 1;
            if (CheckClient(CurrentClient) == false)
            {
                return false;
            }
            DB.db.SaveChanges();
            MessageBox.Show("Сотрудник изменён", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void BtnAddOrEditClient_Click(object sender, RoutedEventArgs e)
        {
            var Client = DataContext as Client;
            (DataContext as Client).Person = PersonData.DataContext as Person;
            Client.IDDiscount = 1;
            if (Client.ClientID == 0)
            {
                AddClient(Client);
                return;
            }
            EditClient();
        }
    }
}
