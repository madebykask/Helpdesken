using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.CaseSolution;

namespace DH.Helpdesk.Web.Models.Case
{    
    public sealed class CaseTemplateTreeModel
    {
        public int CustomerId { get; set; }
        
        public List<CaseTemplateCategoryNode> CaseTemplateCategoryTree { get; set; }

        //public List<CaseSolutionInputViewModel> CaseTemplateCategoryTree { get; set; }
    }
}