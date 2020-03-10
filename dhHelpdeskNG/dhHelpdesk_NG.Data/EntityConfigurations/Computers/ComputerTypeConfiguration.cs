namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerTypeConfiguration : EntityTypeConfiguration<ComputerType>
    {
        public ComputerTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(x => x.Name).HasColumnName("ComputerType").IsRequired().HasMaxLength(60);
            this.Property(x => x.ComputerTypeDescription).IsRequired().HasMaxLength(50);
            this.HasRequired(x => x.inventory)
               .WithMany()
               .HasForeignKey(x => x.InventoryType_Id)
               .WillCascadeOnDelete(false);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblComputerType");
        }
    }
}