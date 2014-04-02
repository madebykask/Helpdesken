using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.EntityConfigurations.Users
{
    internal sealed class ModuleConfiguration : EntityTypeConfiguration<ModuleEntity>
    {
        internal ModuleConfiguration()
        {
            HasKey(m => m.Id);
            Property(m => m.Name).HasMaxLength(50);
            Property(m => m.Description).HasMaxLength(500);

            ToTable("tblModule");
        }
    }
}