namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class DeletedFile
    {
        public DeletedFile(Subtopic subtopic, string name)
        {
            this.Subtopic = subtopic;
            this.Name = name;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public Subtopic Subtopic { get; private set; }
    }
}
