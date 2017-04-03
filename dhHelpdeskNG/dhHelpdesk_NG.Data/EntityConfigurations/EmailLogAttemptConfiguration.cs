using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
	public class EmailLogAttemptConfiguration : EntityTypeConfiguration<EmailLogAttempt>
	{
		internal EmailLogAttemptConfiguration()
		{
			this.HasKey(l => l.Id);
			this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.HasRequired(l => l.EmailLog)
							.WithMany(l => l.EmailLogAttempts)
							.HasForeignKey(l => l.EmailLog_Id)
							.WillCascadeOnDelete(false);

			this.Property(l => l.Message).IsOptional();

			this.ToTable("tblEmailLogAttempts");
		}
	}
}
