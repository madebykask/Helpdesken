namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UserGroupConfiguration : EntityTypeConfiguration<UserGroup>
    {
        internal UserGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("UserGroup");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblusergroups");
        }
    }
}
