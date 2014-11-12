namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Accounts;

    public class AccountTypeConfiguration : EntityTypeConfiguration<AccountType>
    {
        internal AccountTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.AccountActivity)
                .WithMany()
                .HasForeignKey(a => a.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).HasColumnName("AccountType").HasMaxLength(50).IsRequired();
            this.Property(x => x.AccountField).IsRequired();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();
        }
    }
}