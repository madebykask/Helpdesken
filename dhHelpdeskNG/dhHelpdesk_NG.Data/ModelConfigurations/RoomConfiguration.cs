using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class RoomConfiguration : EntityTypeConfiguration<Room>
    {
        internal RoomConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(f => f.Floor)
                .WithMany()
                .HasForeignKey(f => f.Floor_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Floor_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Room");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblroom");
        }
    }
}
