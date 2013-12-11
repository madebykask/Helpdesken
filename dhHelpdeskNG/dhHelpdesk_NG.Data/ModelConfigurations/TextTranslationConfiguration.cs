using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class TextTranslationConfiguration : EntityTypeConfiguration<TextTranslation>
    {
        internal TextTranslationConfiguration()
        {
            HasKey(x => x.TextTranslation_Id);

            Property(x => x.TextTranslated).IsRequired().HasMaxLength(50).HasColumnName("TextTranslation");
            Property(x => x.Language_Id).IsRequired();
            Property(x => x.Text_Id).IsRequired();
            Property(x => x.TextTranslation_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");

            ToTable("tbltexttranslation");
        }
    }
}
