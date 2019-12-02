using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.BusinessData.Models.Calendar.Output;

namespace DH.Helpdesk.Web.Models.Widgets
{
    public class WidgetModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = new T[] { };
        public bool ShowMore { get; set; }

    }
}