using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon
{
    public partial class ProcedureInRecording
    {
        public string IsCanceledString
        {
            get
            {
                return IsCanceled ? "Отменён" : "Не отменён";
            }
        }
        public string IsVisitedString
        {
            get
            {
                return IsVisited ? "Посещён" : "Не посещён";
            }
        }
    }
}
