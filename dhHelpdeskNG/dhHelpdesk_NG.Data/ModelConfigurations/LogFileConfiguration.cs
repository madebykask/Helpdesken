using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class LogFileConfiguration : EntityTypeConfiguration<LogFile>
    {
        internal LogFileConfiguration()
        {
            HasKey(l => l.Id);

            HasRequired(l => l.Log)
                .WithMany(l => l.LogFiles)
                .HasForeignKey(l => l.Log_Id)
                .WillCascadeOnDelete(false);

            Property(l => l.FileName).IsRequired().HasMaxLength(200);
            Property(l => l.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbllogfile");
        }
    }
}
