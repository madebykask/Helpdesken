namespace DH.Helpdesk.Domain
{
    public class MergedCases
    {
        public int MergedParentId { get; set; }

        public int MergedChildId { get; set; }

        public virtual Case Case { get; set; }
    }
}
