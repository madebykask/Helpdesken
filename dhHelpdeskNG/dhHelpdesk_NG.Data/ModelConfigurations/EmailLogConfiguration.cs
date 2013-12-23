using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;


namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class EmailLogConfiguration : EntityTypeConfiguration<EmailLog>
    {
        internal EmailLogConfiguration()
        {
            HasKey(l => l.Id);

            Property(l => l.EmailLogGUID).IsRequired();
            Property(l => l.Log_Id).IsOptional();
            Property(l => l.EmailAddress).IsRequired().HasMaxLength(1000);
            Property(l => l.MailId).IsRequired();
            Property(l => l.MessageId).IsOptional();
            Property(l => l.CaseHistory_Id).IsOptional();
            Property(l => l.ChangedDate).IsRequired();
            Property(l => l.ChangedDate).IsRequired();
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblEmaillog");
        }
    }
}
