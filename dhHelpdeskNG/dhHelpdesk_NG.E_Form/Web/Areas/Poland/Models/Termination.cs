using System.Globalization;
using Model.Entities;
using Web.Models;

namespace Web.Areas.Poland.Models
{
    public class Termination : BaseModel
    {
        private int _status;

        public Termination(string xml)
            : base(xml)
        {}

        public string ActiveTab { get; set; }
        public int Status { get { return _status; } set { _status = value; SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
    }
}