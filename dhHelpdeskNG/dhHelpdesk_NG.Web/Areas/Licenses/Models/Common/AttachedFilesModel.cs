namespace DH.Helpdesk.Web.Areas.Licenses.Models.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.Enums;

    public sealed class AttachedFilesModel
    {
        public AttachedFilesModel(
                string entityId,
                AttachedFileType type,
                List<string> files)
        {
            this.EntityId = entityId;
            this.Type = type;
            this.Files = files;
        }

        public string EntityId { get; set; }

        public AttachedFileType Type { get; set; }

        [NotNull]
        public List<string> Files { get; set; }
    }
}