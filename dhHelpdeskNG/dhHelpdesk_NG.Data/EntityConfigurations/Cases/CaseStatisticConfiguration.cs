namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    internal sealed class CaseStatisticConfiguration : EntityTypeConfiguration<CaseStatistic>
    {
        internal CaseStatisticConfiguration()
        {
            this.HasKey(it => it.Id);
            this.Property(it => it.Id).HasColumnName("Id");
            this.Property(it => it.CaseId).HasColumnName("Case_Id");
            this.Property(it => it.WasSolvedInTime).IsOptional();

            this.ToTable("tblCaseStatistics");
        }
    }
}
