namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class EmploymentTypeConfiguration : EntityTypeConfiguration<EmploymentType>
    {
        internal EmploymentTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("EmploymentType").HasMaxLength(50).IsRequired();
            this.Property(x => x.Status).IsRequired();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();
        }
    }
}