namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{        
    using System.Data.Entity.ModelConfiguration;

    internal sealed class CaseDocument_CaseDocumentParagraphConfiguration : EntityTypeConfiguration<Domain.CaseDocument_CaseDocumentParagraphEntity>
    {
        #region Constructors and Destructors

        internal CaseDocument_CaseDocumentParagraphConfiguration()
        {
            HasKey(e => new { e.CaseDocument_Id, e.CaseDocumentParagraph_Id });
            Property(x => x.SortOrder).IsRequired();
            
            HasRequired(t => t.CaseDocument)
                .WithMany()
                .HasForeignKey(d => d.CaseDocument_Id)
                .WillCascadeOnDelete(true);

            HasRequired(t => t.CaseDocumentParagraph)
                .WithMany()
                .HasForeignKey(d => d.CaseDocumentParagraph_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseDocument_CaseDocumentParagraph");
        }
        #endregion
    }
}