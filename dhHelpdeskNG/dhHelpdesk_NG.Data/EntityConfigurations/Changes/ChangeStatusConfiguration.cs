namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeStatusConfiguration : EntityTypeConfiguration<ChangeStatusEntity>
    {
        internal ChangeStatusConfiguration()
        {
            this.HasKey(s => s.Id);
            this.Property(s => s.ChangeStatus).IsRequired().HasMaxLength(50);
            this.Property(s => s.isDefault).IsRequired();
            this.Property(s => s.CompletionStatus).IsRequired();
            this.Property(s => s.Customer_Id).IsRequired();
            this.HasRequired(s => s.Customer).WithMany().HasForeignKey(s => s.Customer_Id).WillCascadeOnDelete(false);
            this.Property(s => s.InformOrderer).IsRequired();
            this.Property(s => s.CreatedDate).IsRequired();
            this.Property(s => s.ChangedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblchangestatus");
        }
    }
}
