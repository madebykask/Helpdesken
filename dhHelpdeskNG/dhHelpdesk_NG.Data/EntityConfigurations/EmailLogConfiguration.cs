using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class EmailLogConfiguration : EntityTypeConfiguration<EmailLog>
    {
        internal EmailLogConfiguration()
        {
            this.HasKey(l => l.Id);

            this.Property(l => l.EmailLogGUID).IsRequired();
            this.Property(l => l.Log_Id).IsOptional();
            this.Property(l => l.EmailAddress).IsRequired().HasMaxLength(1000);
            this.Property(l => l.MailId).IsRequired();
            this.Property(l => l.MessageId).IsOptional();
            this.Property(l => l.CaseHistory_Id).IsOptional();
            this.Property(l => l.ChangedDate).IsRequired();
            this.Property(l => l.CreatedDate).IsRequired();
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.SendTime).IsOptional();
            this.Property(l => l.ResponseMessage).IsOptional();

            this.HasMany(x => x.EmailLogAttempts)
                .WithRequired(x => x.EmailLog)
                .HasForeignKey(x => x.EmailLog_Id);

            this.HasOptional(l => l.CaseHistory)
                .WithMany(l => l.Emaillogs)
                .HasForeignKey(l => l.CaseHistory_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblEmaillog");
        }
    }
}

