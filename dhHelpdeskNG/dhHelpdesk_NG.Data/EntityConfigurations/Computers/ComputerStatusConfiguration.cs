using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    public class ComputerStatusConfiguration : EntityTypeConfiguration<ComputerStatus>
    {
        public ComputerStatusConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Name).HasColumnName("ComputerStatus").IsRequired().HasMaxLength(50);
            Property(x => x.Type).IsRequired();
            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.ChangedDate).IsRequired();

            ToTable("tblComputerStatus");
        }
    }
}