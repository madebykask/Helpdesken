using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        internal OrderConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Department_Id).IsRequired();

            ToTable("tblorder");
        }
    }
}
