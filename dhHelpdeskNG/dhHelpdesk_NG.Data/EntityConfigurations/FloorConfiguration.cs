namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FloorConfiguration : EntityTypeConfiguration<Floor>
    {
        internal FloorConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(f => f.Building)
                .WithMany()
                .HasForeignKey(f => f.Building_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Building_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Floor");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            this.ToTable("tblfloor");
        }
    }
}
