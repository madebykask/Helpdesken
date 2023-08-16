namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class StateSecondaryConfiguration : EntityTypeConfiguration<StateSecondary>
    {
        internal StateSecondaryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            this.HasOptional(x => x.WorkingGroup)
               .WithMany()
               .HasForeignKey(x => x.WorkingGroup_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.MailTemplate)
                .WithMany()
                .HasForeignKey(x => x.MailTemplate_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FinishingCause)
                .WithMany()
                .HasForeignKey(x => x.FinishingCause_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IncludeInCaseStatistics).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("StateSecondary");
            this.Property(x => x.NoMailToNotifier).IsRequired();
            this.Property(x => x.ResetOnExternalUpdate).IsRequired();
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.Property(x => x.MailTemplate_Id).IsOptional();
            this.Property(x => x.ReminderDays).IsOptional();
            this.Property(x => x.StateSecondaryGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(x => x.AlternativeStateSecondaryName).IsOptional().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.StateSecondaryId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblstatesecondary");
        }
    }
}