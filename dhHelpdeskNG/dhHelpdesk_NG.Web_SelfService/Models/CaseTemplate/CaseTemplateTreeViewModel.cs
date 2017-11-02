using System.Collections.Generic;

namespace DH.Helpdesk.SelfService.Models.CaseTemplate
{
    public class CaseTemplateTreeViewModel
    {
        public CaseTemplateTreeViewModel()
        {
            Templates = new List<CaseTemplateViewModel>();
        }

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<CaseTemplateViewModel> Templates { get; set; }

    }

    public class CaseTemplateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string TemplatePath { get; set; }
        public int? TemplateCategory_Id { get; set; }
        public int? OrderNum { get; set; }
        public bool ContainsExtendedForm { get; set; }

    }

}