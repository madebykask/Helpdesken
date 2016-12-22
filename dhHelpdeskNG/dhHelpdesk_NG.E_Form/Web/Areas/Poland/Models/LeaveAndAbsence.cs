using Model.Entities;
using System.Globalization;
using Web.Models;

namespace Web.Areas.Poland.Models
{
    public class LeaveAndAbsence : BaseModel
    {
         private int _status;

        public LeaveAndAbsence(string xml)
            : base(xml)
        {}

        public string ActiveTab { get; set; }
        public int Status { get { return _status; } set { _status = value; SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
    }
}
    