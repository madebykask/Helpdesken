namespace dhHelpdesk_NG.Data.ModelConfigurations.Changes
{
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeDepartmentConfiguration : EntityTypeConfiguration<ChangeDepartmentEntity>
    {
        internal ChangeDepartmentConfiguration()
        {
            this.HasKey(d => new { d.Change_Id, d.Department_Id });

            this.Property(d => d.Change_Id).IsRequired();
            this.Property(d => d.Department_Id).IsRequired();

            this.ToTable("tblChange_tblDepartment");
        }
    }
}
