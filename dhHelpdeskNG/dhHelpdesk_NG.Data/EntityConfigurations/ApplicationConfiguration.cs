namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        internal ApplicationConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("Application");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblapplication");
        }
    }
}
