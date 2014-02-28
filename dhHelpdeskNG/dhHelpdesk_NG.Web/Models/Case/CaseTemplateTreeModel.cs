using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.Case
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
        public List<CaseTemplateNode> CaseTemplates { get; set; }
    }

    public sealed class CaseTemplateTreeModel
    {
        public int CustomerId { get; set; }
        public List<CaseTemplateCategoryNode> CaseTemplateCategoryTree { get; set; }
    }
}