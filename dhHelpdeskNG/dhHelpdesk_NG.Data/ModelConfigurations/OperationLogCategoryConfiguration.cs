using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OperationLogCategoryConfiguration : EntityTypeConfiguration<OperationLogCategory>
    {
        internal OperationLogCategoryConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.OLCName).IsRequired().HasMaxLength(100).HasColumnName("OperationLogCategory");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbloperationlogcategory");
        }
    }
}

