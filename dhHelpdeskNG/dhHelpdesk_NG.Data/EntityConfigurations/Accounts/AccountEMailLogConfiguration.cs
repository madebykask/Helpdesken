namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Accounts;

    public class AccountEMailLogConfiguration : EntityTypeConfiguration<AccountEMailLog>
    {
        internal AccountEMailLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Account)
                .WithMany()
                .HasForeignKey(a => a.Account_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AccountEMailLogGUID).IsRequired();
            this.Property(x => x.EMailAddress).HasMaxLength(1000).IsOptional();
            this.Property(x => x.MailID).IsRequired();
            this.Property(x => x.MessageId).HasMaxLength(100).IsOptional();
            this.Property(x => x.CreatedDate).IsRequired();
        }
    }
}