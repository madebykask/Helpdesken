namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    public class DomainConfiguration : EntityTypeConfiguration<Domain.Domain>
    {
        internal DomainConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id);

            this.Property(x => x.Base).IsRequired().HasMaxLength(100).HasColumnName("LDAPBase");
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.FileFolder).IsOptional().HasMaxLength(100).HasColumnName("XMLFileFolder");
            this.Property(x => x.Filter).IsRequired().HasMaxLength(50).HasColumnName("LDAPFilter");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("DomainName");
            this.Property(x => x.Password).IsRequired().HasMaxLength(20).HasColumnName("LDAPPassword");
            this.Property(x => x.ServerName).IsRequired().HasMaxLength(100).HasColumnName("LDAPServerName");
            this.Property(x => x.UserName).IsRequired().HasMaxLength(100).HasColumnName("LDAPUserName");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbldomain");
        }
    }
}
