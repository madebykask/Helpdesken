namespace DH.Helpdesk.Web.Models.Documents
{
    public class DocumentsFilterModel
    {
        public DocumentsFilterModel(int documentType, int categoryId)
        {
            this.CategoryId = categoryId;
            this.DocumentType = documentType;
        }

        private DocumentsFilterModel()
        {            
        }

        public int DocumentType { get; private set; } 

        public int CategoryId { get; private set; }

        public static DocumentsFilterModel CreateDefault()
        {
            return new DocumentsFilterModel();
        }
    }
}