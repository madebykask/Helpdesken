namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class DepartmentUserConfiguration : EntityTypeConfiguration<DepartmentUser>
    {
        internal DepartmentUserConfiguration()
        {
            this.HasKey(x => new { x.User_Id, x.Department_Id });

            this.ToTable("tbldepartmentuser");
        }
        
    }
}
