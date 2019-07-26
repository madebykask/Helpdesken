using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class LogFileExistingConfiguration : EntityTypeConfiguration<LogFileExisting>
    {
        internal LogFileExistingConfiguration()
        {
            HasKey(l => l.Id);
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(l => l.FileName).IsRequired().HasMaxLength(200);
            Property(l => l.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(l => l.LogType).IsRequired();
            Property(l => l.IsInternalLogNote).IsRequired();
            
            ToTable("tblLogFileExisting");
        }
    }
}
