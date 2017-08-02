using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    public class CaseSectionFieldConfiguration : EntityTypeConfiguration<CaseSectionField>
    {
        internal CaseSectionFieldConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CaseFieldSetting_Id).IsRequired();
            this.Property(x => x.CaseSection_Id).IsRequired();

            this.HasRequired(x => x.CaseSection)
                .WithMany(x => x.CaseSectionFields)
                .HasForeignKey(x => x.CaseSection_Id)
                .WillCascadeOnDelete(false);
            this.HasRequired(x => x.CaseFieldSetting)
                .WithMany()
                .HasForeignKey(x => x.CaseFieldSetting_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseSectionFields");
        }
    }
}
