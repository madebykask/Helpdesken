namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OperatingSystemConfiguration : EntityTypeConfiguration<OperatingSystem>
    {
        internal OperatingSystemConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Name).IsRequired().HasColumnName("OperatingSystem");
            this.ToTable("tbloperatingsystem");
        }
    }
}
