namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentParagraphConfiguration : EntityTypeConfiguration<CaseDocumentParagraphEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentParagraphConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Name).IsOptional().HasMaxLength(50);
            Property(e => e.Description).IsOptional().HasMaxLength(50);
            Property(e => e.ParagraphType).IsRequired();
            ToTable("tblCaseDocumentParagraph");
        }

        #endregion
    }
}