namespace DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.WorkstationModules;

    public class ProcessorConfiguration : EntityTypeConfiguration<Processor>
    {
        public ProcessorConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("Processor").IsRequired().HasMaxLength(100);
            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblProcessor");
        }
    }
}