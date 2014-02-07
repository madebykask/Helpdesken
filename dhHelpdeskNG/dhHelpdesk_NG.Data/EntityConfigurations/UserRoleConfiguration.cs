namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        internal UserRoleConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Description).IsRequired().HasMaxLength(50).HasColumnName("UserRoleName");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("UserRole");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblUserRole");
        }
    }
}
