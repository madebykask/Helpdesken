using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseFieldSettingConfiguration : EntityTypeConfiguration<CaseFieldSetting>
    {
        internal CaseFieldSettingConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Customer)
                .WithMany(c => c.CaseFieldSettings)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.DefaultValue).IsOptional().HasMaxLength(10);
            Property(x => x.FieldSize).IsRequired();
            Property(x => x.ListEdit).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseField");
            Property(x => x.NameOrigin).IsRequired().HasMaxLength(50).HasColumnName("CaseFieldName");
            Property(x => x.Required).IsRequired();
            Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("Show");
            Property(x => x.ShowExternal).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasefieldsettings");
        }
    }

    public class CaseFieldSettingLanguageConfiguration : EntityTypeConfiguration<CaseFieldSettingLanguage>
    {
        internal CaseFieldSettingLanguageConfiguration()
        {
            HasKey(x => new { x.CaseFieldSettings_Id, x.Language_Id });

            HasRequired(x => x.CaseFieldSetting)
                .WithMany(x => x.CaseFieldSettingLanguages)
                .HasForeignKey(x => x.CaseFieldSettings_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.FieldHelp).IsOptional().HasMaxLength(200);
            Property(x => x.Label).IsOptional().HasMaxLength(50);

            ToTable("tblCaseFieldSettings_tblLang");
        }
    }
}
