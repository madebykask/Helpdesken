using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseSectionOptionsData
    {
        public int SectionId { get; set; } 
        public int CustomerId { get; set; } 
        public int IsNewCollapsed { get; set; } 
        public int IsEditCollapsed { get; set; } 
        public int SectionType { get; set; } 
        public List<int> SelectedFields { get; set; } 
        public int LanguageId { get; set; }
        public int? DefaultUserSearchCategory { get; set; }
        public bool ShowUserSearchCategory { get; set; }
    }
}