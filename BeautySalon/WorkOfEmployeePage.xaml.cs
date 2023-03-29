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
    /// Логика взаимодействия для WorkOfEmployeePage.xaml
    /// </summary>
    public partial class WorkOfEmployeePage : Page
    {
        ObservableCollection<Procedure> ProceduresOfMaster = new ObservableCollection<Procedure>();
        public WorkOfEmployeePage()
        {
            InitializeComponent();
            DataContext = new Employee();
            PersonData.DataContext = new Person();
            Load();
        }
        public WorkOfEmployeePage(int IDEmployee)
        {
            InitializeComponent();
            var FindEmployee = DB.db.Employee.Find(IDEmployee);
            DataContext = FindEmployee;
            PersonData.DataContext = FindEmployee.Person;
            if (FindEmployee.IDRole == 2)
            {
                var FindMaster = DB.db.Master.FirstOrDefault(x=>x.IDEmployee == FindEmployee.EmployeeID);
                foreach (var item in FindMaster.ProcedureOfMaster)
                {
                    ProceduresOfMaster.Add(item.Procedure);
                }
                CmbPosition.SelectedItem = FindMaster.Position;

            }
            Load();
            Title = "Редатирование сотрудника";
            BtnAddOrEditEmployee.Content = "Редактировать";

        }

        public void Load()
        {
            CmbRole.ItemsSource = DB.db.Role.ToList();
            CmbPosition.ItemsSource = DB.db.Position.ToList();
            CmbProcedure.ItemsSource = DB.db.Procedure.ToList();
            ProcedureOfMasterList.ItemsSource = ProceduresOfMaster;
        }

        public bool CheckEmployee(Employee NewEmployee)
        {
            if (NewEmployee.Person.LastName == string.Empty ||
                NewEmployee.Person.Name == string.Empty ||
                NewEmployee.Person.MiddleName == string.Empty ||
                NewEmployee.BirthDate == null ||
                NewEmployee.Role == null
                )
            {
                MessageBox.Show("Не все значения введены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (NewEmployee.Person.LastName.All(char.IsLetter) == false ||
                NewEmployee.Person.Name.All(char.IsLetter) == false ||
                (NewEmployee.Person.MiddleName != string.Empty && NewEmployee.Person.MiddleName.All(char.IsLetter) == false))
            {
                MessageBox.Show("Не корректное ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool AddEmployee(Employee NewEmployee)
        {
            if (CheckEmployee(NewEmployee) == false)
            {
                return false;
            }
            DB.db.Employee.Add(NewEmployee);
            DB.db.SaveChanges();
            if (NewEmployee.IDRole == 2)
            {
                Master master = new Master
                {
                    IDEmployee = NewEmployee.EmployeeID,
                    IDPosition = (CmbPosition.SelectedItem as Position).PositionID
                };
                DB.db.Master.Add(master);
                DB.db.SaveChanges();
                int IdMaster = DB.db.Master.Select(x => x.MasterID).Max();
                foreach (var item in ProceduresOfMaster)
                {
                    ProcedureOfMaster procedureOfMaster = new ProcedureOfMaster
                    {
                        IDMaster = IdMaster,
                        Procedure = item
                    };
                    DB.db.ProcedureOfMaster.Add(procedureOfMaster);
                    DB.db.SaveChanges();
                }
            }
            MessageBox.Show("Сотрудник добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            DataContext = new Employee();
            PersonData.DataContext = new Person();
            return true;
        }

        private void BtnAddOrEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var Employee = DataContext as Employee;
            Employee.Person = PersonData.DataContext as Person;
            Employee.IsWorking = true;
            if (Employee.EmployeeID == 0)
            {
                AddEmployee(Employee);
                return;
            }
        }

        private void BtnDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            Procedure CurrentProcedure = (sender as Button).DataContext as Procedure;
            ProceduresOfMaster.Remove(CurrentProcedure);
            DB.db.SaveChanges();
        }

        private void BtnAddInTable_Click(object sender, RoutedEventArgs e)
        {
            ProceduresOfMaster.Add(CmbProcedure.SelectedItem as Procedure);
        }

        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Role = CmbRole.SelectedItem as Role;
            if (Role == null || Role.RoleID == 1)
            {
                DataOfMaster.Visibility = Visibility.Collapsed;
                ProceduresOfMaster.Clear();
            }
            else
            {
                DataOfMaster.Visibility = Visibility.Visible;
            }
        }
    }
}
