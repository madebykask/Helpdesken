namespace DH.Helpdesk.Dal.EntityConfigurations.MailTemplates
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.MailTemplates;

    public sealed class MailTemplateLanguageConfiguration : EntityTypeConfiguration<MailTemplateLanguageEntity>
    {
        #region Constructors and Destructors

        internal MailTemplateLanguageConfiguration()
        {
            this.HasKey(tl => new { tl.MailTemplate_Id, tl.Language_Id });

            this.HasRequired(tl => tl.MailTemplate)
                .WithMany()
                .HasForeignKey(tl => tl.MailTemplate_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(tl => tl.Language)
                .WithMany()
                .HasForeignKey(tl => tl.Language_Id)
                .WillCascadeOnDelete(false);

            this.Property(tl => tl.MailTemplateName).IsOptional().HasMaxLength(50);
            this.Property(tl => tl.Subject).IsRequired().HasMaxLength(200);
            this.Property(tl => tl.Body).IsRequired().HasMaxLength(4000);
            this.Property(tl => tl.MailFooter).IsOptional().HasMaxLength(500);

            this.ToTable("tblMailTemplate_tblLanguage");
        }

        #endregion
    }
}