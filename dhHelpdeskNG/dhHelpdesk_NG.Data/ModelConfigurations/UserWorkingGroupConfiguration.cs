using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class UserWorkingGroupConfiguration : EntityTypeConfiguration<UserWorkingGroup>
    {
        internal UserWorkingGroupConfiguration()
        {
            HasKey(x => new { x.User_Id, x.WorkingGroup_Id });

            Property(x => x.UserRole).IsRequired();

            ToTable("tblUserWorkingGroup");
        }
    }
}
