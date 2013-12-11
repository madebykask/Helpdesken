namespace dhHelpdesk_NG.Data.ModelConfigurations.Faq
{
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain;

    public sealed class FaqCategoryLanguageConfiguration : EntityTypeConfiguration<FAQCategoryLanguage>
    {
        internal FaqCategoryLanguageConfiguration()
        {
            this.HasKey(cl => new { cl.FAQCategory_Id, cl.Language_Id });

            this.Property(cl => cl.FAQCategory_Id).IsRequired().HasColumnName("FAQCat_Id");
            this.Property(cl => cl.Language_Id).IsRequired();
            this.Property(cl => cl.Name).IsRequired().HasMaxLength(50);

            this.ToTable("tblfaqcat_tbllanguage");
        }
    }
}
