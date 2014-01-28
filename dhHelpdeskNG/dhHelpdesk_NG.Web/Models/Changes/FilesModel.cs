namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class FilesModel
    {
        public FilesModel(string changeId, Subtopic subtopic, List<string> files)
        {
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
            this.Files = files;
        }

        public string ChangeId { get; private set; }

        public Subtopic Subtopic { get; private set; }

        [NotNull]
        public List<string> Files { get; private set; }
    }
}