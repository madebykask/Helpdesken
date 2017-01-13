namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Accounts;

    public class AccountActivityGroupConfiguration : EntityTypeConfiguration<AccountActivityGroup>
    {
        internal AccountActivityGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("AccountActivityGroup");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.AccountActivityGroupGUID).IsOptional();

            this.ToTable("tblaccountactivitygroup");
        }
    }
}