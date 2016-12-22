using System.Globalization;
using Model.Entities;
using Web.Models;

namespace Web.Areas.Poland.Models
{
    public class DataChange : BaseModel
    {
        private int _status;

        public DataChange(string xml)
            : base(xml)
        {}

        public string ActiveTab { get; set; }
        public int Status { get { return _status; } set { _status = value; SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
    }
}