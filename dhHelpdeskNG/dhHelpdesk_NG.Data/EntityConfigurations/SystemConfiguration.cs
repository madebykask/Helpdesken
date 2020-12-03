namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class SystemConfiguration : EntityTypeConfiguration<System>
    {
        internal SystemConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.Supplier_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.SystemOwnerUser)
                .WithMany()
                .HasForeignKey(x => x.SystemOwnerUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.SystemResponsibleUser)
                .WithMany()
                .HasForeignKey(x => x.SystemResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.Urgency_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ViceSystemResponsibleUser)
                .WithMany()
                .HasForeignKey(x => x.ViceSystemResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Domain)
                .WithMany()
                .HasForeignKey(x => x.Domain_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Info).IsOptional().HasMaxLength(500);
            this.Property(x => x.Owner).IsOptional().HasMaxLength(50);
            this.Property(x => x.Priority).IsRequired();
            this.Property(x => x.Supplier_Id).IsOptional();
            this.Property(x => x.Domain_Id).IsOptional();
            this.Property(x => x.SystemAdministratorName).IsOptional().HasMaxLength(50);
            this.Property(x => x.SystemAdministratorPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.SystemName).IsRequired().HasMaxLength(50);
            this.Property(x => x.SystemOwnerUser_Id).IsOptional();
            this.Property(x => x.SystemOwnerUserId).IsOptional().HasMaxLength(50);
            this.Property(x => x.SystemResponsibleUser_Id).IsOptional();
            this.Property(x => x.Urgency_Id).IsOptional();
            this.Property(x => x.Status).IsRequired();
            this.Property(x => x.ViceSystemResponsibleUser_Id).IsOptional();
            this.Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactEMail).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblsystem");
        }
    }
}
