using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class InfoTextConfiguration : EntityTypeConfiguration<InfoText>
    {
        internal InfoTextConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsOptional();
            Property(x => x.Language_Id).IsOptional();
            Property(x => x.Name).IsRequired().HasColumnName("InfoText");
            Property(x => x.Type).IsOptional().HasColumnName("InfoTextType");
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblinfotext");
        }
    }
}
