using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class FloorConfiguration : EntityTypeConfiguration<Floor>
    {
        internal FloorConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(f => f.Building)
                .WithMany()
                .HasForeignKey(f => f.Building_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Building_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Floor");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            ToTable("tblfloor");
        }
    }
}
