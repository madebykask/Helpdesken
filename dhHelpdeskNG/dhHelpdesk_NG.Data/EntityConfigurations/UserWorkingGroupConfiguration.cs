namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UserWorkingGroupConfiguration : EntityTypeConfiguration<UserWorkingGroup>
    {
        internal UserWorkingGroupConfiguration()
        {
            this.HasKey(x => new { x.User_Id, x.WorkingGroup_Id });

            this.Property(x => x.UserRole).IsRequired();
            this.Property(x => x.IsDefault).IsRequired();
            this.Property(x => x.IsMemberOfGroup).IsRequired();

            this.ToTable("tblUserWorkingGroup");
        }
    }
}
