namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LogFileConfiguration : EntityTypeConfiguration<LogFile>
    {
        internal LogFileConfiguration()
        {
            this.HasKey(l => l.Id);

            this.HasRequired(l => l.Log)
                .WithMany(l => l.LogFiles)
                .HasForeignKey(l => l.Log_Id)
                .WillCascadeOnDelete(false);

            this.Property(l => l.FileName).IsRequired().HasMaxLength(200);
            this.Property(l => l.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.IsCaseFile).IsOptional();
            this.Property(l => l.ParentLog_Id).IsOptional();

            this.ToTable("tbllogfile");
        }
    }
}
