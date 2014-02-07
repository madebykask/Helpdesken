namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

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