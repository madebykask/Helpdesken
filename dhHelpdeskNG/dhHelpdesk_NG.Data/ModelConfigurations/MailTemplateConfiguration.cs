using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class MailTemplateConfiguration : EntityTypeConfiguration<MailTemplate>
    {
        internal MailTemplateConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.AccountActivity)
                .WithMany()
                .HasForeignKey(x => x.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.OrderType)
                .WithMany()
                .HasForeignKey(x => x.OrderType_Id)
                .WillCascadeOnDelete(false);
                        
            Property(x => x.AccountActivity_Id).IsOptional();
            Property(x => x.Customer_Id).IsOptional();
            Property(x => x.IsStandard).IsRequired();
            Property(x => x.MailID).IsRequired();
            Property(x => x.MailTemplateGUID).IsRequired();
            Property(x => x.OrderType_Id).IsOptional();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblmailtemplate");
        }
    }

    public class MailTemplateLanguageConfiguration : EntityTypeConfiguration<MailTemplateLanguage>
    {
        internal MailTemplateLanguageConfiguration()
        {
            HasKey(x => new { x.Language_Id, x.MailTemplate_Id });

            HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.MailTemplate)
                .WithMany(x => x.MailTemplateLanguages)
                .HasForeignKey(x => x.MailTemplate_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Body).IsRequired().HasMaxLength(2000);
            Property(x => x.MailFooter).IsOptional().HasMaxLength(500);
            Property(x => x.Name).IsOptional().HasMaxLength(50).HasColumnName("MailTemplateName");
            Property(x => x.Subject).IsRequired().HasMaxLength(200);

            ToTable("tblmailtemplate_tbllanguage");
        }
    }
}
