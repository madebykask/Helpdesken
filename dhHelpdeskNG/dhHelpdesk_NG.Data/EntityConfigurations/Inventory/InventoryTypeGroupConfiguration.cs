namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypeGroupConfiguration : EntityTypeConfiguration<InventoryTypeGroup>
    {
        public InventoryTypeGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.InventoryType)
                .WithMany()
                .HasForeignKey(x => x.InventoryType_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).HasColumnName("InventoryTypeGroup").IsRequired().HasMaxLength(60);
            this.Property(x => x.SortOrder).IsRequired();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblInventoryTypeGroup");
        }
    }
}