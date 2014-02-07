namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeGroupConfiguration : EntityTypeConfiguration<ChangeGroupEntity>
    {
        internal ChangeGroupConfiguration()
        {
            this.HasKey(g => g.Id);
            this.Property(g => g.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(g => g.ChangeGroup).IsRequired().HasMaxLength(50);
            this.Property(g => g.Customer_Id).IsRequired();
            this.Property(g => g.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(g => g.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblchangegroup");
        }
    }
}
