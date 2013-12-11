using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        internal AccountConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.AccountActivity_Id).IsRequired();
            Property(x => x.AccountType_Id).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Department_Id).IsRequired();
            Property(x => x.Department_Id2).IsRequired();
            Property(x => x.OU_Id).IsRequired();
            //Property(x => x.AccountType3).IsRequired();
            ToTable("tblaccount");
        }
    }
}
