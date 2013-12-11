namespace dhHelpdesk_NG.Data.ModelConfigurations.Notifiers
{
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class ComputerUserFieldSettingsLanguageConfiguration : EntityTypeConfiguration<ComputerUserFieldSettingsLanguage>
    {
        internal ComputerUserFieldSettingsLanguageConfiguration()
        {
            this.HasKey(sl => new { sl.ComputerUserFieldSettings_Id, sl.Language_Id });

            this.Property(sl => sl.ComputerUserFieldSettings_Id).IsRequired();
            this.Property(sl => sl.Language_Id).IsRequired();
            this.Property(sl => sl.Label).IsRequired().HasMaxLength(50);
            this.Property(sl => sl.FieldHelp).IsOptional().HasMaxLength(200);

            this.ToTable("tblComputerUserFS_tblLanguage");
        }
    }
}
