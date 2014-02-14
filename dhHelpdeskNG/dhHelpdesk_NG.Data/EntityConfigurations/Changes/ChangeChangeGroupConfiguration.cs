namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeChangeGroupConfiguration : EntityTypeConfiguration<ChangeChangeGroupEntity>
    {
        internal ChangeChangeGroupConfiguration()
        {
            this.HasKey(cg => new { cg.Change_Id, cg.ChangeGroup_Id });
            this.Property(cg => cg.Change_Id).IsRequired();
            this.HasRequired(cg => cg.Change).WithMany().HasForeignKey(cg => cg.Change_Id).WillCascadeOnDelete(false);
            this.Property(cg => cg.ChangeGroup_Id).IsRequired();

            this.HasRequired(cg => cg.ChangeGroup)
                .WithMany()
                .HasForeignKey(cg => cg.ChangeGroup_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblChange_tblChangeGroup");
        }
    }
}
