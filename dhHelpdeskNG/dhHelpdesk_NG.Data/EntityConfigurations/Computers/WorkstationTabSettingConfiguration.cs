using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    public class WorkstationTabSettingConfiguration: EntityTypeConfiguration<WorkstationTabSetting>
    {
        internal WorkstationTabSettingConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.TabField).IsRequired().HasMaxLength(50);

            Property(x => x.Show).IsRequired();
            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.ChangedDate).IsRequired();

            ToTable("tblWorkstationTabSettings");
        }
    }
}