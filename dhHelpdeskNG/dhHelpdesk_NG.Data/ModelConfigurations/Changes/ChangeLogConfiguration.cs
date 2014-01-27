namespace dhHelpdesk_NG.Data.ModelConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeLogConfiguration : EntityTypeConfiguration<ChangeLogEntity>
    {
        internal ChangeLogConfiguration()
        {
            this.HasKey(l => l.Id);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.LogText).IsOptional();
            this.Property(l => l.Change_Id).IsRequired();
            this.HasRequired(l => l.Change).WithMany().HasForeignKey(l => l.Change_Id).WillCascadeOnDelete(false);
            this.Property(l => l.ChangePart).IsRequired();
            this.Property(l => l.ChangeEMailLog_Id).IsOptional();

            this.HasOptional(l => l.ChangeEMailLog)
                .WithMany()
                .HasForeignKey(l => l.ChangeEMailLog_Id)
                .WillCascadeOnDelete(false);

            this.Property(l => l.ChangeHistory_Id).IsOptional();

            this.HasOptional(l => l.ChangeHistory)
                .WithMany()
                .HasForeignKey(l => l.ChangeHistory_Id)
                .WillCascadeOnDelete(false);

            this.Property(l => l.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.CreatedByUser_Id).IsOptional();

            this.ToTable("tblChangeLog");
        }
    }
}