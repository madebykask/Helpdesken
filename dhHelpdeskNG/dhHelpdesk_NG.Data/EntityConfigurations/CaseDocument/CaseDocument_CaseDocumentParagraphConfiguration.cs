namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{        
    using System.Data.Entity.ModelConfiguration;

    internal sealed class CaseDocument_CaseDocumentParagraphConfiguration : EntityTypeConfiguration<Domain.CaseDocument_CaseDocumentParagraphEntity>
    {
        #region Constructors and Destructors
        internal CaseDocument_CaseDocumentParagraphConfiguration()
        {
            HasKey(e => new { e.CaseDocument_Id, e.CaseDocumentParagraph_Id });

            HasRequired(t => t.CaseDocument)
                .WithMany(t => t.CaseDocumentParagraphs)
                .HasForeignKey(d => d.CaseDocument_Id)
                .WillCascadeOnDelete(true);

            HasRequired(t => t.CaseDocumentParagraph)
                .WithMany(t => t.CaseDocumentParagraphs)
                .HasForeignKey(d => d.CaseDocumentParagraph_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseDocument_CaseDocumentParagraph");
        }
        #endregion
    }
}