namespace DH.Helpdesk.Dal.EntityConfigurations.Inventory
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Inventory;

    public class InventoryConfiguration : EntityTypeConfiguration<Inventory>
    {
        public InventoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.InventoryType)
                .WithMany()
                .HasForeignKey(x => x.InventoryType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Room)
                .WithMany()
                .HasForeignKey(x => x.Room_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.InventoryName).IsRequired().HasMaxLength(60);
            this.Property(x => x.InventoryModel).IsOptional().HasMaxLength(100);
            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(50);
            this.Property(x => x.SerialNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.TheftMark).IsRequired().HasMaxLength(20);
            this.Property(x => x.BarCode).IsRequired().HasMaxLength(20);
            this.Property(x => x.PurchaseDate).IsOptional();
            this.Property(x => x.Info).IsOptional().HasMaxLength(1000);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();
            this.Property(x => x.SyncChangedDate).IsOptional();

            this.ToTable("tblInventory");
        }
    }
}