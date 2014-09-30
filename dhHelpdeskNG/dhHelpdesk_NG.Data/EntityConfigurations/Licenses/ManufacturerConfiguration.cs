namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class ManufacturerConfiguration : EntityTypeConfiguration<Manufacturer>
    {
        internal ManufacturerConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Name).HasColumnName("Manufacturer").IsRequired().HasMaxLength(100);
            this.Property(m => m.ChangedDate).IsRequired();
            this.Property(m => m.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); 

            this.HasRequired(m => m.Customer)
                .WithMany()
                .HasForeignKey(m => m.Customer_Id);

            this.ToTable("tblManufcturer");       
        }
    }
}