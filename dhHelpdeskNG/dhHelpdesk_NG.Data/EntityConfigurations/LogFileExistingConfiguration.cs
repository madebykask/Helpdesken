using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class LogFileExistingConfiguration : EntityTypeConfiguration<LogFileExisting>
    {
        internal LogFileExistingConfiguration()
        {
            this.HasKey(l => l.Id);

            this.Property(l => l.FileName).IsRequired().HasMaxLength(200);
            this.Property(l => l.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblLogFileExisting");
        }
    }
}
