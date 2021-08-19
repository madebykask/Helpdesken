namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerModelConfiguration : EntityTypeConfiguration<ComputerModel>
    {
        public ComputerModelConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("ComputerModel").IsRequired().HasMaxLength(50);
            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblComputerModel");
        }
    }
}