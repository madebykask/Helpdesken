namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    public class FilesModel
    {
        public FilesModel(string id,
                          List<string>files,
                          bool virtualDirectory)
        {
            this.Id = id;
            this.Files = files;
            this.VirtualDirectory = virtualDirectory;
        }

        public FilesModel() {}

        public string Id { get; set; }
        public List<string> Files { get; set; }
        public bool VirtualDirectory { get; set; }
    }
}
