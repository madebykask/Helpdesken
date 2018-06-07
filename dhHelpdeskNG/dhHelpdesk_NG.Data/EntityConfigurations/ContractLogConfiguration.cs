using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class ContractLogConfiguration : EntityTypeConfiguration<ContractLog>
    {
        internal ContractLogConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Case_Id).IsOptional();
            Property(x => x.Contract_Id).IsRequired();
            Property(x => x.Email).HasMaxLength(50).IsOptional();
            Property(x => x.LogType).IsRequired();
            Property(x => x.CreatedDate).IsRequired();

            HasRequired(x => x.Contract)
                .WithMany(x => x.ContractLogs)
                .HasForeignKey(x => x.Contract_Id);

            HasOptional(x => x.Case)
                .WithMany()
                .HasForeignKey(x => x.Case_Id);

            ToTable("tblContractLog");

        }
    }
}
