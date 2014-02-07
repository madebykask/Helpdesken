namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    public class FilesModel
    {
        public FilesModel(string id, List<string>files)
        {
            this.Id = id;
            this.Files = files;
        }

        public FilesModel() {}

        public string Id { get; set; }
        public List<string> Files { get; set; }
    }
}
