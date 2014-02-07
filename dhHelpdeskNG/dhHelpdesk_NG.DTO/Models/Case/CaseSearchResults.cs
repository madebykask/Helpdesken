namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;

    public class CaseSearchResult
    {
        public int Id { get; set; }
        public GlobalEnums.CaseIcon CaseIcon { get; set; }
        public string SortOrder { get; set; }
        public string Tooltip { get; set; }
        public IList<Universal> Columns { get; set; }
    }
}
