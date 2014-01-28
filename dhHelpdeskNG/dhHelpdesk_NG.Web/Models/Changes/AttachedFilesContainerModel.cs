namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class AttachedFilesContainerModel
    {
        public AttachedFilesContainerModel(string changeId, Subtopic subtopic)
        {
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
        }

        public string ChangeId { get; private set; }

        public Subtopic Subtopic { get; private set; }
    }
}