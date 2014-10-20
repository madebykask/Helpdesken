namespace DH.Helpdesk.Web.Areas.Licenses.Models.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AttachedFilesModel
    {
        public AttachedFilesModel(
                string entityId,
                List<string> files)
        {
            this.EntityId = entityId;
            this.Files = files;
        }

        public string EntityId { get; set; }

        [NotNull]
        public List<string> Files { get; set; }
    }
}