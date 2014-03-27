namespace DH.Helpdesk.Dal.EntityConfigurations.Faq
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Faq;

    public sealed class FaqFileConfiguration : EntityTypeConfiguration<FaqFileEntity>
    {
        internal FaqFileConfiguration()
        {
            this.HasKey(f => f.Id);

            this.Property(f => f.FAQ_Id).IsRequired();

            this.HasRequired(f => f.FAQ)
                .WithMany(f => f.FAQFiles)
                .HasForeignKey(f => f.FAQ_Id)
                .WillCascadeOnDelete(false);

            this.Property(f => f.FileName).IsRequired().HasMaxLength(200);
            this.Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.ToTable("tblFAQFile");
        }
    }
}
