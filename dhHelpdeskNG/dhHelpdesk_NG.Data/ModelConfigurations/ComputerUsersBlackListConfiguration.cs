using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ComputerUsersBlackListConfiguration : EntityTypeConfiguration<ComputerUsersBlackList>
    {
        internal ComputerUsersBlackListConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.User_Id).IsRequired().HasMaxLength(50).HasColumnName("UserId");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcomputerusersblackList");
        }
    }
}
