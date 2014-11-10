

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OrderFieldSettingsConfiguration : EntityTypeConfiguration<OrderFieldSettings>
    {
        internal OrderFieldSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.OrderType)
                .WithMany()
                .HasForeignKey(o => o.OrderType_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(o => o.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.OrderType_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.OrderField).IsRequired().HasMaxLength(50);
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.ShowInList).IsRequired();
            this.Property(x => x.ShowExternal).IsRequired();
            this.Property(x => x.Label).IsRequired().HasMaxLength(50);
            this.Property(x => x.DefaultValue).IsOptional().HasMaxLength(50);
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.EMailIdentifier).IsOptional().HasMaxLength(10);
    
            this.ToTable("tblorderfieldsettings");
        }
    }
}
