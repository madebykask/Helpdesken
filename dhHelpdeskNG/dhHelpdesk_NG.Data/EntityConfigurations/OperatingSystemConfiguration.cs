namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.WorkstationModules;

    public class OperatingSystemConfiguration : EntityTypeConfiguration<OperatingSystem>
    {
        internal OperatingSystemConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Name).IsRequired().HasColumnName("OperatingSystem");
            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);
            this.ToTable("tbloperatingsystem");
        }
    }
}
