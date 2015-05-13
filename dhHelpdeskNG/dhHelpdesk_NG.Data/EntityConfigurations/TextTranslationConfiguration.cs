namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class TextTranslationConfiguration : EntityTypeConfiguration<TextTranslation>
    {
        internal TextTranslationConfiguration()
        {
            this.HasKey(x => x.TextTranslation_Id);

            this.Property(x => x.TextTranslated).IsRequired().HasMaxLength(1000).HasColumnName("TextTranslation");
            this.Property(x => x.Language_Id).IsRequired();
            this.Property(x => x.Text_Id).IsRequired();
            this.Property(x => x.TextTranslation_Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            this.Property(x => x.ChangedByUser_Id).IsOptional();
            this.Property(x => x.ChangedDate).IsOptional();
            this.Property(x => x.CreatedDate).IsOptional();

            this.ToTable("tbltexttranslation");
        }
    }
}
