namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeChangeConfiguration : EntityTypeConfiguration<ChangeChangeEntity>
    {
        internal ChangeChangeConfiguration()
        {
            this.HasKey(cc => new { cc.Change_Id, cc.RelatedChange_Id });
            this.HasRequired(cc => cc.Change).WithMany().HasForeignKey(cc => cc.Change_Id).WillCascadeOnDelete(false);
            
            this.HasRequired(cc => cc.RelatedChange)
                .WithMany()
                .HasForeignKey(cc => cc.RelatedChange_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblChange_tblChange");
        }
    }
}
