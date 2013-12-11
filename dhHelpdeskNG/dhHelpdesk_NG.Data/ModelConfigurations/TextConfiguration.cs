using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class TextConfiguration : EntityTypeConfiguration<Text>
    {
        internal TextConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(o => o.TextTranslations)
                .WithRequired(o => o.Text)
                .HasForeignKey(o => o.Text_Id).WillCascadeOnDelete(false);

            Property(x => x.TextToTranslate).IsRequired().HasMaxLength(50).HasColumnName("TextString");
            Property(x => x.Type).IsRequired().HasColumnName("TextType");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbltext");
        }
    }
}
