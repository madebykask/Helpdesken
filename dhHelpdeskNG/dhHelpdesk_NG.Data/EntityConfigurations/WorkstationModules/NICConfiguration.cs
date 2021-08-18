namespace DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.WorkstationModules;

    public class NICConfiguration : EntityTypeConfiguration<NIC>
    {
        public NICConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("NIC").IsRequired().HasMaxLength(200);
            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblNIC");
        }
    }
}