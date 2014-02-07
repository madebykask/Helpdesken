namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        internal AccountConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.AccountActivity_Id).IsRequired();
            this.Property(x => x.AccountType_Id).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Department_Id).IsRequired();
            this.Property(x => x.Department_Id2).IsRequired();
            this.Property(x => x.OU_Id).IsRequired();
            //Property(x => x.AccountType3).IsRequired();
            this.ToTable("tblaccount");
        }
    }
}
