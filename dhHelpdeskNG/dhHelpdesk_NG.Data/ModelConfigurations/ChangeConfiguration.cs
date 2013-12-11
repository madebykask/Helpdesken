using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ChangeConfiguration : EntityTypeConfiguration<Change>
    {
        internal ChangeConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Customer_Id).IsRequired();
            //Property(x => x.ChangeCategory_Id).IsOptional();
            //Property(x => x.ChangePriority_Id).IsOptional();
            //Property(x => x.WorkingGroup_Id).IsOptional();
            
            //Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(x => x.ChangeDescription).IsRequired().HasMaxLength(50).HasColumnName("ChangeDescription");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            ToTable("tblchange");
        }
    }
}

