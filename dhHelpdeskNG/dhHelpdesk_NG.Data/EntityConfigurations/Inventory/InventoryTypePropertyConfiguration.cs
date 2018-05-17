namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypePropertyConfiguration : EntityTypeConfiguration<InventoryTypeProperty>
    {
        public InventoryTypePropertyConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.InventoryType)
                .WithMany()
                .HasForeignKey(x => x.InventoryType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.InventoryTypeGroup)
                .WithMany()
                .HasForeignKey(x => x.InventoryTypeGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.PropertyValue).IsRequired().HasMaxLength(50);
            this.Property(x => x.PropertyPos).IsRequired();
            this.Property(x => x.PropertyDefault).IsRequired().HasMaxLength(50);
            this.Property(x => x.PropertyType).IsRequired();
            this.Property(x => x.PropertySize).IsRequired();
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.ShowInList).IsRequired();
            this.Property(x => x.PropertyPos).IsRequired();
            this.Property(x => x.XMLTag).IsOptional().HasMaxLength(200);
            this.Property(x => x.ReadOnly).IsRequired();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblInventoryTypeProperty");
        }
    }
}