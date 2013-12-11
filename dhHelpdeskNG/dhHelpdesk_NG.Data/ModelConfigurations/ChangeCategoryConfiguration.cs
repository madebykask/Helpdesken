using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ChangeCategoryConfiguration : EntityTypeConfiguration<ChangeCategory>
    {
        internal ChangeCategoryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ChangeCategory");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            ToTable("tblchangecategory");
        }
    }
}

