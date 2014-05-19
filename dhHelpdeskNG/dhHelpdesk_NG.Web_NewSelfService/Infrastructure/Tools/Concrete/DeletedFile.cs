namespace DH.Helpdesk.NewSelfService.Infrastructure.Tools.Concrete
{
    using DH.Helpdesk.Common.ValidationAttributes;

    internal sealed class DeletedFile
    {
        public DeletedFile(string name, string subtopic)
        {
            this.Name = name;
            this.Subtopic = subtopic;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Subtopic { get; private set; }
    }
}