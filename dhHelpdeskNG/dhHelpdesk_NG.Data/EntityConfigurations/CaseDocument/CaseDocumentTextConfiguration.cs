namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentTextConfiguration : EntityTypeConfiguration<CaseDocumentTextEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentTextConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.CaseDocumentParagraph_Id).IsRequired();
            Property(e => e.Name).IsOptional().HasMaxLength(50);
            Property(e => e.Description).IsOptional().HasMaxLength(50);
            Property(e => e.Text).IsRequired();
            Property(e => e.Headline).IsOptional().HasMaxLength(200);
            Property(e => e.SortOrder).IsRequired();

            HasRequired(t => t.CaseDocumentParagraph)
             .WithMany(t => t.CaseDocumentTexts)
             .HasForeignKey(d => d.CaseDocumentParagraph_Id).WillCascadeOnDelete(false);

            HasMany(x => x.Conditions)
                .WithOptional()
                .HasForeignKey(x => x.CaseDocumentText_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseDocumentText");
        }

        #endregion
    }
}