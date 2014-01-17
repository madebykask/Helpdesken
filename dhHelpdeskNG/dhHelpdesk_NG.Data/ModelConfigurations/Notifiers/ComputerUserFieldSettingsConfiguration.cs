namespace dhHelpdesk_NG.Data.ModelConfigurations.Notifiers
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class ComputerUserFieldSettingsConfiguration : EntityTypeConfiguration<ComputerUserFieldSettings>
    {
        internal ComputerUserFieldSettingsConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.Customer_Id).IsRequired();

            this.HasRequired(c => c.Customer)
                .WithMany(c => c.ComputerUserFieldSettings)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ComputerUserField).IsRequired().HasMaxLength(50);
            this.Property(c => c.Show).IsRequired();
            this.Property(c => c.Required).IsRequired();
            this.Property(c => c.MinLength).IsRequired();
            this.Property(c => c.ShowInList).IsRequired();
            this.Property(c => c.LDAPAttribute).IsRequired().HasMaxLength(50);
            this.Property(c => c.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(c => c.ChangedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblComputerUserFieldSettings");
        }
    }
}
