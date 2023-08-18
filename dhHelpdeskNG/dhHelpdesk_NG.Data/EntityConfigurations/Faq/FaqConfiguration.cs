using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Faq;

namespace DH.Helpdesk.Dal.EntityConfigurations.Faq
{
    public sealed class FaqConfiguration : EntityTypeConfiguration<FaqEntity>
    {
        internal FaqConfiguration()
        {
            HasKey(f => f.Id);
            Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(f => f.FAQCategory_Id).IsRequired().HasColumnName("Id_FAQCategory");
            Property(f => f.FAQQuery).IsRequired().HasMaxLength(100);
            Property(f => f.Answer).IsRequired().HasMaxLength(4000);
            Property(f => f.Answer_Internal).IsRequired().HasMaxLength(4000);
            Property(f => f.InformationIsAvailableForNotifiers).HasColumnName("PublicFAQ").IsRequired();
            Property(f => f.URL1).IsRequired().HasMaxLength(2000);
            Property(f => f.URL2).IsRequired().HasMaxLength(2000);
            Property(f => f.Customer_Id).IsOptional();
            Property(f => f.WorkingGroup_Id).IsOptional();
            Property(f => f.ShowOnStartPage).IsRequired();
            Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(f => f.ChangedDate).IsRequired();

            HasRequired(f => f.FAQCategory)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.FAQCategory_Id)
                .WillCascadeOnDelete(false);

            HasOptional(f => f.Customer)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(f => f.WorkingGroup)
                .WithMany(f => f.FAQs)
                .HasForeignKey(f => f.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            HasMany(f => f.FAQFiles)
                .WithRequired(f => f.FAQ)
                .HasForeignKey(f => f.FAQ_Id)
                .WillCascadeOnDelete(false);

            HasMany(f => f.FaqLanguages)
                .WithRequired(f => f.Faq)
                .HasForeignKey(f => f.FAQ_Id)
                .WillCascadeOnDelete(false);
            
            ToTable("tblFAQ");
        }
    }
}