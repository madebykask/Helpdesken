using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        internal SupplierConfiguration() 
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.Country_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            Property(x => x.Country_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();           
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Supplier");
            Property(x => x.SortOrder).IsRequired();
            Property(x => x.SupplierNumber).IsRequired().HasMaxLength(50);
            Property(x => x.SyncChangedDate).IsOptional();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblsupplier");
        }
    }
}
