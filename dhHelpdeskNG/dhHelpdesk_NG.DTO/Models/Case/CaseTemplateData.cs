namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseTemplateData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string TemplatePath { get; set; }
        public int? CaseSolutionCategory_Id { get; set; }
        public string CaseSolutionCategoryName { get; set; }
        public bool ContainsExtendedForm { get; set; }
        public int? OrderNum { get; set; }
    }
}