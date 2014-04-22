namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DeletedFile
    {
        public DeletedFile(Subtopic subtopic, string name)
        {
            this.Subtopic = subtopic;
            this.Name = name;
        }

        [IsId]
        internal int ChangeId { get; set; }

        public Subtopic Subtopic { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }
    }
}
