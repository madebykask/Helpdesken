namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangePriorityConfiguration : EntityTypeConfiguration<ChangePriorityEntity>
    {
        internal ChangePriorityConfiguration()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.ChangePriority).IsRequired().HasMaxLength(50);
            this.Property(p => p.Customer_Id).IsRequired();
            this.HasRequired(p => p.Customer).WithMany().HasForeignKey(p => p.Customer_Id).WillCascadeOnDelete(false);
            this.Property(p => p.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(p => p.ChangedDate).IsRequired();

            this.ToTable("tblchangepriority");
        }
    }
}
