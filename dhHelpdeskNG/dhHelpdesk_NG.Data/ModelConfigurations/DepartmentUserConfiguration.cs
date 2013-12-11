using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class DepartmentUserConfiguration : EntityTypeConfiguration<DepartmentUser>
    {
        internal DepartmentUserConfiguration()
        {
            HasKey(x => new { x.User_Id, x.Department_Id });

            ToTable("tbldepartmentuser");
        }
        
    }
}
