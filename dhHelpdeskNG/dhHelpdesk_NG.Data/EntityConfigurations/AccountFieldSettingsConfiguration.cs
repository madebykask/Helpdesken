
namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class AccountFieldSettingsConfiguration : EntityTypeConfiguration<AccountFieldSettings>
    {
        internal AccountFieldSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.AccountActivity)
                .WithMany()
                .HasForeignKey(a => a.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(a => a.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AccountActivity_Id).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.AccountField).IsRequired();
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.ShowInList).IsRequired();
            this.Property(x => x.ShowExternal).IsRequired();
            this.Property(x => x.Label).IsRequired().HasMaxLength(50);
            this.Property(x => x.FieldHelp).IsOptional().HasMaxLength(200);
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.EMailIdentifier).IsOptional().HasMaxLength(10);
    
            this.ToTable("tblaccountfieldsettings");
        }
    }
}
