using DH.Helpdesk.Domain.Computers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    public sealed class ComputerUserCategoryConfiguration : EntityTypeConfiguration<ComputerUserCategory>
    {
        internal ComputerUserCategoryConfiguration()
        {
            this.HasKey(o => o.ID);
            this.Property(o => o.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.CaseSolutionID).IsOptional();
            this.Property(o => o.CustomerID).IsRequired();
            this.Property(o => o.ComputerUsersCategoryGuid).IsRequired();
            this.Property(o => o.Name).IsRequired();
            this.Property(o => o.IsReadOnly).IsRequired();
            this.Property(o => o.ExtendedCaseFormID).IsOptional();
            this.Property(o => o.IsEmpty).IsOptional();

            this.HasOptional(o => o.CaseSolution)
                .WithMany()
                .HasForeignKey(o => o.CaseSolutionID)
                .WillCascadeOnDelete(false);

            this.HasRequired(o => o.Customer).WithMany().HasForeignKey(o => o.CustomerID).WillCascadeOnDelete(false);

            this.HasOptional(o => o.ExtendedCaseForm)
                .WithMany()
                .HasForeignKey(o => o.ExtendedCaseFormID)
                .WillCascadeOnDelete(false);

            this.ToTable("tblComputerUsersCategory");
        }
    }
}
