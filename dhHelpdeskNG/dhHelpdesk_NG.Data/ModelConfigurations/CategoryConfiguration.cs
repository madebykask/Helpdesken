using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        internal CategoryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Description).IsOptional().HasMaxLength(300).HasColumnName("CategoryDescription");
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Category");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcategory");
        }
    }
}
