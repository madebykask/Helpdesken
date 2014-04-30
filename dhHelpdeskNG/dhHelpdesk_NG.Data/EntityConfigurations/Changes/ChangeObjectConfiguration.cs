namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeObjectConfiguration : EntityTypeConfiguration<ChangeObjectEntity>
    {
        internal ChangeObjectConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.ChangeObject).IsRequired().HasMaxLength(50);
            this.Property(o => o.Customer_Id).IsRequired();
            this.HasRequired(o => o.Customer).WithMany().HasForeignKey(o => o.Customer_Id).WillCascadeOnDelete(false);
            this.Property(o => o.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //this.Property(o => o.ChangedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblchangeobject");
        }
    }
}
