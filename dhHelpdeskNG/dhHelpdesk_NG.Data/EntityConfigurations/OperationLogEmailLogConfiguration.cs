namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OperationLogEmailLogConfiguration : EntityTypeConfiguration<OperationLogEMailLog>
    {
        internal OperationLogEmailLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.OperationLog)
                .WithMany()
                .HasForeignKey(x => x.OperationLog_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Recipients).IsOptional().HasMaxLength(500).HasColumnName("Recipients");
            this.Property(x => x.SMSText).IsRequired().HasMaxLength(500).HasColumnName("SMSText");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblOperationLogEMailLog");
        }
    }
}