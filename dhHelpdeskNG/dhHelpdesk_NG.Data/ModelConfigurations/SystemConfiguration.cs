using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class SystemConfiguration : EntityTypeConfiguration<dhHelpdesk_NG.Domain.System>
    {
        internal SystemConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.Supplier_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.SystemOwnerUser)
                .WithMany()
                .HasForeignKey(x => x.SystemOwnerUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.SystemResponsibleUser)
                .WithMany()
                .HasForeignKey(x => x.SystemResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.Urgency_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ViceSystemResponsibleUser)
                .WithMany()
                .HasForeignKey(x => x.ViceSystemResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Domain)
                .WithMany()
                .HasForeignKey(x => x.Domain_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Info).IsOptional().HasMaxLength(500);
            Property(x => x.Owner).IsOptional().HasMaxLength(50);
            Property(x => x.Priority).IsRequired();
            Property(x => x.Supplier_Id).IsOptional();
            Property(x => x.Domain_Id).IsOptional();
            Property(x => x.SystemAdministratorName).IsOptional().HasMaxLength(50);
            Property(x => x.SystemAdministratorPhone).IsOptional().HasMaxLength(50);
            Property(x => x.SystemName).IsRequired().HasMaxLength(50);
            Property(x => x.SystemOwnerUser_Id).IsOptional();
            Property(x => x.SystemOwnerUserId).IsOptional().HasMaxLength(50);
            Property(x => x.SystemResponsibleUser_Id).IsOptional();
            Property(x => x.Urgency_Id).IsOptional();
            Property(x => x.ViceSystemResponsibleUser_Id).IsOptional();
            Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            Property(x => x.ContactEMail).IsOptional().HasMaxLength(50);
            Property(x => x.ContactPhone).IsOptional().HasMaxLength(50);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblsystem");
        }
    }
}
