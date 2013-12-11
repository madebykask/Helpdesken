using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ProductAreaConfiguration : EntityTypeConfiguration<ProductArea>
    {
        internal ProductAreaConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.MailTemplate)
                .WithMany()
                .HasForeignKey(x => x.MailID)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ParentProductArea)
                .WithMany(x => x.SubProductAreas)
                .HasForeignKey(x => x.Parent_ProductArea_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsOptional().HasMaxLength(300);
            Property(x => x.InformUserText).IsOptional().HasMaxLength(300);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.MailID).IsOptional();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ProductArea");
            Property(x => x.Parent_ProductArea_Id).IsOptional();
            Property(x => x.WorkingGroup_Id).IsOptional();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblproductarea");
        }
    }
}
