namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class MailTemplateConfiguration : EntityTypeConfiguration<MailTemplate>
    {
        internal MailTemplateConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.AccountActivity)
                .WithMany()
                .HasForeignKey(x => x.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.OrderType)
                .WithMany()
                .HasForeignKey(x => x.OrderType_Id)
                .WillCascadeOnDelete(false);
                        
            this.Property(x => x.AccountActivity_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.IsStandard).IsRequired();
            this.Property(x => x.MailID).IsRequired();
            this.Property(x => x.MailTemplateGUID).IsRequired();
            this.Property(x => x.OrderType_Id).IsOptional();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblmailtemplate");
        }
    }

    public class MailTemplateLanguageConfiguration : EntityTypeConfiguration<MailTemplateLanguage>
    {
        internal MailTemplateLanguageConfiguration()
        {
            this.HasKey(x => new { x.Language_Id, x.MailTemplate_Id });

            this.HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.MailTemplate)
                .WithMany(x => x.MailTemplateLanguages)
                .HasForeignKey(x => x.MailTemplate_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Body).IsRequired().HasMaxLength(2000);
            this.Property(x => x.MailFooter).IsOptional().HasMaxLength(500);
            this.Property(x => x.Name).IsOptional().HasMaxLength(50).HasColumnName("MailTemplateName");
            this.Property(x => x.Subject).IsRequired().HasMaxLength(200);

            this.ToTable("tblmailtemplate_tbllanguage");
        }
    }
}
