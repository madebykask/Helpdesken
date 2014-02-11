namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class EmailLogConfiguration : EntityTypeConfiguration<EmailLog>
    {
        internal EmailLogConfiguration()
        {
            this.HasKey(l => l.Id);

            this.HasOptional(l => l.CaseHistory)
                            .WithMany(l => l.Emaillogs)
                            .HasForeignKey(l => l.CaseHistory_Id)
                            .WillCascadeOnDelete(false);

            this.Property(l => l.EmailLogGUID).IsRequired();
            this.Property(l => l.Log_Id).IsOptional();
            this.Property(l => l.EmailAddress).IsRequired().HasMaxLength(1000);
            this.Property(l => l.MailId).IsRequired();
            this.Property(l => l.MessageId).IsOptional();
            this.Property(l => l.CaseHistory_Id).IsOptional();
            this.Property(l => l.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblEmaillog");
        }
    }
}

