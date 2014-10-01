namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        internal ProductConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Name).HasColumnName("Product").HasMaxLength(50);
            this.Property(p => p.ChangedDate).IsRequired();
            this.Property(p => p.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); 

            this.HasRequired(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.Customer_Id);

            this.HasRequired(p => p.Manufacturer)
                .WithMany()
                .HasForeignKey(p => p.Manufacturer_Id);

            this.HasMany(p => p.Licenses)
                .WithRequired(l => l.Product);

            this.HasMany(p => p.Applications)
                .WithMany(a => a.Products)
                .Map(m =>
                        {
                            m.MapLeftKey("Product_Id");
                            m.MapRightKey("Application_Id");
                            m.ToTable("tblProduct_tblApplication");
                        });

            this.ToTable("tblProduct");       
        }
    }
}