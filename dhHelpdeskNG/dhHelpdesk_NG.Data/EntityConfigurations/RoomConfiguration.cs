namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class RoomConfiguration : EntityTypeConfiguration<Room>
    {
        internal RoomConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(f => f.Floor)
                .WithMany()
                .HasForeignKey(f => f.Floor_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Floor_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Room");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblroom");
        }
    }
}
