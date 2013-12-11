namespace dhHelpdesk_NG.Data.ModelConfigurations.Faq
{
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain;

    public sealed class FaqLanguageConfiguration : EntityTypeConfiguration<FAQLanguage>
    {
        internal FaqLanguageConfiguration()
        {
            this.HasKey(fl => new { fl.FAQ_Id, fl.Language_Id });

            this.Property(fl => fl.FAQ_Id).IsRequired();
            this.Property(fl => fl.Language_Id).IsRequired();
            this.Property(fl => fl.FAQQuery).IsRequired().HasMaxLength(100);
            this.Property(fl => fl.Answer).IsRequired().HasMaxLength(2000);
            this.Property(fl => fl.Answer_Internal).IsRequired().HasMaxLength(1000);

            this.ToTable("tblfaq_tbllanguage");
        }
    }
}
