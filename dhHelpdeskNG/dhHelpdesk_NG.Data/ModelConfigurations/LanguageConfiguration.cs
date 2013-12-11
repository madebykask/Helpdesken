using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class LanguageConfiguration : EntityTypeConfiguration<Language>
    {
        internal LanguageConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(x => x.TextTranslations)
                .WithRequired(x => x.Language)
                .HasForeignKey(x => x.Language_Id).WillCascadeOnDelete(false);

            Property(x => x.IsActive).IsRequired().HasColumnName("Active");
            Property(x => x.LanguageID).IsRequired().HasMaxLength(10);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("LanguageName");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbllanguage");
        }
    }
}
