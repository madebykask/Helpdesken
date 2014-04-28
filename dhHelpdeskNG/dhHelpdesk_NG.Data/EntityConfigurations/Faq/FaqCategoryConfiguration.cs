namespace DH.Helpdesk.Dal.EntityConfigurations.Faq
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Faq;

    public sealed class FaqCategoryConfiguration : EntityTypeConfiguration<FaqCategoryEntity>
    {
        internal FaqCategoryConfiguration()
        {
            this.HasKey(c => c.Id);
            
            this.Property(c => c.Name).IsRequired().HasMaxLength(50);
            this.Property(c => c.Customer_Id).IsOptional();
            this.HasOptional(c => c.Customer).WithMany().HasForeignKey(c => c.Customer_Id).WillCascadeOnDelete(false);
            this.Property(c => c.Parent_FAQCategory_Id).IsOptional().HasColumnName("Parent_FAQCat_Id");

            this.HasOptional(c => c.ParentFAQCategory)
                .WithMany(c => c.SubFAQCategories)
                .HasForeignKey(c => c.Parent_FAQCategory_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(c => c.PublicFAQCategory).IsRequired().HasColumnName("PublicFAQCat");
            this.Property(c => c.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(c => c.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.ToTable("tblFAQCat");
        }
    }
}
