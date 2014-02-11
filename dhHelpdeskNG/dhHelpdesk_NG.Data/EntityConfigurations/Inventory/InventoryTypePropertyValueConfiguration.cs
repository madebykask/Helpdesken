namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypePropertyValueConfiguration : EntityTypeConfiguration<InventoryTypePropertyValue>
    {
        public InventoryTypePropertyValueConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Inventory)
                .WithMany()
                .HasForeignKey(x => x.Inventory_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.InventoryTypeProperty)
                .WithMany()
                .HasForeignKey(x => x.InventoryTypeProperty_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Value).HasColumnName("InventoryTypePropertyValue").HasMaxLength(2000).IsOptional();

            this.ToTable("tblInventoryTypePropertyValue");
        }
    }
}