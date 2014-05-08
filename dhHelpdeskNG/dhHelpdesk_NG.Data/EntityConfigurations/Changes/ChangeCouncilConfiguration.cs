namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeCouncilConfiguration : EntityTypeConfiguration<ChangeCouncilEntity>
    {
        internal ChangeCouncilConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.ChangeCouncilGUID).IsRequired();
            this.Property(c => c.Change_Id).IsRequired();
            this.HasRequired(c => c.Change).WithMany().HasForeignKey(c => c.Change_Id).WillCascadeOnDelete(false);
            this.Property(c => c.ChangeCouncilName).IsRequired().HasMaxLength(100);
            this.Property(c => c.ChangeCouncilEmail).IsOptional().HasMaxLength(50);
            this.Property(c => c.ChangeCouncilStatus).IsRequired();
            this.Property(c => c.ChangeCouncilNote).IsOptional();
            this.Property(c => c.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(c => c.ChangedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblChangeCouncil");
        }
    }
}