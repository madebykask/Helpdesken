namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeDepartmentConfiguration : EntityTypeConfiguration<ChangeDepartmentEntity>
    {
        internal ChangeDepartmentConfiguration()
        {
            this.HasKey(cd => new { cd.Change_Id, cd.Department_Id });
            this.Property(cd => cd.Change_Id).IsRequired();
            this.HasRequired(cd => cd.Change).WithMany().HasForeignKey(cd => cd.Change_Id).WillCascadeOnDelete(false);
            this.Property(cd => cd.Department_Id).IsRequired();
            
            this.HasRequired(cd => cd.Department)
                .WithMany()
                .HasForeignKey(cd => cd.Department_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblChange_tblDepartment");
        }
    }
}
