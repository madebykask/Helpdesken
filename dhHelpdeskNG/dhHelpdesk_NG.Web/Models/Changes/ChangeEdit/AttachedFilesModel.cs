namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AttachedFilesModel
    {
        public AttachedFilesModel(string changeId, ChangeArea area, List<string> files)
        {
            this.ChangeId = changeId;
            this.Area = area;
            this.Files = files;
        }

        public string ChangeId { get; set; }

        public ChangeArea Area { get; set; }

        [NotNull]
        public List<string> Files { get; set; }
    }
}