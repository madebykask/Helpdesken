namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeEmailLogConfiguration : EntityTypeConfiguration<ChangeEmailLogEntity>
    {
        internal ChangeEmailLogConfiguration()
        {
            this.HasKey(l => l.Id);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.ChangeEMailLogGUID).IsRequired();
            this.Property(l => l.ChangeHistory_Id).IsRequired();

            this.HasRequired(l => l.ChangeHistory)
                .WithMany()
                .HasForeignKey(l => l.ChangeHistory_Id)
                .WillCascadeOnDelete(false);

            this.Property(l => l.EMailAddress).IsOptional().HasMaxLength(1000);
            this.Property(l => l.MailID).IsRequired();
            this.Property(l => l.MessageId).IsOptional().HasMaxLength(100);
            this.Property(l => l.CreatedDate).IsRequired();

            this.ToTable("tblChangeEMailLog");
        }
    }
}