namespace DH.Helpdesk.BusinessData.Models.FinishingCause
{
    public sealed class FinishingCauseInfo
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public bool Merged { get; set; }
    }
}