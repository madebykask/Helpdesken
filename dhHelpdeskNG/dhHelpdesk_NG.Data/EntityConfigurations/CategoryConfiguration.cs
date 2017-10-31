namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        internal CategoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.ParentCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.Parent_Category_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Description).IsOptional().HasMaxLength(300).HasColumnName("CategoryDescription");
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Category");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CategoryGUID).IsOptional();
            this.Property(x => x.Parent_Category_Id).IsOptional();

            this.ToTable("tblcategory");
        }
    }
}
