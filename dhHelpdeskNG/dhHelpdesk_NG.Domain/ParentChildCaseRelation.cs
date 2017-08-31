namespace DH.Helpdesk.Domain
{
    public class ParentChildRelation
    {
        public int AncestorId { get; set; }

        public int DescendantId { get; set; }

		public bool Independent { get; set; }
    }
}
