using System.Collections.Generic;

namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSolutionSettingsField : Entity
    {
        public string CaseSolutionConditionId { get; set; }

        public int CaseSolutionId { get; set; }

        public string PropertyName { get; set; }

        public string CaseSolutionConditionGuid { get; set; }

        public string Text { get; set; }

        public List<string> SelectedValues  { get;set;}

        public List<CaseSolutionFieldItem> SelectList { get; set; }

        public string Table { get; set; }
    }

    public class CaseSolutionFieldItem
    {
        public string Name { get; set; }
        public string FieldGuid { get; set; }
        public bool Selected { get; set; }
        public bool Status { get; set; }
    }
}
    