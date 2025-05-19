using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSchedule.Data
{
    internal class Enums
    {
        public enum RepeatType
        {
            None = 0,           // Ingen upprepning
            Daily = 1,          // Varje dag
            Weekly = 2,         // Varje vecka
            Monthly = 3,        // Varje månad (viss dag eller veckodag)
            Yearly = 4,         // Varje år
            //EveryXDays = 5,     // Vart X dag
            //EveryXWeeks = 6,    // Vart X vecka
            //EveryXMonths = 7,   // Vart X månad
            EveryXYears = 8    // Vart X år
        }
    }
}
