using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations
{    
    using DH.Helpdesk.Domain;

    class RegistrationSourceCustomerConfiguration : EntityTypeConfiguration<RegistrationSourceCustomer>
    {
        internal RegistrationSourceCustomerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("IsActive");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("IsDefault");
            this.Property(x => x.SourceName).IsRequired().HasMaxLength(50).HasColumnName("SourceName");
            this.Property(x => x.SystemCode).IsRequired();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblregistrationsourcecustomer");
        }
    }
}
