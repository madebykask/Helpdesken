namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class TextConfiguration : EntityTypeConfiguration<Text>
    {
        internal TextConfiguration()
        {
            this.HasKey(x => x.Id).Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.HasMany(o => o.TextTranslations)
                .WithRequired(o => o.Text)
                .HasForeignKey(o => o.Text_Id).WillCascadeOnDelete(false);

            this.Property(x => x.TextToTranslate).IsRequired().HasMaxLength(50).HasColumnName("TextString");
            this.Property(x => x.Type).IsRequired().HasColumnName("TextType");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbltext");
        }
    }

    public class TextTypeConfiguration : EntityTypeConfiguration<TextType>
    {
        internal TextTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasColumnName("TextType");
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");

            this.ToTable("tbltexttype");
        }
    }
}
