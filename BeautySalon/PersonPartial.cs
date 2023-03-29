using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon
{
   public partial class Person
    {
        public string FullName
        {
            get
            {
                return $"{LastName} {Name[0]}. {MiddleName[0]}.";
            }
        }
    }
}
