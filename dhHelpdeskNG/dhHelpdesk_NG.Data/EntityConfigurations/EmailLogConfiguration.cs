using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class EmailLogConfiguration : EntityTypeConfiguration<EmailLog>
    {
        internal EmailLogConfiguration()
        {
            HasKey(l => l.Id);
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(l => l.CaseHistory_Id).IsOptional();
            Property(l => l.Log_Id).IsOptional();
            Property(l => l.MailId).IsRequired();
            Property(l => l.EmailAddress).IsRequired().HasMaxLength(1000);
            Property(l => l.MessageId).IsOptional();
            Property(l => l.EmailLogGUID).IsRequired();
            Property(l => l.ChangedDate).IsRequired();
            Property(l => l.CreatedDate).IsRequired();
            Property(l => l.SendTime).IsOptional();
            Property(l => l.ResponseMessage).IsOptional();

            Property(l => l.Body).IsOptional();
            Property(l => l.Subject).IsOptional();
            Property(l => l.From).IsOptional().HasMaxLength(1000); 
            Property(l => l.Cc).IsOptional().HasMaxLength(1000); 
            Property(l => l.Bcc).IsOptional().HasMaxLength(1000); 
            Property(l => l.HighPriority).IsRequired();
            Property(l => l.Files).IsOptional();
            Property(l => l.FilesInternal).IsOptional();
            Property(l => l.SendStatus).IsRequired();
            Property(l => l.LastAttempt).IsOptional();
            Property(l => l.Attempts).IsOptional();

            HasMany(x => x.EmailLogAttempts)
                .WithRequired(x => x.EmailLog)
                .HasForeignKey(x => x.EmailLog_Id);

            HasOptional(l => l.CaseHistory)
                .WithMany(l => l.Emaillogs)
                .HasForeignKey(l => l.CaseHistory_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblEmaillog");
        }
    }
}

