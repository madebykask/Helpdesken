namespace DH.Helpdesk.Dal.EntityConfigurations.Servers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Servers;

    public class ServerFieldSettingsConfiguration : EntityTypeConfiguration<ServerFieldSettings>
    {
        public ServerFieldSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ServerField).IsRequired().HasMaxLength(50);
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.Label).IsRequired().HasMaxLength(50);
            this.Property(x => x.Label_ENG).IsRequired().HasMaxLength(50);
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.FieldHelp).IsRequired().HasMaxLength(200);
            this.Property(x => x.ShowInList).IsRequired();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblServerFieldSettings");
        }
    }
}
