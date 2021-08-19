namespace DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.WorkstationModules;

    public class RAMConfiguration : EntityTypeConfiguration<RAM>
    {
        public RAMConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("RAM").IsRequired().HasMaxLength(50);
            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblRAM");
        }
    }
}
