namespace DH.Helpdesk.BusinessData.Models
{
    public class WorkflowStepModel
    {
        public int CaseTemplateId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int NextStep { get; set; }
    }
}
