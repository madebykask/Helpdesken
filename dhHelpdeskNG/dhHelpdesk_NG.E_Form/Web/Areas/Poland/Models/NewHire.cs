using System.Globalization;
using Model.Entities;
using Web.Models;

namespace Web.Areas.Poland.Models
{
    public class NewHire : BaseModel
    {
        private int _status;

        public NewHire(string xml)
            : base(xml)
        {}

        public string ActiveTab { get; set; }
        public int Status { get { return _status; } set { _status = value; base.SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
    }
}