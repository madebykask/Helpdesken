using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class PriorityConfiguration : EntityTypeConfiguration<Priority>
    {
        internal PriorityConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasMany(x => x.PriorityImpactUrgencies)
                .WithRequired(x => x.Priority)
                .HasForeignKey(x => x.Priority_Id);

            HasMany(x => x.PriorityLanguages)
                .WithRequired(x => x.Priority)
                .HasForeignKey(x => x.Priority_Id);

            Property(x => x.Code).IsRequired().HasMaxLength(5).HasColumnName("Priority");
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsRequired().HasMaxLength(200).HasColumnName("PriorityDescription");
            Property(x => x.EMailImportance).IsRequired();
            Property(x => x.EMailList).IsRequired().HasMaxLength(500);
            Property(x => x.FileName).IsRequired().HasMaxLength(100);
            Property(x => x.InformUser).IsRequired();
            Property(x => x.InformUserText).IsOptional().HasMaxLength(500);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.IsEmailDefault).IsRequired().HasColumnName("isEMailDefault");
            Property(x => x.LogText).IsOptional().HasMaxLength(1000);
            Property(x => x.MailID_Change).IsOptional();
            Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("PriorityName");
            Property(x => x.SLA).IsRequired();
            Property(x => x.SMSNotification).IsRequired();
            Property(x => x.SolutionTime).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblpriority");
        }
    }
}
