using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        public int HolidayHeader_Id { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
