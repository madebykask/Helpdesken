namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class MergedCasesConfiguration : EntityTypeConfiguration<MergedCases>
    {
        internal MergedCasesConfiguration()
        {
            this.HasKey(it => new { it.MergedParentId, it.MergedChildId });
            this.Property(it => it.MergedParentId).HasColumnName("MergedParent_Id");
            this.Property(it => it.MergedChildId).HasColumnName("MergedChild_Id");
            this.ToTable("tblMergedCases");
        }
    }
}
