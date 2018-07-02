namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseFieldSettingConfiguration : EntityTypeConfiguration<CaseFieldSetting>
    {
        internal CaseFieldSettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Customer)
                .WithMany(c => c.CaseFieldSettings)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.DefaultValue).IsOptional().HasMaxLength(10);
            this.Property(x => x.FieldSize).IsRequired();
            this.Property(x => x.ListEdit).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseField");
            //this.Property(x => x.NameOrigin).IsRequired().HasMaxLength(50).HasColumnName("CaseFieldName");
            this.Property(x => x.Active).IsRequired();
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.RequiredIfReopened).IsRequired();
            this.Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("Show");
            this.Property(x => x.ShowExternal).IsRequired();
            this.Property(x => x.ShowStatusBar).IsRequired();
            this.Property(x => x.ShowExternalStatusBar).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CaseFieldSettingsGUID).IsOptional();

            this.ToTable("tblcasefieldsettings");
        }
    }

    public class CaseFieldSettingLanguageConfiguration : EntityTypeConfiguration<CaseFieldSettingLanguage>
    {
        internal CaseFieldSettingLanguageConfiguration()
        {
            this.HasKey(x => new { x.CaseFieldSettings_Id, x.Language_Id });

            this.HasRequired(x => x.CaseFieldSetting)
                .WithMany(x => x.CaseFieldSettingLanguages)
                .HasForeignKey(x => x.CaseFieldSettings_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.FieldHelp).IsOptional().HasMaxLength(200);
            this.Property(x => x.Label).IsOptional().HasMaxLength(50);

            this.ToTable("tblCaseFieldSettings_tblLang");
        }
    }
}
