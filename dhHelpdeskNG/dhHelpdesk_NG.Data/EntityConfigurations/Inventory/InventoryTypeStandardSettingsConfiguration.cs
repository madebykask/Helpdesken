using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Inventory;

namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    public class InventoryTypeStandardSettingsConfiguration : EntityTypeConfiguration<InventoryTypeStandardSettings>
    {
        public InventoryTypeStandardSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(x => x.ShowServers).IsRequired();
            this.Property(x => x.ShowPrinters).IsRequired();
            this.Property(x => x.ShowWorkstations).IsRequired();

            this.ToTable("tblInventoryTypeStandardSettings");
        }
    }
}