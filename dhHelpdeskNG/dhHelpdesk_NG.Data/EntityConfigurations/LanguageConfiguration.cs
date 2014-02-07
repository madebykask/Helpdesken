namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LanguageConfiguration : EntityTypeConfiguration<Language>
    {
        internal LanguageConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(x => x.TextTranslations)
                .WithRequired(x => x.Language)
                .HasForeignKey(x => x.Language_Id).WillCascadeOnDelete(false);

            this.Property(x => x.IsActive).IsRequired().HasColumnName("Active");
            this.Property(x => x.LanguageID).IsRequired().HasMaxLength(10);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("LanguageName");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbllanguage");
        }
    }
}
