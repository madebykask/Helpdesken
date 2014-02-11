namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypeConfiguration : EntityTypeConfiguration<InventoryType>
    {
        public InventoryTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).HasColumnName("InventoryType").IsRequired().HasMaxLength(50);
            this.Property(x => x.XMLElement).IsRequired().HasMaxLength(100);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblInventoryType");
        }
    }
}