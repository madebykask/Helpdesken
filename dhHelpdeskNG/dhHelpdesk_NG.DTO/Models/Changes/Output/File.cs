namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class File
    {
        public File(Subtopic subtopic, string name)
        {
            this.Subtopic = subtopic;
            this.Name = name;
        }

        public Subtopic Subtopic { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }
    }
}
