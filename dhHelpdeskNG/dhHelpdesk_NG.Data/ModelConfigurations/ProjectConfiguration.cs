using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        internal ProjectConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired().HasColumnName("Customer_id");
            Property(x => x.Description).IsRequired().HasMaxLength(2000);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50);
            Property(x => x.ProjectManager).IsOptional();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblproject");
        }
    }
}
