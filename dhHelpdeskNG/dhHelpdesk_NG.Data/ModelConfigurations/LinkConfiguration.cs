using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class LinkConfiguration : EntityTypeConfiguration<Link>
    {
        internal LinkConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Customer)
              .WithMany()
              .HasForeignKey(x => x.Customer_Id)
              .WillCascadeOnDelete(false);

            HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsOptional();
            Property(x => x.Document_Id).IsOptional();
            Property(x => x.OpenInNewWindow).IsRequired();
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.URLAddress).IsRequired().HasMaxLength(100);
            Property(x => x.URLName).IsRequired().HasMaxLength(50);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbllink");
        }
    }
}
