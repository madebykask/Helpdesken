using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.EntityConfigurations.Users
{
    internal sealed class UserModuleConfiguration : EntityTypeConfiguration<UserModuleEntity>
    {
        internal UserModuleConfiguration()
        {
            Property(u => u.User_Id).IsRequired();
            Property(u => u.Module_Id).IsRequired();
            Property(u => u.Position).IsRequired();
            Property(u => u.isVisible).IsRequired();
            Property(u => u.NumberOfRows).IsRequired();

            HasRequired(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.User_Id)
                .WillCascadeOnDelete(false);

            HasRequired(u => u.Module)
                .WithMany()
                .HasForeignKey(u => u.Module_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblUsers_tblModule");
        }
    }
}