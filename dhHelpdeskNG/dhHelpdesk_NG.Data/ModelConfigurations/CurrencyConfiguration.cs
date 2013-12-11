using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CurrencyConfiguration : EntityTypeConfiguration<Currency>
    {
        internal CurrencyConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Code).IsRequired().HasMaxLength(10).HasColumnName("CurrencyCode");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcurrency");
        }
    }
}
