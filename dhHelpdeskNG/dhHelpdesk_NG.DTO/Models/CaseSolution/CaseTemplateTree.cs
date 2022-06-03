using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.CaseSolution
{
    public sealed class CaseTemplateNode
    {
        public int CaseTemplateId { get; set; }
        public string CaseTemplateName { get; set; }
        public string WorkingGroup { get; set; }
    }
    
    public sealed class CaseTemplateCategoryNode
    {   
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsRootTemplate { get; set; }
        public List<CaseTemplateNode> CaseTemplates { get; set; }

        public static CaseTemplateCategoryNode CreateRoot(int id, string name)
        {
            return new CaseTemplateCategoryNode
            {
                CategoryId = id,
                CategoryName = name,
                IsRootTemplate = true
            };
        }
    }    

}
