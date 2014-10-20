namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class VendorConfiguration : EntityTypeConfiguration<Vendor>
    {
        internal VendorConfiguration()
        {
            this.HasKey(v => v.Id);
            this.Property(v => v.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(v => v.Name).HasColumnName("VendorName").IsRequired().HasMaxLength(50);
            this.Property(v => v.Contact).IsRequired().HasMaxLength(50);
            this.Property(v => v.Address).IsRequired().HasMaxLength(50);
            this.Property(v => v.PostalCode).IsRequired().HasMaxLength(10);
            this.Property(v => v.PostalAddress).IsRequired().HasMaxLength(50);
            this.Property(v => v.Phone).IsRequired().HasMaxLength(50);
            this.Property(v => v.EMail).IsRequired().HasMaxLength(50);
            this.Property(v => v.HomePage).IsRequired().HasMaxLength(50);
            this.Property(v => v.ChangedDate).IsRequired();
            this.Property(v => v.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasRequired(v => v.Customer)
                .WithMany()
                .HasForeignKey(v => v.Customer_Id);

            this.ToTable("tblVendor");       
        }
    }
}