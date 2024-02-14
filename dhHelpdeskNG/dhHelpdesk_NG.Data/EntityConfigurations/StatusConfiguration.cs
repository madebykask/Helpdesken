namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class StatusConfiguration : EntityTypeConfiguration<Status>
    {
        internal StatusConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.WorkingGroup)
               .WithMany()
               .HasForeignKey(x => x.WorkingGroup_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.StateSecondary)
               .WithMany()
               .HasForeignKey(x => x.StateSecondary_Id)
               .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("StatusName");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.StatusGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.SplitOnSave).IsRequired().HasColumnName("SplitOnSave");

            this.ToTable("tblstatus");
        }
    }
}
