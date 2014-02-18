namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerInventoryConfiguration : EntityTypeConfiguration<ComputerInventory>
    {
        public ComputerInventoryConfiguration()
        {
            this.Property(x => x.Computer_Id).IsRequired();

            this.Property(x => x.Inventory_Id).IsRequired();

            this.ToTable("tblComputer_tblInventory");
        }
    }
}