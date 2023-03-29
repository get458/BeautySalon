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
    /// Логика взаимодействия для ProcedurePage.xaml
    /// </summary>
    public partial class ProcedurePage : Page
    {
        public ProcedurePage()
        {
            InitializeComponent();
            ShowProcedure();
        }

        public void ShowProcedure()
        {
            ProcedureList.ItemsSource = DB.db.Procedure.Where(item =>
            item.ProceduresName.Contains(TxtSearch.Text)).OrderBy(x => x.ProceduresName).ToList();
        }

        public bool DeleteProcedure(Procedure CurrentProcedure)
        {
            var result = MessageBox.Show("Удалить процедуру?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DB.db.Procedure.Remove(CurrentProcedure);
                DB.db.SaveChanges();
                return true;

            }
            return false;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowProcedure();
        }

        private void BtnAddProcedure_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.ContentFrame.Navigate(new WorkOfProcedurePage());
        }

        private void BtnEditProcedure_Click(object sender, RoutedEventArgs e)
        {
            Procedure CurrentProcedure = (Procedure)ProcedureList.SelectedItem;
            if (CurrentProcedure != null)
            {
                ManagerFrame.ContentFrame.Navigate(new WorkOfProcedurePage(CurrentProcedure.ProcedureID));
            }

        }

        private void BtnDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            Procedure CurrentProcedure = (Procedure)ProcedureList.SelectedItem;
            if (CurrentProcedure != null)
            {
                DeleteProcedure(CurrentProcedure);
            }
            ShowProcedure();
        }
    }
}
