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
    /// Логика взаимодействия для WorkOfProcedurePage.xaml
    /// </summary>
    public partial class WorkOfProcedurePage : Page
    {
        public WorkOfProcedurePage()
        {
            InitializeComponent();
            DataContext = new Procedure();
        }
        public WorkOfProcedurePage(int IDProcedure)
        {
            InitializeComponent();
            var FindProcedure = DB.db.Procedure.Find(IDProcedure);
            DataContext = FindProcedure;
            Title = "Редатирование процедуры";
            BtnAddOrEditProcedure.Content = "Редактировать";
        }

        public bool CheckProcedure(Procedure NewProcedure)
        {
            if (NewProcedure.ProceduresName == string.Empty)
            {
                MessageBox.Show("Название не введено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (NewProcedure.Cost <=0)
            {
                MessageBox.Show("Не корректная стоимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public bool AddProcedure(Procedure NewProcedure)
        {
            if (CheckProcedure(NewProcedure) == false)
            {
                return false;
            }
            try
            {
                DB.db.Procedure.Add(NewProcedure);
                MessageBox.Show("Процедура добавлена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DataContext = new Procedure();
                DB.db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Не корректная стоимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }


        public bool EditProcedure()
        {
            var CurrentProcedure = DataContext as Procedure;
            if (CheckProcedure(CurrentProcedure) == false)
            {
                return false;
            }
            DB.db.SaveChanges();
            MessageBox.Show("Процедура изменёна", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void BtnAddOrEditProcedure_Click(object sender, RoutedEventArgs e)
        {
            var Procedure = DataContext as Procedure;
            if (Procedure.ProcedureID == 0)
            {
                AddProcedure(Procedure);
                return;
            }
            EditProcedure();
        }
    }
}
