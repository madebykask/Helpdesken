using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class EMailGroupConfiguration : EntityTypeConfiguration<EMailGroup>
    {
        internal EMailGroupConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsOptional();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Members).IsRequired().HasMaxLength(3000);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("EMailGroup");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblemailgroup");
        }
    }
}
