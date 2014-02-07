namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeContactConfiguration : EntityTypeConfiguration<ChangeContactEntity>
    {
        internal ChangeContactConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.Change_Id).IsRequired();
            this.HasRequired(c => c.Change).WithMany().HasForeignKey(c => c.Change_Id).WillCascadeOnDelete(false);
            this.Property(c => c.ContactName).IsOptional().HasMaxLength(50);
            this.Property(c => c.ContactPhone).IsOptional().HasMaxLength(50);
            this.Property(c => c.ContactEMail).IsOptional().HasMaxLength(50);
            this.Property(c => c.ContactCompany).IsOptional().HasMaxLength(50);
            this.Property(c => c.CreatedDate).IsRequired();
            this.Property(c => c.ChangedDate).IsRequired();

            this.ToTable("tblChangeContact");
        }
    }
}