using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon
{
   public partial class Employee
    {
        public string StatusEmployee
        {
            get
            {
                return IsWorking ? "Работает" : "Уволен";
            }
        }
    }
}
