namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerLogConfiguration : EntityTypeConfiguration<ComputerLog>
    {
        public ComputerLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Computer)
                .WithMany()
                .HasForeignKey(x => x.Computer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ComputerLogCategory).IsRequired().HasMaxLength(50);
            this.Property(x => x.ComputerLogText).IsRequired().HasMaxLength(200);
            this.Property(x => x.CreatedDate).IsRequired();

            this.ToTable("tblComputerLog");
        }
    }
}