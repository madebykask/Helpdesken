namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CaseTypeOverview
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public int ShowOnExternalPage { get; set; }

        public int ShowOnExtPageCases { get; set; }
    }
}