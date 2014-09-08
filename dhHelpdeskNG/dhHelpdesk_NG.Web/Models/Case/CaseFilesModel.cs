namespace DH.Helpdesk.Web.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;

    public sealed class CaseFilesModel
    {
        public CaseFilesModel(
                    string objectId, 
                    CaseFileModel[] files)
        {
            this.Files = files;
            this.ObjectId = objectId;
        }

        public CaseFilesModel()
        {
        }

        public string ObjectId { get; private set; }

        public CaseFileModel[] Files { get; private set; }
    }
}