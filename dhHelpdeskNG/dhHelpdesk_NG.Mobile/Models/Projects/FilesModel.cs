namespace DH.Helpdesk.Mobile.Models.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FilesModel
    {
        public FilesModel(string guid, List<string> files)
        {
            this.Guid = guid;
            this.Files = files;
        }

        public string Guid { get; private set; }

        [NotNull]
        public List<string> Files { get; private set; }
    }
}