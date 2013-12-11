using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class DomainConfiguration : EntityTypeConfiguration<Domain.Domain>
    {
        internal DomainConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id);

            Property(x => x.Base).IsRequired().HasMaxLength(100).HasColumnName("LDAPBase");
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.FileFolder).IsOptional().HasMaxLength(100).HasColumnName("XMLFileFolder");
            Property(x => x.Filter).IsRequired().HasMaxLength(50).HasColumnName("LDAPFilter");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("DomainName");
            Property(x => x.Password).IsRequired().HasMaxLength(20).HasColumnName("LDAPPassword");
            Property(x => x.ServerName).IsRequired().HasMaxLength(100).HasColumnName("LDAPServerName");
            Property(x => x.UserName).IsRequired().HasMaxLength(100).HasColumnName("LDAPUserName");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbldomain");
        }
    }
}
