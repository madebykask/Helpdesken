using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.DTO.DTOs.Case
{
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
