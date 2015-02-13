namespace DH.Helpdesk.Dal.EntityConfigurations.Users
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UsersPasswordHistoryConfiguration : EntityTypeConfiguration<UsersPasswordHistory>
    {
        internal UsersPasswordHistoryConfiguration()
        {
            this.HasKey(h => h.Id);
            this.Property(h => h.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(h => h.User_Id).IsRequired();
            this.Property(h => h.Password).IsRequired().HasMaxLength(200);
            this.Property(h => h.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblUsersPasswordHistory");
        }
    }
}