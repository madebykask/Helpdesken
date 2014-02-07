namespace DH.Helpdesk.Dal.EntityConfigurations.Faq
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

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
