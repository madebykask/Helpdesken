using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    public class CaseExtraFollowerConfiguration : EntityTypeConfiguration<CaseExtraFollower>
    {
        internal CaseExtraFollowerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Follower).IsRequired();

            this.HasRequired(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Case)
                .WithMany()
                .HasForeignKey(x => x.CaseId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseExtraFollowers");
        }
    }
}
