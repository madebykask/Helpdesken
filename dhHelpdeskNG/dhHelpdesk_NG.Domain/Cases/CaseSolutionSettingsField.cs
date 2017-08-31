using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSolutionSettingsField : Entity
    {
        public string CaseSolutionConditionId { get; set; }

        public int CaseSolutionId { get; set; }

        public string PropertyName { get; set; }

        public string CaseSolutionConditionGuid { get; set; }

        public string Text { get; set; }

        //public string[] SelectedValues { get; set; }

        public List<string> SelectedValues  { get;set;}

        public List<SelectListItem> SelectList { get; set; }

        public string Table { get; set; }

    }
}
    