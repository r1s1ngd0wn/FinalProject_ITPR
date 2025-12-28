using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACK_ITPROJECT
{
    public static class SessionManager
    {
        public static string CurrentLoggedInEmployeeId { get; set; }

        public static void Clear()
        {
            CurrentLoggedInEmployeeId = null;
        }
    }
}
