namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CurrencyConfiguration : EntityTypeConfiguration<Currency>
    {
        internal CurrencyConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Code).IsRequired().HasMaxLength(10).HasColumnName("CurrencyCode");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcurrency");
        }
    }
}
