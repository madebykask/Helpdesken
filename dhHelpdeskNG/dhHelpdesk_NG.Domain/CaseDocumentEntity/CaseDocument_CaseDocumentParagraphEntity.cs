namespace DH.Helpdesk.Domain
{
    public class CaseDocument_CaseDocumentParagraphEntity
    {
        public int CaseDocument_Id { get; set; }
        public int CaseDocumentParagraph_Id { get; set; }
        public int SortOrder { get; set; }

        public virtual CaseDocumentEntity CaseDocument { get; set; }
        public virtual CaseDocumentParagraphEntity CaseDocumentParagraph { get; set; }
    }
}
