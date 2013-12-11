using System.Collections.Generic;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.Utils;  

namespace dhHelpdesk_NG.DTO.DTOs.Case
{
    public class CaseSearchResult
    {
        public int Id { get; set; }
        public GlobalEnums.CaseIcon CaseIcon { get; set; }
        public string SortOrder { get; set; }
        public string Tooltip { get; set; }
        public IList<Universal> Columns { get; set; }
    }
}
