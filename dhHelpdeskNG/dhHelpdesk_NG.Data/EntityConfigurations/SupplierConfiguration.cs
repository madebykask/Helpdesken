namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        internal SupplierConfiguration() 
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.Country_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            this.Property(x => x.Country_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();           
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Supplier");
            this.Property(x => x.SortOrder).IsRequired();
            this.Property(x => x.SupplierNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.SyncChangedDate).IsOptional();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblsupplier");
        }
    }
}
