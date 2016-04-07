namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class PriorityConfiguration : EntityTypeConfiguration<Priority>
    {
        internal PriorityConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.PriorityImpactUrgencies)
                .WithRequired(x => x.Priority)
                .HasForeignKey(x => x.Priority_Id);

            this.HasMany(x => x.PriorityLanguages)
                .WithRequired(x => x.Priority)
                .HasForeignKey(x => x.Priority_Id);

            this.Property(x => x.Code).IsRequired().HasMaxLength(5).HasColumnName("Priority");
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsRequired().HasMaxLength(200).HasColumnName("PriorityDescription");
            this.Property(x => x.EMailImportance).IsRequired();
            this.Property(x => x.EMailList).IsRequired().HasMaxLength(500);
            this.Property(x => x.FileName).IsRequired().HasMaxLength(100);
            this.Property(x => x.InformUser).IsRequired();
            this.Property(x => x.InformUserText).IsOptional().HasMaxLength(500);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.IsEmailDefault).IsRequired().HasColumnName("isEMailDefault");
            this.Property(x => x.LogText).IsOptional().HasMaxLength(1000);
            this.Property(x => x.MailID_Change).IsOptional();
            this.Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("PriorityName");
            this.Property(x => x.SLA).IsRequired();
            this.Property(x => x.SMSNotification).IsRequired();
            this.Property(x => x.SolutionTime).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.OrderNum).IsOptional();

            this.ToTable("tblpriority");
        }
    }
}
