using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    public class CaseFollowUpConfiguration : EntityTypeConfiguration<CaseFollowUp>
    {
        internal CaseFollowUpConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.FollowUpDate).IsRequired();
            this.Property(x => x.IsActive).IsRequired();

            this.HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Case)
                .WithMany()
                .HasForeignKey(x => x.Case_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseFollowUps");
        }
    }
}
