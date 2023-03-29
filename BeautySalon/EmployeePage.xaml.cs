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
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public EmployeePage()
        {
            InitializeComponent();
            ShowEmployee();
        }

        public void ShowEmployee()
        {
            EmployeeList.ItemsSource = DB.db.Employee.Where(item => 
            (item.Person.LastName + " " +
            item.Person.Name + " "
            + item.Person.MiddleName).Contains(TxtSearch.Text)).OrderByDescending(x => x.IsWorking).ThenBy(x => x.Person.LastName).ToList();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new WorkOfEmployeePage());
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee CurrentEmployee = (Employee)EmployeeList.SelectedItem;
            if (CurrentEmployee != null)
            {
                ManagerFrame.ContentFrame.Navigate(new WorkOfEmployeePage(CurrentEmployee.EmployeeID));
            }

        }

        public bool DismissialEmployee(Employee CurrentEmployee)
        {
            var result = MessageBox.Show("Уволить сотрудника?", "Увольнение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (CurrentEmployee.IsWorking == false)
                {
                    MessageBox.Show("Сотрудник уволен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                var FindEmployee = DB.db.Employee.Find(CurrentEmployee.EmployeeID);
                FindEmployee.IsWorking = false;
                DB.db.SaveChanges();
                return true;

            }
            return false;
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee CurrentEmployee = (Employee)EmployeeList.SelectedItem;
            if (CurrentEmployee != null)
            {
                DismissialEmployee(CurrentEmployee);
            }
            ShowEmployee();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowEmployee();
        }
    }
}
