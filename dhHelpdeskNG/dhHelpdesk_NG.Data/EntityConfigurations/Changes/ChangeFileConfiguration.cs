namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeFileConfiguration : EntityTypeConfiguration<ChangeFileEntity>
    {
        internal ChangeFileConfiguration()
        {
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.ChangeFileGUID).IsRequired();
            this.Property(f => f.FileName).IsRequired().HasMaxLength(100);
            this.Property(f => f.ContentType).IsRequired().HasMaxLength(100);
            this.Property(f => f.ChangeFile).IsOptional();
            this.Property(f => f.Change_Id).IsRequired();
            this.HasRequired(f => f.Change).WithMany().HasForeignKey(f => f.Change_Id).WillCascadeOnDelete(false);
            this.Property(f => f.ChangeArea).IsRequired();
            this.Property(f => f.CreatedDate);

            this.ToTable("tblChangeFile");
        }
    }
}