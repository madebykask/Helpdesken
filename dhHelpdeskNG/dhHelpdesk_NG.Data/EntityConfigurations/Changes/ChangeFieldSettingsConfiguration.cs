namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeFieldSettingsConfiguration : EntityTypeConfiguration<ChangeFieldSettingsEntity>
    {
        internal ChangeFieldSettingsConfiguration()
        {
            this.HasKey(s => s.Id);
            this.Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(s => s.Customer).WithMany().HasForeignKey(s => s.Customer_Id).WillCascadeOnDelete(false);
            this.Property(s => s.ChangeField).IsRequired().HasMaxLength(50);
            this.Property(s => s.Show).IsRequired();
            this.Property(s => s.ShowInList).IsRequired();
            this.Property(s => s.ShowExternal).IsRequired();
            this.Property(s => s.Label).IsRequired().HasMaxLength(50);
            this.Property(s => s.Label_ENG).IsRequired().HasMaxLength(50);
            this.Property(s => s.FieldHelp).IsOptional().HasMaxLength(200);
            this.Property(s => s.Required).IsRequired();
            this.Property(s => s.InitialValue).IsOptional().HasMaxLength(1000);
            this.Property(s => s.Bookmark).IsOptional().HasMaxLength(100);
            this.Property(s => s.CreatedDate).IsRequired();
            this.Property(s => s.ChangedDate).IsRequired();

            this.ToTable("tblChangeFieldSettings");
        }
    }
}
