﻿namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class RegistrationSourceCustomerConfiguration : EntityTypeConfiguration<RegistrationSourceCustomer>
    {
        internal RegistrationSourceCustomerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("IsActive");
            this.Property(x => x.SourceName).IsRequired().HasMaxLength(50).HasColumnName("SourceName");
            this.Property(x => x.SystemCode).IsOptional();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblregistrationsourcecustomer");
        }
    }
}
