namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerInventoryConfiguration : EntityTypeConfiguration<ComputerInventory>
    {
        public ComputerInventoryConfiguration()
        {
            this.HasRequired(x => x.Computer)
                .WithMany()
                .HasForeignKey(x => x.Computer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Inventory_Id).IsRequired();

            this.ToTable("tblComputer_tblInventory");
        }
    }
}