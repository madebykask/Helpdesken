using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    public class TabSetting
    {
        public TabSetting(string tabField, bool show, string caption)
        {
            TabField = tabField;
            Show = show;
            Caption = caption;
        }

        public bool Show { get; private set; }
        [NotNull]
        public string Caption { get; private set; }
        [NotNull]
        public string TabField { get; private set; }
    }
}
