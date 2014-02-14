namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerHistoryConfiguration : EntityTypeConfiguration<ComputerHistory>
    {
        public ComputerHistoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Computer)
                .WithMany()
                .HasForeignKey(x => x.Computer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.UserId).IsOptional().HasMaxLength(50);
            this.Property(x => x.CreatedDate).IsRequired();

            this.ToTable("tblComputer_History");
        }
    }
}