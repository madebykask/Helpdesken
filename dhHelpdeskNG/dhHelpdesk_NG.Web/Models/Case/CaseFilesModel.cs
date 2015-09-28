namespace DH.Helpdesk.Web.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;

    public sealed class CaseFilesModel
    {
        public CaseFilesModel(
                    string objectId, 
                    CaseFileModel[] files,
                    bool virtualDirectory)
        {
            this.Files = files;
            this.ObjectId = objectId;
            this.VirtualDirectory = virtualDirectory;
        }

        public CaseFilesModel()
        {
        }

        public string ObjectId { get; private set; }

        public CaseFileModel[] Files { get; private set; }

        public bool VirtualDirectory { get; set; }
    }
}